using SampSharp.GameMode;
using SampSharp.GameMode.Controllers;
using SampSharp.GameMode.Display;
using SampSharp.GameMode.Events;
using SampSharp.GameMode.SAMP;
using SampSharp.GameMode.World;
using System;
using TruckingSharp.Constants;
using TruckingSharp.Database.Repositories;

namespace TruckingSharp.Missions.BusDriver
{
    [Controller]
    public class BusDriverController : IEventListener
    {
        private static PlayerAccountRepository AccountRepository => new PlayerAccountRepository(ConnectionFactory.GetConnection);

        public void RegisterEvents(BaseMode gameMode)
        {
            gameMode.PlayerEnterRaceCheckpoint += BusDriver_PlayerEnterRaceCheckpoint;
        }

        private void BusDriver_PlayerEnterRaceCheckpoint(object sender, EventArgs e)
        {
            if (!(sender is Player player))
                return;

            if (player.PlayerClass != PlayerClasses.Data.PlayerClassType.BusDriver)
                return;

            if (player.Vehicle != player.MissionVehicle)
                return;

            var random = new Random();

            player.ToggleControllable(false);

            var passangersInBusStop = random.Next(20) + 10;

            player.MissionTextDraw.Text = $"~w~Line ~y~{player.BusRoute.LineNumber}~w~ (~g~{player.BusRoute.Description}~w~): ~b~{player.BusPassengers}~w~ on bus, ~b~{passangersInBusStop}~w~ in busstop.";

            player.MissionLoadingTimer = new Timer(TimeSpan.FromSeconds(5), false);
            player.MissionLoadingTimer.Tick += (senderObject, ev) => BusDriverLoadUnload_Tick(senderObject, ev, player, passangersInBusStop);
        }

        private async void BusDriverLoadUnload_Tick(object sender, EventArgs e, Player player, int passangersInBusStop)
        {
            var passangersGettingOffTheBus = 0;
            var random = new Random();

            player.DisableCheckpoint();
            player.DisableRaceCheckpoint();

            player.MissionStep++;

            if (player.BusPassengers > 0)
                passangersGettingOffTheBus = random.Next(player.BusPassengers);

            player.BusPassengers -= passangersGettingOffTheBus;
            player.BusPassengers += passangersInBusStop;

            var nextStep = player.MissionStep;
            var nextLocationIndex = player.BusRoute.Locations[nextStep];

            if (nextLocationIndex == -1)
            {
                BasePlayer.SendClientMessageToAll(Color.White, $"BusDriver {{FF00FF}}{player.Name}{{FFFFFF}} succesfully completed bus-line {{0000FF}}{player.BusRoute.LineNumber}{{FFFFFF}}.");

                nextStep = 1;
                player.MissionStep = nextStep;
                nextLocationIndex = player.BusRoute.Locations[nextStep];

                var depot = BusRoute.GetLocation(player.BusRoute.HomeDepot);
                player.SetCheckpoint(depot.Position, 7.0f);
                await player.RewardAsync(0, player.BusRoute.Score);

                var playerAccount = player.Account;
                playerAccount.BusDriverJobs++;
                await AccountRepository.UpdateAsync(playerAccount);
            }

            player.MissionTextDraw.Text = $"~w~Line ~y~{player.BusRoute.LineNumber}~w~ (~g~{player.BusRoute.Description}~w~): ~b~{player.BusPassengers}~w~ on bus.";

            var nextLocation = BusRoute.GetLocation(nextLocationIndex);
            player.SetRaceCheckpoint(SampSharp.GameMode.Definitions.CheckpointType.Nothing, nextLocation.Position, Vector3.Zero, 7.0f);

            player.ToggleControllable(true);

            if (passangersGettingOffTheBus != 0)
            {
                var payment = passangersGettingOffTheBus * 9;
                await player.RewardAsync(payment);
                player.GameText($"~g~You've earned ${payment}~w~", 3000, 4);
            }
        }

        public static void StartMission(Player player, BusRoute busRoute)
        {
            player.IsDoingMission = true;
            player.MissionVehicle = (Vehicle)player.Vehicle;
            player.BusRoute = busRoute;
            player.MissionStep = 0;

            player.BusPassengers = 0;
            player.MissionTextDraw.Text = $"~w~Line ~y~{busRoute.LineNumber}~w~ (~g~{busRoute.Description}~w~): ~b~{player.BusPassengers}~w~ on bus.";

            var startLocation = BusRoute.GetLocation(busRoute.Locations[0]);
            player.SetRaceCheckpoint(SampSharp.GameMode.Definitions.CheckpointType.Nothing, startLocation.Position, Vector3.Zero, 7.0f);
            player.MissionVehicleTime = Configuration.Instance.FailMissionSeconds;
        }

        public static void EndMission(Player player)
        {
            if (!player.IsDoingMission)
                return;

            player.IsDoingMission = false;
            player.MissionStep = 0;
            player.BusRoute = null;
            player.BusPassengers = 0;
            player.MissionVehicleTime = 0;
            player.MissionVehicle = null;

            player.DisableCheckpoint();
            player.DisableRaceCheckpoint();
            player.MissionTextDraw.Text = Messages.NoMissionText;
        }

        public static void DialogBusDriverMission_Response(object sender, DialogResponseEventArgs e)
        {
            if (e.DialogButton != SampSharp.GameMode.Definitions.DialogButton.Left)
                return;

            if (!(e.Player is Player player))
                return;

            if (player.IsDoingMission)
            {
                player.SendClientMessage(Color.Red, Messages.AlreadyDoingAMission);
                return;
            }

            var selectRouteDialog = new ListDialog("Select busroute: ", Messages.DialogButtonSelect, Messages.DialogButtonCancel);

            switch (e.ListItem)
            {
                case 0:
                    foreach (var busRoute in BusRoute.BusRoutes)
                    {
                        selectRouteDialog.AddItem($"Line {busRoute.LineNumber} ({busRoute.Description})\n");
                    }

                    selectRouteDialog.Show(player);
                    selectRouteDialog.Response += SelectRouteDialog_Response;
                    break;

                case 1:
                    var random = new Random();

                    StartMission(player, BusRoute.BusRoutes[random.Next(BusRoute.BusRoutes.Count)]);
                    break;
            }
        }

        private static void SelectRouteDialog_Response(object sender, DialogResponseEventArgs e)
        {
            if (e.DialogButton != SampSharp.GameMode.Definitions.DialogButton.Left)
                return;

            if (!(e.Player is Player player))
                return;

            StartMission(player, BusRoute.BusRoutes[e.ListItem]);
        }
    }
}