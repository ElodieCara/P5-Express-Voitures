using ExpressVoitures.Data;
using ExpressVoitures.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using ExpressVoitures.Services;

[Route("Cars")]
public class CarsController : Controller
{
    private readonly ICarService _carService;
    private readonly IMakeService _makeService;
    private readonly IModelService _modelService;

    public CarsController(ICarService carService, IMakeService makeService, IModelService modelService)
    {
        _carService = carService;
        _makeService = makeService;
        _modelService = modelService;
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
        return View(availableCars);
    }

    private async Task PopulateDropdowns(Car? car = null)
    {
        ViewBag.MakeId = new SelectList(await _makeService.GetAllMakesAsync(), "MakeId", "Name", car?.MakeId);
        ViewBag.ModelId = new SelectList(await _modelService.GetAllModelsAsync(), "ModelId", "Name", car?.ModelId);
    }

    [Authorize(Policy = "AdminOnly")]
    [HttpGet("Create")]
    public async Task<IActionResult> Create()
    {
        await PopulateDropdowns();
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
                await _carService.AddRepairAsync(car.CarId, newRepair);
            }

            car.SalePrice = car.PurchasePrice + RepairsCosts.Sum() + 500;
            await _carService.UpdateCarAsync(car);

            return RedirectToAction(nameof(Index));
        }

        await PopulateDropdowns(car);
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

        await PopulateDropdowns(car);
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
            await PopulateDropdowns(car);
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
                // Assurez-vous que ce champ n'est pas marqué comme modifié si aucune nouvelle photo n'est téléchargée
            }

            // Mise à jour de la voiture
            await _carService.UpdateCarAsync(car);

            // Supprimer les réparations existantes pour ajouter les nouvelles
            var existingCar = await _carService.GetCarByIdAsync(car.CarId);
            if (existingCar != null)
            {
                existingCar.Repairs.Clear();

                for (int i = 0; i < RepairsDescriptions.Length; i++)
                {
                    var newRepair = new Repair
                    {
                        CarId = car.CarId,
                        RepairDescription = RepairsDescriptions[i],
                        Cost = RepairsCosts[i]
                    };
                    await _carService.AddRepairAsync(car.CarId, newRepair);
                }
            }

            car.SalePrice = car.PurchasePrice + RepairsCosts.Sum() + 500;
            await _carService.UpdateCarAsync(car);
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!await _carService.CarExistsAsync(car.CarId))
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
        var car = await _carService.GetCarByIdAsync(id);
        if (car == null)
        {
            return NotFound();
        }

        await _carService.DeleteCarAsync(id);
        return RedirectToAction(nameof(Index));
    }
}
