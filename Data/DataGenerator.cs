using Microsoft.AspNetCore.DataProtection.KeyManagement;
using MovieAPI.Models;
using MovieAPI.Models.TMDBModels;
using System;
using System.Collections.Generic;
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
        private readonly ApiContext _apiContext;
        public DataGenerator(string apikey, ApiContext context)
        {
            apiKey = apikey;
            _apiContext = context;
            GetDataFromTMDB2().GetAwaiter().GetResult();
        }

        async Task GetDataFromTMDB2()
        {
            client.BaseAddress = new Uri("https://api.themoviedb.org/3/");
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            //we're going to start with a single actor with a known id, Brendan Fraser, id = 18269
            Models.TMDBModels.Actor a = await GetActorAsync(18269);

            //Get all of the movies that our chosen actor has appeared in
            Models.TMDBModels.MovieCreditsForActor m = await GetAllMoviesForActor(a.id);

            List<Movie> ApiMovies = m.cast.Select(o => new Movie { TMDBId = o.id, Overview = o.overview, Title = o.title, Release_date = o.release_date }).ToList();
            _apiContext.Movies.AddRange(ApiMovies);
            _apiContext.SaveChanges();

            foreach (var movie in _apiContext.Movies)
            {
                Models.TMDBModels.MovieCredits movieCredits = await GetAllActorsInMovie(movie.TMDBId);
                var Actors = movieCredits.cast.Select(o => new Models.Actor { TMDBId = o.id, Gender = o.gender, Name = o.name }).ToList();

                foreach (var act in Actors)
                {
                    //check if the actor already exists
                    var existingActor = _apiContext.Actors.FirstOrDefault(a => a.TMDBId == act.TMDBId); 
                    if (existingActor == null) {
                        _apiContext.Actors.Add(act);
                        _apiContext.ActorMovieRelations.Add(new ActorMovieRelation { ActorId = act.Id, MovieId = movie.Id });
                    }
                    else
                    {
                        _apiContext.ActorMovieRelations.Add(new ActorMovieRelation { ActorId = existingActor.Id, MovieId = movie.Id });
                    }

                    _apiContext.SaveChanges();
                }

                Models.TMDBModels.Review reviews = await GetAllRatingsForMovie(movie.TMDBId);

                List<MovieRating> ratings = new List<MovieRating>();
                foreach (var result in reviews.results)
                {
                    _apiContext.MovieRatings.Add(new MovieRating { MovieTMDBId = reviews.id, UserId = result.author_details.username, Rating = result.author_details.rating, MovieId = movie.Id });
                    _apiContext.SaveChanges();
                }
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
