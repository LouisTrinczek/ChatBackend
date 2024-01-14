using Chat.Application.Contracts.Repositories;
using Chat.Application.Exceptions;
using Chat.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace Chat.Infrastructure.Database.Repositories;

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

    public T? GetById(string id)
    {
        return _table.FirstOrDefault(t => t.Id == id);
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

    public void SoftDelete(string id)
    {
        var existing = _table.Where(it => it.Id == id).ToList();

        existing[0].DeletedAt = DateTime.Now;
    }

    public void Save()
    {
        _context.SaveChanges();
    }
}
