using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using mmedia.Models;
using WebVideo.Models;
using WebVideo.MovieService;

namespace WebVideo.Controllers {

  //[EnableCors("AllowAll")]
  [Route("api/[controller]")]
  [ApiController]
  public class GroupsController : ControllerBase {

    IMovieService MovieServices;

    public GroupsController(IMovieService movieServices) {
      MovieServices = movieServices;
    }

    [HttpGet]
    public IActionResult Get(string group) {
      return Ok(MovieServices.GetGroups(WebUtility.UrlDecode(group)));
    }
  }
}
