using Microsoft.EntityFrameworkCore;
using star_events.Data;
using star_events.Models;
using star_events.Repository.Interfaces;

namespace star_events.Repository.Services;

public class TicketTypeRepository : GenericRepository<TicketType>, ITicketTypeRepository
{
    public TicketTypeRepository(ApplicationDbContext context) : base(context)
    {
        
    }

    public override IEnumerable<TicketType> GetAll()
    {
        return _context.TicketTypes.Include(t => t.Event).ToList();
    }

    public override TicketType GetById(object id)
    {
        return _context.TicketTypes.Include(t => t.Event).FirstOrDefault(t => t.TicketTypeID == (int)id);
    }
}