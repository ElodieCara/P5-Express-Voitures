using ExpressVoitures.Data;
using ExpressVoitures.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

[Route("Cars")]
public class CarsController : Controller
{
    private readonly ApplicationDbContext _context;

    public CarsController(ApplicationDbContext context)
    {
        _context = context;
    }

    [HttpGet("")]
    public async Task<IActionResult> Index()
    {
        var cars = await _context.Cars
            .Include(c => c.Make)
            .Include(c => c.Model)
            .Include(c => c.Repairs)
            .ToListAsync();
        return View(cars);
    }

    [HttpGet("Catalogue")]
    public async Task<IActionResult> Catalogue()
    {
        var cars = await _context.Cars
            .Include(c => c.Make)
            .Include(c => c.Model)
            .Include(c => c.Repairs)
            .ToListAsync();
        return View(cars);
    }

    private void PopulateDropdowns(Car? car = null)
    {
        ViewBag.MakeId = new SelectList(_context.Makes, "MakeId", "Name", car?.MakeId);
        ViewBag.ModelId = new SelectList(_context.Models, "ModelId", "Name", car?.ModelId);
    }

    [HttpGet("Create")]
    public IActionResult Create()
    {
        PopulateDropdowns();
        return View();
    }

    [HttpPost("Create")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("CarId,VIN,Year,MakeId,ModelId,Trim,PurchaseDate,PurchasePrice,AvailabilityDate,IsAvailable,Description,Status")] Car car, IFormFile photo, string[] RepairsDescriptions, decimal[] RepairsCosts)
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

            _context.Add(car);
            await _context.SaveChangesAsync();

            for (int i = 0; i < RepairsDescriptions.Length; i++)
            {
                var newRepair = new Repair
                {
                    CarId = car.CarId,
                    RepairDescription = RepairsDescriptions[i],
                    Cost = RepairsCosts[i]
                };
                _context.Repairs.Add(newRepair);
            }

            await _context.SaveChangesAsync();

            // Calculer et mettre à jour le prix de vente après l'ajout des réparations
            car.SalePrice = car.PurchasePrice + RepairsCosts.Sum() + 500;
            _context.Update(car);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        PopulateDropdowns(car);
        return View(car);
    }

    [HttpGet("Edit/{id}")]
    public async Task<IActionResult> Edit(int id)
    {
        if (id == 0)
        {
            return NotFound();
        }

        var car = await _context.Cars
            .Include(c => c.Repairs)
            .FirstOrDefaultAsync(m => m.CarId == id);
        if (car == null)
        {
            return NotFound();
        }

        PopulateDropdowns(car);
        return View(car);
    }

    [HttpPost("Edit/{id}")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, [Bind("CarId,VIN,Year,MakeId,ModelId,Trim,PurchaseDate,PurchasePrice,AvailabilityDate,IsAvailable,Description,PhotoPath")] Car car, IFormFile photo, string[] RepairsDescriptions, decimal[] RepairsCosts)
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
                _context.Entry(car).Property(x => x.PhotoPath).IsModified = false;
            }

            // Mise à jour de la voiture
            _context.Update(car);
            await _context.SaveChangesAsync();

            // Supprimer les réparations existantes pour ajouter les nouvelles
            var existingRepairs = await _context.Repairs.Where(r => r.CarId == car.CarId).ToListAsync();
            foreach (var repair in existingRepairs)
            {
                _context.Repairs.Remove(repair);
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
                _context.Repairs.Add(newRepair);
            }

            //// Calculer et mettre à jour le prix de vente après l'ajout des réparations
            //car.SalePrice = car.CalculatedSalePrice;
            //_context.Update(car);

            car.SalePrice = car.PurchasePrice + RepairsCosts.Sum() + 500;
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!CarExists(car.CarId))
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

    [HttpGet("Delete/{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        if (id == 0)
        {
            return NotFound();
        }

        var car = await _context.Cars
            .Include(c => c.Make)
            .Include(c => c.Model)
            .FirstOrDefaultAsync(m => m.CarId == id);
        if (car == null)
        {
            return NotFound();
        }

        return View(car);
    }

    [HttpPost("Delete/{id}"), ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var car = await _context.Cars.FindAsync(id);
        if (car == null)
        {
            return NotFound();
        }
        _context.Cars.Remove(car);
        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    private bool CarExists(int id)
    {
        return _context.Cars.Any(e => e.CarId == id);
    }
}
