using Microsoft.AspNetCore.Mvc;
using star_events.Models;
using star_events.Repository.Interfaces;

namespace star_events.Controllers;

public class CategoryController : Controller
{
    private readonly ICategoryRepository _categoryRepository;

    public CategoryController(ICategoryRepository categoryRepository)
    {
        _categoryRepository = categoryRepository;
    }

    // GET: Category
    public async Task<IActionResult> Index()
    {
        return View(_categoryRepository.GetAll());
    }

    // GET: Category/Details/5
    public async Task<IActionResult> Details(int? id)
    {
        if (id == null) return NotFound();

        var category = _categoryRepository.GetById(id);
        if (category == null) return NotFound();

        return View(category);
    }

    // GET: Category/Create
    public IActionResult Create()
    {
        return View();
    }

    // POST: Category/Create
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("Name,Description")] Category category)
    {
        if (ModelState.IsValid)
        {
            _categoryRepository.Insert(category);
            _categoryRepository.Save();
            TempData["SuccessMessage"] = $"Category '{category.Name}' created successfully!";
            return RedirectToAction(nameof(Index));
        }

        return View(category);
    }

    // GET: Category/Edit/5
    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null) return NotFound();

        var category = _categoryRepository.GetById(id);
        if (category == null) return NotFound();
        return View(category);
    }

    // POST: Category/Edit/5
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, [Bind("Name,Description")] Category category)
    {
        if (id != category.CategoryID) return NotFound();

        if (ModelState.IsValid)
        {
            _categoryRepository.Update(category);
            _categoryRepository.Save();
            TempData["SuccessMessage"] = $"Category '{category.Name}' updated successfully!";
            return RedirectToAction(nameof(Index));
        }

        return View(category);
    }

    // GET: Category/Delete/5
    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null) return NotFound();

        var category = _categoryRepository.GetById(id);
        if (category == null) return NotFound();

        return View(category);
    }

    // POST: Category/Delete/5
    [HttpPost]
    [ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var category = _categoryRepository.GetById(id);
        if (category != null)
        {
            _categoryRepository.Delete(id);
            _categoryRepository.Save();
            TempData["SuccessMessage"] = $"Category '{category.Name}' deleted successfully!";
        }
        else
        {
            TempData["ErrorMessage"] = "Category not found or already deleted.";
        }

        return RedirectToAction(nameof(Index));
    }
}