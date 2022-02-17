using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using SmartBooking.Models;
using SmartBooking.Services;
using System.Threading.Tasks;

namespace SmartBooking.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MoviesController : ControllerBase
    {
        private readonly IMovieService _movieService;
        private readonly ILogger<MoviesController> _logger;
        public MoviesController(IMovieService movieService, ILogger<MoviesController> logger)
        {
            _movieService = movieService;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> GetAvailableMovies()
        {
            var result = await _movieService.GetAvailableMovies();
            return Ok(JsonConvert.SerializeObject(result, Formatting.Indented));
        }

        [HttpPost]
        [Route("details")]
        public async Task<IActionResult> GetMovieDetails([FromBody] MovieReq movieReq)
        {
            _logger.LogInformation($"Movire request {JsonConvert.SerializeObject(movieReq)}");
            var result = await _movieService.GetMovieDetails(movieReq);
            return Ok(JsonConvert.SerializeObject(result, Formatting.Indented));
        }
    }
}
