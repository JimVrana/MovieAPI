using MovieAPI.Models;

namespace MovieAPI.Data
{
    public interface IRatingsRepository
    {
        Task<MovieRating> GetMovieRatingById(int Id);
        Task<IEnumerable<MovieRating>> GetMovieRatingsByMovie(int Id);
        Task<MovieRating> InsertMovieRating(MovieRating rating);
        Task<MovieRating> UpdateMovieRating(MovieRating rating);
        Task<MovieRating> DeleteMovieRating(int id);
    }
}
