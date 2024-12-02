using System.Collections.Generic;
using System.Threading.Tasks;
using SolarApp.Models;

namespace SolarApp.Repositories
{
    public interface ISolarPlantRepository<T> where T : class
    {
        Task<List<T>> GetAllAsync();
        Task<T> GetByIdAsync(int id);
        Task AddAsync(T entity);
        Task UpdateAsync(T entity);
        Task DeleteAsync(int id);
    }
}
