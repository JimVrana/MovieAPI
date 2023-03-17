using Microsoft.EntityFrameworkCore;
using MovieAPI.Models;
using System;

namespace MovieAPI.Data
{
    public class ActorRepository : IActorRepository
    {

        private readonly ApiContext _apiContext;

        public ActorRepository(ApiContext apiContext)
        {
            _apiContext = apiContext ?? throw new ArgumentNullException(nameof(apiContext));
        }

        public async Task<Actor> DeleteActor(int id)
        {
            var result = await _apiContext.Actors
                .FirstOrDefaultAsync(e => e.Id == id);
            if (result != null)
            {
                _apiContext.Actors.Remove(result);
                await _apiContext.SaveChangesAsync();
                return result;
            }

            return null;
        }

        public async Task<Actor> GetActorById(int id)
        {
            return await _apiContext.Actors.FindAsync(id);
        }

        public async Task<IEnumerable<Actor>> GetActorByMovie(string title)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<Actor>> GetActorByName(string Name)
        {
            return await _apiContext.Actors.Where(c => c.Name.ToUpper().Contains(Name.ToUpper())).ToListAsync();
        }

        public async Task<IEnumerable<Actor>> GetActors()
        {
            return await _apiContext.Actors.ToListAsync();
        }

        public async Task<Actor> InsertActor(Actor actor)
        {
            _apiContext.Actors.Add(actor);
            await _apiContext.SaveChangesAsync();
            return actor;
        }

        public async Task<Actor> UpdateActor(Actor actor)
        {
            _apiContext.Entry(actor).State = EntityState.Modified;
            await _apiContext.SaveChangesAsync();
            return actor;
        }
    }
}
