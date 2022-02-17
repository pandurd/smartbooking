using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using SmartBooking.Clients;
using SmartBooking.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SmartBooking.Services
{
    public class MovieService : IMovieService
    {
        private readonly List<string> _movieProviders;
        private readonly IMovieClientFactory _movieClientFactory;
        private readonly ILogger<MovieService> _logger;
        private readonly IMemoryCache _cache;
        public MovieService(IOptions<List<string>> movieProviders,IMovieClientFactory movieClientFactory, ILogger<MovieService> logger, IMemoryCache memoryCache)

        {
            _movieProviders = movieProviders.Value;
            _movieClientFactory = movieClientFactory;
            _logger = logger;
            _cache = memoryCache;
        }

        public async Task<MoviesResult> GetAvailableMovies()
        {
            var cacheKey = "availableMovies";
           // var result = new MoviesResult();

            //get from cache
            //Assumptions - Available movies remain same for certain period of time
            if (!_cache.TryGetValue(cacheKey, out MoviesResult result))
            {
                result = new MoviesResult();
                var movies = new List<MovieCard>();

                foreach (var clientName in _movieProviders)
                {
                    try
                    {
                        IMovieClient provider = _movieClientFactory.GetMovieClient(clientName);
                        var currentProviderMovies = await provider.GetAvailableMovies();

                        if (currentProviderMovies != null)
                        {
                            var currentMovieCards = currentProviderMovies.Select(x => new MovieCard()
                            {
                                Title = x.Title,
                                Year = x.Year,
                                Type = x.Type,
                                Poster = x.Poster,
                                IDs = new Dictionary<string, string>() { { clientName, x.ID } },
                                Posters = new List<string>() { x.Poster }
                            });

                            if (result == null)
                            {
                                movies.AddRange(currentMovieCards);
                            }
                            else
                            {
                                //to do merger with another provider
                                //var existingCards;
                                var existingCards = movies.Select(x => new { x.Title, x.Year })
                                                    .Intersect(currentMovieCards
                                                    .Select(y => new { y.Title, y.Year }))
                                                    .ToList();

                                if (existingCards.Any())
                                {
                                    //add ids to existing collection record
                                    foreach (var card in existingCards)
                                    {
                                        var found = movies.Find(x => x.Title == card.Title && x.Year == card.Year);
                                        var current = currentMovieCards
                                                        .First(x => x.Title == card.Title && x.Year == card.Year);
                                        found.IDs.Add(clientName, current.IDs.First().Value);
                                        found.Posters.Add(current.Poster);
                                    }

                                    //add new
                                    var newcards = currentMovieCards
                                        .Select(x => new { x.Title, x.Year })
                                        .Where(x => !existingCards.Contains(x));

                                    //add new film existing collection
                                    foreach (var card in newcards)
                                    {
                                        var found = currentMovieCards
                                            .ToList()
                                            .Find(x => x.Title == card.Title && x.Year == card.Year);
                                        movies.Add(found);
                                    }
                                }
                                else
                                {
                                    //no common movies found, add all new films
                                    movies.AddRange(currentMovieCards);
                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError($"Error while fetching movies from provider {clientName}", ex);
                        result.Errors.Add(clientName, ex.Message);
                    }
                }

                result.Movies = movies.OrderBy(x => x.Title);

                var cacheExpiryOptions = new MemoryCacheEntryOptions
                {
                    AbsoluteExpiration = DateTime.Now.AddMinutes(10),
                    Priority = CacheItemPriority.High,
                    SlidingExpiration = TimeSpan.FromMinutes(5)
                };

                if (result.Errors.Count == 0)
                    _cache.Set(cacheKey, result, cacheExpiryOptions);
            }

            return result;
        }

        public async Task<MovieResult> GetMovieDetails(MovieReq movieReq)
        {
            //var result = new MovieResult();
            
            var cacheKey = $"movie-{movieReq.Title.ToLower()}";

            //get from cache
            //Assumptions - Price of movies remain same for certain period of time
            //for same provider
            if (!_cache.TryGetValue(cacheKey, out MovieResult result))
            {
                result = new MovieResult();
                MovieOptions movie = null;

                foreach (var clientName in _movieProviders)
                {
                    try
                    {
                        string id = null;
                        if (!movieReq.Ids.TryGetValue(clientName, out id))
                            continue;

                        IMovieClient provider = _movieClientFactory.GetMovieClient(clientName);
                        var currentResult = await provider.GetMovieDetails(id);

                        if (currentResult != null)
                        {
                            if (movie == null)
                            {
                                //no existing movie data
                                movie = new MovieOptions(currentResult);
                                movie.Price = currentResult.Price;
                                movie.EconomicPrice = new EconomicPrice()
                                {
                                    ID = currentResult.ID,
                                    Price = currentResult.Price,
                                    Provider = clientName
                                };

                                movie.Posters = new List<string>() { currentResult.Poster };
                            }
                            else if (currentResult.Price < movie.Price)
                            {
                                //cheaper price than previous provider
                                movie.Price = currentResult.Price;
                                movie.EconomicPrice = new EconomicPrice()
                                {

                                    ID = currentResult.ID,
                                    Price = currentResult.Price,
                                    Provider = clientName
                                };

                                movie.Posters.Add(currentResult.Poster);
                            }
                            else
                            {
                                //price higher than previous provider
                                //no need to update price

                                if (movie.Awards == null)
                                    movie.Awards = currentResult.Awards;

                                movie.Posters.Add(currentResult.Poster);
                            }

                            movie.AllPrices.Add(clientName, currentResult.Price);
                        }

                    }
                    catch (Exception ex)
                    {
                        _logger.LogError($"Error while fetching movie details from provider {clientName}", ex);
                        result.Errors.Add(clientName, ex.Message);
                    }
                }

                result.Movie = movie;

                var cacheExpiryOptions = new MemoryCacheEntryOptions
                {
                    AbsoluteExpiration = DateTime.Now.AddMinutes(10),
                    Priority = CacheItemPriority.High,
                    SlidingExpiration = TimeSpan.FromMinutes(2)
                };

                if(result.Errors.Count == 0)
                    _cache.Set(cacheKey, result, cacheExpiryOptions);
            }
            return result;
        }
    }
}
