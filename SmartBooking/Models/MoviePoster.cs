using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System.Collections.Generic;

namespace SmartBooking.Models
{

    [JsonConverter(typeof(StringEnumConverter))]
    public enum EntertainmentType {
        Movie
    }

    public class AvailableMovies
    {
        public List<MoviePoster> Movies { get; set; }   
    }

    public class MoviePoster
    {
        public string Title { get; set; }
        public int Year { get; set; }
        public string ID { get; set; }

        [JsonConverter(typeof(StringEnumConverter))]
        public EntertainmentType Type { get; set; }
        public string Poster { get; set; }
    }

    public class MovieCard
    {
        public MovieCard ()
        {
            Posters = new List<string>();
        }

        public string Title { get; set; }
        public int Year { get; set; }
        public EntertainmentType Type { get; set; }

        [JsonIgnore]
        public string Poster { get; set; }
        public Dictionary<string, string> IDs { get; set; }
        public List<string> Posters { get; set; }
    }
}
