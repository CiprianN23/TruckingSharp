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
    public class SpeedCameraRepository : IRepository<SpeedCamera>, IDisposable
    {
        private MySqlConnection _connection;
        private bool isDisposed;

        public SpeedCameraRepository(MySqlConnection conenction)
        {
            _connection = conenction;
        }

        #region Sync

        public long Add(SpeedCamera entity)
        {
            return _connection.Execute("INSERT INTO speedcameras (Id, Speed, PositionX, PositionY, PositionZ, Angle) VALUES (@Id, @Speed, @PositionX, @PositionY, @PositionZ, @Angle)", entity);
            //return _connection.Insert(entity);
        }

        public bool Delete(SpeedCamera entity)
        {
            return _connection.Delete(entity);
        }

        public SpeedCamera Find(int id)
        {
            return _connection.Get<SpeedCamera>(id);
        }

        public IEnumerable<SpeedCamera> GetAll()
        {
            return _connection.GetAll<SpeedCamera>();
        }

        public bool Update(SpeedCamera entity)
        {
            return _connection.Update(entity);
        }

        #endregion Sync

        #region Async

        public async Task<long> AddAsync(SpeedCamera entity)
        {
            return await _connection.ExecuteAsync("INSERT INTO speedcameras (Id, Speed, PositionX, PositionY, PositionZ, Angle) VALUES (@Id, @Speed, @PositionX, @PositionY, @PositionZ, @Angle)", entity);
        }

        public async Task<bool> DeleteAsync(SpeedCamera entity)
        {
            return await _connection.DeleteAsync(entity);
        }

        public async Task<IEnumerable<SpeedCamera>> GetAllAsync()
        {
            return await _connection.GetAllAsync<SpeedCamera>();
        }

        public async Task<bool> UpdateAsync(SpeedCamera entity)
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
            if (!isDisposed && disposing)
            {
                // Dispose other resources here
            }

            _connection.Dispose();
            isDisposed = true;
        }

        ~SpeedCameraRepository()
        {
            Dispose(false);
        }
    }
}