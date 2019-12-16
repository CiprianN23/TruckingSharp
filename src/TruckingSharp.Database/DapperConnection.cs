using System;

namespace TruckingSharp.Database
{
    public static class DapperConnection
    {
        public static string ConnectionString { get; private set; }

        public static void LoadConnectionStringFromSystem() => ConnectionString = Environment.GetEnvironmentVariable("ConnectionString");
    }
}