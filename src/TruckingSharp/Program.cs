using SampSharp.Core;
using Serilog;
using System.Threading.Tasks;
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

            Task.Run(() => Configuration.LoadConfigurationFromFileAsync());

            DapperConnection.LoadConnectionStringFromSystem();

            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Debug()
                .WriteTo.File("logs\\my_log.log", rollingInterval: RollingInterval.Day)
                .CreateLogger();
        }
    }
}