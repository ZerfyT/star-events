using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using star_events.Models;
using star_events.Repository.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.IO;

namespace star_events.Controllers
{
    public class EventController : Controller
    {
        private readonly IEventRepository _eventRepository;
        private readonly ILocationRepository _locationRepository;
        private readonly ICategoryRepository _categoryRepository;
        private readonly UserManager<ApplicationUser> _userManager;

        public EventController(IEventRepository eventRepository, ILocationRepository locationRepository, 
            ICategoryRepository categoryRepository, UserManager<ApplicationUser> userManager)
        {
            _eventRepository = eventRepository;
            _locationRepository = locationRepository;
            _categoryRepository = categoryRepository;
            _userManager = userManager;
        }

        // GET: Event
        public IActionResult Index()
        {
            var events = _eventRepository.GetAll();
            return View(events);
        }

        // GET: Event/Details/5
        public IActionResult Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var @event = _eventRepository.GetById(id);
            if (@event == null)
            {
                return NotFound();
            }

            return View(@event);
        }

        // GET: Event/Create
        public IActionResult Create()
        {
            ViewData["CategoryID"] = new SelectList(_categoryRepository.GetAll(), "CategoryID", "Name");
            ViewData["LocationID"] = new SelectList(_locationRepository.GetAll(), "LocationID", "Address");
            ViewData["OrganizerID"] = new SelectList(_userManager.Users, "Id", "UserName");
            return View();
        }

        // POST: Event/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create([Bind("EventID,LocationID,CategoryID,OrganizerID,Title,Description,StartDateTime,EndDateTime,Status")] Event @event, IFormFile[] uploadedImages)
        {
            // Console.WriteLine($"Model State: {ModelState.IsValid}");
            // foreach (var error in ModelState.SelectMany(state => state.Value.Errors))
            // {
            //     Console.WriteLine($"    Error: {error.ErrorMessage}");
            // }
            if (ModelState.IsValid)
            {
                var allImagePaths = new List<string>();

                // Handle file uploads
                if (uploadedImages != null && uploadedImages.Length > 0)
                {
                    var uploadPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads", "events");
                    if (!Directory.Exists(uploadPath))
                    {
                        Directory.CreateDirectory(uploadPath);
                    }

                    for (int i = 0; i < Math.Min(uploadedImages.Length, 10); i++)
                    {
                        if (uploadedImages[i] != null && uploadedImages[i].Length > 0)
                        {
                            var fileName = $"{Guid.NewGuid()}_{uploadedImages[i].FileName}";
                            var filePath = Path.Combine(uploadPath, fileName);
                            
                            using (var stream = new FileStream(filePath, FileMode.Create))
                            {
                                uploadedImages[i].CopyTo(stream);
                            }
                            
                            var relativePath = $"/uploads/events/{fileName}";
                            allImagePaths.Add(relativePath);
                        }
                    }
                }

                // Store all image paths as JSON array
                @event.AllImagePaths = allImagePaths;

                _eventRepository.Insert(@event);
                _eventRepository.Save();
                return RedirectToAction(nameof(Index));
            }
            
            ViewData["CategoryID"] = new SelectList(_categoryRepository.GetAll(), "CategoryID", "Name", @event.CategoryID);
            ViewData["LocationID"] = new SelectList(_locationRepository.GetAll(), "LocationID", "Address", @event.LocationID);
            ViewData["OrganizerID"] = new SelectList(_userManager.Users, "Id", "UserName", @event.Organizer?.Id);
            return View(@event);
        }

        // GET: Event/Edit/5
        public IActionResult Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var @event = _eventRepository.GetById(id);
            if (@event == null)
            {
                return NotFound();
            }
            ViewData["CategoryID"] = new SelectList(_categoryRepository.GetAll(), "CategoryID", "Name", @event.CategoryID);
            ViewData["LocationID"] = new SelectList(_locationRepository.GetAll(), "LocationID", "Address", @event.LocationID);
            ViewData["OrganizerID"] = new SelectList(_userManager.Users, "Id", "UserName", @event.Organizer?.Id);
            return View(@event);
        }

        // POST: Event/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, [Bind("EventID,LocationID,CategoryID,OrganizerID,Title,Description,StartDateTime,EndDateTime,Status")] Event @event, IFormFile[] uploadedImages, string deletedImageIndexes)
        {
            if (id != @event.EventID)
            {
                return NotFound();
            }
            
            // var existingEvent = _eventRepository.GetById(id);

            if (ModelState.IsValid)
            {
                try
                {
                    var currentImagePaths = @event.AllImagePaths ?? [];

                    // Handle deleted images
                    if (!string.IsNullOrEmpty(deletedImageIndexes))
                    {
                        var deletedIndexes = deletedImageIndexes.Split(',')
                            .Where(x => int.TryParse(x, out _))
                            .Select(int.Parse)
                            .OrderByDescending(x => x) // Delete from end to maintain indexes
                            .ToList();

                        foreach (var index in deletedIndexes)
                        {
                            if (index >= 0 && index < currentImagePaths.Count)
                            {
                                // Delete physical file if it's an uploaded file
                                var imagePath = currentImagePaths[index];
                                if (!imagePath.StartsWith("http") && !string.IsNullOrEmpty(imagePath))
                                {
                                    var fullPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", imagePath.TrimStart('/'));
                                    if (System.IO.File.Exists(fullPath))
                                    {
                                        try
                                        {
                                            System.IO.File.Delete(fullPath);
                                        }
                                        catch
                                        {
                                            // Log error but continue - file might be in use
                                        }
                                    }
                                }
                                
                                currentImagePaths.RemoveAt(index);
                            }
                        }
                    }

                    // Handle new file uploads
                    if (uploadedImages != null && uploadedImages.Length > 0)
                    {
                        var uploadPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads", "events");
                        if (!Directory.Exists(uploadPath))
                        {
                            Directory.CreateDirectory(uploadPath);
                        }

                        for (int i = 0; i < Math.Min(uploadedImages.Length, 10); i++)
                        {
                            if (uploadedImages[i] != null && uploadedImages[i].Length > 0)
                            {
                                var fileName = $"{Guid.NewGuid()}_{uploadedImages[i].FileName}";
                                var filePath = Path.Combine(uploadPath, fileName);
                                
                                using (var stream = new FileStream(filePath, FileMode.Create))
                                {
                                    uploadedImages[i].CopyTo(stream);
                                }
                                
                                var relativePath = $"/uploads/events/{fileName}";
                                currentImagePaths.Add(relativePath);
                            }
                        }
                    }


                    // Store all image paths as JSON array
                    @event.AllImagePaths = currentImagePaths;

                    _eventRepository.Update(@event);
                    _eventRepository.Save();
                }
                catch (Exception)
                {
                    if (!EventExists(@event.EventID))
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
            ViewData["CategoryID"] = new SelectList(_categoryRepository.GetAll(), "CategoryID", "Name", @event.CategoryID);
            ViewData["LocationID"] = new SelectList(_locationRepository.GetAll(), "LocationID", "Address", @event.LocationID);
            ViewData["OrganizerID"] = new SelectList(_userManager.Users, "Id", "UserName", @event.Organizer?.Id);
            return View(@event);
        }

        // GET: Event/Delete/5
        public IActionResult Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var @event = _eventRepository.GetById(id);
            if (@event == null)
            {
                return NotFound();
            }

            return View(@event);
        }

        // POST: Event/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            var @event = _eventRepository.GetById(id);
            if (@event != null)
            {
                _eventRepository.Delete(id);
                _eventRepository.Save();
            }

            return RedirectToAction(nameof(Index));
        }

        private bool EventExists(int id)
        {
            return _eventRepository.GetById(id) != null;
        }
    }
}
