using ExpressVoitures.Models;
using ExpressVoitures.Repositories;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ExpressVoitures.Services
{
    public class RepairService : IRepairService
    {
        private readonly IRepairRepository _repairRepository;

        public RepairService(IRepairRepository repairRepository)
        {
            _repairRepository = repairRepository;
        }

        public async Task<IEnumerable<Repair>> GetAllRepairsAsync()
        {
            return await _repairRepository.GetAllAsync();
        }

        public async Task<Repair?> GetRepairByIdAsync(int id)
        {
            return await _repairRepository.GetByIdAsync(id);
        }

        public async Task AddRepairAsync(Repair repair)
        {
            await _repairRepository.AddAsync(repair);
        }

        public async Task UpdateRepairAsync(Repair repair)
        {
            await _repairRepository.UpdateAsync(repair);
        }

        public async Task DeleteRepairAsync(int id)
        {
            var repair = await _repairRepository.GetByIdAsync(id);
            if (repair != null)
            {
                await _repairRepository.DeleteAsync(id);
            }
        }

        public async Task<bool> RepairExistsAsync(int id)
        {
            return await _repairRepository.RepairExistsAsync(id);
        }
    }
}
