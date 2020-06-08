using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using mmedia.Models;

using WebVideo.Models;

namespace WebVideo.MovieService {
  public interface IMovieService {

    string RootPath { get; }

    #region --- Movies --------------------------------------------
    List<string> ExcludedExtensions { get; }
    IMovies GetMovies(string group, int groupLevel = 1, int startPage = 1, int pageSize = 20);
    Task<byte[]> GetPicture(string pathname);
    #endregion --- Movies --------------------------------------------

    #region --- Groups --------------------------------------------
    string CurrentGroup { get; }
    IMovieGroups GetGroups(string group = "");
    #endregion --- Groups --------------------------------------------

    
  }
}
