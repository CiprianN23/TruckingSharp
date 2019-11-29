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
            try
            {
                return _connection.Execute("INSERT INTO speedcameras (Id, Speed, PositionX, PositionY, PositionZ, Angle) VALUES (@Id, @Speed, @PositionX, @PositionY, @PositionZ, @Angle)", entity);
            }
            catch (Exception ex)
            {
                Log.Error(ex, $"Failed to insert speed camera with id: {entity.Id}.");
                throw;
            }
            
        }

        public bool Delete(SpeedCamera entity)
        {
            try
            {
                return _connection.Delete(entity);
            }
            catch (Exception ex)
            {
                Log.Error(ex, $"Failed to delete speed camera with id: {entity.Id}.");
                throw;
            }
            
        }

        public SpeedCamera Find(int id)
        {
            try
            {
                return _connection.Get<SpeedCamera>(id);
            }
            catch (Exception ex)
            {
                Log.Error(ex, $"Failed to find speed camera with id: {id}.");
                throw;
            }
            
        }

        public IEnumerable<SpeedCamera> GetAll()
        {
            try
            {
                return _connection.GetAll<SpeedCamera>();
            }
            catch (Exception ex)
            {
                Log.Error(ex, $"Failed to get all speed cameras.");
                throw;
            }
            
        }

        public bool Update(SpeedCamera entity)
        {
            try
            {
                return _connection.Update(entity);
            }
            catch (Exception ex)
            {
                Log.Error(ex, $"Failed to update speed camera with id: {entity.Id}.");
                throw;
            }
            
        }

        #endregion Sync

        #region Async

        public async Task<long> AddAsync(SpeedCamera entity)
        {
            try
            {
                return await _connection.ExecuteAsync("INSERT INTO speedcameras (Id, Speed, PositionX, PositionY, PositionZ, Angle) VALUES (@Id, @Speed, @PositionX, @PositionY, @PositionZ, @Angle)", entity);
            }
            catch (Exception ex)
            {
                Log.Error(ex, $"Failed to insert async speed camera with id: {entity.Id}.");
                throw;
            }
            
        }

        public async Task<bool> DeleteAsync(SpeedCamera entity)
        {
            try
            {
                return await _connection.DeleteAsync(entity);
            }
            catch (Exception ex)
            {
                Log.Error(ex, $"Failed to delete async speed camera with id: {entity.Id}.");
                throw;
            }
            
        }

        public async Task<IEnumerable<SpeedCamera>> GetAllAsync()
        {
            try
            {
                return await _connection.GetAllAsync<SpeedCamera>();
            }
            catch (Exception ex)
            {
                Log.Error(ex, $"Failed to get all speed cameras async.");
                throw;
            }
            
        }

        public async Task<bool> UpdateAsync(SpeedCamera entity)
        {
            try
            {
                return await _connection.UpdateAsync(entity);
            }
            catch (Exception ex)
            {
                Log.Error(ex, $"Failed to update async speed camera with id: {entity.Id}.");
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

        ~SpeedCameraRepository()
        {
            Dispose(false);
        }
    }
}