using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace Express_Voitures.Controllers
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
            var applicationDbContext = _context.Repairs.Include(r => r.Car).Include(r => r.User);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Repairs/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }


            var repair = await _context.Repairs
                .Include(r => r.Car)
                .Include(r => r.User)
                .FirstOrDefaultAsync(m => m.RepairId == id);
            if (repair == null)
            {
                return NotFound();
            }

            return View(repair);
        }

        // GET: Repairs/Create
        public IActionResult Create()
        {
            ViewData["VIN"] = new SelectList(_context.Cars, "VIN", "VIN");
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id");
            return View();
        }

        // POST: Repairs/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("RepairId,VIN,Description,Cost,Date,UserId")] Repair repair)
        {
            if (ModelState.IsValid)
            {
                _context.Add(repair);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["VIN"] = new SelectList(_context.Cars, "VIN", "VIN", repair.VIN);
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id", repair.UserId);
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
            ViewData["VIN"] = new SelectList(_context.Cars, "VIN", "VIN", repair.VIN);
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id", repair.UserId);
            return View(repair);
        }

        // POST: Repairs/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("RepairId,VIN,Description,Cost,Date,UserId")] Repair repair)
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
            ViewData["VIN"] = new SelectList(_context.Cars, "VIN", "VIN", repair.VIN);
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id", repair.UserId);
            return View(repair);
        }

        // GET: Repairs/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var repair = await _context.Repairs
                .Include(r => r.Car)
                .Include(r => r.User)
                .FirstOrDefaultAsync(m => m.RepairId == id);
            if (repair == null)
            {
                return NotFound();
            }

            return View(repair);
        }

        // POST: Repairs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var repair = await _context.Repairs.FindAsync(id);
            if (repair != null)
            {
                _context.Repairs.Remove(repair);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool RepairExists(int id)
        {
            return _context.Repairs.Any(e => e.RepairId == id);
        }
    }
}
