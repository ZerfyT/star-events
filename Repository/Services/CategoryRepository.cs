using star_events.Data;
using star_events.Models;
using star_events.Repository.Interfaces;

namespace star_events.Repository.Services;

public class CategoryRepository : GenericRepository<Category>, ICategoryRepository
{
    public CategoryRepository(ApplicationDbContext context) : base(context)
    {
    }
}