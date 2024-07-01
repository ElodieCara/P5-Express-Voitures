using ExpressVoitures.Models;
using ExpressVoitures.Repositories;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ExpressVoitures.Services
{
    public class CarService : ICarService
    {
        private readonly ICarRepository _carRepository;
        private readonly IRepairRepository _repairRepository; 

        public CarService(ICarRepository carRepository, IRepairRepository repairRepository)
        {
            _carRepository = carRepository;
            _repairRepository = repairRepository;
        }

        public async Task<IEnumerable<Car>> GetAllCarsAsync()
        {
            return await _carRepository.GetAllAsync();
        }

        public async Task<Car?> GetCarByIdAsync(int id)
        {
            return await _carRepository.GetByIdAsync(id);
        }

        public async Task AddCarAsync(Car car)
        {
            await _carRepository.AddAsync(car);
        }

        public async Task UpdateCarAsync(Car car)
        {
            await _carRepository.UpdateAsync(car);
        }

        public async Task DeleteCarAsync(int id)
        {
            var car = await _carRepository.GetByIdAsync(id);
            if (car != null)
            {
                await _carRepository.DeleteAsync(id);
            }
        }

        public async Task<bool> CarExistsAsync(int id)
        {
            return await _carRepository.CarExistsAsync(id);
        }

        public async Task AddRepairAsync(int carId, Repair repair) 
        {
            var car = await _carRepository.GetByIdAsync(carId);
            if (car != null)
            {
                car.Repairs.Add(repair);
                await _carRepository.UpdateAsync(car);
            }
        }
    }
}
