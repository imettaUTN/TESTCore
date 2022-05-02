using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using MovieService.WebAPI.Data;
using MovieService.WebAPI.Services;

namespace MovieService.WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]

    public class MoviesController : ControllerBase
    {
        private readonly IMoviesService _movieService;

        public MoviesController(IMoviesService movieService)
        {
            _movieService = movieService;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var movie = (await _movieService.Get(new[] { id })).FirstOrDefault();
            if (movie == null)
            {
                return NotFound();
            }

            return Ok(movie);
        }

        [HttpPost("")]
        public async Task<IActionResult> Add(Movie movie)
        {
            await _movieService.Add(movie);
            return Ok();
        }
    }

}
