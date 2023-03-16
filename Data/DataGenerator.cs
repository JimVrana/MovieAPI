using Microsoft.AspNetCore.DataProtection.KeyManagement;
using MovieAPI.Models;
using System;
using System.IO;
using System.Net.Http.Headers;
using System.Numerics;
using static System.Net.WebRequestMethods;

namespace MovieAPI.Data
{
    /// <summary>
    /// This will pull in all sample data for the in-memory database from TMDB.com
    /// </summary>
    public class DataGenerator
    {
        static string apiKey; 
        static HttpClient client = new HttpClient();

        public DataGenerator(string apikey)
        {
            apiKey = apikey;
            GetData().GetAwaiter().GetResult();
        }

        static async Task GetData()
        {
            client.BaseAddress = new Uri("https://api.themoviedb.org/3/");
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));          

            //we're going to start with a single actor with a known id, Brendan Fraser, id = 18269
            Models.TMDBModels.Actor a = await GetActorAsync(18269);

            //Get all of the movies that our chosen actor has appeared in
            Models.TMDBModels.MovieCreditsForActor m = await GetAllMoviesForActor(a.id);
            List<Models.TMDBModels.Actor> actors = new List<Models.TMDBModels.Actor>();
            List<MovieRating> ratings = new List<MovieRating>();

            //Just to limit the sample data imported
            int numberOfMoviesToImport = 5;
            int moviesImported = 0;

            //For each movie found, import all actors that appeared in those movies, and ratings 
            foreach (var cast in m.cast)
            {
                if (moviesImported == numberOfMoviesToImport)
                    break;
                Models.TMDBModels.MovieCredits movieCredits = await GetAllActorsInMovie(cast.id);
                foreach (var actorItem in movieCredits.cast)
                {
                    actors.Add(await GetActorAsync(actorItem.id));
                }

                //get and convert the ratings 
                Models.TMDBModels.Review review = await GetAllRatingsForMovie(cast.id);
                foreach (var result  in review.results)
                {
                    ratings.Add(new MovieRating { MovieId = review.id, UserId = result.author_details.username, Rating = result.author_details.rating });
                }
                moviesImported++;
            }
        }

        /// <summary>
        /// Get Actor by Id, from TMDB.com
        /// </summary>
        /// <param name="actorId"></param>
        /// <returns></returns>
        static async Task<Models.TMDBModels.Actor> GetActorAsync(int actorId)
        {
            Models.TMDBModels.Actor? actor = null;
            string path = $"https://api.themoviedb.org/3/person/{actorId}?api_key={apiKey}";
            HttpResponseMessage response = await client.GetAsync(path);
            if (response.IsSuccessStatusCode)
            {
                actor = await response.Content.ReadFromJsonAsync<Models.TMDBModels.Actor>();
            }
            return actor;
        }

        /// <summary>
        /// Get all movies that an actor is in, from TMDB.com
        /// </summary>
        /// <param name="ActorId"></param>
        /// <returns></returns>
        static async Task<Models.TMDBModels.MovieCreditsForActor> GetAllMoviesForActor(int ActorId)
        {
            string path = $"https://api.themoviedb.org/3/person/{ActorId}/movie_credits?api_key={apiKey}";
            Models.TMDBModels.MovieCreditsForActor? movieCreditsForActor = null;
            HttpResponseMessage response = await client.GetAsync(path);
            if (response.IsSuccessStatusCode)
            {
                movieCreditsForActor = await response.Content.ReadFromJsonAsync<Models.TMDBModels.MovieCreditsForActor>();
            }
            return movieCreditsForActor;
        }

        /// <summary>
        /// Get all actors in a specified movie, from TMDB.com
        /// </summary>
        /// <param name="MovieId"></param>
        /// <returns></returns>
        static async Task<Models.TMDBModels.MovieCredits> GetAllActorsInMovie(int MovieId)
        {
            string path = $"https://api.themoviedb.org/3/movie/{MovieId}/credits?api_key={apiKey}";
            Models.TMDBModels.MovieCredits? movies = null;
            HttpResponseMessage response = await client.GetAsync(path);
            if (response.IsSuccessStatusCode)
            {
                movies = await response.Content.ReadFromJsonAsync<Models.TMDBModels.MovieCredits>();
            }
            return movies;
        }

        /// <summary>
        /// Get all user ratings for a movie, from TMDB.com
        /// </summary>
        /// <param name="MovieId"></param>
        /// <returns></returns>
        static async Task<Models.TMDBModels.Review> GetAllRatingsForMovie(int MovieId)
        {
            string path = $"https://api.themoviedb.org/3/movie/{MovieId}/reviews?api_key={apiKey}";
            Models.TMDBModels.Review? review = null;
            HttpResponseMessage response = await client.GetAsync(path);
            if (response.IsSuccessStatusCode)
            {
                review = await response.Content.ReadFromJsonAsync<Models.TMDBModels.Review>();
            }
            return review;
        }
    }
}
