using ExpressVoitures.Models;
using ExpressVoitures.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Authorization;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

[Route("Cars")]
public class CarsController : Controller
{
    private readonly ICarService _carService;

    public CarsController(ICarService carService)
    {
        _carService = carService;
    }

    [HttpGet("")]
    public async Task<IActionResult> Index()
    {
        var cars = await _carService.GetAllCarsAsync();
        return View(cars);
    }

    [HttpGet("Catalogue")]
    public async Task<IActionResult> Catalogue()
    {
        var availableCars = await _carService.GetAllCarsAsync();
        availableCars = availableCars.OrderBy(c => c.PurchaseDate).ToList();
        return View(availableCars);
    }

    private void PopulateDropdowns(Car? car = null)
    {
        ViewBag.MakeId = new SelectList(_carService.GetMakes(), "MakeId", "Name", car?.MakeId);
        ViewBag.ModelId = new SelectList(_carService.GetModels(), "ModelId", "Name", car?.ModelId);
    }

    [Authorize(Policy = "AdminOnly")]
    [HttpGet("Create")]
    public IActionResult Create()
    {
        PopulateDropdowns();
        return View();
    }

    [Authorize(Policy = "AdminOnly")]
    [HttpPost("Create")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("CarId,VIN,Year,MakeId,ModelId,Trim,PurchaseDate,PurchasePrice,AvailabilityDate,IsAvailable,Description,Status,SaleDate")] Car car, IFormFile photo, string[] RepairsDescriptions, decimal[] RepairsCosts)
    {
        if (ModelState.IsValid)
        {
            if (photo != null && photo.Length > 0)
            {
                var fileName = Guid.NewGuid().ToString() + "_" + Path.GetFileName(photo.FileName);
                var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images", fileName);

                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    await photo.CopyToAsync(fileStream);
                }

                car.PhotoPath = "/images/" + fileName;
            }

            await _carService.AddCarAsync(car);

            for (int i = 0; i < RepairsDescriptions.Length; i++)
            {
                var newRepair = new Repair
                {
                    CarId = car.CarId,
                    RepairDescription = RepairsDescriptions[i],
                    Cost = RepairsCosts[i]
                };
                await _carService.AddRepairAsync(newRepair);
            }

            car.SalePrice = car.PurchasePrice + RepairsCosts.Sum() + 500;
            await _carService.UpdateCarAsync(car);

            return RedirectToAction(nameof(Index));
        }

        PopulateDropdowns(car);
        return View(car);
    }

    [Authorize(Policy = "AdminOnly")]
    [HttpGet("Edit/{id}")]
    public async Task<IActionResult> Edit(int id)
    {
        if (id == 0)
        {
            return NotFound();
        }

        var car = await _carService.GetCarByIdAsync(id);
        if (car == null)
        {
            return NotFound();
        }

        PopulateDropdowns(car);
        return View(car);
    }

    [Authorize(Policy = "AdminOnly")]
    [HttpPost("Edit/{id}")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, [Bind("CarId,VIN,Year,MakeId,ModelId,Trim,PurchaseDate,PurchasePrice,AvailabilityDate,IsAvailable,Description,PhotoPath,Status,SaleDate")] Car car, IFormFile photo, string[] RepairsDescriptions, decimal[] RepairsCosts)
    {
        if (id != car.CarId)
        {
            return NotFound();
        }

        ModelState.Remove(nameof(photo)); // Ignorer la validation du champ photo

        if (!ModelState.IsValid)
        {
            PopulateDropdowns(car);
            return View(car);
        }

        try
        {
            if (photo != null && photo.Length > 0)
            {
                var fileName = Guid.NewGuid().ToString() + "_" + Path.GetFileName(photo.FileName);
                var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images", fileName);

                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    await photo.CopyToAsync(fileStream);
                }

                if (!string.IsNullOrEmpty(car.PhotoPath))
                {
                    var oldFilePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", car.PhotoPath.TrimStart('/'));
                    if (System.IO.File.Exists(oldFilePath))
                    {
                        System.IO.File.Delete(oldFilePath);
                    }
                }

                car.PhotoPath = "/images/" + fileName;
            }
            else
            {
                // Conserver le chemin de la photo existante
                _carService.SetPhotoPathUnmodified(car);
            }

            // Mise à jour de la voiture
            await _carService.UpdateCarAsync(car);

            // Supprimer les réparations existantes pour ajouter les nouvelles
            var existingRepairs = await _carService.GetRepairsByCarIdAsync(car.CarId);
            foreach (var repair in existingRepairs)
            {
                await _carService.RemoveRepairAsync(repair);
            }

            // Ajouter les nouvelles réparations
            for (int i = 0; i < RepairsDescriptions.Length; i++)
            {
                var newRepair = new Repair
                {
                    CarId = car.CarId,
                    RepairDescription = RepairsDescriptions[i],
                    Cost = RepairsCosts[i]
                };
                await _carService.AddRepairAsync(newRepair);
            }

            car.SalePrice = car.PurchasePrice + RepairsCosts.Sum() + 500;
            await _carService.UpdateCarAsync(car);
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!_carService.CarExists(car.CarId))
            {
                return NotFound();
            }
            else
            {
                throw;
            }
        }

        return RedirectToAction(nameof(Index));
    }

    [Authorize(Policy = "AdminOnly")]
    [HttpGet("Delete/{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        if (id == 0)
        {
            return NotFound();
        }

        var car = await _carService.GetCarByIdAsync(id);
        if (car == null)
        {
            return NotFound();
        }

        return View(car);
    }

    [Authorize(Policy = "AdminOnly")]
    [HttpPost("Delete/{id}"), ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        await _carService.DeleteCarAsync(id);
        return RedirectToAction(nameof(Index));
    }

    private bool CarExists(int id)
    {
        return _carService.CarExists(id);
    }
}
