using SampSharp.Core;
using Serilog;

namespace TruckingSharp
{
    internal class Program
    {
        private static void Main()
        {
            new GameModeBuilder()
                .Use<GameMode>()
                .Run();

            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Debug()
                .WriteTo.File("logs\\my_log.log", rollingInterval: RollingInterval.Day)
                .CreateLogger();

            Configuration.LoadConfigurationFromFileAsync();
        }
    }
}