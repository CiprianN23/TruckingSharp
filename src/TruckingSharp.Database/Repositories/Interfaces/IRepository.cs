using System.Collections.Generic;

namespace TruckingSharp.Database.Repositories.Interfaces
{
    public interface IRepository<T> where T : class
    {
        IEnumerable<T> GetAll();
        T Find(int id);
        T Find(string name);
        long Add(T entity);
        bool Update(T entity);
        bool Delete(T entity);
    }
}
