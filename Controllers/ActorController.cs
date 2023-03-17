
using Microsoft.AspNetCore.Mvc;
using MovieAPI.Data;
using MovieAPI.Models;

namespace ActorAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ActorController : ControllerBase
    {
        private readonly IActorRepository _actorRepository;

        public ActorController(IActorRepository actorRepository)
        {
            _actorRepository = actorRepository ?? throw new ArgumentNullException(nameof(actorRepository));
        }


        [HttpGet]
        [Route("GetAllActors")]
        public async Task<IActionResult> Get()
        {
            try
            {
                return Ok(await _actorRepository.GetActors());
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error retrieving data from the database");
            }
        }

        [HttpGet]
        [Route("GetActorById/{Id}")]
        public async Task<IActionResult> GetActorById(int Id)
        {
            try
            {
                var result = await _actorRepository.GetActorById(Id);

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
        [Route("GetActorByName/{Name}")]
        public async Task<IActionResult> GetActorByName(string Name)
        {
            try
            {
                var result = await _actorRepository.GetActorByName(Name);

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

        [HttpPost]
        public async Task<ActionResult<Actor>> CreateActor([FromBody] Actor Actor)
        {
            try
            {
                if (Actor == null)
                    return BadRequest();

                var createdActor = await _actorRepository.InsertActor(Actor);

                return CreatedAtAction(nameof(GetActorById),
                    new { id = createdActor.Id }, createdActor);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error creating new Actor record");
            }
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<Actor>> Update([FromBody] Actor Actor)
        {
            try
            {
                if (Actor == null)
                    return BadRequest();

                var updatedActor = await _actorRepository.UpdateActor(Actor);

                if (updatedActor == null)
                {
                    return NotFound();
                }
                return Ok(updatedActor);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error updating Actor record");
            }
        }

        [HttpDelete("{id:int}")]
        public async Task<ActionResult<Actor>> DeleteActor(int id)
        {
            try
            {
                var ActorToDelete = await _actorRepository.GetActorById(id);

                if (ActorToDelete == null)
                {
                    return NotFound($"Actor with Id = {id} not found");
                }

                return await _actorRepository.DeleteActor(id);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error deleting Actor");
            }
        }

    }

}

