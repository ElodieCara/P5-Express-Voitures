using ExpressVoitures.Data;
using ExpressVoitures.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.IO;
using System.Threading.Tasks;


namespace ExpressVoitures.Controllers
{
    public class CarsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public CarsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Cars
        public async Task<IActionResult> Index()
        {
            var cars = await _context.Cars
                .Include(c => c.Make)
                .Include(c => c.Model)
                .ToListAsync();
            return View(cars);
        }

        // GET: Cars/Create
        public IActionResult Create()
        {
            ViewBag.MakeId = new SelectList(_context.Makes, "MakeId", "Name");
            ViewBag.ModelId = new SelectList(_context.Models, "ModelId", "Name");
            return View();
        }

        // POST: Cars/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("CarId,VIN,Year,MakeId,ModelId,Trim,PurchaseDate,PurchasePrice,AvailabilityDate,SaleDate,SalePrice,IsAvailable,Description,RepairCost")] Car car, IFormFile photo)
        {
            car.Model = _context.Models.Where<Model>(c => c.ModelId == car.ModelId).First();
            car.Make = _context.Makes.Where<Make>(c => c.MakeId == car.MakeId).First();
            ViewBag.MakeId = new SelectList(_context.Makes, "MakeId", "Name", car.MakeId);
            ViewBag.ModelId = new SelectList(_context.Models, "ModelId", "Name", car.ModelId);
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
                return RedirectToAction(nameof(Index));
            }

            return View(car);
        }

        //// GET: Cars/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            var car = await _context.Cars.FindAsync(id);

            if (car == null)
            {
                return NotFound();
            }

            ViewBag.MakeId = new SelectList(_context.Makes, "MakeId", "Name", car.MakeId);
            ViewBag.ModelId = new SelectList(_context.Models, "ModelId", "Name", car.ModelId);

            return View(car);
        }

        // POST: Cars/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("CarId,VIN,Year,MakeId,ModelId,Trim,PurchaseDate,PurchasePrice,AvailabilityDate,SaleDate,SalePrice,IsAvailable,PhotoPath,Description,RepairCost")] Car car, IFormFile photo)
        {
            if (id != car.CarId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    if (photo != null && photo.Length > 0)
                    {
                        var fileName = Path.GetFileName(photo.FileName);
                        var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images", fileName);

                        using (var fileStream = new FileStream(filePath, FileMode.Create))
                        {
                            await photo.CopyToAsync(fileStream);
                        }

                        car.PhotoPath = "/images/" + fileName;
                    }

                    _context.Update(car);
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
            ViewBag.MakeId = new SelectList(_context.Makes, "MakeId", "Name", car.MakeId);
            ViewBag.ModelId = new SelectList(_context.Models, "ModelId", "Name", car.ModelId);
            return View(car);
        }

        // POST: Cars/MarkAsSold/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> MarkAsSold(int id)
        {
            var car = await _context.Cars.FindAsync(id);
            if (car == null)
            {
                return NotFound();
            }
            car.IsAvailable = false;
            car.SaleDate = DateTime.Now;
            _context.Update(car);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // POST: Cars/MarkAsAvailable/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> MarkAsAvailable(int id)
        {
            var car = await _context.Cars.FindAsync(id);
            if (car == null)
            {
                return NotFound();
            }
            car.IsAvailable = true;
            car.SaleDate = null;
            _context.Update(car);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CarExists(int id)
        {
            return _context.Cars.Any(e => e.CarId == id);
        }
    }
}

