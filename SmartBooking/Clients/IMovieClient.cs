using SmartBooking.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SmartBooking.Services
{
    public interface IMovieClient
    {
        public Task<IEnumerable<MoviePoster>> GetAvailableMovies();
        public Task<Movie> GetMovieDetails(string id);
    }
}
