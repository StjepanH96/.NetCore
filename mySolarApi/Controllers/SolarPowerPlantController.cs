using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SolarApp.Data;
using SolarApp.Models;
using SolarApp.Services;

namespace SolarApp.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SolarPowerPlantController : ControllerBase
    {
        private readonly SolarDbContext _context;
        private readonly ILogger<SolarPowerPlantController> _logger;
        private readonly WeatherService _weatherService;
        private readonly ProductionService _productionService;
        private readonly AuthService _authService;
        public SolarPowerPlantController(SolarDbContext context, ILogger<SolarPowerPlantController> logger, WeatherService weatherService, ProductionService productionService, AuthService authService)
        {
            _context = context;
            _logger = logger;
            _weatherService = weatherService;
            _productionService = productionService;
            _authService = authService;
        }



[Authorize]
[HttpPost("addSolarPowerPlant")]
public async Task<IActionResult> AddSolarPowerPlant([FromBody] SolarPowerPlant solarPowerPlant)
{
    if (solarPowerPlant == null)
    {
        return BadRequest("Invalid solar power plant data.");
    }

    try
    {
        var userId = _authService.GetUserIdFromToken(Request);
        if (userId == null)
        {
            return Unauthorized("Token is not valid!");
        }

        solarPowerPlant.UserId = userId.Value;

        _context.SolarPowerPlants.Add(solarPowerPlant);
        await _context.SaveChangesAsync();

        var weatherData = await _weatherService.GetWeatherDataForSolarPlantAsync(solarPowerPlant.Latitude, solarPowerPlant.Longitude);

        if (weatherData == null)
        {
            return NotFound("Weather data could not be retrieved or is empty.");
        }


        var productionDataList = new List<ProductionData>();

        var production = _productionService.CalculateForecastedProduction(solarPowerPlant.InstalledPower, weatherData);

        productionDataList.Add(new ProductionData
        {
            Timestamp = DateTimeOffset.FromUnixTimeSeconds(weatherData.Dt).DateTime, 
            Production = production,
            TimeseriesType = "Forecasted",
            SolarPowerPlantId = solarPowerPlant.Id
        });

        _context.ProductionData.AddRange(productionDataList);

        await _context.SaveChangesAsync();

        return Ok(new { Message = "Solar Power Plant added and production data calculated." });
    }
    catch (Exception ex)
    {
        _logger.LogError(ex, "An error occurred while adding Solar Power Plant and calculating production data.");
        return StatusCode(500, new { Message = "An error occurred while processing your request.", Error = ex.Message });
    }
}




        [Authorize]
        [HttpPut("updateSolarPowerPlant/{id}")]
        public IActionResult UpdateSolarPowerPlant(int id, [FromBody] SolarPowerPlant updatedSolarPowerPlant)
        {
            var userId = _authService.GetUserIdFromToken(Request);  
            if (userId == null)
                return Unauthorized("Token nije valjan!");

            var solarPowerPlant = _context.SolarPowerPlants
                .FirstOrDefault(sp => sp.Id == id && sp.UserId == userId.Value);

            if (solarPowerPlant == null)
                return NotFound("Solarna elektrana nije pronađena ili nemate pristup!");

            solarPowerPlant.Name = updatedSolarPowerPlant.Name;
            solarPowerPlant.InstalledPower = updatedSolarPowerPlant.InstalledPower;
            solarPowerPlant.DateOfInstallation = updatedSolarPowerPlant.DateOfInstallation;
            solarPowerPlant.Latitude = updatedSolarPowerPlant.Latitude;
            solarPowerPlant.Longitude = updatedSolarPowerPlant.Longitude;

            _context.SolarPowerPlants.Update(solarPowerPlant);
            _context.SaveChanges();

            return Ok("Solarna elektrana uspješno ažurirana!");
        }

        [Authorize]
[HttpDelete("deleteSolarPowerPlant/{id}")]
public IActionResult DeleteSolarPowerPlant(int id)
{
    var solarPowerPlant = _context.SolarPowerPlants.FirstOrDefault(sp => sp.Id == id);

    if (solarPowerPlant == null)
        return NotFound("Solar Power Plant not found.");

    _context.SolarPowerPlants.Remove(solarPowerPlant);


    _context.SaveChanges();

    return Ok("Solar power plant  deleted.");
}
         [Authorize]
   [HttpGet("getSolarPowerPlants")]
public IActionResult GetSolarPowerPlants()
{
    var userId = _authService.GetUserIdFromToken(Request);  

    if (userId == null)
        return Unauthorized("Token is invalid!");


    var solarPowerPlants = _context.SolarPowerPlants
                                   .Where(sp => sp.UserId == userId)  
                                   .ToList();

    if (solarPowerPlants.Count == 0)
        return NotFound("No solar power plants found for this user.");

    return Ok(solarPowerPlants);  
}
    }
}