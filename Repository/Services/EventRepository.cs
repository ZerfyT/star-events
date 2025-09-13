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

        public new IEnumerable<Event> GetAll()
        {
            return _context.Events
                .Include(e => e.Category)
                .Include(e => e.Location)
                .Include(e => e.Organizer)
                .ToList();
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
                .Where(e => e.OrganizerId == organizerId)
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
