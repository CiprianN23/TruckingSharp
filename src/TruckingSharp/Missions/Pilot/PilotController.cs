using SampSharp.GameMode;
using SampSharp.GameMode.Controllers;
using SampSharp.GameMode.Definitions;
using SampSharp.GameMode.SAMP;
using SampSharp.GameMode.World;
using System;
using TruckingSharp.Constants;
using TruckingSharp.Database;
using TruckingSharp.Database.Repositories;
using TruckingSharp.Extensions.PlayersExtensions;
using TruckingSharp.Missions.Data;

namespace TruckingSharp.Missions.Pilot
{
    [Controller]
    public class PilotController : IEventListener
    {
        private PlayerBankAccountRepository6 AccountRepository => new PlayerBankAccountRepository6(ConnectionFactory.GetConnection);

        public static void EndMission(Player player)
        {
            if (!player.IsDoingMission)
                return;

            player.MissionLoadingTimer?.Dispose();

            player.IsDoingMission = false;
            player.MissionStep = 0;
            player.MissionCargo = null;
            player.FromLocation = null;
            player.ToLocation = null;

            player.DisableCheckpoint();
            player.MissionTextDraw.Text = Messages.NoMissionText;
        }

        public static void StartRandomMission(Player player)
        {
            bool isMissionSetSucessfully = SetRandomMission(player);

            if (!isMissionSetSucessfully)
                return;

            player.IsDoingMission = true;
            player.MissionStep = 1;

            player.MissionTextDraw.Text = $"~w~Transporting ~b~{player.MissionCargo.Name}~w~ from ~r~{player.FromLocation.Name}~w~ to {player.ToLocation.Name}.";

            player.SetCheckpoint(player.FromLocation.Position, 7.0f);
            player.SendClientMessage(Color.GreenYellow, $"Pickup the {player.MissionCargo.Name} at {player.FromLocation.Name}.");
        }

        public void RegisterEvents(BaseMode gameMode)
        {
            gameMode.PlayerEnterCheckpoint += Pilot_PlayerEnterCheckpoint;
            gameMode.PlayerExitVehicle += Pilot_PlayerExitVehicle;
        }

        private static bool SetRandomMission(Player player)
        {
            if (!player.IsDriving())
                return false;

            switch (player.Vehicle.Model)
            {
                case VehicleModelType.Nevada:
                case VehicleModelType.Shamal:
                    player.MissionCargo = MissionCargo.GetRandomCargo(MissionCargoVehicleType.Plane);
                    player.FromLocation = MissionCargo.GetRandomStartLocation(player.MissionCargo);
                    player.ToLocation = MissionCargo.GetRandomEndLocation(player.MissionCargo);
                    player.MissionVehicle = (Vehicle)player.Vehicle;

                    while (!MissionsController.CheckDistanceBetweenLocations(player.ToLocation, player.FromLocation, 1000.0f))
                    {
                        player.ToLocation = MissionCargo.GetRandomEndLocation(player.MissionCargo);
                    }

                    return true;

                case VehicleModelType.Maverick:
                case VehicleModelType.Cargobob:
                    player.MissionCargo = MissionCargo.GetRandomCargo(MissionCargoVehicleType.Helicopter);
                    player.FromLocation = MissionCargo.GetRandomStartLocation(player.MissionCargo);
                    player.ToLocation = MissionCargo.GetRandomEndLocation(player.MissionCargo);
                    player.MissionVehicle = (Vehicle)player.Vehicle;

                    while (!MissionsController.CheckDistanceBetweenLocations(player.ToLocation, player.FromLocation, 1000.0f))
                    {
                        player.ToLocation = MissionCargo.GetRandomEndLocation(player.MissionCargo);
                    }

                    return true;
            }

            return false;
        }

        private async void MissionLoadingTimer_Tick(object sender, EventArgs e, Player player)
        {
            switch (player.MissionStep)
            {
                case 1:
                    player.MissionStep = 2;
                    player.DisableCheckpoint();

                    player.MissionTextDraw.Text = $"~w~Transporting ~b~{player.MissionCargo.Name}~w~ from {player.FromLocation.Name} to ~r~{player.ToLocation.Name}~w~";

                    player.SetCheckpoint(player.ToLocation.Position, 7.0f);
                    player.SendClientMessage(Color.GreenYellow, Messages.MissionTruckerDeliverTo, player.MissionCargo.Name, player.ToLocation.Name);
                    break;

                case 2:
                    BasePlayer.SendClientMessageToAll(Color.White, $"Pilot {{FF00FF}}{player.Name}{{FFFFFF}} succesfully transported {{0000FF}}{player.MissionCargo.Name}");
                    BasePlayer.SendClientMessageToAll(Color.White, $"from {{00FF00}}{player.FromLocation.Name}{{FFFFFF}} to {{00FF00}}{player.ToLocation.Name}{{FFFFFF}}.");

                    var payment = MissionsController.CalculatePayment(player.FromLocation, player.ToLocation, player.MissionCargo);
                    player.Reward(payment);

                    player.SendClientMessage(Color.GreenYellow, $"You finished the mission and earned ${payment}.");

                    var playerAccount = player.Account;
                    playerAccount.PilotJobs++;
                    await AccountRepository.UpdateAsync(playerAccount);

                    EndMission(player);
                    break;
            }

            player.ToggleControllable(true);
            player.Vehicle.Engine = true;
        }

        private void Pilot_PlayerEnterCheckpoint(object sender, EventArgs e)
        {
            if (!(sender is Player player))
                return;

            if (player.PlayerClass != PlayerClasses.Data.PlayerClassType.Pilot)
                return;

            if (player.Vehicle != player.MissionVehicle)
                return;

            var loadMessage = string.Empty;

            switch (player.MissionStep)
            {
                case 1:
                    loadMessage = $"~r~Loading {player.MissionCargo.Name}... ~w~Please Wait";
                    break;

                case 2:
                    loadMessage = $"~r~Unloading {player.MissionCargo.Name}... ~w~Please Wait";
                    break;
            }

            player.ToggleControllable(false);

            switch (player.Vehicle.Model)
            {
                case VehicleModelType.Nevada:
                case VehicleModelType.Shamal:
                    player.GameText(loadMessage, 5000, 4);
                    player.MissionLoadingTimer = new Timer(TimeSpan.FromSeconds(5), false);
                    break;

                case VehicleModelType.Cargobob:
                case VehicleModelType.Maverick:
                    player.GameText(loadMessage, 3000, 4);
                    player.MissionLoadingTimer = new Timer(TimeSpan.FromSeconds(3), false);
                    player.Vehicle.Engine = false;
                    break;
            }

            player.MissionLoadingTimer.Tick += (senderObject, ev) => MissionLoadingTimer_Tick(senderObject, ev, player);
        }

        private void Pilot_PlayerExitVehicle(object sender, SampSharp.GameMode.Events.PlayerVehicleEventArgs e)
        {
            if (!(sender is Player player))
                return;

            if (player.PlayerClass != PlayerClasses.Data.PlayerClassType.Pilot)
                return;

            if (player.IsDoingMission)
                EndMission(player);
        }
    }
}