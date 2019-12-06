using SampSharp.GameMode;
using SampSharp.GameMode.Controllers;
using SampSharp.GameMode.SAMP;
using SampSharp.GameMode.World;
using System;
using TruckingSharp.Constants;
using TruckingSharp.Database;
using TruckingSharp.Missions.Data;
using TruckingSharp.Missions.Police;
using TruckingSharp.PlayerClasses.Data;

namespace TruckingSharp.Missions.Mafia
{
    [Controller]
    public class MafiaController : IEventListener
    {
        public void RegisterEvents(BaseMode gameMode)
        {
            gameMode.PlayerEnterCheckpoint += Mafia_PlayerEnterCheckpoint;
        }

        private async void Mafia_PlayerEnterCheckpoint(object sender, EventArgs e)
        {
            if (!(sender is Player player))
                return;

            if (player.PlayerClass != PlayerClassType.Mafia)
                return;

            if (!player.IsDoingMission)
            {
                var playerVehicleTrailer = player.Vehicle.Trailer;

                if (playerVehicleTrailer != null)
                {
                    player.Vehicle.Trailer = null;
                    playerVehicleTrailer.Respawn();
                }
                else
                {
                    player.RemoveFromVehicle();
                    player.Vehicle.Respawn();
                }

                player.Reward(5000, 2);
                player.SendClientMessage(Color.GreenYellow, "You delivered a stolen mafia-load, you earned $5000.");

                var playerAccount = player.Account;
                playerAccount.MafiaStolen++;
                await RepositoriesInstances.AccountRepository.UpdateAsync(playerAccount);

                player.MissionVehicle = null;
                player.MissionTrailer = null;

                player.DisableCheckpoint();

                player.MissionTextDraw.Text = Messages.NoMissionText;
            }
            else
            {
                if (player.Vehicle != player.MissionVehicle)
                    return;

                string message = string.Empty;

                switch (player.MissionStep)
                {
                    case 1:
                        message = $"~r~Loading {player.MissionCargo.Name}... ~w~Please Wait";
                        break;

                    case 2:
                        message = $"~r~Unloading {player.MissionCargo.Name}... ~w~Please Wait";
                        break;
                }

                player.ToggleControllable(false);

                player.GameText(message, 5000, 5);
                player.MissionLoadingTimer = new Timer(TimeSpan.FromSeconds(5), false);
                player.MissionLoadingTimer.Tick += (objectSender, ev) => MafiaMissionLoadingTimer_Tick(objectSender, ev, player);
            }
        }

        internal static void PoliceCheckTimer_Tick(object senderObject, EventArgs ev, Player player)
        {
            foreach (var basePlayer in BasePlayer.All)
            {
                var serverPlayer = (Player)basePlayer;

                if (!serverPlayer.IsLoggedIn)
                    continue;

                if (serverPlayer.IsMafiaLoaded)
                    serverPlayer.SetPlayerMarker(player, Color.Red);
                else
                    serverPlayer.SetPlayerMarker(player, PlayerClassColor.TruckerColor);

                if (!player.IsDoingMission)
                {
                    if (player.Vehicle == null)
                        return;

                    var playerVehicle = (Vehicle)player.Vehicle;
                    var playerVehicleTrailer = (Vehicle)player.Vehicle.Trailer;
                    

                    if (!player.MafiaLoadHijacked)
                    {
                        if (playerVehicle?.IsWantedByMafia == true || playerVehicleTrailer?.IsWantedByMafia == true)
                        {
                            player.MissionVehicle = playerVehicle;
                            player.MissionTrailer = playerVehicleTrailer;
                            player.MafiaLoadHijacked = true;
                            player.SetCheckpoint(new Vector3(2867, 939, 10.8), 7.0f);
                            player.MissionTextDraw.Text = "~w~Bring the ~b~stolen load~w~ to the ~r~mafia-hideout~w~";
                        }
                    }

                    if (player.MafiaLoadHijacked)
                    {
                        if (player.MissionVehicle != playerVehicle || player.MissionTrailer != playerVehicleTrailer)
                        {
                            player.MissionVehicle = null;
                            player.MissionTrailer = null;
                            player.MafiaLoadHijacked = false;
                            player.DisableCheckpoint();
                            player.MissionTextDraw.Text = Messages.NoMissionTextMafia;
                        }
                    }
                }
            }
        }

        private async void MafiaMissionLoadingTimer_Tick(object sender, EventArgs e, Player player)
        {
            switch (player.MissionStep)
            {
                case 1:
                    player.MissionStep = 2;
                    player.DisableCheckpoint();

                    player.MissionTextDraw.Text = $"~w~Hauling ~b~{player.MissionCargo.Name}~w~ from {player.FromLocation.Name} to ~r~{player.ToLocation.Name}~w~";

                    player.SetCheckpoint(player.ToLocation.Position, 7.0f);
                    player.SetWantedLevel(player.Account.Wanted + 4);

                    PoliceController.SendMessage(Color.GreenYellow, $"Mafia {{FFFF00}}{player.Name}{{00FF00}} is transporting illegal goods, pursue and fine him.");

                    player.SendClientMessage(Color.GreenYellow, $"Deliver the {player.MissionCargo.Name} to {player.ToLocation.Name}.");
                    break;

                case 2:
                    BasePlayer.SendClientMessageToAll(Color.White, $"Mafia {{FF00FF}}{player.Name}{{FFFFFF}} succesfully transported {{0000FF}}{player.MissionCargo.Name}");
                    BasePlayer.SendClientMessageToAll(Color.White, $"from {{00FF00}}{player.FromLocation.Name}{{FFFFFF}} to {{00FF00}}{player.ToLocation.Name}.");

                    var payment = MissionsController.CalculatePayment(player.FromLocation, player.ToLocation, player.MissionCargo);
                    player.Reward(payment, 2);

                    player.SendClientMessage(Color.GreenYellow, $"You finished the mission and earned ${payment}.");

                    var playerAccount = player.Account;
                    playerAccount.MafiaJobs++;
                    await RepositoriesInstances.AccountRepository.UpdateAsync(playerAccount);

                    EndMission(player);
                    break;
            }

            player.ToggleControllable(true);
        }

        public static void EndMission(Player player)
        {
            player.CheckTimer?.Dispose();

            foreach (var basePlayer in BasePlayer.All)
            {
                var serverPlayer = (Player)basePlayer;

                if (!serverPlayer.IsLoggedIn || player.PlayerClass != PlayerClasses.Data.PlayerClassType.TruckDriver)
                    continue;

                serverPlayer.SetPlayerMarker(player, PlayerClassColor.TruckerColor);
            }

            if (player.IsDoingMission)
            {
                player.IsDoingMission = false;
                player.MissionCargo = null;
                player.MissionStep = 0;
                player.FromLocation = null;
                player.ToLocation = null;

                player.MissionTextDraw.Text = Messages.NoMissionTextMafia;

                player.MissionLoadingTimer?.Dispose();

                if (player.Account.Wanted >= 4)
                    player.SetWantedLevel(player.Account.Wanted - 4);
                else
                    player.SetWantedLevel(0);
            }

            player.MissionVehicle = null;
            player.MissionTrailer = null;
            player.DisableCheckpoint();
        }

        public static void StartRandomMission(Player player)
        {
            if (player.IsDoingMission)
                return;

            player.IsDoingMission = true;

            player.MissionCargo = MissionCargo.GetRandomCargo(MissionCargoVehicleType.MafiaVan);
            player.FromLocation = MissionCargo.GetRandomStartLocation(player.MissionCargo);
            player.ToLocation = MissionCargo.GetRandomEndLocation(player.MissionCargo);

            player.MissionStep = 1;
            player.MissionTextDraw.Text = $"~w~Hauling ~b~{player.MissionCargo.Name}~w~ from ~r~{player.FromLocation.Name}~w~ to {player.ToLocation.Name}.";

            player.SetCheckpoint(player.FromLocation.Position, 7.0f);
            player.MissionVehicle = (Vehicle)player.Vehicle;
            player.MissionVehicleTime = Configuration.Instance.FailMissionSeconds;
            player.SendClientMessage(Color.GreenYellow, $"Pickup the {player.MissionCargo.Name} at {player.FromLocation.Name}.");
        }
    }
}