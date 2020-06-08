using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using WebVideo.Models;

namespace mmedia.Models {
  public class TMovieGroups : IMovieGroups {
    public string Name { get; set; }
    public List<IMovieGroup> Groups { get; } = new List<IMovieGroup>();
  }
}
