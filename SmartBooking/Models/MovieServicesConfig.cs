using System.Collections.Generic;

namespace SmartBooking.Models
{
    public class MovieServicesConfig
    {
        public List<ServicesConfigURL> servicesConfigURLs { get; set; }
    }

    public class ServicesConfigURL
    {
        public string Name { get; set; }
        public string BaseUrl { get; set; }
        public string MoviesPath { get; set; }
        public string MoviePath { get; set; }
    }
}
