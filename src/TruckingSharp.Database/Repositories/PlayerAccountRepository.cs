using Dapper;
using Serilog;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TruckingSharp.Database.Entities;

namespace TruckingSharp.Database.Repositories
{
    public sealed class PlayerAccountRepository
    {
        private readonly IDatabaseConnection _databaseConnectionFactory;

        public PlayerAccountRepository(IDatabaseConnection databaseConnectionFactory) => _databaseConnectionFactory = databaseConnectionFactory;

        public PlayerAccount Find(string name)
        {
            try
            {
                const string command = "SELECT * FROM player_accounts WHERE name = @Name;";

                using var sqlConnection = _databaseConnectionFactory.CreateConnection();

                return sqlConnection.QueryFirstOrDefault<PlayerAccount>(command, new { Name = name });
            }
            catch (Exception ex)
            {
                Log.Error(ex, $"Failed to fetch player account with name: {name}.");
                throw;
            }
        }

        #region Async

        public async Task<PlayerAccount> FindAsync(int id)
        {
            try
            {
                const string command = "SELECT * FROM player_accounts WHERE id = @Id;";

                using var sqlConnection = await _databaseConnectionFactory.CreateConnectionAsync();

                return await sqlConnection.QueryFirstOrDefaultAsync(command, new
                {
                    Id = id
                });
            }
            catch (Exception ex)
            {
                Log.Error(ex, $"Failed to fetch async player account with id: {id}.");
                throw;
            }
        }

        public async Task<PlayerAccount> FindAsync(string name)
        {
            try
            {
                const string command = "SELECT * FROM player_accounts WHERE name = @Name;";

                using var sqlConnection = await _databaseConnectionFactory.CreateConnectionAsync();

                return await sqlConnection.QueryFirstOrDefaultAsync(command, new { Name = name });
            }
            catch (Exception ex)
            {
                Log.Error(ex, $"Failed to fetch async player account with name: {name}.");
                throw;
            }
        }

        public async Task<int> UpdateAsync(PlayerAccount entity)
        {
            try
            {
                const string command = "UPDATE player_accounts " +
                    "SET name = @Name, password = @Password, money = @Money, score = @Score, admin_level = @AdminLevel, rules_read = @RulesRead, muted = @Muted, jailed = @Jailed," +
                    " wanted = @Wanted, bans = @Bans, trucker_license = @TruckerLicense, bus_license = @BusLicense, meters_driven = @MetersDriven, trucker_jobs = @TruckerJobs," +
                    " convoy_jobs = @ConvoyJobs, busdriver_jobs = @BusDriverJobs," +
                    " pilot_jobs = @PilotJobs, mafia_jobs = @MafiaJobs, mafia_stolen = @MafiaStolen, police_fined = @PoliceFined, police_jailed = @PoliceJailed, assistance_jobs = @AssistanceJobs WHERE id = @Id;";

                using var sqlConnection = await _databaseConnectionFactory.CreateConnectionAsync();

                return await sqlConnection.ExecuteAsync(command, new
                {
                    entity.Name,
                    entity.Password,
                    entity.Money,
                    entity.Score,
                    entity.AdminLevel,
                    entity.RulesRead,
                    entity.Muted,
                    entity.Jailed,
                    entity.Wanted,
                    entity.Bans,
                    entity.TruckerLicense,
                    entity.BusLicense,
                    entity.MetersDriven,
                    entity.TruckerJobs,
                    entity.ConvoyJobs,
                    entity.BusDriverJobs,
                    entity.PilotJobs,
                    entity.MafiaJobs,
                    entity.MafiaStolen,
                    entity.PoliceFined,
                    entity.PoliceJailed,
                    entity.AssistanceJobs,
                    entity.Id
                });
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
                const string command = "SELECT * FROM player_accounts;";

                using var sqlConnection = await _databaseConnectionFactory.CreateConnectionAsync();

                return await sqlConnection.QueryAsync<PlayerAccount>(command);
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Failed to fetch all player accounts from database async.");
                throw;
            }
        }

        public async Task<long> AddAsync(PlayerAccount entity)
        {
            try
            {
                const string command = "INSERT INTO player_accounts (name, password) VALUES (@Name, @Password);";

                using var sqlConnection = await _databaseConnectionFactory.CreateConnectionAsync();

                return await sqlConnection.ExecuteAsync(command, new
                {
                    entity.Name,
                    entity.Password
                });
            }
            catch (Exception ex)
            {
                Log.Error(ex, $"Failed to insert async player account with name: {entity.Name}.");
                throw;
            }
        }

        public async Task<int> DeleteAsync(PlayerAccount entity)
        {
            try
            {
                const string command = "DELETE FROM player_accounts WHERE id = @Id;";

                using var sqlConnection = await _databaseConnectionFactory.CreateConnectionAsync();

                return await sqlConnection.ExecuteAsync(command, new
                {
                    entity.Id
                });
            }
            catch (Exception ex)
            {
                Log.Error(ex, $"Failed to delete async player account with name: {entity.Name}.");
                throw;
            }
        }

        #endregion Async
    }
}