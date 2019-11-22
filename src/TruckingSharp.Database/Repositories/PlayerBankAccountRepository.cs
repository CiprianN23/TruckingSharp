using Dapper;
using Dapper.Contrib.Extensions;
using MySql.Data.MySqlClient;
using System.Collections.Generic;
using System.Threading.Tasks;
using TruckingSharp.Database.Entities;
using TruckingSharp.Database.Repositories.Interfaces;

namespace TruckingSharp.Database.Repositories
{
    public class PlayerBankAccountRepository : IRepository<PlayerBankAccount>
    {
        private MySqlConnection _connection;

        public PlayerBankAccountRepository(MySqlConnection connection)
        {
            _connection = connection;
        }

        #region Sync

        public IEnumerable<PlayerBankAccount> GetAll()
        {
            return _connection.GetAll<PlayerBankAccount>();
        }

        public PlayerBankAccount Find(int id)
        {
            return _connection.QueryFirstOrDefault<PlayerBankAccount>("SELECT * FROM bankaccounts WHERE PlayerId = @Id;", new { Id = id });
        }

        public long Add(PlayerBankAccount entity)
        {
            return _connection.Insert(entity);
        }

        public bool Update(PlayerBankAccount entity)
        {
            return _connection.Update(entity);
        }

        public bool Delete(PlayerBankAccount entity)
        {
            return _connection.Delete(entity);
        }

        #endregion Sync

        #region Async

        public async Task<IEnumerable<PlayerBankAccount>> GetAllAsync()
        {
            return await _connection.GetAllAsync<PlayerBankAccount>();
        }

        public async Task<long> AddAsync(PlayerBankAccount entity)
        {
            return await _connection.InsertAsync(entity);
        }

        public async Task<bool> UpdateAsync(PlayerBankAccount entity)
        {
            return await _connection.UpdateAsync(entity);
        }

        public async Task<bool> DeleteAsync(PlayerBankAccount entity)
        {
            return await _connection.DeleteAsync(entity);
        }

        #endregion Async
    }
}