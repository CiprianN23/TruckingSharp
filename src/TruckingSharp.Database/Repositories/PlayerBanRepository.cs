using Dapper;
using Dapper.Contrib.Extensions;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TruckingSharp.Database.Entities;
using TruckingSharp.Database.Repositories.Interfaces;

namespace TruckingSharp.Database.Repositories
{
    public class PlayerBanRepository : IRepository<PlayerBan>, IDisposable
    {
        private MySqlConnection _connection;
        private bool isDisposed;

        public PlayerBanRepository(MySqlConnection connection)
        {
            _connection = connection;
        }

        #region Sync

        public long Add(PlayerBan entity)
        {
            return _connection.Insert(entity);
        }

        public bool Delete(PlayerBan entity)
        {
            return _connection.Delete(entity);
        }

        public PlayerBan Find(int ownerId)
        {
            return _connection.QueryFirstOrDefault("SELECT * FROM playerbans WHERE OwnerId = @Id", new { Id = ownerId });
        }

        public PlayerBan Find(string ownerName)
        {
            return _connection.Query<PlayerBan>("SELECT playerbans.* FROM playerbans LEFT JOIN accounts ON playerbans.OwnerId = accounts.Id WHERE accounts.Name = @Name", new { Name = ownerName }).FirstOrDefault();
        }

        public IEnumerable<PlayerBan> GetAll()
        {
            return _connection.GetAll<PlayerBan>();
        }

        public bool Update(PlayerBan entity)
        {
            return _connection.Update(entity);
        }

        #endregion Sync

        #region Async

        public async Task<long> AddAsync(PlayerBan entity)
        {
            return await _connection.InsertAsync(entity);
        }

        public async Task<bool> DeleteAsync(PlayerBan entity)
        {
            return await _connection.DeleteAsync(entity);
        }

        public async Task<IEnumerable<PlayerBan>> GetAllAsync()
        {
            return await _connection.GetAllAsync<PlayerBan>();
        }

        public async Task<bool> UpdateAsync(PlayerBan entity)
        {
            return await _connection.UpdateAsync(entity);
        }

        #endregion Async

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!isDisposed)
            {
                if (disposing)
                    _connection.Dispose();
            }


            _connection.Dispose();
            isDisposed = true;
        }

        ~PlayerBanRepository()
        {
            Dispose(false);
        }
    }
}