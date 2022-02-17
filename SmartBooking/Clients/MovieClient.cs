using Newtonsoft.Json;
using SmartBooking.Models;
using SmartBooking.Services;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace SmartBooking.Clients
{
    public class MovieClient : IMovieClient
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly ServicesConfigURL _config;

        public MovieClient(ServicesConfigURL serviceConfig, IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
            _config = serviceConfig;
        }

        public async Task<IEnumerable<MoviePoster>> GetAvailableMovies()
        {
            var httpClient = _httpClientFactory.CreateClient("serviceClient");
            httpClient.BaseAddress = new Uri (_config.BaseUrl);
            //to reduce long running /error calls
            httpClient.Timeout = TimeSpan.FromSeconds(5);

            var response = await httpClient.GetAsync(_config.MoviesPath);          
            response.EnsureSuccessStatusCode();

            var resultString = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<AvailableMovies>(resultString).Movies;

            return result;
        }

        public async Task<Movie> GetMovieDetails(string id)
        {
            var httpClient = _httpClientFactory.CreateClient("serviceClient");
            httpClient.BaseAddress = new Uri(_config.BaseUrl);
            httpClient.Timeout  = TimeSpan.FromSeconds(5);

            var response = await httpClient.GetAsync($"{_config.MoviePath}{id}");
            response.EnsureSuccessStatusCode();

            var resultString = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<Movie>(resultString);

            return result;
        }
    }
}
