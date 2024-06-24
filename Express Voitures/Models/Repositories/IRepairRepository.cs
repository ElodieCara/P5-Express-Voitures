using System.Collections.Generic;
using System.Threading.Tasks;
using ExpressVoitures.Models;

namespace ExpressVoitures.Repositories
{
    public interface IRepairRepository
    {
        Task<List<Repair>> GetAllAsync();
        Task<Repair?> GetByIdAsync(int id);
        Task AddAsync(Repair repair);
        Task UpdateAsync(Repair repair);
        Task DeleteAsync(int id);
        Task<bool> RepairExistsAsync(int id);
    }
}
