using System.Collections.Generic;
using System.Threading.Tasks;
using ExpressVoitures.Models;

namespace ExpressVoitures.Services
{
    public interface IMakeService
    {
        Task<List<Make>> GetAllMakesAsync();
        Task<Make?> GetMakeByIdAsync(int id);
        Task AddMakeAsync(Make make);
        Task UpdateMakeAsync(Make make);
        Task DeleteMakeAsync(int id);
        bool MakeExists(int id);
    }
}
