using System.ComponentModel.DataAnnotations.Schema;

namespace MovieAPI.Models
{
    public class ActorMovieRelation
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public int ActorId { get; set; }
        public int MovieId { get; set; }


    }
}
