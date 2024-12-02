using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using SolarApp.Data;
using SolarApp.Models;

namespace SolarApp.Services
{
    public class ProductionService
    {
        private readonly WeatherService _weatherService;
        private readonly SolarDbContext _context;
        private readonly ILogger<ProductionService> _logger;

        public ProductionService(
            WeatherService weatherService,
            SolarDbContext context,
            ILogger<ProductionService> logger)
        {
            _weatherService = weatherService;
            _context = context;
            _logger = logger;
        }

        public async Task<List<ProductionData>> GetProductionDataForSolarPlantAsync(
            int solarPowerPlantId,
            DateTime startDate,
            DateTime endDate,
            string granularity)
        {
            var solarPowerPlant = await _context.SolarPowerPlants.FindAsync(solarPowerPlantId);
            if (solarPowerPlant == null)
            {
                _logger.LogWarning($"Solar power plant with ID {solarPowerPlantId} not found.");
                return null;
            }

            var weatherData = await _weatherService.GetWeatherDataForSolarPlantAsync(
                solarPowerPlant.Latitude, 
                solarPowerPlant.Longitude);

            if (weatherData == null)
            {
                _logger.LogWarning("Weather data could not be retrieved.");
                return null;
            }

            _logger.LogInformation("Weather data retrieved, production simulation should be handled by the background service.");
            return new List<ProductionData>(); 
        }

        public decimal CalculateForecastedProduction(decimal installedPower, WeatherData forecast)
        {
            decimal baseProduction = installedPower * 0.8m;

            decimal cloudFactor = 1 - (forecast.Clouds.All / 100m);
            decimal adjustedProduction = baseProduction * cloudFactor;

            if (forecast.Main.Temp < 10 || forecast.Main.Temp > 35)
            {
                adjustedProduction *= 0.9m;
            }

            if (forecast.Wind.Speed > 5)
            {
                adjustedProduction *= 0.95m; 
            }

            if (forecast.Main.Humidity > 80)
            {
                adjustedProduction *= 0.95m; 
            }

            return Math.Max(adjustedProduction, 0);
        }
    }
}
