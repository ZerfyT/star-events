using Microsoft.AspNetCore.Mvc;
using star_events.Models;
using star_events.Repository.Interfaces;

namespace star_events.Controllers;

public class LocationController : Controller
{
    private readonly ILocationRepository _locationRepository;

    public LocationController(ILocationRepository locationRepository)
    {
        _locationRepository = locationRepository;
    }

    // GET: Location
    public async Task<IActionResult> Index()
    {
        return View(_locationRepository.GetAll());
    }

    // GET: Location/Details/5
    public async Task<IActionResult> Details(int? id)
    {
        if (id == null) return NotFound();

        var location = _locationRepository.GetById(id);
        if (location == null) return NotFound();

        return View(location);
    }

    // GET: Location/Create
    public async Task<IActionResult> Create()
    {
        return View();
    }

    // POST: Location/Create
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("Name,Address,City,Capacity")] Location location)
    {
        if (ModelState.IsValid)
        {
            _locationRepository.Insert(location);
            _locationRepository.Save();
            TempData["SuccessMessage"] = $"Location '{location.Name}' created successfully!";
            return RedirectToAction(nameof(Index));
        }
        else
        {
            // foreach (var modelStateEntry in ModelState.Values)
            // {
            //     foreach (var error in modelStateEntry.Errors)
            //     {
            //         // Log or display error.ErrorMessage
            //         Console.WriteLine(error.ErrorMessage);
            //     }
            // }
        }
        
        return View(location);
    }

    // GET: Location/Edit/5
    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null) return NotFound();

        var location = _locationRepository.GetById(id);
        if (location == null) return NotFound();
        return View(location);
    }

    // POST: Location/Edit/5
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, [Bind("LocationID,Name,Address,City,Capacity")] Location location)
    {
        if (id != location.LocationID) return NotFound();

        if (ModelState.IsValid)
        {
            _locationRepository.Update(location);
            _locationRepository.Save();
            TempData["SuccessMessage"] = $"Location '{location.Name}' updated successfully!";
            return RedirectToAction(nameof(Index));
        }

        return View(location);
    }

    // GET: Location/Delete/5
    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null) return NotFound();

        var location = _locationRepository.GetById(id);
        if (location == null) return NotFound();

        return View(location);
    }

    // POST: Location/Delete/5
    [HttpPost]
    [ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var location = _locationRepository.GetById(id);
        if (location != null)
        {
            _locationRepository.Delete(id);
            _locationRepository.Save();
            TempData["SuccessMessage"] = $"Location '{location.Name}' deleted successfully!";
        }
        else
        {
            TempData["ErrorMessage"] = "Location not found or already deleted.";
        }

        return RedirectToAction(nameof(Index));
    }
}