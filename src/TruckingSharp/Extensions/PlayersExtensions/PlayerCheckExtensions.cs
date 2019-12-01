using SampSharp.GameMode.Definitions;
using SampSharp.GameMode.World;

namespace TruckingSharp.Extensions.PlayersExtensions
{
    public static class PlayerCheckExtensions
    {
        public static bool IsInBuilding(this BasePlayer player) => player.Interior != 0;

        public static bool IsDriving(this BasePlayer player) => player.State == PlayerState.Driving;
    }
}