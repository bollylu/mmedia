using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using BLTools;
using BLTools.Diagnostic.Logging;
using Microsoft.Extensions.Logging;

using mmedia.Models;

using WebVideo.Models;

namespace WebVideo.MovieService {
  public class TMovieService : ALoggable, IMovieService {

    private const string ROOT_NAME = "Root";

    public string RootPath => @"\\andromeda.sharenet.priv\films\";
    public string Source => "Andromeda";

    public List<string> ExcludedExtensions { get; } = new List<string>() { ".nfo", ".jpg", ".vsmeta" };

    #region --- Constructor(s) ---------------------------------------------------------------------------------
    public TMovieService() {
      Logger = new TConsoleLogger();
      _Initialize();
    }

    private bool _IsInitialized = false;
    private void _Initialize() {
      if (_IsInitialized) {
        return;
      }
      Task.Run(() => LoadMoviesCache());

      _DemoCache.Movies.Add(new TMovie() { LocalName = "Alien agent (2007).avi", LocalPath = @"Science-fiction\[Aliens, créatures, ...]\Alien agent (2007)" });
      _DemoCache.Movies.Add(new TMovie() { LocalName = "Godzilla (1954).mkv", LocalPath = @"Science-fiction\[Aliens, créatures, ...]\Godzilla #\Godzilla (1954)" });

      _IsInitialized = true;

    }
    #endregion --- Constructor(s) ------------------------------------------------------------------------------

    #region --- Cache management --------------------------------------------
    private readonly List<IMovie> _MoviesCache = new List<IMovie>();
    private IEnumerable<IMovie> MoviesWithGroup => _MoviesCache.Where(m => !string.IsNullOrWhiteSpace(m.Group));

    private readonly object _LockCache = new object();

    public Task LoadMoviesCache() {
      Log("Initializing movies cache");

      lock (_LockCache) {
        _MoviesCache.Clear();

        DirectoryInfo RootFolder = new DirectoryInfo(RootPath);
        IEnumerable<FileInfo> MoviesInfo = RootFolder.GetFiles("*.*", new EnumerationOptions() { RecurseSubdirectories = true })
                                                     .Where(f => !ExcludedExtensions.Contains(f.Extension));

        foreach (FileInfo MovieInfoItem in MoviesInfo.OrderBy(f => f.FullName)) {

          string GroupName = MovieInfoItem.DirectoryName.After(RootPath, System.StringComparison.InvariantCultureIgnoreCase)
                                                        .BeforeLast(Path.DirectorySeparatorChar)
                                                        .Replace('\\', '/');

          if (!GroupName.EndsWith('/')) {
            GroupName = $"{GroupName}/";
          }
                                                        

          //Logger.LogInformation($"Group name = {GroupName}");

          string PictureLocation = MovieInfoItem.DirectoryName.After(RootPath, System.StringComparison.InvariantCultureIgnoreCase)
                                                              .Replace('\\', '/');

          //Logger.LogInformation($"PictureLocation = {PictureLocation}");

          _MoviesCache.Add(new TMovie() {
            LocalName = MovieInfoItem.Name,
            LocalPath = MovieInfoItem.DirectoryName.After(RootPath, System.StringComparison.InvariantCultureIgnoreCase),
            Group = GroupName,
            Size = MovieInfoItem.Length,
            Picture = PictureLocation
          });
        }
      }

      Log("Cache initialized successfully");
      return Task.CompletedTask;
    }

    public void ClearCache() {
      lock (_LockCache) {
        _MoviesCache.Clear();
      }
    }

    private static readonly IMovies _DemoCache = new TMovies() {
      Source = "Demo",
      Page = 1,
      AvailablePages = 1
    };

    #endregion --- Cache management --------------------------------------------

    public string CurrentGroup { get; private set; }
    public IMovieGroups GetGroups(string group = "") {
      if (group == null) {
        group = "";
      }

      int GroupLevel = group.Count(x => x == '/') + 1;

      CurrentGroup = group == "" ? ROOT_NAME : group.EndsWith('/') ? group : $"{group}/";

      Log($"Getting groups for {group}, level={GroupLevel}");
      Log($"Current group {CurrentGroup}");

      IMovieGroups RetVal = new TMovieGroups() { Name = CurrentGroup };

      if (_MoviesCache.IsEmpty()) {
        return RetVal;
      }

      lock (_LockCache) {

        if (CurrentGroup == ROOT_NAME) {

          foreach (IEnumerable<IMovie> MovieItems in _MoviesCache.GroupBy(m => _getGroupFilter(m.Group, 1))) {
            RetVal.Groups.Add(new TMovieGroup() { Name = _getGroupFilter(MovieItems.First().Group, 1), Count = MovieItems.Count() });
          }

        } else {

          IEnumerable<IMovie> FilteredCache = _MoviesCache.Where(m => m.Group.StartsWith(CurrentGroup, StringComparison.CurrentCultureIgnoreCase));

          IEnumerable<IMovie> MoviesNotInCurrentGroup = FilteredCache.Where(m => !m.Group.Equals(CurrentGroup, StringComparison.CurrentCultureIgnoreCase));

          if (FilteredCache.Select(m => m.Group).Distinct().Count() > 1) {
            foreach (IEnumerable<IMovie> MovieItems in MoviesNotInCurrentGroup.GroupBy(m => _getGroupFilter(m.Group, GroupLevel + 1))) {
              RetVal.Groups.Add(new TMovieGroup() { Name = _getGroupFilter(MovieItems.First().Group, GroupLevel + 1), Count = MovieItems.Count() });
            }
          }

        }

      }

      return RetVal;
    }

    public IMovies GetMovies(string group, int groupLevel = 1, int startPage = 1, int pageSize = 20) {



      IMovies RetVal;

      if (_MoviesCache.IsEmpty()) {
        return _DemoCache;
      }

      #region --- All movies --------------------------------------------
      if (string.IsNullOrWhiteSpace(group)) {
        lock (_LockCache) {
          RetVal = new TMovies() {
            Source = Source,
            Page = startPage,
            AvailablePages = _MoviesCache.Count() % pageSize == 0 ?
                             _MoviesCache.Count() / pageSize :
                             (int)(_MoviesCache.Count() / pageSize) + 1
          };

          foreach (IMovie MovieItem in _MoviesCache.Skip((startPage.WithinLimits(1, int.MaxValue) - 1) * pageSize).Take(pageSize)) {
            RetVal.Movies.Add(MovieItem);
          }

          return RetVal;
        }
      }
      #endregion --- All movies --------------------------------------------

      string CurrentGroup = $"{group.TrimEnd('/')}/";

      lock (_LockCache) {
        IEnumerable<IMovie> FilteredCache = _MoviesCache.Where(m => m.Group.StartsWith(CurrentGroup, StringComparison.CurrentCultureIgnoreCase));

        RetVal = new TMovies() {
          Source = Source,
          Page = startPage,
          AvailablePages = FilteredCache.Count() % pageSize == 0 ?
                           FilteredCache.Count() / pageSize :
                           (int)(FilteredCache.Count() / pageSize) + 1
        };

        foreach (IMovie MovieItem in FilteredCache
                                     .Skip((startPage.WithinLimits(1, int.MaxValue) - 1) * pageSize).Take(pageSize)) {
          RetVal.Movies.Add(MovieItem);
        }

        return RetVal;
      }
    }

    private static byte[] _MissingPicture = File.ReadAllBytes("Pictures\\missing.jpg");

    public async Task<byte[]> GetPicture(string pathname) {
      if (string.IsNullOrWhiteSpace(pathname)) {
        return _MissingPicture;
      }

      string FullFileName = Path.Combine(RootPath, pathname, "folder.jpg");

      if (!File.Exists(FullFileName)) {
        return _MissingPicture;
      }

      try {
        CancellationTokenSource TimeOut = new CancellationTokenSource(1000);
        return await File.ReadAllBytesAsync(FullFileName, TimeOut.Token);
      } catch {
        return _MissingPicture;
      }
    }

    private string _getGroupFilter(string group, int level = 1) {
      if (string.IsNullOrWhiteSpace(group)) {
        return "";
      }
      return string.Join("/", group.Split('/').Take(level));
    }
  }
}
