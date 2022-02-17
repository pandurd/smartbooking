using System.Collections.Generic;

namespace SmartBooking.Models
{
    public class MovieReq
    {
        public string Title { get; set; }
        public Dictionary<string, string> Ids { get; set; }
    }
}
