using SmartBooking.Services;

namespace SmartBooking.Clients
{
    public interface IMovieClientFactory
    {
        public IMovieClient GetMovieClient(string db);
    }
}
