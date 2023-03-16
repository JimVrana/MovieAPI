namespace MovieAPI.Models
{
    public class Actor
    {
        public int Id { get; set; }
        public string Birthday { get; set; }
        public int Gender { get; set; }
        public string Name { get; set; }

        public Movie Movie { get; set; }
    }
}
