using System;

namespace TruckingSharp.Database
{
    public static class DapperConnection
    {
        public static string ConnectionString { get; private set; }

        public static void SetupConnectionString()
        {
            ConnectionString = Environment.GetEnvironmentVariable("connString");
        }
    }
}
