using SampSharp.GameMode.Definitions;
using SampSharp.GameMode.World;

namespace TruckingSharp.Extensions.PlayersExtensions
{
    public static class PlayerCheckExtensions
    {
        public static bool IsPlayerInBuilding(this BasePlayer player)
        {
            return player.Interior != 0;
        }

        public static bool IsPlayerDriving(this BasePlayer player)
        {
            return player.State == PlayerState.Driving;
        }
    }
}