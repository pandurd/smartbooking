using SmartBooking.Models;
using System.Threading.Tasks;

namespace SmartBooking.Services
{
    public interface IMovieService
    {
        public Task<MoviesResult> GetAvailableMovies();
        public Task<MovieResult> GetMovieDetails(MovieReq movieReq);
    }
}
