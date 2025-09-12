using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using star_events.Models;
using star_events.Repository.Interfaces;
using Microsoft.AspNetCore.Identity;

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
        public IActionResult Create([Bind("EventID,LocationID,CategoryID,OrganizerID,Title,Description,StartDateTime,EndDateTime,ImageURL,Status")] Event @event)
        {
            if (ModelState.IsValid)
            {
                _eventRepository.Insert(@event);
                _eventRepository.Save();
                return RedirectToAction(nameof(Index));
            }
            ViewData["CategoryID"] = new SelectList(_categoryRepository.GetAll(), "CategoryID", "Name", @event.CategoryID);
            ViewData["LocationID"] = new SelectList(_locationRepository.GetAll(), "LocationID", "Address", @event.LocationID);
            ViewData["OrganizerID"] = new SelectList(_userManager.Users, "Id", "UserName", @event.OrganizerID);
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
            ViewData["OrganizerID"] = new SelectList(_userManager.Users, "Id", "UserName", @event.OrganizerID);
            return View(@event);
        }

        // POST: Event/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, [Bind("EventID,LocationID,CategoryID,OrganizerID,Title,Description,StartDateTime,EndDateTime,ImageURL,Status")] Event @event)
        {
            if (id != @event.EventID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
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
            ViewData["OrganizerID"] = new SelectList(_userManager.Users, "Id", "UserName", @event.OrganizerID);
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
