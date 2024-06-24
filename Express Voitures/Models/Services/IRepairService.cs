using System.Collections.Generic;
using System.Threading.Tasks;
using ExpressVoitures.Models;

namespace ExpressVoitures.Services
{
    public interface IRepairService
    {
        Task<IEnumerable<Repair>> GetAllRepairsAsync();
        Task<Repair?> GetRepairByIdAsync(int id);
        Task AddRepairAsync(Repair repair);
        Task UpdateRepairAsync(Repair repair);
        Task<bool> RepairExistsAsync(int id);
    }
}
