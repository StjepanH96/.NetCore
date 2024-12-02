using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using SolarApp.Data;
using SolarApp.Models;

namespace SolarApp.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class ProductionController : ControllerBase
    {
        private readonly SolarDbContext _context;

        public ProductionController(SolarDbContext context)
        {
            _context = context;
        }

        [HttpGet("{solarPowerPlantId}")]
        public async Task<ActionResult<IEnumerable<ProductionData>>> GetProductionData(int solarPowerPlantId)
        {
           var productionData = await _context.ProductionData
        .Where(pd => pd.SolarPowerPlantId == solarPowerPlantId)
        .Select(pd => new ProductionData
        {
               Id = pd.Id,
            Timestamp = pd.Timestamp,
            Production = pd.Production,
            TimeseriesType = pd.TimeseriesType,
            SolarPowerPlantId = pd.SolarPowerPlantId,
            SolarPowerPlantName = _context.SolarPowerPlants
                .Where(sp => sp.Id == pd.SolarPowerPlantId)
                .Select(sp => sp.Name)
                .FirstOrDefault() 
        })
        .ToListAsync();

    return Ok(productionData);
           
        }
    }
}
