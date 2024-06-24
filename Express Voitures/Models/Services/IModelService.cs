using ExpressVoitures.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ExpressVoitures.Services
{
    public interface IModelService
    {
        Task<IEnumerable<Model>> GetAllModelsAsync();
        Task<Model?> GetModelByIdAsync(int id);
        Task AddModelAsync(Model model);
        Task UpdateModelAsync(Model model);
        Task DeleteModelAsync(int id);
        Task<bool> ModelExistsAsync(int id);
    }
}
