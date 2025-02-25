using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using TravelPlannerAPI.Models;

namespace TravelPlannerAPI.Controllers
{
    [Route("api/travel")]
    [ApiController]
    public class TravelController : ControllerBase
    {
        private static readonly List<Destination> Destinations = new List<Destination>
        {
            new Destination { Name = "Rio", Price = 750, Temperature = 25, ImageUrl = "https://example.com/rio.jpg" },
            new Destination { Name = "Aspen", Price = 800, Temperature = -5, ImageUrl = "https://example.com/aspen.jpg" },
            new Destination { Name = "Dubai", Price = 900, Temperature = 35, ImageUrl = "https://example.com/dubai.jpg" },
            new Destination { Name = "Chamonix", Price = 700, Temperature = -10, ImageUrl = "https://example.com/chamonix.jpg" }
        };

        private Destination GetRandomDestination(string environment, int budget)
        {
            var filteredDestinations = Destinations
                .Where(d => d.Price <= budget &&
                            ((environment == "sunny" && d.Temperature > 0) ||
                             (environment == "snowy" && d.Temperature < 0)))
                .ToList();

            if (!filteredDestinations.Any())
                return null;

            return filteredDestinations[new System.Random().Next(filteredDestinations.Count)];
        }

        [HttpPost("preferences")]
        public IActionResult GetDestination([FromBody] TravelPreference preference)
        {
            var destination = GetRandomDestination(preference.Environment, preference.Budget);
            if (destination == null)
                return NotFound(new { Message = "No destinations match your preferences." });

            return Ok(destination);
        }

        [HttpGet("booking")]
        public IActionResult GetBookingLinks([FromQuery] string destination)
        {
            return Ok(new
            {
                AirFrance = $"https://www.airfrance.com/book/{destination}",
                Airbnb = $"https://www.airbnb.com/s/{destination}"
            });
        }
    }
}
