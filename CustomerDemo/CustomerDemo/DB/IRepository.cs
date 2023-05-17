using System.Security.Principal;
using CustomerDemo.Model;

namespace CustomerDemo.DB
{
    public interface IRepository<T> where T : class, IEntity
    {
        List<T> GetAll();
        T Get(Guid id);
        T Add(T? entity);
        T Update(T entity);
        T Delete(Guid id);
    }
}
