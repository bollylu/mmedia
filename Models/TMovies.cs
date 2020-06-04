using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebVideo.Models {
  public class TMovies : IMovies {
    public List<IMovie> Movies { get; }
    public int Page { get; set; }
    public int AvailablePages { get; set; }
    public string Source { get; set; }

    public TMovies() {
      Movies = new List<IMovie>();
    }
  }
}
