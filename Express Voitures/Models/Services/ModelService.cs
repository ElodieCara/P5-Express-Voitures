using ExpressVoitures.Models;
using ExpressVoitures.Repositories;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ExpressVoitures.Services
{
    public class ModelService : IModelService
    {
        private readonly IModelRepository _modelRepository;

        public ModelService(IModelRepository modelRepository)
        {
            _modelRepository = modelRepository;
        }

        public async Task<IEnumerable<Model>> GetAllModelsAsync()
        {
            return await _modelRepository.GetAllAsync();
        }

        public async Task<Model?> GetModelByIdAsync(int id)
        {
            return await _modelRepository.GetByIdAsync(id);
        }

        public async Task AddModelAsync(Model model)
        {
            await _modelRepository.AddAsync(model);
        }

        public async Task UpdateModelAsync(Model model)
        {
            await _modelRepository.UpdateAsync(model);
        }

        public async Task DeleteModelAsync(int id)
        {
            var model = await _modelRepository.GetByIdAsync(id);
            if (model != null)
            {
                await _modelRepository.DeleteAsync(id);
            }
        }

        public async Task<bool> ModelExistsAsync(int id)
        {
            return await _modelRepository.ModelExistsAsync(id);
        }
    }
}
