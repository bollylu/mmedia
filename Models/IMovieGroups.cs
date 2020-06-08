using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using WebVideo.Models;

namespace mmedia.Models {
  public interface IMovieGroups {
    string Name { get; }
    List<IMovieGroup> Groups { get; }
  }
}
