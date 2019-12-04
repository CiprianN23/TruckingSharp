using Dapper;
using Serilog;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TruckingSharp.Database.Entities;

namespace TruckingSharp.Database.Repositories
{
    public sealed class PlayerBankAccountRepository
    {
        private readonly IDatabaseConnectionFactory _databaseConnectionFactory;

        public PlayerBankAccountRepository(IDatabaseConnectionFactory databaseConnectionFactory) => _databaseConnectionFactory = databaseConnectionFactory;

        public PlayerBankAccount Find(int id)
        {
            try
            {
                const string command = "SELECT * FROM playerbankaccounts WHERE player_id = @Id;";

                using (var sqlConnection = _databaseConnectionFactory.CreateConnection())
                {
                    return sqlConnection.QueryFirstOrDefault<PlayerBankAccount>(command, new { Id = id });
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex, $"Failed to fwtch player bank account with id: {id}.");
                throw;
            }
        }

        #region Async

        public async Task<IEnumerable<PlayerBankAccount>> GetAllAsync()
        {
            try
            {
                const string command = "SELECT * FROM playerbankaccounts;";

                using (var sqlConnection = await _databaseConnectionFactory.CreateConnectionAsync())
                {
                    return await sqlConnection.QueryAsync<PlayerBankAccount>(command);
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Failed to fetch all player bank accounts async.");
                throw;
            }
        }

        public async Task<long> AddAsync(PlayerBankAccount entity)
        {
            try
            {
                const string command = "INSERT INTO playerbankaccounts (password, player_id) VALUES (@Password, @PlayerId);";

                using (var sqlConnection = await _databaseConnectionFactory.CreateConnectionAsync())
                {
                    return await sqlConnection.ExecuteAsync(command, new
                    {
                        entity.Password,
                        entity.PlayerId
                    });
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex, $"Failed to insert async player bank account with player id: {entity.PlayerId}.");
                throw;
            }
        }

        public async Task<int> UpdateAsync(PlayerBankAccount entity)
        {
            try
            {
                const string command = "UPDATE playerbankaccounts SET password = @Password, money = @Money, player_id = @PlayerId WHERE id = @Id;";

                using (var sqlConnection = await _databaseConnectionFactory.CreateConnectionAsync())
                {
                    return await sqlConnection.ExecuteAsync(command, new
                    {
                        entity.Password,
                        entity.Money,
                        entity.PlayerId,
                        entity.Id
                    });
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex, $"Failed to update async player bank account with player id: {entity.PlayerId}.");
                throw;
            }
        }

        public async Task<int> DeleteAsync(PlayerBankAccount entity)
        {
            try
            {
                const string command = "DELETE FROM playerbankaccounts WHERE id = @Id;";

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
                Log.Error(ex, $"Failed to delete async player bank account with player id: {entity.PlayerId}.");
                throw;
            }
        }

        public async Task<PlayerBankAccount> FindAsync(int id)
        {
            try
            {
                const string command = "SELECT * FROM playerbankaccounts WHERE player_id = @Id;";

                using (var sqlConnection = await _databaseConnectionFactory.CreateConnectionAsync())
                {
                    return await sqlConnection.QueryFirstOrDefaultAsync(command, new
                    {
                        Id = id
                    });
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex, $"Failed to fetch async player bank account with player id: {id}.");
                throw;
            }
        }

        #endregion Async
    }
}