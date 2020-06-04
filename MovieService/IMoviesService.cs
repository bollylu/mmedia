using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using WebVideo.Models;

namespace WebVideo.MovieService {
  public interface IMoviesService {

    string RootPath { get; }
    List<string> ExcludedExtensions { get; }

    IMovies GetAll(int startPage = 1, int pageSize = 20);
    IMovies GetMovies(string group, int groupLevel = 1, int startPage = 1, int pageSize = 20);

    string CurrentGroup { get; }
    IEnumerable<IMovieGroup> GetGroups(string group = "", int level = 1);
    Task<byte[]> GetPicture(string pathname);
  }
}
