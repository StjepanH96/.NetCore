using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SolarApp.Models;
using SolarApp.Data;

namespace SolarApp.Repositories
{
    public class SolarPowerPlantRepository : ISolarPlantRepository<SolarPowerPlant>
    {
        private readonly SolarDbContext _dbContext;

        public SolarPowerPlantRepository(SolarDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<List<SolarPowerPlant>> GetAllAsync()
        {
            return await _dbContext.SolarPowerPlants.ToListAsync();
        }

        public async Task<SolarPowerPlant> GetByIdAsync(int id)
        {
            return await _dbContext.SolarPowerPlants.FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task AddAsync(SolarPowerPlant entity)
        {
            await _dbContext.SolarPowerPlants.AddAsync(entity);
            await _dbContext.SaveChangesAsync();
        }

        public async Task UpdateAsync(SolarPowerPlant entity)
        {
            _dbContext.SolarPowerPlants.Update(entity);
            await _dbContext.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var entity = await GetByIdAsync(id);
            if (entity != null)
            {
                _dbContext.SolarPowerPlants.Remove(entity);
                await _dbContext.SaveChangesAsync();
            }
        }
    }
}
