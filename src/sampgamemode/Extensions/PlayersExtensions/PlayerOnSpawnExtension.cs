using sampgamemode.Data;
using sampgamemode.World;
using SampSharp.GameMode.Events;

namespace sampgamemode.Extensions.PlayersExtensions
{
    public static class PlayerOnSpawnExtension
    {
        public static void SetPlayerClassColor(this Player player, SpawnEventArgs e)
        {
            switch (player.PlayerClass)
            {
                case PlayerClasses.TruckDriver:
                    player.Color = PlayerClassesColor.TruckerColor;
                    break;
                case PlayerClasses.BusDriver:
                    player.Color = PlayerClassesColor.BusDriverColor;
                    break;
                case PlayerClasses.Pilot:
                    player.Color = PlayerClassesColor.PilotColor;
                    break;
                case PlayerClasses.Police:
                    player.Color = PlayerClassesColor.PoliceColor;
                    break;
                case PlayerClasses.Mafia:
                    player.Color = PlayerClassesColor.MafiaColor;
                    break;
                case PlayerClasses.Courier:
                    player.Color = PlayerClassesColor.CourierColor;
                    break;
                case PlayerClasses.Assistance:
                    player.Color = PlayerClassesColor.AssistanceColor;
                    break;
                case PlayerClasses.RoadWorker:
                    player.Color = PlayerClassesColor.RoadWorkerColor;
                    break;
            }
        }
    }
}
