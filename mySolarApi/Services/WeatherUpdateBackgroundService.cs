using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using SolarApp.Data;
using SolarApp.Models;
using SolarApp.Repositories;
using System;
using System.Linq;
using System.Collections.Generic; // Dodano za Dictionary
using System.Threading;
using System.Threading.Tasks;
using SolarApp.Services;

namespace SolarApp.Services
{
    public class WeatherUpdateBackgroundService : BackgroundService
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly ILogger<WeatherUpdateBackgroundService> _logger;
        private Dictionary<int, decimal> _totalProductionByPlant = new(); 
        private Dictionary<int, int> _countByPlant = new(); 

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
                            if (!_totalProductionByPlant.ContainsKey(plant.Id))
                            {
                                _totalProductionByPlant[plant.Id] = 0; 
                            }
                            if (!_countByPlant.ContainsKey(plant.Id))
                            {
                                _countByPlant[plant.Id] = 0; 
                            }
                        }

                   
                        foreach (var plant in solarPowerPlants)
                        {
                            var weatherData = await weatherService.GetWeatherDataForSolarPlantAsync(plant.Latitude, plant.Longitude);
                            if (weatherData != null)
                            {
                                _logger.LogInformation($"Weather data for plant {plant.Name} retrieved successfully.");

                                var production = productionService.CalculateForecastedProduction(plant.InstalledPower, weatherData);
                                _totalProductionByPlant[plant.Id] += production; // Akumuliramo proizvodnju za tu elektranu
                                _countByPlant[plant.Id]++; 

                                var productionData = new ProductionData
                                {
                                    Timestamp = DateTime.UtcNow,
                                    Production = production,
                                    TimeseriesType = "15-minutes", 
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

                        // Provjeravamo je li prošlo 4 puta po 15  za svaku elektranu.
                        foreach (var plant in solarPowerPlants)
                        {
                            if (_countByPlant[plant.Id] >= 4) 
                            {
                                var productionData4Min = new ProductionData
                                {
                                    Timestamp = DateTime.UtcNow, // Koristimo trenutno vrijeme
                                    Production = _totalProductionByPlant[plant.Id], 
                                    TimeseriesType = "Hourly-Save",
                                    SolarPowerPlantId = plant.Id,
                                    SolarPowerPlantName = plant.Name
                                };

                                await productionDataRepository.AddProductionDataAsync(productionData4Min);
                                _logger.LogInformation($"Hourly production data saved: Plant: {plant.Name}, Timestamp: {productionData4Min.Timestamp}, Total Production: {productionData4Min.Production} kW.");

                                _totalProductionByPlant[plant.Id] = 0;
                                _countByPlant[plant.Id] = 0;
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error occurred while updating production data for solar plants.");
                }

                await Task.Delay(TimeSpan.FromMinutes(15), stoppingToken);
            }
        }
    }
}
