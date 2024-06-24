using ExpressVoitures.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ExpressVoitures.Repositories
{
    public interface IModelRepository
    {
        Task<List<Model>> GetAllAsync();
        Task<Model?> GetByIdAsync(int id);
        Task AddAsync(Model model);
        Task UpdateAsync(Model model);
        Task DeleteAsync(int id);
        Task<bool> ModelExistsAsync(int id);
    }
}
