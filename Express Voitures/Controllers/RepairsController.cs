using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using ExpressVoitures.Models;
using ExpressVoitures.Services;
using Microsoft.EntityFrameworkCore;

namespace ExpressVoitures.Controllers
{
    public class RepairsController : Controller
    {
        private readonly IRepairService _repairService;
        private readonly ICarService _carService;

        public RepairsController(IRepairService repairService, ICarService carService)
        {
            _repairService = repairService;
            _carService = carService;
        }

        // GET: Repairs
        public async Task<IActionResult> Index()
        {
            var repairs = await _repairService.GetAllRepairsAsync();
            return View(repairs);
        }

        // GET: Repairs/Create
        public async Task<IActionResult> Create()
        {
            ViewData["CarId"] = new SelectList(await _carService.GetAllCarsAsync(), "CarId", "VIN");
            return View();
        }

        // POST: Repairs/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("RepairId,CarId,RepairDescription,Cost")] Repair repair)
        {
            if (ModelState.IsValid)
            {
                await _repairService.AddRepairAsync(repair);
                return RedirectToAction(nameof(Index));
            }
            ViewData["CarId"] = new SelectList(await _carService.GetAllCarsAsync(), "CarId", "VIN", repair.CarId);
            return View(repair);
        }

        // GET: Repairs/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var repair = await _repairService.GetRepairByIdAsync(id.Value);
            if (repair == null)
            {
                return NotFound();
            }
            ViewData["CarId"] = new SelectList(await _carService.GetAllCarsAsync(), "CarId", "VIN", repair.CarId);
            return View(repair);
        }

        // POST: Repairs/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("RepairId,CarId,RepairDescription,Cost")] Repair repair)
        {
            if (id != repair.RepairId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    await _repairService.UpdateRepairAsync(repair);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!await _repairService.RepairExistsAsync(repair.RepairId))
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
            ViewData["CarId"] = new SelectList(await _carService.GetAllCarsAsync(), "CarId", "VIN", repair.CarId);
            return View(repair);
        }

        private async Task<bool> RepairExists(int id)
        {
            return await _repairService.RepairExistsAsync(id);
        }
    }
}
