using TruckingSharp.Database;
using SampSharp.Core;

namespace TruckingSharp
{
    class Program
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
