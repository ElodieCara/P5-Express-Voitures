using ExpressVoitures.Data;
using ExpressVoitures.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ExpressVoitures.Services
{
    public class MakeService : IMakeService
    {
        private readonly ApplicationDbContext _context;

        public MakeService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<Make>> GetAllMakesAsync()
        {
            return await _context.Makes.ToListAsync();
        }

        public async Task<Make?> GetMakeByIdAsync(int id)
        {
            return await _context.Makes.FindAsync(id);
        }

        public async Task AddMakeAsync(Make make)
        {
            _context.Makes.Add(make);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateMakeAsync(Make make)
        {
            _context.Makes.Update(make);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteMakeAsync(int id)
        {
            var make = await _context.Makes.FindAsync(id);
            if (make != null)
            {
                _context.Makes.Remove(make);
                await _context.SaveChangesAsync();
            }
        }

        public bool MakeExists(int id)
        {
            return _context.Makes.Any(e => e.MakeId == id);
        }
    }
}
