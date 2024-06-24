using ExpressVoitures.Data;
using ExpressVoitures.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ExpressVoitures.Services
{
    public class CarService : ICarService
    {
        private readonly ApplicationDbContext _context;

        public CarService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<Car>> GetAllCarsAsync()
        {
            return await _context.Cars
                .Include(c => c.Make)
                .Include(c => c.Model)
                .Include(c => c.Repairs)
                .ToListAsync();
        }

        public async Task<Car?> GetCarByIdAsync(int id)
        {
            return await _context.Cars
                .Include(c => c.Repairs)
                .FirstOrDefaultAsync(c => c.CarId == id);
        }

        public async Task AddCarAsync(Car car)
        {
            _context.Cars.Add(car);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateCarAsync(Car car)
        {
            _context.Cars.Update(car);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteCarAsync(int id)
        {
            var car = await _context.Cars.FindAsync(id);
            if (car != null)
            {
                _context.Cars.Remove(car);
                await _context.SaveChangesAsync();
            }
        }

        public bool CarExists(int id)
        {
            return _context.Cars.Any(e => e.CarId == id);
        }

        public IEnumerable<Make> GetMakes()
        {
            return _context.Makes.ToList();
        }

        public IEnumerable<Model> GetModels()
        {
            return _context.Models.ToList();
        }

        public async Task AddRepairAsync(Repair repair)
        {
            _context.Repairs.Add(repair);
            await _context.SaveChangesAsync();
        }

        public async Task RemoveRepairAsync(Repair repair)
        {
            _context.Repairs.Remove(repair);
            await _context.SaveChangesAsync();
        }

        public async Task<List<Repair>> GetRepairsByCarIdAsync(int carId)
        {
            return await _context.Repairs.Where(r => r.CarId == carId).ToListAsync();
        }

        public void SetPhotoPathUnmodified(Car car)
        {
            _context.Entry(car).Property(x => x.PhotoPath).IsModified = false;
        }
    }
}
