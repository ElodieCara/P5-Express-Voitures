using System.Collections.Generic;
using System.Threading.Tasks;
using ExpressVoitures.Models;

namespace ExpressVoitures.Repositories
{
    public interface IMakeRepository
    {
        Task<List<Make>> GetAllAsync();
        Task<Make> GetByIdAsync(int id);
        Task AddAsync(Make make);
        Task UpdateAsync(Make make);
        Task DeleteAsync(int id);
        Task<bool> MakeExistsAsync(int id);
    }
}
