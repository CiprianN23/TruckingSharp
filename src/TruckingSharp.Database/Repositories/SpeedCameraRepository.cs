using Dapper;
using Serilog;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TruckingSharp.Database.Entities;

namespace TruckingSharp.Database.Repositories
{
    public sealed class SpeedCameraRepository
    {
        private readonly IDatabaseConnection _databaseConnectionFactory;

        public SpeedCameraRepository(IDatabaseConnection databaseConnectionFactory) => _databaseConnectionFactory = databaseConnectionFactory;

        #region Async

        public async Task<long> AddAsync(SpeedCamera entity)
        {
            try
            {
                const string command = "INSERT INTO speedcameras (id, position_x, position_y, position_z, angle, speed) VALUES (@Id, @PositionX, @PositionY, @PositionZ, @Angle, @Speed);";

                using (var sqlConnection = await _databaseConnectionFactory.CreateConnectionAsync())
                {
                    return await sqlConnection.ExecuteAsync(command, new
                    {
                        entity.Id,
                        entity.PositionX,
                        entity.PositionY,
                        entity.PositionZ,
                        entity.Angle,
                        entity.Speed
                    });
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex, $"Failed to insert async speed camera with id: {entity.Id}.");
                throw;
            }
        }

        public async Task<int> DeleteAsync(SpeedCamera entity)
        {
            try
            {
                const string command = "DELETE FROM speedcameras WHERE id = @Id;";

                using (var sqlConnection = await _databaseConnectionFactory.CreateConnectionAsync())
                {
                    return await sqlConnection.ExecuteAsync(command, new
                    {
                        entity.Id
                    });
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex, $"Failed to delete async speed camera with id: {entity.Id}.");
                throw;
            }
        }

        public async Task<SpeedCamera> FindAsync(int id)
        {
            try
            {
                const string command = "SELECT * FROM speedcameras WHERE id = @Id;";

                using (var sqlConnection = await _databaseConnectionFactory.CreateConnectionAsync())
                {
                    return await sqlConnection.QueryFirstOrDefaultAsync<SpeedCamera>(command, new
                    {
                        Id = id
                    });
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex, $"Failed to fetch async speed camera with id: {id}.");
                throw;
            }
        }

        public async Task<IEnumerable<SpeedCamera>> GetAllAsync()
        {
            try
            {
                const string command = "SELECT * FROM speedcameras;";

                using (var sqlConnection = await _databaseConnectionFactory.CreateConnectionAsync())
                {
                    return await sqlConnection.QueryAsync<SpeedCamera>(command);
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Failed to fetch all speed cameras async.");
                throw;
            }
        }

        public async Task<int> UpdateAsync(SpeedCamera entity)
        {
            try
            {
                const string command = "UPDATE speedcameras SET id = @Id, position_x = @PositionX, position_y = @PositionY, position_z = @PositionZ, angle = @Angle, speed = @Speed WHERE id = @Id1;";

                using (var sqlConnection = await _databaseConnectionFactory.CreateConnectionAsync())
                {
                    return await sqlConnection.ExecuteAsync(command, new
                    {
                        entity.Id,
                        entity.PositionX,
                        entity.PositionY,
                        entity.PositionZ,
                        entity.Angle,
                        entity.Speed,
                        Id1 = entity.Id
                    });
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex, $"Failed to update async speed camera with id: {entity.Id}.");
                throw;
            }
        }

        #endregion Async
    }
}