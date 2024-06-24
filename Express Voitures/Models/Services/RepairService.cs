using System.Collections.Generic;
using System.Threading.Tasks;
using ExpressVoitures.Data;
using ExpressVoitures.Models;
using Microsoft.EntityFrameworkCore;

namespace ExpressVoitures.Services
{
    public class RepairService : IRepairService
    {
        private readonly ApplicationDbContext _context;

        public RepairService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Repair>> GetAllRepairsAsync()
        {
            return await _context.Repairs
                .Include(r => r.Car)
                .ThenInclude(c => c.Make)
                .Include(r => r.Car)
                .ThenInclude(c => c.Model)
                .ToListAsync();
        }

        public async Task<Repair> GetRepairByIdAsync(int id)
        {
            return await _context.Repairs.FindAsync(id);
        }

        public async Task AddRepairAsync(Repair repair)
        {
            _context.Repairs.Add(repair);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateRepairAsync(Repair repair)
        {
            _context.Entry(repair).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public async Task<bool> RepairExistsAsync(int id)
        {
            return await _context.Repairs.AnyAsync(e => e.RepairId == id);
        }
    }
}
