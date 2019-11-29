using Dapper;
using Dapper.Contrib.Extensions;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TruckingSharp.Database.Entities;
using TruckingSharp.Database.Repositories.Interfaces;

namespace TruckingSharp.Database.Repositories
{
    public class PlayerAccountRepository : IRepository<PlayerAccount>, IDisposable
    {
        private MySqlConnection _connection;
        private bool isDisposed;

        public PlayerAccountRepository(MySqlConnection conenction)
        {
            _connection = conenction;
        }

        #region Sync

        public long Add(PlayerAccount entity)
        {
            return _connection.Insert(entity);
        }

        public bool Delete(PlayerAccount entity)
        {
            return _connection.Delete(entity);
        }

        public IEnumerable<PlayerAccount> GetAll()
        {
            return _connection.GetAll<PlayerAccount>();
        }

        public PlayerAccount Find(int id)
        {
            return _connection.Get<PlayerAccount>(id);
        }

        public PlayerAccount Find(string name)
        {
            return _connection.QueryFirstOrDefault<PlayerAccount>("SELECT * FROM accounts WHERE Name = @Name;", new { Name = name });
        }

        public bool Update(PlayerAccount entity)
        {
            return _connection.Update(entity);
        }

        #endregion Sync

        #region Async

        public async Task<bool> UpdateAsync(PlayerAccount entity)
        {
            return await _connection.UpdateAsync(entity);
        }

        public async Task<IEnumerable<PlayerAccount>> GetAllAsync()
        {
            return await _connection.GetAllAsync<PlayerAccount>();
        }

        public async Task<long> AddAsync(PlayerAccount entity)
        {
            return await _connection.InsertAsync(entity);
        }

        public async Task<bool> DeleteAsync(PlayerAccount entity)
        {
            return await _connection.DeleteAsync(entity);
        }

        #endregion Async

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!isDisposed && disposing)
            {
                // Dispose other resources here
            }

            _connection.Dispose();
            isDisposed = true;
        }

        ~PlayerAccountRepository()
        {
            Dispose(false);
        }
    }
}