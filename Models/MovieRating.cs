using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace MovieAPI.Models
{
    public class MovieRating
    {
        [ DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public int TMDBId { get; set; }
        public int MovieTMDBId { get; set; }
        public decimal Rating { get; set; }
        public string UserId { get; set; }

        public int MovieId { get; set; }


    }
}
