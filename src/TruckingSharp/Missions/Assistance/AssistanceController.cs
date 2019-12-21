using SampSharp.GameMode;
using SampSharp.GameMode.Controllers;
using SampSharp.GameMode.SAMP;
using SampSharp.GameMode.Tools;
using SampSharp.GameMode.World;
using System;
using System.Threading.Tasks;
using TruckingSharp.Database.Repositories;
using TruckingSharp.Extensions.PlayersExtensions;
using TruckingSharp.PlayerClasses.Data;

namespace TruckingSharp.Missions.Assistance
{
    [Controller]
    public class AssistanceController : IEventListener
    {
        public void RegisterEvents(BaseMode gameMode)
        {
            gameMode.PlayerKeyStateChanged += Assistance_PlayerKeyStateChanged;
        }

        private async void Assistance_PlayerKeyStateChanged(object sender, SampSharp.GameMode.Events.KeyStateChangedEventArgs e)
        {
            if (!(sender is Player player))
                return;

            if (player.PlayerClass != PlayerClassType.Assistance)
                return;

            if (KeyUtils.HasPressed(e.NewKeys, e.OldKeys, SampSharp.GameMode.Definitions.Keys.Aim))
                await FixPlayerVehicleAsync(player);

            if (KeyUtils.HasPressed(e.NewKeys, e.OldKeys, SampSharp.GameMode.Definitions.Keys.Fire))
                FixOwnVehicle(player);
        }

        private async Task FixPlayerVehicleAsync(Player player)
        {
            foreach (var basePlayer in BasePlayer.All)
            {
                var serverPlayer = (Player)basePlayer;

                if (!serverPlayer.IsLoggedIn)
                    return;

                if (!serverPlayer.AssistanaceNeeded)
                    return;

                if (!serverPlayer.IsDriving())
                    return;

                if (!player.IsInRangeOfPoint(5.0f, serverPlayer.Position))
                    return;

                serverPlayer.AssistanaceNeeded = false;
                var serverPlayerVehicle = (Vehicle)serverPlayer.Vehicle;

                var payment = (int)((1000.0f - serverPlayerVehicle.Health) * 1.0f);
                serverPlayerVehicle.Repair();

                await player.RewardAsync(payment * 2, 1);
                await serverPlayer.RewardAsync(-payment);

                serverPlayer.SendClientMessage(Color.GreenYellow, $"Your vehicle has been repaired for {{FFFF00}}${payment}{{00FF00}} by \"{{FFFF00}}{player.Name}{{00FF00}}\"");
                player.SendClientMessage(Color.GreenYellow, $"You have repaired {{FFFF00}}{serverPlayer.Name}{{00FF00}}'s vehicle and earned {{FFFF00}}${payment * 2}.");

                payment = 0;
                var fuel = Configuration.Instance.MaximumFuel - serverPlayerVehicle.Fuel;

                if (fuel > 0)
                {
                    payment = (fuel * Configuration.Instance.RefuelPrice) / Configuration.Instance.MaximumFuel;

                    await player.RewardAsync(payment * 2, 1);
                    await serverPlayer.RewardAsync(-payment);

                    serverPlayerVehicle.Fuel = Configuration.Instance.MaximumFuel;

                    serverPlayer.SendClientMessage(Color.GreenYellow, $"Your vehicle has also been refuelled for {{FFFF00}}${payment}{{00FF00}} by \"{{FFFF00}}{player.Name}{{00FF00}}\"");
                    player.SendClientMessage(Color.GreenYellow, $"You have also refuelled {{FFFF00}}{serverPlayer.Name}{{00FF00}}'s vehicle and earned {{FFFF00}}${payment * 2}.");
                }

                var playerAccount = player.Account;
                playerAccount.AssistanceJobs++;
                await new PlayerAccountRepository(ConnectionFactory.GetConnection).UpdateAsync(playerAccount);
            }
        }

        public static void AssistanceCheckTimer_Tick(object senderObject, EventArgs ev, Player player)
        {
            foreach (var basePlayer in BasePlayer.All)
            {
                var serverPlayer = (Player)basePlayer;

                if (!serverPlayer.IsLoggedIn)
                    continue;

                if (serverPlayer.AssistanaceNeeded)
                {
                    player.SetPlayerMarker(serverPlayer, Color.Red);
                }
                else
                {
                    SetPlayerDefaultClassColor(player, serverPlayer);
                }
            }
        }

        public static void EndMission(Player player)
        {
            player.CheckTimer.Dispose();

            foreach (var basePlayer in BasePlayer.All)
            {
                var serverPlayer = (Player)basePlayer;

                if (!serverPlayer.IsLoggedIn)
                    continue;

                SetPlayerDefaultClassColor(player, serverPlayer);
            }
        }

        private static void SetPlayerDefaultClassColor(Player player, Player serverPlayer)
        {
            switch (serverPlayer.PlayerClass)
            {
                case PlayerClassType.TruckDriver:
                    player.SetPlayerMarker(serverPlayer, PlayerClassColor.TruckerColor);
                    break;

                case PlayerClassType.BusDriver:
                    player.SetPlayerMarker(serverPlayer, PlayerClassColor.BusDriverColor);
                    break;

                case PlayerClassType.Pilot:
                    player.SetPlayerMarker(serverPlayer, PlayerClassColor.PilotColor);
                    break;

                case PlayerClassType.Police:
                    player.SetPlayerMarker(serverPlayer, PlayerClassColor.PoliceColor);
                    break;

                case PlayerClassType.Mafia:
                    player.SetPlayerMarker(serverPlayer, PlayerClassColor.MafiaColor);
                    break;

                case PlayerClassType.Assistance:
                    player.SetPlayerMarker(serverPlayer, PlayerClassColor.AssistanceColor);
                    break;
            }
        }

        public static void FixOwnVehicle(Player player)
        {
            var playerVehicle = (Vehicle)player.Vehicle;

            playerVehicle.Repair();
            playerVehicle.Fuel = Configuration.Instance.MaximumFuel;
            player.SendClientMessage(Color.GreenYellow, "You have repaired and refuelled your own vehicle.");
        }
    }
}