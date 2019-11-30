using SampSharp.GameMode;
using SampSharp.GameMode.Controllers;
using SampSharp.GameMode.Definitions;
using SampSharp.GameMode.Display;
using SampSharp.GameMode.Events;
using SampSharp.GameMode.SAMP;
using System;
using TruckingSharp.Constants;
using TruckingSharp.Missions.BusDriver;
using TruckingSharp.Missions.Convoy;
using TruckingSharp.Missions.Trucker;
using TruckingSharp.PlayerClasses.Data;

namespace TruckingSharp.Missions
{
    [Controller]
    public class MissionsController : IEventListener
    {
        private Timer _missionTimer;

        public void RegisterEvents(BaseMode gameMode)
        {
            gameMode.PlayerDied += Mission_PlayerDied;
            gameMode.PlayerConnected += Mission_PlayerConnected;
            gameMode.PlayerDisconnected += Mission_PlayerDisconnected;
            gameMode.Initialized += Mission_GamemodeInitialized;
            gameMode.Exited += Mission_GamemodeExited;
            gameMode.PlayerSpawned += Mission_PlayerSpawned;
        }

        public static void ClassEndMission(Player player)
        {
            switch (player.PlayerClass)
            {
                case PlayerClassType.TruckDriver:
                    TruckerController.EndMission(player);
                    break;
                case PlayerClassType.BusDriver:
                    BusDriverController.EndMission(player);
                    break;
            }
        }

        private void _missionTimer_Tick(object sender, EventArgs e)
        {
            foreach (var basePlayer in Player.All)
            {
                var player = (Player)basePlayer;

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
                                player.MissionVehicleTime = Configuration.Instance.FailMissionSeconds;
                            else
                                PlayerLeftVehicle(player);
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
                                player.MissionVehicleTime = Configuration.Instance.FailMissionSeconds;
                            else
                                PlayerLeftVehicle(player);
                        }
                        else
                        {
                            PlayerFailMission(player);
                        }

                        break;
                }
            }
        }

        private void Mission_GamemodeExited(object sender, EventArgs e)
        {
            _missionTimer.Dispose();
        }

        private void Mission_GamemodeInitialized(object sender, EventArgs e)
        {
            _missionTimer = new Timer(TimeSpan.FromSeconds(1), true);
            _missionTimer.Tick += _missionTimer_Tick;
        }

        private void Mission_PlayerConnected(object sender, EventArgs e)
        {
            if (!(sender is Player player))
                return;

            player.MissionTextDraw = new PlayerTextDraw(player, new Vector2(320.0, 430.0), string.Empty)
            {
                Alignment = TextDrawAlignment.Center,
                UseBox = true,
                BoxColor = Color.Black
            };
        }

        private void Mission_PlayerDied(object sender, DeathEventArgs e)
        {
            if (!(sender is Player player))
                return;

            ClassEndMission(player);

            player.MissionTextDraw.Text = string.Empty;
            player.MissionTextDraw.Hide();
        }

        private void Mission_PlayerDisconnected(object sender, DisconnectEventArgs e)
        {
            if (!(sender is Player player))
                return;

            ClassEndMission(player);

            player.MissionTextDraw.Dispose();
        }

        private void Mission_PlayerSpawned(object sender, SpawnEventArgs e)
        {
            if (!(sender is Player player))
                return;

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
            MissionConvoy.PlayerLeaveConvoy(player);

            var message = string.Format(Messages.MissionFailed, Configuration.Instance.FailedMissionPrice);
            player.GameText(message, 5000, 4);
            player.Reward(-Configuration.Instance.FailedMissionPrice);
        }

        private void PlayerLeftVehicle(Player player)
        {
            player.MissionVehicleTime -= 1;
            player.GameText($"{player.MissionVehicleTime}", 1000, 4);

            if (player.MissionVehicleTime != Configuration.Instance.FailMissionSeconds - 1)
                return;

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