using SampSharp.Core;
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
        }
    }
}