using MovieAPI.Models;

namespace MovieAPI.Data
{
    public interface IActorRepository
    {
        Task<IEnumerable<Actor>> GetActors();
        Task<IEnumerable<Actor>> GetActorByName(string Name);
        Task<IEnumerable<Actor>> GetActorsByMovie(int MovieId);
        Task<Actor> GetActorById(int Id);
        Task<Actor> InsertActor(Actor actor, int MovieId);
        Task<Actor> UpdateActor(Actor actor);
        Task<Actor> DeleteActor(int Id);

    }
}
