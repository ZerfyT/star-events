using Microsoft.EntityFrameworkCore;
using star_events.Models;
using star_events.Repository.Interfaces;
using star_events.Data;
using Microsoft.EntityFrameworkCore;

namespace star_events.Repository.Services
{
    public class EventRepository : GenericRepository<Event>, IEventRepository
    {
        public EventRepository(ApplicationDbContext context) : base(context)
        {
        }

        public override IEnumerable<Event> GetAll()
        {
            return _context.Events
                .Include(e => e.Category)
                .Include(e => e.Location)
                .Include(e => e.Organizer)
                .ToList();
        }

        public override Event GetById(object id)
        {
            return _context.Events
                .Include(e => e.Category)
                .Include(e => e.Location)
                .Include(e => e.Organizer)
                .FirstOrDefault(e => e.EventID == (int)id);
        }

        public IEnumerable<Event> GetActiveEvents()
        {
            return _context.Events
                .Include(e => e.Category)
                .Include(e => e.Location)
                .Include(e => e.Organizer)
                .Where(e => e.Status == "Active")
                .ToList();
        }

        public IEnumerable<Event> GetEventsByOrganizer(string organizerId)
        {
            return _context.Events
                .Include(e => e.Category)
                .Include(e => e.Location)
                .Include(e => e.Organizer)
                .Where(e => e.Organizer.Id == organizerId)
                .ToList();
        }

        public IEnumerable<Event> GetUpcomingEvents()
        {
            return _context.Events
                .Include(e => e.Category)
                .Include(e => e.Location)
                .Include(e => e.Organizer)
                .Where(e => e.StartDateTime > DateTime.Now && e.Status == "Active")
                .ToList();
        }
    }
}
