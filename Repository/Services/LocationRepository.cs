using star_events.Data;
using star_events.Models;
using star_events.Repository.Interfaces;

namespace star_events.Repository.Services;

public class LocationRepository : GenericRepository<Location>, ILocationRepository
{
    public LocationRepository(ApplicationDbContext context) : base(context)
    {
    }
}