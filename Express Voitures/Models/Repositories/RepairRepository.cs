using ExpressVoitures.Data;
using ExpressVoitures.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ExpressVoitures.Repositories
{
    public class RepairRepository : IRepairRepository
    {
        private readonly ApplicationDbContext _context;

        public RepairRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<Repair>> GetAllAsync()
        {
            return await _context.Repairs.ToListAsync();
        }

        public async Task<Repair?> GetByIdAsync(int id)
        {
            return await _context.Repairs.FindAsync(id);
        }

        public async Task AddAsync(Repair repair)
        {
            _context.Repairs.Add(repair);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Repair repair)
        {
            _context.Entry(repair).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var repair = await _context.Repairs.FindAsync(id);
            if (repair != null)
            {
                _context.Repairs.Remove(repair);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<bool> RepairExistsAsync(int id)
        {
            return await _context.Repairs.AnyAsync(e => e.RepairId == id);
        }
    }
}
