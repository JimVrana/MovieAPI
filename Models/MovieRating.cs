namespace MovieAPI.Models
{
    public class MovieRating
    {
        public int Id { get; set; }
        public int MovieId { get; set; }
        public decimal Rating { get; set; }
        public string UserId { get; set; }

        public Movie Movie { get; set; }

    }
}
