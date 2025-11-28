using System.Collections.Generic;

namespace Warehouse.Interfaces
{
    public interface IRepository<T> where T : class
    {
        void Add(T entity);
        void Update(T entity);
        void Delete(T entity);
        void HardDelete(T entity);
        T? GetById(int id);
        IEnumerable<T> GetAll();
        int Count();
    }
}