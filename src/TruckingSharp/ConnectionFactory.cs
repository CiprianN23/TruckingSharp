using TruckingSharp.Database;

namespace TruckingSharp
{
    public static class ConnectionFactory
    {
        public static IDatabaseConnection GetConnection => new PostgresConnection();
    }
}
