using System.Collections.Generic;

namespace BuildBoard.Services
{
    public interface ILocationProvider
    {
        IEnumerable<Location> GetLocations();
    }

    public class LocationProvider : ILocationProvider
    {
        public IEnumerable<Location> GetLocations()
        {
            return new List<Location>
            {
                new Location { Id = 1, DisplayName = "BME" },
                new Location { Id = 2, DisplayName = "LogMeIn" }
            };
        }
    }
}