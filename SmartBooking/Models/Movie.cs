using System;
using System.Collections.Generic;

namespace SmartBooking.Models
{
    public class MovieBase
    {
        public string Title { get; set; }
        public int Year { get; set; }
        public EntertainmentType Type { get; set; }
        public string Poster { get; set; }
        public string Rated { get; set; }
        public DateTime Released { get; set; }
        public string Runtime { get; set; }
        public string Genre { get; set; }
        public string Director { get; set; }
        public string Writer { get; set; }
        public string Actors { get; set; }
        public string Awards { get; set; }   
        public string Plot { get; set; }
        public string Language { get; set; }
        public string Country { get; set; }
        public int Metascore { get; set; }
        public decimal Rating { get; set; }
        public string Votes { get; set; }
        public decimal Price { get; set; }
    }

    public class Movie : MovieBase
    {
        public string ID { get; set; }
    }

    public class EconomicPrice 
    {
        public string Provider { get; set; }
        public string ID { get; set; }
        public decimal Price { get; set; }
    }

    public class MovieOptions : MovieBase
    {
        public MovieOptions (Movie movie)
        {
            this.Runtime = movie.Runtime;
            this.Actors = movie.Actors;
            this.Awards = movie.Awards;
            this.Country = movie.Country;
            this.Director = movie.Director;
            this.Writer = movie.Writer;
            this.Plot = movie.Plot;
            this.Language = movie.Language;
            this.Genre = movie.Genre;
            this.Rating = movie.Rating;
            this.Votes = movie.Votes;
            this.Released = movie.Released;
            this.Year = movie.Year;
            this.Writer = movie.Writer;
            this.Type = movie.Type;
            this.Title  = movie.Title;
            this.Poster = movie.Poster;

            this.AllPrices = new Dictionary<string, decimal>();
        }

        public EconomicPrice EconomicPrice { get; set; }

        public List<string> Posters { get; set; }
        public Dictionary<string, decimal> AllPrices { get; set;  }
    }
}
