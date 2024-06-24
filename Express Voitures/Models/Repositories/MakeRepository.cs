using System.Collections.Generic;
using System.Threading.Tasks;
using ExpressVoitures.Data;
using ExpressVoitures.Models;
using Microsoft.EntityFrameworkCore;

namespace ExpressVoitures.Repositories
{
    public class MakeRepository : IMakeRepository
    {
        private readonly ApplicationDbContext _context;

        public MakeRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<Make>> GetAllAsync()
        {
            return await _context.Makes.ToListAsync();
        }

        public async Task<Make?> GetByIdAsync(int id)
        {
            return await _context.Makes.FindAsync(id);
        }

        public async Task AddAsync(Make make)
        {
            _context.Makes.Add(make);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Make make)
        {
            _context.Makes.Update(make);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var make = await _context.Makes.FindAsync(id);
            if (make != null)
            {
                _context.Makes.Remove(make);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<bool> MakeExistsAsync(int id)
        {
            return await _context.Makes.AnyAsync(e => e.MakeId == id);
        }
    }
}
