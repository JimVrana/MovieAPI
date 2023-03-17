using MovieAPI.Models;

namespace MovieAPI.Data
{
    public interface IActorRepository
    {
        Task<IEnumerable<Actor>> GetActors();
        Task<IEnumerable<Actor>> GetActorByName(string Name);
        Task<IEnumerable<Actor>> GetActorByMovie(string title);
        Task<Actor> GetActorById(int id);
        Task<Actor> InsertActor(Actor actor);
        Task<Actor> UpdateActor(Actor actor);
        Task<Actor> DeleteActor(int id);

    }
}
