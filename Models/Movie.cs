namespace MovieAPI.Models
{
    public class Movie
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Overview { get; set; }
        public string Release_date { get; set; }

        public List<Actor> Actors { get; set; }
        public List<MovieRating> MovieRatings { get;set; }
    }
}
