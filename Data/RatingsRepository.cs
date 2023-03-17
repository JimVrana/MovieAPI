using Microsoft.EntityFrameworkCore;
using MovieAPI.Models;

namespace MovieAPI.Data
{
    public class RatingsRepository : IRatingsRepository
    {

        private readonly ApiContext _apiContext;

        public RatingsRepository(ApiContext apiContext)
        {
            _apiContext = apiContext ?? throw new ArgumentNullException(nameof(apiContext));
        }

        public async Task<MovieRating> GetMovieRatingById(int id)
        {
            return await _apiContext.MovieRatings.FindAsync(id);
        }

        public async Task<IEnumerable<MovieRating>> GetMovieRatingsByMovie(int Id)
        {
            var query = from r in _apiContext.MovieRatings
                        where r.MovieId == Id
                        select r;

            var result = await query.ToListAsync().ConfigureAwait(false);

            return result;
        }

        public async Task<MovieRating> InsertMovieRating(MovieRating rating)
        {
            _apiContext.MovieRatings.Add(rating);
            await _apiContext.SaveChangesAsync();
            return rating;
        }

        public async Task<MovieRating> UpdateMovieRating(MovieRating rating)
        {
            _apiContext.Entry(rating).State = EntityState.Modified;
            await _apiContext.SaveChangesAsync();
            return rating;
        }

       public async Task<MovieRating> DeleteMovieRating(int id)
        {
            var result = await _apiContext.MovieRatings
                .FirstOrDefaultAsync(e => e.Id == id);

            if (result != null)
            {
                _apiContext.MovieRatings.Remove(result);
                await _apiContext.SaveChangesAsync();
                return result;
            }

            return null;
        }
    }
}
