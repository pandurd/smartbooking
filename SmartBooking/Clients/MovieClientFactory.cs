using Microsoft.Extensions.Options;
using SmartBooking.Models;
using SmartBooking.Services;
using System;
using System.Collections.Generic;
using System.Net.Http;

namespace SmartBooking.Clients
{
    public class MovieClientFactory : IMovieClientFactory
    {
        private readonly List<ServicesConfigURL> _serviceConfig;
        private readonly IHttpClientFactory _httpClientFactory;

        public MovieClientFactory(IOptions<List<ServicesConfigURL>> serviceConfig, IHttpClientFactory httpClientFactory)
        {
            _serviceConfig = serviceConfig.Value;
            _httpClientFactory = httpClientFactory;
        }

        public IMovieClient GetMovieClient(string db)
        {
            //var serviceConfig = _serviceConfig.servicesConfigURLs.Find(x => x.Name == db);
            var serviceConfig = _serviceConfig.Find(x => x.Name == db);
            if (serviceConfig == null)
                throw new Exception($"Configuration missing for db - {db}");

            return new MovieClient(serviceConfig, _httpClientFactory);
        }
    }
}
