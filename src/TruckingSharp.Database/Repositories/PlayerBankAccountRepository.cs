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
    public sealed class PlayerBankAccountRepository : IRepository<PlayerBankAccount>, IDisposable
    {
        private readonly MySqlConnection _connection;
        private bool _isDisposed;

        public PlayerBankAccountRepository(MySqlConnection connection)
        {
            _connection = connection;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        private void Dispose(bool disposing)
        {
            if (!_isDisposed && disposing)
            {
                // Dispose other resources here
            }

            _connection.Dispose();
            _isDisposed = true;
        }

        ~PlayerBankAccountRepository()
        {
            Dispose(false);
        }

        #region Sync

        public IEnumerable<PlayerBankAccount> GetAll()
        {
            try
            {
                return _connection.GetAll<PlayerBankAccount>();
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Failed to get all player bank accounts from database.");
                throw;
            }
        }

        public PlayerBankAccount Find(int id)
        {
            try
            {
                return _connection.QueryFirstOrDefault<PlayerBankAccount>(
                    "SELECT * FROM bankaccounts WHERE PlayerId = @Id;", new { Id = id });
            }
            catch (Exception ex)
            {
                Log.Error(ex, $"Failed to get player bank account with id: {id}.");
                throw;
            }
        }

        public long Add(PlayerBankAccount entity)
        {
            try
            {
                return _connection.Insert(entity);
            }
            catch (Exception ex)
            {
                Log.Error(ex, $"Failed to insert player bank account with player id: {entity.PlayerId}.");
                throw;
            }
        }

        public bool Update(PlayerBankAccount entity)
        {
            try
            {
                return _connection.Update(entity);
            }
            catch (Exception ex)
            {
                Log.Error(ex, $"Failed to update player bank account with player id: {entity.PlayerId}.");
                throw;
            }
        }

        public bool Delete(PlayerBankAccount entity)
        {
            try
            {
                return _connection.Delete(entity);
            }
            catch (Exception ex)
            {
                Log.Error(ex, $"Failed to delete player bank account with player id: {entity.PlayerId}.");
                throw;
            }
        }

        #endregion Sync

        #region Async

        public async Task<IEnumerable<PlayerBankAccount>> GetAllAsync()
        {
            try
            {
                return await _connection.GetAllAsync<PlayerBankAccount>();
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Failed to get all player bank accounts async.");
                throw;
            }
        }

        public async Task<long> AddAsync(PlayerBankAccount entity)
        {
            try
            {
                return await _connection.InsertAsync(entity);
            }
            catch (Exception ex)
            {
                Log.Error(ex, $"Failed to insert async player bank account with player id: {entity.PlayerId}.");
                throw;
            }
        }

        public async Task<bool> UpdateAsync(PlayerBankAccount entity)
        {
            try
            {
                return await _connection.UpdateAsync(entity);
            }
            catch (Exception ex)
            {
                Log.Error(ex, $"Failed to update async player bank account with player id: {entity.PlayerId}.");
                throw;
            }
        }

        public async Task<bool> DeleteAsync(PlayerBankAccount entity)
        {
            try
            {
                return await _connection.DeleteAsync(entity);
            }
            catch (Exception ex)
            {
                Log.Error(ex, $"Failed to delete async player bank account with player id: {entity.PlayerId}.");
                throw;
            }
        }

        #endregion Async
    }
}