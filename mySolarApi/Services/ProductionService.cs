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


        var productionData = SimulateProductionData(solarPowerPlant, weatherData, startDate, endDate, granularity);
        
        
        return productionData;
    }

  private List<ProductionData> SimulateProductionData(
    SolarPowerPlant solarPowerPlant,
    WeatherData weatherData,
    DateTime startDate,
    DateTime endDate,
    string granularity)
{
    var productionDataList = new List<ProductionData>();
    var currentTime = startDate;

    // Ovdje spremamo proizvodnju za svaki 15-minutni interval, testni slučaj, inače sat vremena
    decimal totalProductionFor15Min = 0;
    int count = 0; // Brojač za sat

    while (currentTime <= endDate)
    {
        decimal production = CalculateForecastedProduction(solarPowerPlant.InstalledPower, weatherData);
        
        // Akumuliramo proizvodnju
        totalProductionFor15Min += production;
        count++;

        // Provjeravamo da li smo dosegli 15 minuta, ali dali smo dosegli četri minute u našem slučaju
        if (count == 4) 
        {
            productionDataList.Add(new ProductionData
            {
                Timestamp = currentTime.AddMinutes(15), // Spremamo proizvodnju na kraju intervala od penaest minuta
                Production = totalProductionFor15Min,
                TimeseriesType = "15-minute",
                SolarPowerPlantId = solarPowerPlant.Id,
            });

          
            totalProductionFor15Min = 0;
            count = 0;
        }

      
        currentTime = currentTime.AddMinutes(1);
    }

    return productionDataList;
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