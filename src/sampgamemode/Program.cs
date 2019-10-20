using sampgamemode.Database;
using SampSharp.Core;

namespace sampgamemode
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
