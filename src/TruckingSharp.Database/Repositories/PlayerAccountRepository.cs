using Dapper;
using Dapper.Contrib.Extensions;
using MySql.Data.MySqlClient;
using System.Collections.Generic;
using System.Threading.Tasks;
using TruckingSharp.Database.Entities;
using TruckingSharp.Database.Repositories.Interfaces;

namespace TruckingSharp.Database.Repositories
{
    public class PlayerAccountRepository : IPlayerAccountRepository
    {
        private MySqlTransaction _transaction;
        private MySqlConnection Connection => _transaction.Connection;

        public PlayerAccountRepository(MySqlTransaction transaction)
        {
            _transaction = transaction;
        }

        public long Add(PlayerAccount entity)
        {
            return Connection.Insert(entity, transaction: _transaction);
        }

        public bool Delete(PlayerAccount entity)
        {
            return Connection.Delete(entity, transaction: _transaction);
        }

        public IEnumerable<PlayerAccount> GetAll()
        {
            return Connection.Query<PlayerAccount>("SELECT * FROM accounts;", transaction: _transaction);
        }

        public IEnumerable<PlayerAccount> GetBestPlayers(int amountOfPlayers)
        {
            throw new System.NotImplementedException();
        }

        public PlayerAccount Find(int id)
        {
            return Connection.QueryFirstOrDefault<PlayerAccount>("SELECT * FROM accounts WHERE Id = @Id;", new { Id = id }, transaction: _transaction);
        }

        public PlayerAccount Find(string name)
        {
            return Connection.QueryFirstOrDefault<PlayerAccount>("SELECT * FROM accounts WHERE Name = @Name;", new { Name = name }, transaction: _transaction);
        }

        public bool Update(PlayerAccount entity)
        {
            return Connection.Update(entity, transaction: _transaction);
        }

        public async Task<bool> UpdateAsync(PlayerAccount entity)
        {
            return await Connection.UpdateAsync(entity, transaction: _transaction).ConfigureAwait(false);
        }
    }
}