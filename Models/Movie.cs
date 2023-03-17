using Microsoft.Extensions.Hosting;
using System.Text.Json.Serialization;

namespace MovieAPI.Models
{
    public class Movie
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Overview { get; set; }
        public string Release_date { get; set; }

        [JsonIgnore]
        public ICollection<Actor> Actors { get; set; }

        [JsonIgnore]
        public List<MovieRating> MovieRatings { get;set; }
    }
}
