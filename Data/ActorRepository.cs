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

        public async Task<IEnumerable<Actor>> GetActorsByMovie(int MovieId)
        {
            //join the Actors/Movies/Relations tables
            var query = from a in _apiContext.Actors
                        join rel in _apiContext.ActorMovieRelations
                        on a.Id equals rel.ActorId
                        join m in _apiContext.Movies
                        on rel.MovieId equals m.Id
                        where m.Id == MovieId
                        select a;

            var result  = await query.ToListAsync().ConfigureAwait(false);                

            return result; 
        }

        public async Task<IEnumerable<Actor>> GetActorByName(string Name)
        {
            return await _apiContext.Actors.Where(c => c.Name.ToUpper().Contains(Name.ToUpper())).ToListAsync();
        }

        public async Task<IEnumerable<Actor>> GetActors()
        {
            return await _apiContext.Actors.ToListAsync();
        }

        public async Task<Actor> InsertActor(Actor actor, int MovieId)
        {
            _apiContext.Actors.Add(actor);

            //Add the relation every time an actor is added
            _apiContext.ActorMovieRelations.Add(new ActorMovieRelation { ActorId = actor.Id, MovieId = MovieId});
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
