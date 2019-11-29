using Dapper;
using Dapper.Contrib.Extensions;
using MySql.Data.MySqlClient;
using Serilog;
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
            try
            {
                return _connection.Insert(entity);
            }
            catch (Exception ex)
            {
                Log.Error(ex, $"Failed to insert player account with name: {entity.Name}.");
                throw;
            }
            
        }

        public bool Delete(PlayerAccount entity)
        {
            try
            {
                return _connection.Delete(entity);
            }
            catch (Exception ex)
            {
                Log.Error(ex, $"Failed to delete player account with name: {entity.Name}.");
                throw;
            }
            
        }

        public IEnumerable<PlayerAccount> GetAll()
        {
            try
            {
                return _connection.GetAll<PlayerAccount>();
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Failed to get all player accounts from database.");
                throw;
            }
            
        }

        public PlayerAccount Find(int id)
        {
            try
            {
                return _connection.Get<PlayerAccount>(id);

            }
            catch (Exception ex)
            {
                Log.Error(ex, $"Failed to find player account with id: {id}.");
                throw;
            }
            
        }

        public PlayerAccount Find(string name)
        {
            try
            {
                return _connection.QueryFirstOrDefault<PlayerAccount>("SELECT * FROM accounts WHERE Name = @Name;", new { Name = name });
            }
            catch (Exception ex)
            {
                Log.Error(ex, $"Failed to find player account with name: {name}.");
                throw;
            }
            
        }

        public bool Update(PlayerAccount entity)
        {
            try
            {
                return _connection.Update(entity);
            }
            catch (Exception ex)
            {
                Log.Error(ex, $"Failed to update player account with name: {entity.Name}.");
                throw;
            }
            
        }

        #endregion Sync

        #region Async

        public async Task<bool> UpdateAsync(PlayerAccount entity)
        {
            try
            {
                return await _connection.UpdateAsync(entity);
            }
            catch (Exception ex)
            {
                Log.Error(ex, $"Failed to update async player account with name: {entity.Name}.");
                throw;
            }
            
        }

        public async Task<IEnumerable<PlayerAccount>> GetAllAsync()
        {
            try
            {
                return await _connection.GetAllAsync<PlayerAccount>();
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Failed to get all player accounts from database async.");
                throw;
            }
            
        }

        public async Task<long> AddAsync(PlayerAccount entity)
        {
            try
            {
                return await _connection.InsertAsync(entity);
            }
            catch (Exception ex)
            {
                Log.Error(ex, $"Failed to insert async player account with name: {entity.Name}.");
                throw;
            }
            
        }

        public async Task<bool> DeleteAsync(PlayerAccount entity)
        {
            try
            {
                return await _connection.DeleteAsync(entity);
            }
            catch (Exception ex)
            {
                Log.Error(ex, $"Failed to delete async player account with name: {entity.Name}.");
                throw;
            }
            
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