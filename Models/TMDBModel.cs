namespace MovieAPI.Models.TMDBModels
{

    public class MovieCreditsForActor
    {
        public Cast[] cast { get; set; }
        public int id { get; set; }
    }

    public class Cast
    {

        public int id { get; set; }
        public string overview { get; set; }
        public string release_date { get; set; }
        public string title { get; set; }
        public string character { get; set; }

    }

    public class Actor  //Person
    {
        public string birthday { get; set; }
        public int gender { get; set; }
        public int id { get; set; }
        public string name { get; set; }
    }

    public class Review
    {
        public int id { get; set; }
        public Result[] results { get; set; }

    }

    public class Result
    {
        public string author { get; set; }
        public Author_Details author_details { get; set; }

    }

    public class Author_Details
    {
        public string username { get; set; }
        public decimal rating { get; set; }  //this is the rating that we want
    }

    public class MovieCredits
    {
        public int id { get; set; }
        public ActorCredits[] cast { get; set; }

    }

    public class ActorCredits
    {
        public int gender { get; set; }
        public int id { get; set; }
        public string name { get; set; }
        public string character { get; set; }
    }


}
