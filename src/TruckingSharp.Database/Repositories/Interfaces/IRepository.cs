using System.Collections.Generic;
using System.Threading.Tasks;

namespace TruckingSharp.Database.Repositories.Interfaces
{
    public interface IRepository<T> where T : class
    {
        #region Sync

        IEnumerable<T> GetAll();

        T Find(int id);

        long Add(T entity);

        bool Update(T entity);

        bool Delete(T entity);

        #endregion Sync

        #region Async

        Task<IEnumerable<T>> GetAllAsync();

        Task<long> AddAsync(T entity);

        Task<bool> UpdateAsync(T entity);

        Task<bool> DeleteAsync(T entity);

        #endregion Async
    }
}