using SampSharp.GameMode;
using SampSharp.GameMode.Controllers;
using SampSharp.GameMode.Definitions;
using SampSharp.GameMode.Display;
using SampSharp.GameMode.SAMP;
using System;
using TruckingSharp.Constants;
using TruckingSharp.Missions.Trucker;
using TruckingSharp.PlayerClasses.Data;

namespace TruckingSharp.Missions
{
    [Controller]
    public class MissionsController : IController, IEventListener
    {
        private Timer _missionTimer;

        public static void ClassEndMission(Player player)
        {
            switch (player.PlayerClass)
            {
                case PlayerClassType.TruckDriver:
                    TruckerController.EndMission(player);
                    break;
            }
        }

        public void RegisterEvents(BaseMode gameMode)
        {
            gameMode.PlayerDied += Mission_PlayerDied;
            gameMode.PlayerConnected += Mission_PlayerConnected;
            gameMode.PlayerDisconnected += Mission_PlayerDisconnected;
            gameMode.Initialized += Mission_GamemodeInitialized;
            gameMode.Exited += Mission_GamemodeExited;
            gameMode.PlayerSpawned += Mission_PlayerSpawned;
        }

        private void _missionTimer_Tick(object sender, System.EventArgs e)
        {
            foreach (Player player in Player.All)
            {
                if (!player.IsLoggedIn || !player.IsDoingMission)
                    continue;

                var oldVehicle = player.MissionVehicle;
                var oldTrailer = player.MissionTrailer;

                var newVehicle = player.Vehicle;
                var newTrailer = player.Vehicle?.Trailer;

                switch (player.PlayerClass)
                {
                    case PlayerClassType.TruckDriver:
                    case PlayerClassType.RoadWorker:
                        if (player.MissionVehicleTime != 0)
                        {
                            if (oldVehicle == newVehicle && oldTrailer == newTrailer)
                            {
                                player.MissionVehicleTime = Configuration.TimeToFailMission;
                            }
                            else
                            {
                                PlayerLeftVehicle(player);
                            }
                        }
                        else
                        {
                            PlayerFailMission(player);
                        }
                        break;

                    case PlayerClassType.BusDriver:
                    case PlayerClassType.Mafia:
                    case PlayerClassType.Courier:
                        if (player.MissionVehicleTime != 0)
                        {
                            if (oldVehicle == newVehicle)
                            {
                                player.MissionVehicleTime = Configuration.TimeToFailMission;
                            }
                            else
                            {
                                PlayerLeftVehicle(player);
                            }
                        }
                        else
                        {
                            PlayerFailMission(player);
                        }
                        break;
                }
            }
        }

        private void Mission_GamemodeExited(object sender, System.EventArgs e)
        {
            _missionTimer.Dispose();
        }

        private void Mission_GamemodeInitialized(object sender, System.EventArgs e)
        {
            _missionTimer = new Timer(1000, true);
            _missionTimer.Tick += _missionTimer_Tick;
        }

        private void Mission_PlayerConnected(object sender, EventArgs e)
        {
            var player = sender as Player;
            player.MissionTextDraw = new PlayerTextDraw(player, new Vector2(320.0, 430.0), string.Empty)
            {
                Alignment = TextDrawAlignment.Center,
                UseBox = true,
                BoxColor = Color.Black
            };
        }

        private void Mission_PlayerDied(object sender, SampSharp.GameMode.Events.DeathEventArgs e)
        {
            var player = sender as Player;
            ClassEndMission(player);

            player.MissionTextDraw.Text = string.Empty;
            player.MissionTextDraw.Hide();
        }

        private void Mission_PlayerDisconnected(object sender, SampSharp.GameMode.Events.DisconnectEventArgs e)
        {
            var player = sender as Player;
            ClassEndMission(player);

            player.MissionTextDraw.Dispose();
        }

        private void Mission_PlayerSpawned(object sender, SampSharp.GameMode.Events.SpawnEventArgs e)
        {
            var player = sender as Player;

            switch (player.PlayerClass)
            {
                case PlayerClassType.TruckDriver:
                case PlayerClassType.BusDriver:
                case PlayerClassType.Pilot:
                case PlayerClassType.Courier:
                case PlayerClassType.RoadWorker:
                    player.MissionTextDraw.Text = Messages.NoMissionText;
                    break;

                case PlayerClassType.Police:
                    player.MissionTextDraw.Text = Messages.NoMissionTextPolice;
                    break;

                case PlayerClassType.Mafia:
                    player.MissionTextDraw.Text = Messages.NoMissionTextMafia;
                    break;

                case PlayerClassType.Assistance:
                    player.MissionTextDraw.Text = Messages.NoMissionTextAssistance;
                    break;
            }

            player.MissionTextDraw.Show();
        }

        private void PlayerFailMission(Player player)
        {
            ClassEndMission(player);
            // TODO: Remove player from convoy

            var message = string.Format(Messages.MissionFailed, Configuration.PriceFailedMission);
            player.GameText(message, 5000, 4);
            player.Reward(-Configuration.PriceFailedMission);
        }

        private void PlayerLeftVehicle(Player player)
        {
            player.MissionVehicleTime -= 1;
            player.GameText($"{player.MissionVehicleTime}", 1000, 4);

            if (player.MissionVehicleTime == (Configuration.TimeToFailMission - 1))
            {
                switch (player.PlayerClass)
                {
                    case PlayerClassType.TruckDriver:
                        player.SendClientMessage(Color.Red, Messages.MissionTruckerMustEnterVehicle);
                        break;

                    case PlayerClassType.BusDriver:
                        player.SendClientMessage(Color.Red, Messages.MissionBusDriverMustEnterVehicle);
                        break;

                    case PlayerClassType.Mafia:
                        player.SendClientMessage(Color.Red, Messages.MissionMafiaMustEnterVehicle);
                        break;

                    case PlayerClassType.RoadWorker:
                        player.SendClientMessage(Color.Red, Messages.MissionRoadWorkerMustEnterVehicle);
                        break;
                }
            }
        }
    }
}