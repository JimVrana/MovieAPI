using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MovieAPI.Data;
using MovieAPI.Models;

namespace MovieAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RatingsController : ControllerBase
    {
        private readonly IRatingsRepository _ratingsRepository;

        public RatingsController(IRatingsRepository ratingsRepository)
        {
            _ratingsRepository = ratingsRepository ?? throw new ArgumentNullException(nameof(ratingsRepository));
        }

        [HttpGet]
        [Route("GetMovieRatingById/{Id}")]
        public async Task<IActionResult> GetMovieRatingById(int Id)
        {
            try
            {
                var result = await _ratingsRepository.GetMovieRatingById(Id);

                if (result == null)
                {
                    return NotFound();
                }
                return Ok(result);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error retrieving data from the database");
            }
        }

        [HttpGet]
        [Route("GetMovieRatingsByMovie/{Id}")]
        public async Task<IActionResult> GetMovieRatingsByMovie(int Id)
        {
            try
            {
                var result = await _ratingsRepository.GetMovieRatingsByMovie(Id);

                if (result == null)
                {
                    return NotFound();
                }
                return Ok(result);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error retrieving data from the database");
            }
        }

        [Authorize]
        [HttpPost]
        public async Task<ActionResult<MovieRating>> CreateMovieRating([FromBody] MovieRating rating)
        {
            try
            {
                if (rating == null)
                    return BadRequest();

                var createdMovie = await _ratingsRepository.InsertMovieRating(rating);

                return CreatedAtAction(nameof(GetMovieRatingById),
                    new { id = createdMovie.Id }, createdMovie);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error creating new movie rating record");
            }
        }

        [Authorize]
        [HttpPut]
        public async Task<ActionResult<MovieRating>> Update([FromBody] MovieRating rating)
        {
            try
            {
                if (rating == null)
                    return BadRequest();

                var updatedMovieRating = await _ratingsRepository.UpdateMovieRating(rating);

                if (updatedMovieRating == null)
                {
                    return NotFound();
                }
                return Ok(updatedMovieRating);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error updating movie rating record");
            }
        }

        [Authorize]
        [HttpDelete("{id:int}")]
        public async Task<ActionResult<MovieRating>> DeleteMovieRating(int id)
        {
            try
            {
                var movieRatingToDelete = await _ratingsRepository.GetMovieRatingById(id);

                if (movieRatingToDelete == null)
                {
                    return NotFound($"MovieRating with Id = {id} not found");
                }

                return await _ratingsRepository.DeleteMovieRating(id);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error deleting movie rating");
            }
        }

    }
}
