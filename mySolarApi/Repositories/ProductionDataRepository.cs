using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SolarApp.Models;
using SolarApp.Data;


namespace SolarApp.Repositories
{
    public class ProductionDataRepository : IProductionDataRepository
    {
        private readonly SolarDbContext _context; 

        public ProductionDataRepository(SolarDbContext context)
        {
            _context = context;
        }

     
        public async Task AddProductionDataAsync(ProductionData productionData)
        {
            await _context.ProductionData.AddAsync(productionData);
            await _context.SaveChangesAsync();
        }

        public async Task<List<ProductionData>> GetProductionDataByPlantIdAsync(int solarPowerPlantId)
        {
            return await _context.ProductionData
                .Where(pd => pd.SolarPowerPlantId == solarPowerPlantId)
                .ToListAsync();
        }

        public async Task UpdateProductionDataAsync(ProductionData productionData)
        {
            _context.ProductionData.Update(productionData);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteProductionDataAsync(int id)
        {
            var productionData = await _context.ProductionData.FindAsync(id);
            if (productionData != null)
            {
                _context.ProductionData.Remove(productionData);
                await _context.SaveChangesAsync();
            }
        }
    }
}
