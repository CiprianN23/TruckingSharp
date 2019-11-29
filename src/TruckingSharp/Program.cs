using SampSharp.Core;
using Serilog;
using TruckingSharp.Database;

namespace TruckingSharp
{
    internal class Program
    {
        private static void Main()
        {
            new GameModeBuilder()
                .Use<GameMode>()
                .Run();

            DapperConnection.SetupConnectionString();

            Log.Logger = new LoggerConfiguration()
            .MinimumLevel.Debug()
            .WriteTo.File("logs\\my_log.log", rollingInterval: RollingInterval.Day)
            .CreateLogger();

            Configuration.LoadConfigurationFromFile();
        }
    }
}