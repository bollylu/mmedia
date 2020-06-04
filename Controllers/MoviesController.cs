using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using WebVideo.MovieService;

using BLTools;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.WebUtilities;
using System.Text.Encodings.Web;
using Microsoft.AspNetCore.Mvc.Routing;
using System.Net;
using WebVideo.Models;

namespace WebVideo.Controllers {

  //[EnableCors("AllowAll")]
  [Route("api/[controller]")]
  [ApiController]
  public class MoviesController : ControllerBase {

    IMoviesService MovieServices;

    public MoviesController(IMoviesService movieServices) {
      MovieServices = movieServices;
    }

    [HttpGet()]
    public IActionResult Get(int page = 1, int items = 20) {
      return Ok(MovieServices.GetAll(page, items));
    }

    [HttpGet("forGroup")]
    public IActionResult Get(string group = "", int level = 1, int page = 1, int items = 20) {
      if (string.IsNullOrWhiteSpace(group)) {
        return Ok(MovieServices.GetAll(page, items));
      } else {
        return Ok(MovieServices.GetMovies(WebUtility.UrlDecode(group), level, page, items));
      }
    }

    [HttpGet("getPicture/{pathname}")]
    public async Task<IActionResult> GetPicture(string pathname) {
      return File(await MovieServices.GetPicture(WebUtility.UrlDecode(pathname)), "image/jpeg");
    }
  }
}
