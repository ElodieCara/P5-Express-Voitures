using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ExpressVoitures.Data;
using ExpressVoitures.Models;

namespace ExpressVoitures.Controllers
{
    public class RepairsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public RepairsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Repairs
        public async Task<IActionResult> Index()
        {
            var repairs = await _context.Repairs
                .Include(r => r.Car)
                .ThenInclude(c => c.Make)
                .Include(r => r.Car)
                .ThenInclude(c => c.Model)
                .ToListAsync();
            return View(repairs);
        }

        // GET: Repairs/Create
        public IActionResult Create()
        {
            ViewData["CarId"] = new SelectList(_context.Cars.Include(c => c.Make).Include(c => c.Model), "CarId", "VIN");
            return View();
        }

        // POST: Repairs/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("RepairId,CarId,Description,Cost")] Repair repair)
        {
            if (ModelState.IsValid)
            {
                _context.Add(repair);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["CarId"] = new SelectList(_context.Cars.Include(c => c.Make).Include(c => c.Model), "CarId", "VIN", repair.CarId);
            return View(repair);
        }

        // GET: Repairs/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var repair = await _context.Repairs.FindAsync(id);
            if (repair == null)
            {
                return NotFound();
            }
            ViewData["CarId"] = new SelectList(_context.Cars.Include(c => c.Make).Include(c => c.Model), "CarId", "VIN", repair.CarId);
            return View(repair);
        }

        // POST: Repairs/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("RepairId,CarId,Description,Cost")] Repair repair)
        {
            if (id != repair.RepairId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(repair);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!RepairExists(repair.RepairId))
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
            ViewData["CarId"] = new SelectList(_context.Cars.Include(c => c.Make).Include(c => c.Model), "CarId", "VIN", repair.CarId);
            return View(repair);
        }

        private bool RepairExists(int id)
        {
            return _context.Repairs.Any(e => e.RepairId == id);
        }
    }
}
