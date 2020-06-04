using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using BLTools;

using Microsoft.Extensions.Logging;

using WebVideo.Models;

namespace WebVideo.MovieService {
  public class TMovieService : IMoviesService {

    public string RootPath => @"\\andromeda.sharenet.priv\films\";
    public string Source => "Andromeda";

    public List<string> ExcludedExtensions { get; } = new List<string>() { ".nfo", ".jpg", ".vsmeta" };

    #region --- Constructor(s) ---------------------------------------------------------------------------------
    public TMovieService() {
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

      lock (_LockCache) {
        _MoviesCache.Clear();

        DirectoryInfo RootFolder = new DirectoryInfo(RootPath);
        IEnumerable<FileInfo> MoviesInfo = RootFolder.GetFiles("*.*", new EnumerationOptions() { RecurseSubdirectories = true })
                                                     .Where(f => !ExcludedExtensions.Contains(f.Extension));

        foreach (FileInfo MovieInfoItem in MoviesInfo.OrderBy(f => f.FullName)) {

          string GroupName = MovieInfoItem.DirectoryName.After(RootPath, System.StringComparison.InvariantCultureIgnoreCase)
                                                        .BeforeLast(Path.DirectorySeparatorChar)
                                                        .Replace('\\', '/');

          string PictureLocation = Path.Combine(GroupName, MovieInfoItem.DirectoryName.After(RootPath, System.StringComparison.InvariantCultureIgnoreCase)
                                                                        .After(GroupName)
                                                                        .After(Path.DirectorySeparatorChar))
                                       .Replace('\\', '/');

          _MoviesCache.Add(new TMovie() {
            LocalName = MovieInfoItem.Name,
            LocalPath = MovieInfoItem.DirectoryName.After(RootPath, System.StringComparison.InvariantCultureIgnoreCase).After(GroupName).TrimStart(Path.DirectorySeparatorChar),
            Group = GroupName,
            Size = MovieInfoItem.Length,
            Picture = PictureLocation
          });
        }
      }
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


    public IMovies GetAll(int startPage = 1, int pageSize = 20) {
      IMovies RetVal;

      if (_MoviesCache.IsEmpty()) {
        return _DemoCache;
      }

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

    public string CurrentGroup { get; private set; }
    public IEnumerable<IMovieGroup> GetGroups(string group = "", int groupLevel = 1) {
      if (group == null) {
        group = "";
      }
      CurrentGroup = string.Join('/', group.Split('/').Take(groupLevel));

      if (_MoviesCache.IsEmpty()) {
        yield break;
      }

      lock (_LockCache) {

        IEnumerable<IMovie> FilteredCache = _MoviesCache.Where(m => m.Group.StartsWith(CurrentGroup, StringComparison.CurrentCultureIgnoreCase));
        foreach (IEnumerable<IMovie> MovieItems in FilteredCache.GroupBy(m => m.Group)) { 
          yield return new TMovieGroup() { Name = MovieItems.First().Group, Count = MovieItems.Count() };
        }

      }
    }

    public IMovies GetMovies(string group, int groupLevel = 1, int startPage = 1, int pageSize = 20) {
      IMovies RetVal;

      if (_MoviesCache.IsEmpty()) {
        return _DemoCache;
      }

      string GroupWithLevel = string.Join('/', group.Split('/').Take(groupLevel));

      lock (_LockCache) {
        IEnumerable<IMovie> FilteredCache = _MoviesCache.Where(m => m.Group.StartsWith(GroupWithLevel, StringComparison.CurrentCultureIgnoreCase));

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
  }
}
