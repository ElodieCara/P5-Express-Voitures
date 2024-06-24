using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using ExpressVoitures.Models;
using ExpressVoitures.Services;
using Microsoft.EntityFrameworkCore;

namespace ExpressVoitures.Controllers
{
    public class ModelsController : Controller
    {
        private readonly IModelService _modelService;
        private readonly IMakeService _makeService;

        public ModelsController(IModelService modelService, IMakeService makeService)
        {
            _modelService = modelService;
            _makeService = makeService;
        }

        // GET: Models
        public async Task<IActionResult> Index()
        {
            return View(await _modelService.GetAllModelsAsync());
        }

        // GET: Models/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var model = await _modelService.GetModelByIdAsync(id.Value);
            if (model == null)
            {
                return NotFound();
            }

            return View(model);
        }

        // GET: Models/Create
        public async Task<IActionResult> Create()
        {
            ViewData["MakeId"] = new SelectList(await _makeService.GetAllMakesAsync(), "MakeId", "Name");
            return View();
        }

        // POST: Models/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ModelId,MakeId,Name")] Model model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    await _modelService.AddModelAsync(model);
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError(string.Empty, $"An error occurred while saving the model: {ex.Message}");
                }
            }

            ViewData["MakeId"] = new SelectList(await _makeService.GetAllMakesAsync(), "MakeId", "Name", model.MakeId);
            return View(model);
        }

        // GET: Models/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var model = await _modelService.GetModelByIdAsync(id.Value);
            if (model == null)
            {
                return NotFound();
            }

            ViewData["MakeId"] = new SelectList(await _makeService.GetAllMakesAsync(), "MakeId", "Name", model.MakeId);
            return View(model);
        }

        // POST: Models/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ModelId,MakeId,Name")] Model model)
        {
            if (id != model.ModelId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    await _modelService.UpdateModelAsync(model);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_modelService.ModelExists(model.ModelId))
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

            ViewData["MakeId"] = new SelectList(await _makeService.GetAllMakesAsync(), "MakeId", "Name", model.MakeId);
            return View(model);
        }

        // GET: Models/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var model = await _modelService.GetModelByIdAsync(id.Value);
            if (model == null)
            {
                return NotFound();
            }

            return View(model);
        }

        // POST: Models/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _modelService.DeleteModelAsync(id);
            return RedirectToAction(nameof(Index));
        }
    }
}
