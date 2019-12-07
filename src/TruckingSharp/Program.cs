using SampSharp.Core;
using Serilog;
using System.Threading.Tasks;

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

            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Debug()
                .WriteTo.File("logs\\my_log.log", rollingInterval: RollingInterval.Day)
                .CreateLogger();
        }
    }
}