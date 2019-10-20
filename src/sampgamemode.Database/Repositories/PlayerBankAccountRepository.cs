using System.Collections.Generic;
using Dapper;
using Dapper.Contrib.Extensions;
using MySql.Data.MySqlClient;
using sampgamemode.Database.Entities;
using sampgamemode.Database.Repositories.Interfaces;

namespace sampgamemode.Database.Repositories
{
    public class PlayerBankAccountRepository : IRepository<PlayerBankAccount>
    {
        private MySqlTransaction _transaction;
        private MySqlConnection Connection => _transaction.Connection;

        public PlayerBankAccountRepository(MySqlTransaction transaction)
        {
            _transaction = transaction;
        }

        public IEnumerable<PlayerBankAccount> GetAll()
        {
            return Connection.Query<PlayerBankAccount>("SELECT * FROM bankaccounts;", transaction: _transaction);
        }

        public PlayerBankAccount Find(int id)
        {
            return Connection.QueryFirstOrDefault<PlayerBankAccount>("SELECT * FROM bankaccounts WHERE PlayerId = @Id;", new { Id = id }, transaction: _transaction);
        }

        public PlayerBankAccount Find(string name)
        {
            throw new System.NotImplementedException();
        }

        public long Add(PlayerBankAccount entity)
        {
            return Connection.Insert(entity, transaction: _transaction);
        }

        public bool Update(PlayerBankAccount entity)
        {
            return Connection.Update(entity, transaction: _transaction);
        }

        public bool Delete(PlayerBankAccount entity)
        {
            return Connection.Delete(entity, transaction: _transaction);
        }

        public int GetPlayerId(string name)
        {
            return Connection.QueryFirstOrDefault<int>("SELECT Id FROM accounts WHERE Name = @Name;", new { Name = name }, transaction: _transaction);
        }
    }
}
