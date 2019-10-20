using sampgamemode.Constants;
using sampgamemode.Data;
using sampgamemode.World;
using SampSharp.GameMode;
using SampSharp.GameMode.Events;

namespace sampgamemode.Extensions.PlayersExtensions
{
    public static class PlayerRequestClassExtension
    {
        public static void SetClassSelection(this Player player, RequestClassEventArgs e)
        {
            player.Interior = 14;
            player.Position = new Vector3(258.4893, -41.4008, 1002.0234);
            player.Angle = 270.0f;
            player.CameraPosition = new Vector3(256.0815, -43.0475, 1004.0234);
            player.SetCameraLookAt(new Vector3(258.4893, -41.4008, 1002.0234));

            switch (e.ClassId)
            {
                case 0:
                case 1:
                case 2:
                case 3:
                case 4:
                case 5:
                case 6:
                case 7:
                    player.GameText(Messages.TruckerClass, 3000, 4);
                    player.PlayerClass = PlayerClasses.TruckDriver;
                    break;
                case 8:
                case 9:
                    player.GameText(Messages.BusDriverClass, 3000, 4);
                    player.PlayerClass = PlayerClasses.BusDriver;
                    break;
                case 10:
                    player.GameText(Messages.PilotClass, 3000, 4);
                    player.PlayerClass = PlayerClasses.Pilot;
                    break;
                case 11:
                case 12:
                case 13:
                    player.GameText(Messages.PoliceClass, 3000, 4);
                    player.PlayerClass = PlayerClasses.Police;
                    break;
                case 14:
                case 15:
                case 16:
                    player.GameText(Messages.MafiaClass, 3000, 4);
                    player.PlayerClass = PlayerClasses.Mafia;
                    break;
                case 17:
                case 18:
                    player.GameText(Messages.CourierClass, 3000, 4);
                    player.PlayerClass = PlayerClasses.Courier;
                    break;
                case 19:
                    player.GameText(Messages.AssistanceClass, 3000, 4);
                    player.PlayerClass = PlayerClasses.Assistance;
                    break;
                case 20:
                case 21:
                case 22:
                    player.GameText(Messages.RoadWorkerClass, 3000, 4);
                    player.PlayerClass = PlayerClasses.RoadWorker;
                    break;
            }
        }
    }
}
