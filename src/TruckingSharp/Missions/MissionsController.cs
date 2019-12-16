using SampSharp.GameMode;
using SampSharp.GameMode.Controllers;
using SampSharp.GameMode.Definitions;
using SampSharp.GameMode.Display;
using SampSharp.GameMode.Events;
using SampSharp.GameMode.SAMP;
using System;
using System.Threading.Tasks;
using TruckingSharp.Constants;
using TruckingSharp.Missions.Assistance;
using TruckingSharp.Missions.BusDriver;
using TruckingSharp.Missions.Convoy;
using TruckingSharp.Missions.Data;
using TruckingSharp.Missions.Mafia;
using TruckingSharp.Missions.Pilot;
using TruckingSharp.Missions.Police;
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

        public static async Task ClassEndMissionAsync(Player player)
        {
            switch (player.PlayerClass)
            {
                case PlayerClassType.TruckDriver:
                    await TruckerController.EndMissionAsync(player);
                    break;

                case PlayerClassType.BusDriver:
                    BusDriverController.EndMission(player);
                    break;

                case PlayerClassType.Pilot:
                    PilotController.EndMission(player);
                    break;

                case PlayerClassType.Police:
                    PoliceController.EndMission(player);
                    break;

                case PlayerClassType.Mafia:
                    await MafiaController.EndMissionAsync(player);
                    break;

                case PlayerClassType.Assistance:
                    AssistanceController.EndMission(player);
                    break;
            }
        }

        private async void _missionTimer_Tick(object sender, EventArgs e)
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
                            await PlayerFailMissionAsync(player);
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
                            await PlayerFailMissionAsync(player);
                        }

                        break;
                }
            }
        }

        private void Mission_GamemodeExited(object sender, EventArgs e)
        {
            _missionTimer?.Dispose();
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

        private async void Mission_PlayerDied(object sender, DeathEventArgs e)
        {
            if (!(sender is Player player))
                return;

            await ClassEndMissionAsync(player);

            player.MissionTextDraw.Text = string.Empty;
            player.MissionTextDraw.Hide();
        }

        private async void Mission_PlayerDisconnected(object sender, DisconnectEventArgs e)
        {
            if (!(sender is Player player))
                return;

            await ClassEndMissionAsync(player);

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

        private async Task PlayerFailMissionAsync(Player player)
        {
            await ClassEndMissionAsync(player);
            await MissionConvoy.PlayerLeaveConvoyAsync(player);

            var message = string.Format(Messages.MissionFailed, Configuration.Instance.FailedMissionPrice);
            player.GameText(message, 5000, 4);
            await player.RewardAsync(-Configuration.Instance.FailedMissionPrice);
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

        public static int CalculatePayment(MissionLocation fromLocation, MissionLocation toLocation,
            MissionCargo missionCargo)
        {
            var distance = GetDistance(fromLocation, toLocation);
            return (int)Math.Floor(distance * missionCargo.PayPerUnit);
        }

        public static double GetDistance(MissionLocation fromLocation, MissionLocation toLocation)
        {
            return Math.Sqrt(Math.Pow(toLocation.Position.X - fromLocation.Position.X, 2) +
                             Math.Pow(toLocation.Position.Y - fromLocation.Position.Y, 2));
        }

        public static bool CheckDistanceBetweenLocations(MissionLocation location1, MissionLocation location2, float range)
        {
            return Vector3.Distance(location1.Position, location2.Position) > range;
        }
    }
}