using SolarApp.Services;  
using SolarApp.Models;   
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Net.Http;
using System.Threading.Tasks;

namespace SolarApp.Services
{
    public class WeatherService 
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IConfiguration _configuration;
        private readonly ILogger<WeatherService> _logger;

        public WeatherService(IHttpClientFactory httpClientFactory, IConfiguration configuration, ILogger<WeatherService> logger)
        {
            _httpClientFactory = httpClientFactory;
            _configuration = configuration;
            _logger = logger;
        }

        public async Task<WeatherData> GetWeatherDataForSolarPlantAsync(decimal latitude, decimal longitude)
        {

            try
            {
                var apiKey = _configuration["OpenWeatherMap:ApiKey"];
                var baseUrl = _configuration["OpenWeatherMap:BaseUrl"];

                var url = $"{baseUrl}?lat={latitude}&lon={longitude}&exclude=current,hourly,daily,alerts&appid={apiKey}&units=metric";

                var client = _httpClientFactory.CreateClient();
                var response = await client.GetAsync(url);

                if (!response.IsSuccessStatusCode)
                {
                    _logger.LogError($"Error fetching weather data from {url} with status code {response.StatusCode}");
                    return null;
                }

                var responseString = await response.Content.ReadAsStringAsync();
                _logger.LogInformation($"Response received: {responseString}");

                var weatherData = JsonConvert.DeserializeObject<WeatherData>(responseString);

                if (weatherData == null)
                {
                    _logger.LogError("Failed to deserialize weather data");
                    return null;
                }

                return weatherData;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Exception while fetching weather data: {ex.Message}");
                return null;
            }
        }
    }
}
