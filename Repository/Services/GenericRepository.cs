using Microsoft.EntityFrameworkCore;
using star_events.Data;
using star_events.Repository.Interfaces;

namespace star_events.Repository.Services;

public class GenericRepository<T> : IGenericRepository<T> where T : class
{
    protected readonly ApplicationDbContext _context;
    private readonly DbSet<T> table;

    public GenericRepository(ApplicationDbContext context)
    {
        _context = context;
        table = _context.Set<T>();
    }

    public virtual IEnumerable<T> GetAll()
    {
        return table.ToList();
    }

    public virtual T GetById(object id)
    {
        return table.Find(id);
    }

    public void Insert(T obj)
    {
        table.Add(obj);
    }

    public void Update(T obj)
    {
        table.Attach(obj);
        _context.Entry(obj).State = EntityState.Modified;
    }

    public void Delete(object id)
    {
        var existing = GetById(id);
        if (existing != null)
            table.Remove(existing);
    }

    public void Save()
    {
        _context.SaveChanges();
    }
}