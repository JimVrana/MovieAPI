using MovieAPI.Models;

namespace MovieAPI.Data
{
    public interface IMovieRepository
    {
        Task<IEnumerable<Movie>> GetMovies();
        Task<IEnumerable<Movie>> GetMoviesByTitle(string Title);
        Task<IEnumerable<Movie>> GetMoviesByActor(int ActorId);
        Task<Movie> GetMovieById(int id);
        Task<Movie> InsertMovie(Movie movie);
        Task<Movie> UpdateMovie(Movie movie);
        Task<Movie> DeleteMovie(int id);
    }
}
