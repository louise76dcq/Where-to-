using Microsoft.AspNetCore.Mvc;
using System.Net.Http;
using System.Threading.Tasks;
using TravelPlannerAPI.Services;
using TravelPlannerAPI.Models;
using System.Text.Json;




namespace TravelPlannerAPI.Controllers
{
    [Route("api/travel")]
    [ApiController]
    public class TravelController : ControllerBase
    {
        private readonly HttpClient _httpClient;
        private readonly AmadeusService _amadeusService;

        public TravelController(HttpClient httpClient)
        {
            _httpClient = httpClient;
            _amadeusService = new AmadeusService(_httpClient);
        }

        /// <summary>
        /// Recherche des vols de Paris (CDG) vers une destination donnée.
        /// </summary>
        /// <param name="destination">Code IATA de la destination (ex: JFK pour New York)</param>
        /// <returns>Liste des vols disponibles</returns>
        [HttpGet("flights")]
        public async Task<IActionResult> GetFlights([FromQuery] string destination, [FromQuery] int budget)
        {
            try
            {
                var token = await _amadeusService.GetAccessToken();
                _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

                string url = $"https://test.api.amadeus.com/v2/shopping/flight-offers?originLocationCode=CDG&destinationLocationCode={destination}&departureDate=2024-06-01&adults=1&max=10";
                var response = await _httpClient.GetAsync(url);

                if (!response.IsSuccessStatusCode)
                    return StatusCode((int)response.StatusCode, new { error = "Erreur lors de la récupération des vols." });

                var responseContent = await response.Content.ReadAsStringAsync();

                using JsonDocument jsonDocument = JsonDocument.Parse(responseContent);
                var flights = jsonDocument.RootElement.GetProperty("data").EnumerateArray()
                    .Where(f => f.GetProperty("price").GetProperty("total").GetDecimal() <= budget)
                    .Select(f => new
                    {
                        Price = f.GetProperty("price").GetProperty("total").GetString(),
                        Airline = f.GetProperty("validatingAirlineCodes")[0].GetString(),
                        Departure = f.GetProperty("itineraries")[0].GetProperty("segments")[0].GetProperty("departure").GetProperty("iataCode").GetString(),
                        Arrival = f.GetProperty("itineraries")[0].GetProperty("segments")[0].GetProperty("arrival").GetProperty("iataCode").GetString()
                    })
                    .ToList();

                return Ok(flights);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = "Erreur interne du serveur.", details = ex.Message });
            }
        }



        /// <summary>
        /// Recherche des hôtels dans une ville donnée.
        /// </summary>
        /// <param name="cityCode">Code de la ville (ex: PAR pour Paris)</param>
        /// <returns>Liste des hôtels disponibles</returns>
        [HttpGet("hotels")]
        public async Task<IActionResult> GetHotels([FromQuery] string cityCode, [FromQuery] int budget)
        {
            try
            {
                var token = await _amadeusService.GetAccessToken();
                _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

                string url = $"https://test.api.amadeus.com/v2/shopping/hotel-offers?cityCode={cityCode}&radius=10&radiusUnit=KM&paymentPolicy=NONE&includeClosed=false&bestRateOnly=true&view=FULL";
                var response = await _httpClient.GetAsync(url);

                if (!response.IsSuccessStatusCode)
                    return StatusCode((int)response.StatusCode, new { error = "Erreur lors de la récupération des hôtels." });

                var responseContent = await response.Content.ReadAsStringAsync();

                using JsonDocument jsonDocument = JsonDocument.Parse(responseContent);
                var hotels = jsonDocument.RootElement.GetProperty("data").EnumerateArray()
                    .Where(h => h.GetProperty("offers")[0].GetProperty("price").GetProperty("total").GetDecimal() <= budget)
                    .Select(h => new
                    {
                        Name = h.GetProperty("hotel").GetProperty("name").GetString(),
                        Price = h.GetProperty("offers")[0].GetProperty("price").GetProperty("total").GetString(),
                        Currency = h.GetProperty("offers")[0].GetProperty("price").GetProperty("currency").GetString(),
                        Address = h.GetProperty("hotel").GetProperty("address").GetProperty("lines")[0].GetString()
                    })
                    .ToList();

                return Ok(hotels);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = "Erreur interne du serveur.", details = ex.Message });
            }
        }


        
        [HttpPost("preferences")]
        public IActionResult SaveUserPreferences([FromBody] TravelPreference preference)
        {
            if (preference.Budget <= 0 || string.IsNullOrEmpty(preference.Environment))
            {
                return BadRequest(new { error = "Veuillez entrer un budget valide et un type d’environnement." });
            }

            // Stocker la préférence temporairement (en attendant une base de données)
            return Ok(new { message = "Préférences enregistrées.", preference });
        }
    }
}
