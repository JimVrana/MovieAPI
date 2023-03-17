using Microsoft.EntityFrameworkCore;
using MovieAPI.Models;
using System;

namespace MovieAPI.Data
{
    public class MovieRepository : IMovieRepository
    {
        private readonly ApiContext _apiContext;

        public MovieRepository(ApiContext apiContext)
        {
            _apiContext = apiContext ?? throw new ArgumentNullException(nameof(apiContext));
        }

        public async Task<Movie> DeleteMovie(int id)
        {
            //remove the relations to actors when deleting a movie
            var result = await _apiContext.Movies               
                .FirstOrDefaultAsync(e => e.Id == id);
            var relations = from rel in _apiContext.ActorMovieRelations
                            where rel.MovieId == id
                            select rel;

            if (result != null)
            {
                _apiContext.ActorMovieRelations.RemoveRange(relations);
                _apiContext.Movies.Remove(result);
                await _apiContext.SaveChangesAsync();
                return result;
            }

            return null;
        }

        public async Task<Movie> GetMovieById(int id)
        {
            return await _apiContext.Movies.FindAsync(id);
        }

        public async Task<IEnumerable<Movie>> GetMoviesByTitle(string title)
        {
            return await _apiContext.Movies.Where(c => c.Title.ToUpper().Contains(title.ToUpper())).ToListAsync();
        }

        public async Task<IEnumerable<Movie>> GetMovies()
        {
            return await _apiContext.Movies.ToListAsync();
        }

        public async Task<Movie> InsertMovie(Movie movie)
        {
            _apiContext.Movies.Add(movie);
            await _apiContext.SaveChangesAsync();
            return movie;
        }

        public async Task<Movie> UpdateMovie(Movie movie)
        {
            _apiContext.Entry(movie).State = EntityState.Modified;
            await _apiContext.SaveChangesAsync();
            return movie;
        }

        public async Task<IEnumerable<Movie>> GetMoviesByActor(int ActorId)
        {
            //Join between the Actors/Movies/relation
            var query = from a in _apiContext.Actors
                        join rel in _apiContext.ActorMovieRelations
                        on a.Id equals rel.ActorId
                        join m in _apiContext.Movies
                        on rel.MovieId equals m.Id
                        where a.Id == ActorId
                        select m;

            var result = await query.ToListAsync().ConfigureAwait(false);

            return result;
        }
    }
}
