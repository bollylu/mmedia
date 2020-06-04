using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebVideo.Models {
  public class TMovie : IMovie {
    public string LocalName { get; set; }
    public string LocalPath { get; set; }
    public string Group { get; set; }
    public long Size { get; set; }
    public string Picture { get; set; }
  }
}
