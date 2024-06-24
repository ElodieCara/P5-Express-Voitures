using System.Collections.Generic;
using System.Threading.Tasks;
using ExpressVoitures.Models;

namespace ExpressVoitures.Services
{
    public interface IModelService
    {
        Task<List<Model>> GetAllModelsAsync();
        Task<Model> GetModelByIdAsync(int id);
        Task AddModelAsync(Model model);
        Task UpdateModelAsync(Model model);
        Task DeleteModelAsync(int id);
        bool ModelExists(int id);
    }
}
