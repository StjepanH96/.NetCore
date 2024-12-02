using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using SolarApp.Data;
using SolarApp.Models;
using SolarApp.Repositories;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using SolarApp.Services;

namespace SolarApp.Services
{
    public class WeatherUpdateBackgroundService : BackgroundService
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly ILogger<WeatherUpdateBackgroundService> _logger;

        public WeatherUpdateBackgroundService(
            IServiceProvider serviceProvider,
            ILogger<WeatherUpdateBackgroundService> logger)
        {
            _serviceProvider = serviceProvider;
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {

              
                    using (var scope = _serviceProvider.CreateScope())
                    {
                        var solarPlantRepository = scope.ServiceProvider.GetRequiredService<ISolarPlantRepository<SolarPowerPlant>>();
                        var productionDataRepository = scope.ServiceProvider.GetRequiredService<IProductionDataRepository>();
                        var weatherService = scope.ServiceProvider.GetRequiredService<WeatherService>();
                        var productionService = scope.ServiceProvider.GetRequiredService<ProductionService>();

                        var solarPowerPlants = await solarPlantRepository.GetAllAsync();

                        foreach (var plant in solarPowerPlants)
                        {

                            var weatherData = await weatherService.GetWeatherDataForSolarPlantAsync(plant.Latitude, plant.Longitude);
                            if (weatherData != null)
                            {
                                _logger.LogInformation($"Weather data for plant {plant.Name} retrieved successfully.");

                                var production = productionService.CalculateForecastedProduction(plant.InstalledPower, weatherData);

                                var productionData = new ProductionData
                                {
                                    Timestamp = DateTime.UtcNow,
                                    Production = production,
                                    TimeseriesType = "1-minute", 
                                    SolarPowerPlantId = plant.Id,
                                    SolarPowerPlantName = plant.Name
                                };

                                await productionDataRepository.AddProductionDataAsync(productionData);
                                _logger.LogInformation($"New production data added: Plant: {plant.Name}, Timestamp: {productionData.Timestamp}, Production: {productionData.Production} kW.");
                            }
                            else
                            {
                                _logger.LogWarning($"Weather data for plant {plant.Name} could not be retrieved.");
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error occurred while updating production data for solar plants.");
                }

                // Pause for 1 minute
                await Task.Delay(TimeSpan.FromMinutes(1), stoppingToken);
            }
        }
    }
}
