using Microsoft.EntityFrameworkCore;
using star_events.Data;
using star_events.Models;
using star_events.Repository.Interfaces;

namespace star_events.Repository.Services;

public class PromotionRepository : GenericRepository<Promotion>, IPromotionRepository
{
    public PromotionRepository(ApplicationDbContext context) : base(context)
    {
    }

    public override IEnumerable<Promotion> GetAll()
    {
        return _context.Promotions.Include(p => p.Event).ToList();
    }

    public override Promotion GetById(object id)
    {
        return _context.Promotions.Include(p => p.Event).FirstOrDefault(p => p.PromotionID == (int)id);
    }
}