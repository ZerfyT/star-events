using star_events.Models;
using star_events.Repository.Interfaces;
using star_events.Data;

namespace star_events.Repository.Services
{
    public class EventRepository : GenericRepository<Event>, IEventRepository
    {
        public EventRepository(ApplicationDbContext context) : base(context)
        {
        }

        public IEnumerable<Event> GetActiveEvents()
        {
            return _context.Events.Where(e => e.Status == "Active").ToList();
        }

        public IEnumerable<Event> GetEventsByOrganizer(string organizerId)
        {
            return _context.Events.Where(e => e.Organizer.Id == organizerId).ToList();
        }

        public IEnumerable<Event> GetUpcomingEvents()
        {
            return _context.Events.Where(e => e.StartDateTime > DateTime.Now && e.Status == "Active").ToList();
        }
    }
}
