using star_events.Models;

namespace star_events.Repository.Interfaces;

public interface IEventRepository : IGenericRepository<Event>
{
    IEnumerable<Event> GetActiveEvents();
    IEnumerable<Event> GetEventsByOrganizer(string organizerId);
    IEnumerable<Event> GetUpcomingEvents();
}