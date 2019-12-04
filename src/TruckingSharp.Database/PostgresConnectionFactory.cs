using Npgsql;
using System.Data;
using System.Threading.Tasks;

namespace TruckingSharp.Database
{
    public class PostgresConnectionFactory : IDatabaseConnectionFactory
    {
        private readonly string _connectionString;

        public PostgresConnectionFactory() => _connectionString = DapperConnection.ConnectionString;

        public async Task<IDbConnection> CreateConnectionAsync()
        {
            var sqlConnection = new NpgsqlConnection(_connectionString);
            Dapper.DefaultTypeMap.MatchNamesWithUnderscores = true;
            await sqlConnection.OpenAsync();
            return sqlConnection;
        }

        public IDbConnection CreateConnection()
        {
            var sqlConnection = new NpgsqlConnection(_connectionString);
            Dapper.DefaultTypeMap.MatchNamesWithUnderscores = true;
            sqlConnection.Open();
            return sqlConnection;
        }
    }
}