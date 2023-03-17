using MovieAPI.Models;

namespace MovieAPI.Data
{
    public interface IRatingsRepository
    {
        Task<IEnumerable<MovieRating>> GetRatingsByMovie(string Title);
        Task<IEnumerable<MovieRating>> GetMovieRatingsByMovie(int Id);
        Task<MovieRating> InsertMovieRating(MovieRating rating);
        Task<MovieRating> UpdateMovieRating(MovieRating rating);
        bool DeleteMovieRating(int id);
    }
}
