using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using MovieAPI.Models;
using System;

namespace MovieAPI.Data
{
    public class ApiContext : DbContext
    {

        protected readonly IConfiguration Configuration;

        public ApiContext(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {
          //  options.UseSqlite(Configuration.GetConnectionString("MovieData.db"));
            options.UseInMemoryDatabase(databaseName: "MovieData");
        }
        public DbSet<Movie> Movies { get; set; }
        public DbSet<Actor> Actors { get; set; }
        public DbSet<MovieRating> MovieRatings { get; set; }

    }
}
