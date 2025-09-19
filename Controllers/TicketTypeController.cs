using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using star_events.Models;
using star_events.Repository.Interfaces;
using Microsoft.AspNetCore.Authorization;

namespace star_events.Controllers
{
    [Authorize(Roles = "Admin,EventOrganizer")]
    public class TicketTypeController : Controller
    {
        private readonly ITicketTypeRepository _ticketTypeRepository;
        private readonly IEventRepository _eventRepository;

        public TicketTypeController(ITicketTypeRepository ticketTypeRepository, IEventRepository eventRepository)
        {
            _ticketTypeRepository = ticketTypeRepository;
            _eventRepository = eventRepository;
        }

        // GET: TicketType
        public IActionResult Index()
        {
            var ticketTypes = _ticketTypeRepository.GetAll();
            return View(ticketTypes);
        }

        // GET: TicketType/Details/5
        public IActionResult Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var ticketType = _ticketTypeRepository.GetById(id);
            if (ticketType == null)
            {
                return NotFound();
            }

            return View(ticketType);
        }

        // GET: TicketType/Create
        public IActionResult Create()
        {
            ViewBag.Events = new SelectList(_eventRepository.GetAll(), "EventID", "Title");
            return View();
        }

        // POST: TicketType/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create([Bind("TicketTypeID,EventID,Name,Price,TotalQuantity,AvailableQuantity")] TicketType ticketType)
        {
        if (ModelState.IsValid)
        {
            _ticketTypeRepository.Insert(ticketType);
            _ticketTypeRepository.Save();
            TempData["SuccessMessage"] = $"Ticket type '{ticketType.Name}' created successfully!";
            return RedirectToAction(nameof(Index));
        }
            ViewBag.Events = new SelectList(_eventRepository.GetAll(), "EventID", "Title", ticketType.EventID);
            return View(ticketType);
        }

        // GET: TicketType/Edit/5
        public IActionResult Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var ticketType = _ticketTypeRepository.GetById(id);
            if (ticketType == null)
            {
                return NotFound();
            }
            ViewBag.Events = new SelectList(_eventRepository.GetAll(), "EventID", "Title", ticketType.EventID);
            return View(ticketType);
        }

        // POST: TicketType/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, [Bind("TicketTypeID,EventID,Name,Price,TotalQuantity,AvailableQuantity")] TicketType ticketType)
        {
            if (id != ticketType.TicketTypeID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _ticketTypeRepository.Update(ticketType);
                    _ticketTypeRepository.Save();
                    TempData["SuccessMessage"] = $"Ticket type '{ticketType.Name}' updated successfully!";
                }
                catch (Exception)
                {
                    if (!TicketTypeExists(ticketType.TicketTypeID))
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
            ViewBag.Events = new SelectList(_eventRepository.GetAll(), "EventID", "Title", ticketType.EventID);
            return View(ticketType);
        }

        // GET: TicketType/Delete/5
        public IActionResult Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var ticketType = _ticketTypeRepository.GetById(id);
            if (ticketType == null)
            {
                return NotFound();
            }

            return View(ticketType);
        }

        // POST: TicketType/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
        var ticketType = _ticketTypeRepository.GetById(id);
        if (ticketType != null)
        {
            _ticketTypeRepository.Delete(id);
            _ticketTypeRepository.Save();
            TempData["SuccessMessage"] = $"Ticket type '{ticketType.Name}' deleted successfully!";
        }
        else
        {
            TempData["ErrorMessage"] = "Ticket type not found or already deleted.";
        }

        return RedirectToAction(nameof(Index));
        }

        private bool TicketTypeExists(int id)
        {
            return _ticketTypeRepository.GetById(id) != null;
        }
    }
}
