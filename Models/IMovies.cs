using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebVideo.Models {
  public interface IMovies {
    string Source { get; }
    List<IMovie> Movies { get; }
    int Page { get; }
    int AvailablePages { get; }

  }
}
