using System.Collections.Generic;
using System.Threading.Tasks;
using ExpressVoitures.Models;

namespace ExpressVoitures.Services
{
    public interface ICarService
    {
        Task<List<Car>> GetAllCarsAsync();
        Task<Car?> GetCarByIdAsync(int id);
        Task AddCarAsync(Car car);
        Task UpdateCarAsync(Car car);
        Task DeleteCarAsync(int id);
        bool CarExists(int id);
        IEnumerable<Make> GetMakes();
        IEnumerable<Model> GetModels();
        Task AddRepairAsync(Repair repair);
        Task RemoveRepairAsync(Repair repair);
        Task<List<Repair>> GetRepairsByCarIdAsync(int carId);
        void SetPhotoPathUnmodified(Car car);
    }
}
