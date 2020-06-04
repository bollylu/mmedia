using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

using WebVideo.Models;
using WebVideo.MovieService;

namespace WebVideo.Controllers {

  //[EnableCors("AllowAll")]
  [Route("api/[controller]")]
  [ApiController]
  public class GroupsController : ControllerBase {

    IMoviesService MovieServices;

    public GroupsController(IMoviesService movieServices) {
      MovieServices = movieServices;
    }

    [HttpGet]
    public IEnumerable<IMovieGroup> Get(string group, int level=1) {
      return MovieServices.GetGroups(WebUtility.UrlDecode(group), level);

    }
  }
}
