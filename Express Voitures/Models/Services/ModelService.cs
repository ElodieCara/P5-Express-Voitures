using System.Collections.Generic;
using System.Threading.Tasks;
using ExpressVoitures.Data;
using ExpressVoitures.Models;
using Microsoft.EntityFrameworkCore;

namespace ExpressVoitures.Services
{
    public class ModelService : IModelService
    {
        private readonly ApplicationDbContext _context;

        public ModelService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<Model>> GetAllModelsAsync()
        {
            return await _context.Models.Include(m => m.Make).ToListAsync();
        }

        public async Task<Model> GetModelByIdAsync(int id)
        {
            var model = await _context.Models.Include(m => m.Make).FirstOrDefaultAsync(m => m.ModelId == id);
            return model ?? throw new InvalidOperationException("Model not found");
        }

        public async Task AddModelAsync(Model model)
        {
            _context.Models.Add(model);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateModelAsync(Model model)
        {
            _context.Entry(model).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public async Task DeleteModelAsync(int id)
        {
            var model = await _context.Models.FindAsync(id);
            if (model != null)
            {
                _context.Models.Remove(model);
                await _context.SaveChangesAsync();
            }
        }

        public bool ModelExists(int id)
        {
            return _context.Models.Any(e => e.ModelId == id);
        }
    }
}
