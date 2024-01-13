namespace Chat.Application.Contracts.Repositories;

public interface IGenericRepository<T>
    where T : class
{
    IEnumerable<T> GetAll();
    T GetById(object id);
    void Insert(T obj);
    void Update(T obj);
    void Delete(object id);
    void SoftDelete(string id);
    void Save();
}
