using SampSharp.GameMode;
using SampSharp.GameMode.Definitions;
using SampSharp.GameMode.Display;
using SampSharp.GameMode.SAMP;
using SampSharp.GameMode.SAMP.Commands;
using SampSharp.GameMode.World;
using System;
using System.Threading.Tasks;
using TruckingSharp.Commands.Permissions;
using TruckingSharp.Constants;
using TruckingSharp.Data;
using TruckingSharp.Database.Entities;
using TruckingSharp.Database.Repositories;
using TruckingSharp.Extensions.PlayersExtensions;

namespace TruckingSharp.Commands.AdminCommands
{
    [CommandGroup("admin", PermissionChecker = typeof(LevelTwoAdminPermission))]
    public class LevelTwoAdminCommands
    {
        [Command("repairall", Shortcut = "repairall")]
        public static void OnRepairAllCommand(Player sender)
        {
            foreach (var vehicle in BaseVehicle.All)
                vehicle.Repair();

            BasePlayer.SendClientMessageToAll(Color.GreenYellow,
                $"{AdminRanks.AdminLevelNames[sender.Account.AdminLevel]} {sender.Name} has repaired all vehicles.");
        }

        [Command("healall", Shortcut = "healall")]
        public static void OnHealAllCommand(Player sender)
        {
            foreach (var player in BasePlayer.All)
                player.Health = 100;

            BasePlayer.SendClientMessageToAll(Color.GreenYellow,
                $"{AdminRanks.AdminLevelNames[sender.Account.AdminLevel]} {sender.Name} has healed all players.");
        }

        [Command("loc", Shortcut = "loc")]
        public static void OnLocCommand(BasePlayer sender)
        {
            sender.SendClientMessage(Color.Red,
                $"Location: x: {sender.Position.X} y: {sender.Position.Y} z: {sender.Position.Z} interior: {sender.Interior} world: {sender.VirtualWorld}");
        }

        [Command("ban", Shortcut = "ban")]
        public static async void OnBanCommandAsync(Player sender, Player target, int days, string reason)
        {
            if (!target.IsLoggedIn)
            {
                sender.SendClientMessage(Color.Red, Messages.PlayerNotLoggedIn);
                return;
            }

            if (target.Account.AdminLevel > 0)
            {
                sender.SendClientMessage(Color.Red, "You can't use this command on admins.");
                return;
            }

            if (target == sender)
            {
                sender.SendClientMessage(Color.Red, Messages.CommandNotAllowedOnSelf);
                return;
            }

            if (days < 1)
            {
                sender.SendClientMessage(Color.Red, "You must enter at least one day.");
                return;
            }

            if (string.IsNullOrEmpty(reason))
            {
                sender.SendClientMessage(Color.Red, "Reason can not be empty.");
                return;
            }

            var account = target.Account;
            account.Bans++;

            var playerBan = new PlayerBan
            {
                Reason = reason,
                AdminId = sender.Account.Id,
                OwnerId = target.Account.Id
            };

            var totalBanTime = DateTime.Now + TimeSpan.FromDays(days);

            if (account.Bans >= Configuration.Instance.MaximumBans)
            {
                playerBan.Duration = DateTime.MaxValue;

                target.SendClientMessage(Color.Red,
                    $"This was the {Configuration.Instance.MaximumBans}ith and last ban. You have been banned permanently by {AdminRanks.AdminLevelNames[sender.Account.AdminLevel]} {sender.Name}.");
            }
            else
            {
                playerBan.Duration = totalBanTime;

                target.SendClientMessage(Color.Red, $"You have been banned by {sender.Name} for {days} day/s.");
                target.SendClientMessage(Color.Red, $"Reason: {reason}.");
            }

            await new PlayerAccountRepository(ConnectionFactory.GetConnection).UpdateAsync(account);
            await new PlayerBanRepository(ConnectionFactory.GetConnection).AddAsync(playerBan);

            BasePlayer.SendClientMessageToAll(Color.LightGray,
                $"{AdminRanks.AdminLevelNames[sender.Account.AdminLevel]} {sender.Name} has banned {target.Name} for {days} day/s.");

            await Task.Delay(Configuration.Instance.KickDelay);
            target.Kick();
        }

        [Command("unban", Shortcut = "unban")]
        public static async void OnUnBanCommandAsync(Player sender, string name)
        {
            var playerBan = await new PlayerBanRepository(ConnectionFactory.GetConnection).FindAsync(name);

            if (playerBan == null)
            {
                sender.SendClientMessage(Color.Red, "That player is not banned.");
                return;
            }

            var wasDeletedSuccessfully = await new PlayerBanRepository(ConnectionFactory.GetConnection).DeleteAsync(playerBan);

            if (wasDeletedSuccessfully > 0)
            {
                sender.SendClientMessage(Color.GreenYellow, "Player has been unbanned successfully.");
                BasePlayer.SendClientMessageToAll(Color.LightGray,
                    $"{AdminRanks.AdminLevelNames[sender.Account.AdminLevel]} {sender.Name} has un-banned {name}.");
            }
            else
            {
                sender.SendClientMessage(Color.Red, "Player couldn't be unbanned.");
            }
        }

        [Command("jetpack", Shortcut = "jetpack")]
        public static void OnJetpackCommand(BasePlayer sender)
        {
            sender.SpecialAction = SpecialAction.Usejetpack;
        }

        [Command("caroption", Shortcut = "caroption")]
        public static void OnCarOptionCommand(BasePlayer sender)
        {
            if (!sender.IsDriving())
            {
                sender.SendClientMessage(Color.Red, Messages.CommandAllowedOnlyAsDriver);
                return;
            }

            var carOptionsDialog = new ListDialog("Select option for your vehicle:", Messages.DialogButtonToggle,
                Messages.DialogButtonCancel);
            carOptionsDialog.AddItem("Engine\nLights\nAlarm\nDoors\nBonnet\nBoot\nObjective");
            carOptionsDialog.Show(sender);

            carOptionsDialog.Response += (objectSender, e) =>
            {
                if (e.DialogButton != DialogButton.Left)
                    return;

                var playerVehicle = sender.Vehicle;

                switch (e.ListItem)
                {
                    case 0:
                        playerVehicle.Engine = !playerVehicle.Engine;
                        break;

                    case 1:
                        playerVehicle.Lights = !playerVehicle.Lights;
                        break;

                    case 2:
                        playerVehicle.Alarm = !playerVehicle.Alarm;
                        break;

                    case 3:
                        playerVehicle.Doors = !playerVehicle.Doors;
                        break;

                    case 4:
                        playerVehicle.Bonnet = !playerVehicle.Bonnet;
                        break;

                    case 5:
                        playerVehicle.Boot = !playerVehicle.Boot;
                        break;

                    case 6:
                        playerVehicle.Objective = !playerVehicle.Objective;
                        break;
                }
            };
        }

        [Command("cleanupallvehicles", Shortcut = "cleanupallvehicles")]
        public static async void OnCleanupAllVehiclesCommandAsync(BasePlayer sender)
        {
            foreach (var baseVehicle in Vehicle.All)
            {
                var vehicle = (Vehicle)baseVehicle;

                if (vehicle.IsOwned || !vehicle.IsAdminSpawned)
                    continue;

                vehicle.RemoveAllPlayersFromVehicle();

                vehicle.Doors = true;

                await Task.Delay(TimeSpan.FromMilliseconds(1500));
                vehicle.Dispose();
            }
        }

        [Command("cleanupvehicle", Shortcut = "cleanupvehicle")]
        public static async void OnCleanupVehicleAsync(BasePlayer sender, Vehicle vehicle)
        {
            if (vehicle.IsOwned || !vehicle.IsAdminSpawned)
            {
                sender.SendClientMessage(Color.Red, "This vehicle can't be cleaned.");
                return;
            }

            vehicle.RemoveAllPlayersFromVehicle();

            vehicle.Doors = true;

            await Task.Delay(TimeSpan.FromMilliseconds(1500));
            vehicle.Dispose();
        }

        [Command("ipban", Shortcut = "ipban")]
        public static async void OnIPBanCommandAsync(Player sender, Player target, string reason)
        {
            if (!target.IsLoggedIn)
            {
                sender.SendClientMessage(Color.Red, Messages.PlayerNotLoggedIn);
                return;
            }

            if (target.Account.AdminLevel > 0)
            {
                sender.SendClientMessage(Color.Red, "You can't use this command on admins.");
                return;
            }

            if (target == sender)
            {
                sender.SendClientMessage(Color.Red, Messages.CommandNotAllowedOnSelf);
                return;
            }

            target.SendClientMessage(Color.Red, $"You have been ip-banned permanently by {sender.Name}");
            target.SendClientMessage(Color.Red, $"Reason: {reason}");

            BasePlayer.SendClientMessageToAll(Color.Gray,
                $"{AdminRanks.AdminLevelNames[sender.Account.AdminLevel]} {sender.Name} has ip-banned {target.Name}.");

            await Task.Delay(TimeSpan.FromSeconds(1));
            target.Ban(reason);
        }

        [Command("rangeban", Shortcut = "rangeban")]
        public static void OnRangeBanCommand(Player sender, Player target, string reason)
        {
            if (!target.IsLoggedIn)
            {
                sender.SendClientMessage(Color.Red, Messages.PlayerNotLoggedIn);
                return;
            }

            if (target.Account.AdminLevel > 0)
            {
                sender.SendClientMessage(Color.Red, "You can't use this command on admins.");
                return;
            }

            if (target == sender)
            {
                sender.SendClientMessage(Color.Red, Messages.CommandNotAllowedOnSelf);
                return;
            }

            var targetIp = target.IP;
            var firstPartOfTheIp = targetIp.Substring(0, targetIp.LastIndexOf(".", StringComparison.Ordinal) + 1);

            target.SendClientMessage(Color.Red, $"You have been ip-range-banned permanently by {sender.Name}.");
            target.SendClientMessage(Color.Red, $"Reason: {reason}.");

            for (var i = 0; i < 256; i++)
                BaseMode.Instance.SendRconCommand($"banip {firstPartOfTheIp}{i}");

            BaseMode.Instance.SendRconCommand("reloadbans");

            BasePlayer.SendClientMessageToAll(Color.Gray,
                $"{AdminRanks.AdminLevelNames[sender.Account.AdminLevel]} {sender.Name} has ip-range-banned {target.Name}.");
        }

        [Command("setscore", Shortcut = "setscore")]
        public static async void OnSetScoreCommandAsync(Player sender, Player target, int score)
        {
            if (target.Account.AdminLevel > 0)
            {
                sender.SendClientMessage(Color.Red, "You can't use this command on admins.");
                return;
            }

            var targetAccount = target.Account;
            targetAccount.Score = score;
            await new PlayerAccountRepository(ConnectionFactory.GetConnection).UpdateAsync(targetAccount);

            target.SendClientMessage(Color.GreenYellow, $"Your score has been set to {score} by {sender.Name}.");
            sender.SendClientMessage(Color.GreenYellow, $"You've set the score of {target.Name} to {score}.");
        }

        [Command("setmoeny", Shortcut = "setmoeny")]
        public static async void OnSetMoneyCommand(Player sender, Player target, int money)
        {
            if (target.Account.AdminLevel > 0)
            {
                sender.SendClientMessage(Color.Red, "You can't use this command on admins.");
                return;
            }

            var targetAccount = target.Account;
            targetAccount.Money = money;
            await new PlayerAccountRepository(ConnectionFactory.GetConnection).UpdateAsync(targetAccount);

            target.SendClientMessage(Color.GreenYellow, $"Your money has been set to {money} by {sender.Name}.");
            sender.SendClientMessage(Color.GreenYellow, $"You've set the money of {target.Name} to {money}.");
        }

        [Command("givelicense", Shortcut = "givelicense")]
        public static async void OnGiveLicenseCommandAsync(BasePlayer sender, Player target)
        {
            if (target.Account.AdminLevel > 0)
            {
                sender.SendClientMessage(Color.Red, "You can't use this command on admins.");
                return;
            }

            var targetAccount = target.Account;
            targetAccount.TruckerLicense = 1;
            await new PlayerAccountRepository(ConnectionFactory.GetConnection).UpdateAsync(targetAccount);

            sender.SendClientMessage(Color.GreenYellow, $"You've given {target.Name} a trucker's license.");
            target.SendClientMessage(Color.GreenYellow,
                $"You've been given a free trucker's license by {sender.Name}.");
        }

        [Command("fuelall", Shortcut = "fuelall")]
        public static void OnFuelAllCommand(BasePlayer sender)
        {
            foreach (var baseVehicle in Vehicle.All)
            {
                var vehicle = (Vehicle)baseVehicle;

                vehicle.Fuel = Configuration.Instance.MaximumFuel;
            }

            sender.SendClientMessage(Color.GreenYellow, "All vehicles have been refueled.");
        }

        [Command("weather", Shortcut = "weather")]
        public static void OnWeatherCommand(BasePlayer sender)
        {
            var weatherListDialog = new ListDialog("Select weather type:", Messages.DialogButtonSelect,
                Messages.DialogButtonCancel);
            weatherListDialog.AddItem(
                "Normal\nStormy\nFoggy\nScorching Hot\nDull, cloudy, rainy\nSandstorm\nGreen Fog\nDark, cloudy, brown\nExtremely bright\nDark toxic clouds\nBlack & white sky");
            weatherListDialog.Show(sender);
            weatherListDialog.Response += (objectSender, e) =>
            {
                switch (e.ListItem)
                {
                    case 0:
                        Server.SetWeather(0);
                        break;

                    case 1:
                        Server.SetWeather(8);
                        break;

                    case 2:
                        Server.SetWeather(9);
                        break;

                    case 3:
                        Server.SetWeather(11);
                        break;

                    case 4:
                        Server.SetWeather(16);
                        break;

                    case 5:
                        Server.SetWeather(19);
                        break;

                    case 6:
                        Server.SetWeather(20);
                        break;

                    case 7:
                        Server.SetWeather(33);
                        break;

                    case 8:
                        Server.SetWeather(39);
                        break;

                    case 9:
                        Server.SetWeather(43);
                        break;

                    case 10:
                        Server.SetWeather(44);
                        break;
                }
            };
        }

        [Command("setskin", Shortcut = "setskin")]
        public static void OnSetSkinCommand(BasePlayer sender, Player target, int skin)
        {
            if (!target.IsLoggedIn)
            {
                sender.SendClientMessage(Color.Red, Messages.PlayerNotLoggedIn);
                return;
            }

            if (skin < 0 && skin > 311)
            {
                sender.SendClientMessage(Color.Red, "Invalid skin-id, you can only use skins 0-311.");
                return;
            }

            target.Skin = skin;

            sender.SendClientMessage(Color.GreenYellow, $"You've changed the skin for {target.Name} to {skin}.");
            target.SendClientMessage(Color.GreenYellow,
                $"Your skin has been changed by admin {sender.Name} to {skin}.");
        }
    }
}