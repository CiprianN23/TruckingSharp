using MySql.Data.MySqlClient;
using TruckingSharp.Database;

namespace TruckingSharp
{
    public static class ConnectionFactory
    {
        public static MySqlConnection GetConnection => new MySqlConnection(DapperConnection.ConnectionString);
    }
}