using SampSharp.GameMode;
using SampSharp.GameMode.Controllers;
using SampSharp.GameMode.SAMP;
using SampSharp.GameMode.Tools;
using SampSharp.GameMode.World;
using System;
using System.Threading.Tasks;
using TruckingSharp.Database.Repositories;
using TruckingSharp.PlayerClasses.Data;

namespace TruckingSharp.Missions.Police
{
    [Controller]
    public class PoliceController : IEventListener
    {
        private static PlayerAccountRepository AccountRepository => new PlayerAccountRepository(ConnectionFactory.GetConnection);

        public static void EndMission(Player player)
        {
            if (player.CheckTimer != null)
                player.CheckTimer.IsRunning = false;

            player.CheckTimer?.Dispose();

            foreach (var basePlayer in BasePlayer.All)
            {
                var serverPlayer = (Player)basePlayer;

                if (!serverPlayer.IsLoggedIn)
                    continue;

                ResetPlayerColors(player, serverPlayer);
            }
        }

        public static async Task JailPlayerAsync(Player finedPlayer, int jailSeconds)
        {
            var finedPlayerAccount = finedPlayer.Account;

            finedPlayer.RemoveFromVehicle();
            finedPlayer.VirtualWorld = Configuration.Instance.JailWorld;
            finedPlayer.Interior = 10;
            finedPlayer.Position = new Vector3(220.0, 110.0, 999.1);

            finedPlayerAccount.Jailed = jailSeconds;
            await AccountRepository.UpdateAsync(finedPlayerAccount);
            finedPlayer.JailingTimer?.Dispose();
            finedPlayer.JailingTimer = new Timer(TimeSpan.FromSeconds(1), true);
            finedPlayer.JailingTimer.Tick += (sender, e) => JailingTimer_Tick(sender, e, finedPlayer);

            if (finedPlayer.IsDoingMission)
            {
                await MissionsController.ClassEndMissionAsync(finedPlayer);

                finedPlayer.GameText($"~w~You ~r~failed~w~ your mission. You lost ~y~${Configuration.Instance.FailedMissionPrice}~w~ to cover expenses.", 5000, 4);
                await finedPlayer.RewardAsync(-Configuration.Instance.FailedMissionPrice);
            }
        }

        public static void PoliceCheckTimer_Tick(object sender, EventArgs e, Player player)
        {
            foreach (var basePlayer in BasePlayer.All)
            {
                var serverPlayer = (Player)basePlayer;

                if (!serverPlayer.IsLoggedIn)
                    continue;

                if (serverPlayer == player)
                    continue;

                if (serverPlayer.Account.Wanted > 0)
                {
                    serverPlayer.SetPlayerMarker(player, Color.Red);
                }
                else
                {
                    ResetPlayerColors(player, serverPlayer);
                }
            }
        }

        public static void SendMessage(Color color, string message)
        {
            foreach (var basePlayer in BasePlayer.All)
            {
                var player = (Player)basePlayer;

                if (!player.IsLoggedIn)
                    continue;

                if (player.PlayerClass != PlayerClassType.Police)
                    continue;

                player.SendClientMessage(color, message);
            }
        }

        public void RegisterEvents(BaseMode gameMode)
        {
            gameMode.PlayerKeyStateChanged += Police_PlayerKeyStateChanged;
            gameMode.PlayerDied += Police_PlayerDied;
            gameMode.PlayerSpawned += Police_PlayerSpawned;
        }

        private async void Police_PlayerSpawned(object sender, SampSharp.GameMode.Events.SpawnEventArgs e)
        {
            if (!(sender is Player player))
                return;

            if (player.Account.Jailed != 0)
                await JailPlayerAsync(player, player.Account.Jailed);
        }

        private async void Police_PlayerDied(object sender, SampSharp.GameMode.Events.DeathEventArgs e)
        {
            if (!(sender is Player victim))
                return;

            if (!(e.Killer is Player killer))
                return;

            if (killer != null && killer.PlayerClass != PlayerClassType.Police)
            {
                await killer.SetWantedLevelAsync(killer.Account.Wanted + 1);
                killer.SendClientMessage(Color.Red, $"You've killed {{FFFF00}}{victim.Name}{{FF0000}}, you're wanted by the police now.");

                SendMessage(Color.GreenYellow, $"Player {{FFFF00}}{killer.Name}{{00FF00}} killed {{FFFF00}}{victim.Name}{{00FF00}}, pursue and fine him.");
            }
        }

        private static async void JailingTimer_Tick(object sender, EventArgs e, Player jailedPlayer)
        {
            var jailedPlayerAccount = jailedPlayer.Account;

            if (jailedPlayerAccount.Jailed <= 0)
            {
                ReleasePlayerFromJail(jailedPlayer);
            }
            else
            {
                if (jailedPlayerAccount.Jailed < 60)
                {
                    jailedPlayer.GameText($"~w~Jailed: ~r~{jailedPlayerAccount.Jailed}~w~", 750, 4);
                }

                jailedPlayerAccount.Jailed -= 1;
                await AccountRepository.UpdateAsync(jailedPlayerAccount);
            }
        }

        public static void ReleasePlayerFromJail(Player jailedPlayer)
        {
            jailedPlayer.VirtualWorld = 0;
            jailedPlayer.Interior = 0;
            jailedPlayer.Spawn();
            jailedPlayer.JailingTimer.IsRunning = false;
            jailedPlayer.JailingTimer?.Dispose();
        }

        private static void ResetPlayerColors(Player player, Player serverPlayer)
        {
            switch (serverPlayer.PlayerClass)
            {
                case PlayerClassType.TruckDriver:
                    serverPlayer.SetPlayerMarker(player, PlayerClassColor.TruckerColor);
                    break;

                case PlayerClassType.BusDriver:
                    serverPlayer.SetPlayerMarker(player, PlayerClassColor.BusDriverColor);
                    break;

                case PlayerClassType.Pilot:
                    serverPlayer.SetPlayerMarker(player, PlayerClassColor.PilotColor);
                    break;

                case PlayerClassType.Police:
                    serverPlayer.SetPlayerMarker(player, PlayerClassColor.PoliceColor);
                    break;

                case PlayerClassType.Mafia:
                    serverPlayer.SetPlayerMarker(player, PlayerClassColor.MafiaColor);
                    break;

                case PlayerClassType.Courier:
                    serverPlayer.SetPlayerMarker(player, PlayerClassColor.CourierColor);
                    break;

                case PlayerClassType.Assistance:
                    serverPlayer.SetPlayerMarker(player, PlayerClassColor.AssistanceColor);
                    break;

                case PlayerClassType.RoadWorker:
                    serverPlayer.SetPlayerMarker(player, PlayerClassColor.RoadWorkerColor);
                    break;
            }
        }

        private async Task FineNearbyPlayersAsync(Player player)
        {
            foreach (var basePlayer in BasePlayer.All)
            {
                var serverPlayer = (Player)basePlayer;

                if (!serverPlayer.IsLoggedIn)
                    continue;

                if (serverPlayer == player)
                    continue;

                if (serverPlayer.Account.Wanted > 0 && player.Speed < 30)
                {
                    if (player.IsInRangeOfPoint(10.0f, serverPlayer.Position))
                    {
                        await FinePlayerAsync(player, serverPlayer);
                        return;
                    }

                    if (player.IsInRangeOfPoint(50.0f, serverPlayer.Position))
                    {
                        serverPlayer.GameText("~r~This is the police! Stop at once!~w~", 3000, 4);

                        if (!serverPlayer.IsWarnedByPolice)
                        {
                            serverPlayer.IsWarnedByPolice = true;
                            serverPlayer.SecondsUntilPoliceCanJail = Configuration.Instance.WarnSecondsBeforeJail;
                            serverPlayer.TimerUntilPoliceCanJail = new Timer(TimeSpan.FromSeconds(5), true);
                            serverPlayer.TimerUntilPoliceCanJail.Tick += (sender, e) => PoliceCanJailPlayer(sender, e, serverPlayer);
                        }
                    }
                }
            }
        }

        private async Task FinePlayerAsync(Player policePlayer, Player finedPlayer)
        {
            var policePlayerAccount = policePlayer.Account;

            if (!finedPlayer.CanPoliceJail)
            {
                var finedPlayerAccount = finedPlayer.Account;
                var fine = finedPlayerAccount.Wanted * Configuration.Instance.FinePerWantedLevel;
                await finedPlayer.RewardAsync(-fine);
                finedPlayer.SendClientMessage(Color.Red, $"You have been caught by {policePlayer.Name} and payed a fine of ${fine}.");

                BasePlayer.SendClientMessageToAll(Color.GreenYellow, $"Officer {policePlayer.Name} has fined {finedPlayer}.");

                await policePlayer.RewardAsync(fine, finedPlayerAccount.Wanted);
                policePlayerAccount.PoliceFined++;
                policePlayer.SendClientMessage(Color.GreenYellow, $"You have fined {finedPlayer.Name} and earned ${fine}.");
            }
            else
            {
                var finedPlayerAccount = finedPlayer.Account;
                var fine = finedPlayerAccount.Wanted * Configuration.Instance.FinePerWantedLevel * 2;
                await finedPlayer.RewardAsync(-fine);
                finedPlayer.SendClientMessage(Color.Red, $"You have been jailed by {policePlayer.Name} for {Configuration.Instance.DefaultJailSeconds / 60} minutes.");

                await JailPlayerAsync(finedPlayer, Configuration.Instance.DefaultJailSeconds);

                BasePlayer.SendClientMessageToAll(Color.GreenYellow, $"Officer {policePlayer.Name} has jailed {finedPlayer.Name} for {Configuration.Instance.DefaultJailSeconds / 60} minutes.");

                await policePlayer.RewardAsync(fine, finedPlayerAccount.Wanted);
                policePlayerAccount.PoliceJailed++;
                policePlayer.SendClientMessage(Color.GreenYellow, $"You have jailed {finedPlayer.Name} and earned ${fine}.");
            }

            await finedPlayer.SetWantedLevelAsync(0);

            await AccountRepository.UpdateAsync(policePlayerAccount);
        }

        private async void Police_PlayerKeyStateChanged(object sender, SampSharp.GameMode.Events.KeyStateChangedEventArgs e)
        {
            if (!(sender is Player player))
                return;

            if (player.PlayerClass != PlayerClassType.Police)
                return;

            if (KeyUtils.HasPressed(e.NewKeys, e.OldKeys, SampSharp.GameMode.Definitions.Keys.Aim) && player.Vehicle == null)
            {
                await FineNearbyPlayersAsync(player);
                return;
            }

            if (KeyUtils.HasPressed(e.NewKeys, e.OldKeys, SampSharp.GameMode.Definitions.Keys.Action) && player.Vehicle != null)
            {
                WarnNearbyPlayers(player);
            }
        }

        private void PoliceCanJailPlayer(object sender, EventArgs e, Player serverPlayer)
        {
            serverPlayer.SendClientMessage(Color.Red, $"You have {serverPlayer.SecondsUntilPoliceCanJail} seconds to pull over and stop.");

            if (serverPlayer.SecondsUntilPoliceCanJail <= 0)
            {
                serverPlayer.CanPoliceJail = true;
                serverPlayer.TimerUntilPoliceCanJail.IsRunning = false;
                serverPlayer.TimerUntilPoliceCanJail?.Dispose();
                serverPlayer.SendClientMessage(Color.Red, "You didn't pull-over and stop, now the police can send you to jail immediately.");
                serverPlayer.SendClientMessage(Color.Red, "Your fines will be doubled as well.");
            }

            serverPlayer.SecondsUntilPoliceCanJail -= 5;
        }

        private void WarnNearbyPlayers(Player player)
        {
            foreach (var basePlayer in BasePlayer.All)
            {
                var serverPlayer = (Player)basePlayer;

                if (!serverPlayer.IsLoggedIn)
                    continue;

                if (serverPlayer.Account.Wanted > 0 && serverPlayer != player)
                {
                    if (player.IsInRangeOfPoint(50.0f, serverPlayer.Position))
                    {
                        serverPlayer.GameText("~r~This is the police! Stop at once!~w~", 3000, 4);

                        if (!serverPlayer.IsWarnedByPolice)
                        {
                            serverPlayer.IsWarnedByPolice = true;
                            serverPlayer.SecondsUntilPoliceCanJail = Configuration.Instance.WarnSecondsBeforeJail;
                            serverPlayer.TimerUntilPoliceCanJail = new Timer(TimeSpan.FromSeconds(5), true);
                            serverPlayer.TimerUntilPoliceCanJail.Tick += (sender, e) => PoliceCanJailPlayer(sender, e, serverPlayer);
                        }
                    }
                }
            }
        }
    }
}