using Microsoft.Extensions.Hosting;

namespace MovieAPI.Models
{
    public class Actor
    {
        public int Id { get; set; }
        public string? Birthday { get; set; }
        public int? Gender { get; set; }
        public string Name { get; set; }

        public ICollection<Movie> Movies { get; set; }
    }
}
