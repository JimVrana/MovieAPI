using Microsoft.AspNetCore.Mvc;
using MovieAPI.Data;
using MovieAPI.Models;

namespace MovieAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MovieController : ControllerBase
    {
        private readonly IMovieRepository _movieRepository;

        public MovieController(IMovieRepository movieRepository)
        {
            _movieRepository = movieRepository ?? throw new ArgumentNullException(nameof(movieRepository));
        }

        [HttpGet]
        [Route("GetAllMovies")]
        public async Task<IActionResult> Get()
        {
            try
            {
                return Ok(await _movieRepository.GetMovies());
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,"Error retrieving data from the database");
            }
        }

        [HttpGet]
        [Route("GetMovieById/{Id}")]
        public async Task<IActionResult> GetMovieById(int Id)
        {
            try
            {
                var result = await _movieRepository.GetMovieById(Id);

                if (result == null)
                {
                    return NotFound();
                }
                return Ok(result);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,"Error retrieving data from the database");
            }
        }

        [HttpGet]
        [Route("GetMovieByTitle/{Title}")]
        public async Task<IActionResult> GetMovieByTitle(string Title)
        {
            try
            {
                var result = await _movieRepository.GetMovieByTitle(Title);

                if (result == null)
                {
                    return NotFound();
                }
                return Ok(result);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,"Error retrieving data from the database");
            }
        }

        [HttpPost]
        public async Task<ActionResult<Movie>>CreateMovie([FromBody] Movie movie)
        {
            try
            {
                if (movie == null)
                    return BadRequest();

                var createdMovie = await _movieRepository.InsertMovie(movie);

                return CreatedAtAction(nameof(GetMovieById),
                    new { id = createdMovie.Id }, createdMovie);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,"Error creating new movie record");
            }
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<Movie>> Update([FromBody] Movie movie)
        {
            try
            {
                if (movie == null)
                    return BadRequest();

                var updatedMovie = await _movieRepository.UpdateMovie(movie);

                if (updatedMovie == null)
                {
                    return NotFound();
                }
                return Ok(updatedMovie);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,"Error updating movie record");
            }
        }

        [HttpDelete("{id:int}")]
        public async Task<ActionResult<Movie>> DeleteMovie(int id)
        {
            try
            {
                var movieToDelete = await _movieRepository.GetMovieById(id);

                if (movieToDelete == null)
                {
                    return NotFound($"Movie with Id = {id} not found");
                }

                return await _movieRepository.DeleteMovie(id);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,"Error deleting movie");
            }
        }

    }
}
