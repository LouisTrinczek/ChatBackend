using Chat.Domain;
using Chat.Domain.Entities;
using Chat.Persistence.Context;
using EntityFramework.Exceptions.Common;
using Microsoft.EntityFrameworkCore;

namespace Chat.Persistence.Repositories;

public class GenericRepository<T> : IGenericRepository<T>
    where T : BaseEntity
{
    protected readonly ChatDataContext _context;
    protected readonly DbSet<T> _table;

    public GenericRepository(ChatDataContext chatDataContext)
    {
        _context = chatDataContext;
        _table = _context.Set<T>();
    }

    public IEnumerable<T> GetAll()
    {
        return _table.ToList().Where(d => d.DeletedAt == null);
    }

    public T GetById(object id)
    {
        return _table.Find(id)!;
    }

    public void Insert(T obj)
    {
        _table.Add(obj);
    }

    public void Update(T obj)
    {
        obj.UpdatedAt = new DateTime();

        _table.Attach(obj);
        _context.Entry(obj).State = EntityState.Modified;
    }

    public void Delete(object id)
    {
        var existing = _table.Find(id);

        _table.Remove(existing!);
    }

    public void SoftDelete(object id)
    {
        var existing = _table.Find(id);

        existing.DeletedAt = DateTime.Now;

        _table.Update(existing);
    }

    public void Save()
    {
        _context.SaveChanges();
    }
}
