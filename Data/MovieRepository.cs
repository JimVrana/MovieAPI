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
            var result = await _apiContext.Movies
                .FirstOrDefaultAsync(e => e.Id == id);
            if (result != null)
            {
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

        public async Task<IEnumerable<Movie>> GetMovieByTitle(string title)
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
    }
}
