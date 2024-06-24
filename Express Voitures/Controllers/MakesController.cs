using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ExpressVoitures.Models;
using ExpressVoitures.Services;
using Microsoft.EntityFrameworkCore;

namespace ExpressVoitures.Controllers
{
    public class MakesController : Controller
    {
        private readonly IMakeService _makeService;

        public MakesController(IMakeService makeService)
        {
            _makeService = makeService;
        }

        // GET: Makes
        public async Task<IActionResult> Index()
        {
            return View(await _makeService.GetAllMakesAsync());
        }

        // GET: Makes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var make = await _makeService.GetMakeByIdAsync(id.Value);
            if (make == null)
            {
                return NotFound();
            }

            return View(make);
        }

        // GET: Makes/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Makes/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("MakeId,Name")] Make make)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    await _makeService.AddMakeAsync(make);
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError(string.Empty, $"An error occurred while saving the make: {ex.Message}");
                }
            }
            return View(make);
        }

        // GET: Makes/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var make = await _makeService.GetMakeByIdAsync(id.Value);
            if (make == null)
            {
                return NotFound();
            }
            return View(make);
        }

        // POST: Makes/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("MakeId,Name")] Make make)
        {
            if (id != make.MakeId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    await _makeService.UpdateMakeAsync(make);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_makeService.MakeExists(make.MakeId))
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
            return View(make);
        }

        // GET: Makes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var make = await _makeService.GetMakeByIdAsync(id.Value);
            if (make == null)
            {
                return NotFound();
            }

            return View(make);
        }

        // POST: Makes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _makeService.DeleteMakeAsync(id);
            return RedirectToAction(nameof(Index));
        }
    }
}
