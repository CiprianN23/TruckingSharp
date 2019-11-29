using Dapper;
using Dapper.Contrib.Extensions;
using MySql.Data.MySqlClient;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TruckingSharp.Database.Entities;
using TruckingSharp.Database.Repositories.Interfaces;

namespace TruckingSharp.Database.Repositories
{
    public sealed class PlayerBanRepository : IRepository<PlayerBan>, IDisposable
    {
        private readonly MySqlConnection _connection;
        private bool _isDisposed;

        public PlayerBanRepository(MySqlConnection connection)
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

        ~PlayerBanRepository()
        {
            Dispose(false);
        }

        #region Sync

        public long Add(PlayerBan entity)
        {
            try
            {
                return _connection.Insert(entity);
            }
            catch (Exception ex)
            {
                Log.Error(ex, $"Failed to insert player ban with owner id: {entity.OwnerId}.");
                throw;
            }
        }

        public bool Delete(PlayerBan entity)
        {
            try
            {
                return _connection.Delete(entity);
            }
            catch (Exception ex)
            {
                Log.Error(ex, $"Failed to delete player ban with owner id: {entity.OwnerId}.");
                throw;
            }
        }

        public PlayerBan Find(int ownerId)
        {
            try
            {
                return _connection.QueryFirstOrDefault("SELECT * FROM playerbans WHERE OwnerId = @Id",
                    new { Id = ownerId });
            }
            catch (Exception ex)
            {
                Log.Error(ex, $"Failed to find player ban with owner id: {ownerId}.");
                throw;
            }
        }

        public PlayerBan Find(string ownerName)
        {
            try
            {
                return _connection
                    .Query<PlayerBan>(
                        "SELECT playerbans.* FROM playerbans LEFT JOIN accounts ON playerbans.OwnerId = accounts.Id WHERE accounts.Name = @Name",
                        new { Name = ownerName }).FirstOrDefault();
            }
            catch (Exception ex)
            {
                Log.Error(ex, $"Failed to find player ban with owner name: {ownerName}.");
                throw;
            }
        }

        public IEnumerable<PlayerBan> GetAll()
        {
            try
            {
                return _connection.GetAll<PlayerBan>();
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Failed to get all player bans.");
                throw;
            }
        }

        public bool Update(PlayerBan entity)
        {
            try
            {
                return _connection.Update(entity);
            }
            catch (Exception ex)
            {
                Log.Error(ex, $"Failed to to update player ban with owner id: {entity.OwnerId}.");
                throw;
            }
        }

        #endregion Sync

        #region Async

        public async Task<long> AddAsync(PlayerBan entity)
        {
            try
            {
                return await _connection.InsertAsync(entity);
            }
            catch (Exception ex)
            {
                Log.Error(ex, $"Failed to to insert async player ban with owner id: {entity.OwnerId}.");
                throw;
            }
        }

        public async Task<bool> DeleteAsync(PlayerBan entity)
        {
            try
            {
                return await _connection.DeleteAsync(entity);
            }
            catch (Exception ex)
            {
                Log.Error(ex, $"Failed to to delete async player ban with owner id: {entity.OwnerId}.");
                throw;
            }
        }

        public async Task<IEnumerable<PlayerBan>> GetAllAsync()
        {
            try
            {
                return await _connection.GetAllAsync<PlayerBan>();
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Failed to get all players bans async.");
                throw;
            }
        }

        public async Task<bool> UpdateAsync(PlayerBan entity)
        {
            try
            {
                return await _connection.UpdateAsync(entity);
            }
            catch (Exception ex)
            {
                Log.Error(ex, $"Failed to to update async player ban with owner id: {entity.OwnerId}.");
                throw;
            }
        }

        #endregion Async
    }
}