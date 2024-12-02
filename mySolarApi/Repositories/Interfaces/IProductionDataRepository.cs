using System.Collections.Generic;
using System.Threading.Tasks;
using SolarApp.Models;

namespace SolarApp.Repositories
{
    public interface IProductionDataRepository
    {
        Task AddProductionDataAsync(ProductionData productionData);
        Task<List<ProductionData>> GetProductionDataByPlantIdAsync(int solarPowerPlantId);
        Task UpdateProductionDataAsync(ProductionData productionData);
        Task DeleteProductionDataAsync(int id);
    }
}
