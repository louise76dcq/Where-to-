using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace TravelPlannerAPI.Services
{
    public class AmadeusService
    {
        private readonly HttpClient _httpClient;
        private readonly string _clientId = "7QPYdbapbmCCicW8QosLwrWzfHq2oIdC";  // Remplace par ta clé
        private readonly string _clientSecret = "atPT986yuSiM3qFN";  // Remplace par ta clé
        private string _accessToken;

        public AmadeusService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<string> GetAccessToken()
        {
            if (!string.IsNullOrEmpty(_accessToken))
                return _accessToken;

            var credentials = new { grant_type = "client_credentials", client_id = _clientId, client_secret = _clientSecret };
            var content = new StringContent(JsonSerializer.Serialize(credentials), Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync("https://test.api.amadeus.com/v1/security/oauth2/token", content);
            var responseContent = await response.Content.ReadAsStringAsync();

            using var jsonDoc = JsonDocument.Parse(responseContent);
            _accessToken = jsonDoc.RootElement.GetProperty("access_token").GetString();

            return _accessToken;
        }
    }
}