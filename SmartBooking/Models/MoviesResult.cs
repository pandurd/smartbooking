using System.Collections.Generic;

namespace SmartBooking.Models
{
    public class MoviesResult
    {
        public MoviesResult()
        {
            Errors = new Dictionary<string, string>();
        }

        public IEnumerable<MovieCard> Movies { get; set; }
        public IDictionary<string, string> Errors { get; set; }
    }

    public class MovieResult
    {
        public MovieResult()
        {
            Errors = new Dictionary<string, string>();
        }

        public MovieOptions Movie { get; set; }
        public IDictionary<string, string> Errors { get; set; }
    }
}
