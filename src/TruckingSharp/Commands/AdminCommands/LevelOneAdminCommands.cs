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
using TruckingSharp.Database;
using TruckingSharp.Database.Repositories;
using TruckingSharp.Extensions.PlayersExtensions;
using TruckingSharp.World;

namespace TruckingSharp.Commands.AdminCommands
{
    [CommandGroup("admin", PermissionChecker = typeof(LevelOneAdminPermission))]
    public class LevelOneAdminCommands
    {
        [Command("kick", Shortcut = "kick")]
        public static async void OnKickCommand(Player sender, Player target, string reason)
        {
            if (!target.IsLoggedIn)
            {
                sender.SendClientMessage(Color.Red, Messages.PlayerNotLoggedIn);
                return;
            }

            if (sender == target)
            {
                sender.SendClientMessage(Color.Red, Messages.CommandNotAallowedOnSelf);
                return;
            }

            target.SendClientMessage(Color.Red, $"You have been kicked by {AdminRanks.AdminLevelNames[sender.Account.AdminLevel]} {sender.Name}.");
            target.SendClientMessage(Color.Red, $"Reason: {reason}");

            sender.SendClientMessage(Color.Red, $"You have kicked {target.Name} for {reason}.");

            await Task.Delay(Configuration.KickDelay);
            target.Kick();
        }

        [Command("port", Shortcut = "port")]
        public static void OnPortCommand(Player sender, Player target)
        {
            if (sender.Account.Jailed != 0)
            {
                sender.SendClientMessage(Color.Red, Messages.CommandOnlyIfNotJailed);
                return;
            }

            if (sender.Account.Wanted != 0)
            {
                sender.SendClientMessage(Color.Red, Messages.MustBeInnocent);
                return;
            }

            if (!target.IsLoggedIn)
            {
                sender.SendClientMessage(Color.Red, Messages.PlayerNotLoggedIn);
                return;
            }

            if (sender == target)
            {
                sender.SendClientMessage(Color.Red, Messages.CommandNotAallowedOnSelf);
                return;
            }

            sender.VirtualWorld = target.VirtualWorld;
            sender.Interior = target.Interior;
            sender.Position = target.Position + new Vector3(0.0, 0.0, 3);

            sender.SendClientMessage(Color.GreenYellow, $"You have been ported to location: {target.Position.X} {target.Position.Y} {target.Position.Z + 3.0f}");
        }

        [Command("portloc", Shortcut = "portloc")]
        public static void OnPortlocCommand(Player sender, float x, float y, float z)
        {
            if (sender.Account.Jailed != 0)
            {
                sender.SendClientMessage(Color.Red, Messages.CommandOnlyIfNotJailed);
                return;
            }

            if (sender.Account.Wanted != 0)
            {
                sender.SendClientMessage(Color.Red, Messages.MustBeInnocent);
                return;
            }

            sender.Position = new Vector3(x, y, z);
            sender.SendClientMessage(Color.GreenYellow, $"You have been ported to location: {x} {y} {z}");
        }

        [Command("repair", Shortcut = "repair")]
        public static void OnRepairCommand(BasePlayer sender)
        {
            if (sender.IsPlayerInBuilding())
            {
                sender.SendClientMessage(Color.Red, Messages.CommandNotAllowedInsideBuilding);
                return;
            }

            if (!sender.IsPlayerDriving())
            {
                sender.SendClientMessage(Color.Red, Messages.CommandAllowedOnlyAsDriver);
                return;
            }

            sender.Vehicle.Repair();

            sender.SendClientMessage(Color.Blue, "Your vehicle has been repaired.");
        }

        [Command("reports", Shortcut = "reports")]
        public static void OnReportsCommand(BasePlayer sender)
        {
            if (Report.Reports[0].IsEmpty)
            {
                sender.SendClientMessage(Color.Red, Messages.NoReports);
                return;
            }

            var reportsDialog = new ListDialog("Reports list:", "Ok");

            for (int i = 0; i < Report.Reports.Length; i++)
            {
                if (!Report.Reports[i].IsEmpty)
                    reportsDialog.AddItem($"{Report.Reports[i].OffenderName}: {Report.Reports[i].Reason}");
            }

            reportsDialog.Show(sender);
        }

        [Command("spawnvehicle", Shortcut = "spawnvehicle")]
        public static void OnSpawnVehicleCommand(BasePlayer sender, VehicleModelType model)
        {
            if (sender.Vehicle != null)
            {
                sender.SendClientMessage(Color.IndianRed, Messages.AlreadyInAVehicle);
                return;
            }

            var car = (Vehicle)BaseVehicle.Create(model, sender.Position + Vector3.One, sender.Angle, -1, -1, 600);
            car.Engine = true;
            car.IsAdminSpawned = true;
            sender.PutInVehicle(car, 0);
        }

        [Command("tele", Shortcut = "tele")]
        public static void OnTeleCommand(BasePlayer sender, Player player1, Player player2)
        {
            if (player1 == player2)
            {
                sender.SendClientMessage(Color.Red, Messages.PlayersMustBeDifferent);
                return;
            }

            if (!player1.IsLoggedIn)
            {
                sender.SendClientMessage(Color.Red, Messages.PlayerOneNotLoggedIn);
                return;
            }

            if (!player2.IsLoggedIn)
            {
                sender.SendClientMessage(Color.Red,  Messages.PlayerTwoNotLoggedIn);
                return;
            }

            player1.VirtualWorld = player2.VirtualWorld;
            player1.Interior = player2.Interior;
            player1.Position = player2.Position + new Vector3(0.0, 0.0, 3);

            player1.SendClientMessage(Color.GreenYellow, $"{{00FF00}}You have been ported to player {{FFFF00}}{player2.Name}{{00FF00}} by {{FFFF00}}{sender.Name}");
        }

        [Command("warn", Shortcut = "warn")]
        public static async void OnWarnCommand(Player sender, Player target, string reason)
        {
            if (!target.IsLoggedIn)
            {
                sender.SendClientMessage(Color.Red, Messages.PlayerNotLoggedIn);
                return;
            }

            if (sender == target)
            {
                sender.SendClientMessage(Color.Red, Messages.CommandNotAallowedOnSelf);
                return;
            }

            target.Warnings++;

            target.SendClientMessage(Color.Red, $"You have been warned by {AdminRanks.AdminLevelNames[sender.Account.AdminLevel]} {sender.Name}.");
            target.SendClientMessage(Color.Red, $"Reason: {reason}");
            target.GameText($"~w~Warning {target.Warnings}/{Configuration.MaxWarnBeforeKick}: ~r~{reason}", 5000, 4);

            sender.SendClientMessage(Color.GreenYellow, $"You have warned {target.Name} (warnings: {target.Warnings}/{Configuration.MaxWarnBeforeKick}).");
            sender.SendClientMessage(Color.Red, $"Reason: {reason}");

            if (target.Warnings == Configuration.MaxWarnBeforeKick && Configuration.CanAutoKickAfterWarn)
            {
                target.SendClientMessage(Color.Red, $"This was the {Configuration.MaxWarnBeforeKick}th and last warning. You have been kicked.");
                await Task.Delay(Configuration.KickDelay);
                target.Kick();
            }
        }

        [Command("portvehicle", Shortcut = "portvehicle")]
        public static void OnPortVehicleCommand(Player sender, BaseVehicle vehicle)
        {
            if (sender.Account.Jailed != 0)
            {
                sender.SendClientMessage(Color.Red, Messages.CommandOnlyIfNotJailed);
                return;
            }

            if (sender.Account.Wanted != 0)
            {
                sender.SendClientMessage(Color.Red, Messages.MustBeInnocent);
                return;
            }

            if (vehicle == null)
            {
                sender.SendClientMessage(Color.Red, Messages.VehicleIsNotValid);
                return;
            }

            sender.Position = vehicle.Position + new Vector3(0, 0, 5);
            sender.SendClientMessage(Color.GreenYellow, $"You have been ported to location: {vehicle.Position.X} {vehicle.Position.Y} {vehicle.Position.Z + 5.0f}");
        }

        [Command("fuel", Shortcut = "fuel")]
        public static void OnFuelCommand(BasePlayer sender)
        {
            if (!sender.IsPlayerDriving())
            {
                sender.SendClientMessage(Color.Red, Messages.CommandAllowedOnlyAsDriver);
                return;
            }

            Vehicle playerCar = (Vehicle)sender.Vehicle;
            playerCar.Fuel = Configuration.MaxFuel;

            sender.SendClientMessage(Color.GreenYellow, Messages.VehicleHasBeenRefuelled);
        }

        [Command("get", Shortcut = "get")]
        public static void OnGetCommand(BasePlayer sender, Player target)
        {
            if (!target.IsLoggedIn)
            {
                sender.SendClientMessage(Color.Red, Messages.PlayerNotLoggedIn);
                return;
            }

            if (sender == target)
            {
                sender.SendClientMessage(Color.Red, Messages.CommandNotAallowedOnSelf);
                return;
            }

            if (target.Account.Jailed != 0)
            {
                sender.SendClientMessage(Color.Red, Messages.TargetIsinJailCommandNotAllowed);
                return;
            }

            target.VirtualWorld = sender.VirtualWorld;
            target.Interior = sender.Interior;
            target.Position = sender.Position + new Vector3(0.0, 0.0, 3);

            sender.SendClientMessage(Color.GreenYellow, $"{{00FF00}}You have ported {{FFFF00}}{target.Name}{{00FF00}} to your location.");
        }

        [Command("nos", Shortcut = "nos")]
        public static void OnNosCommand(BasePlayer sender)
        {
            if (!sender.IsPlayerDriving())
            {
                sender.SendClientMessage(Color.Red, Messages.CommandAllowedOnlyAsDriver);
                return;
            }

            // TODO: Owned vehicles need to buy
            sender.Vehicle.AddComponent(1010);
            sender.SendClientMessage(Color.GreenYellow, Messages.NosHasBeenAddedToTheVehicle);
        }

        [Command("mute", Shortcut = "mute")]
        public static async void OnMuteCommand(BasePlayer sender, Player target, int duration, string reason)
        {
            if (!target.IsLoggedIn)
            {
                sender.SendClientMessage(Color.Red, Messages.PlayerNotLoggedIn);
                return;
            }

            if (sender == target)
            {
                sender.SendClientMessage(Color.Red, Messages.CommandNotAallowedOnSelf);
                return;
            }

            if (target.Account.Muted > DateTime.Now)
            {
                sender.SendClientMessage(Color.Red, Messages.TargetIsAlreadyMuted);
                return;
            }

            if (duration < 1 || duration > 60)
            {
                sender.SendClientMessage(Color.Red, Messages.MuteDurationMustBeInRange);
                return;
            }

            target.GameText("~w~You have been~n~~r~muted", 2000, 6);

            DateTime currentTime = DateTime.Now;

            var account = target.Account;
            account.Muted = currentTime.AddMinutes(duration);

            await new PlayerAccountRepository().UpdateAsync(account);

            target.SendClientMessage(Color.Red, $"You have been muted by {{FFFF00}}{sender.Name} {{FF0000}}for {{FFFF00}}{reason}");
            target.ShowRemainingMuteTime();

            sender.SendClientMessage($"{{00FF00}}You have muted {{FFFF00}}{target.Name}");
        }

        [Command("unmute", Shortcut = "unmute")]
        public static async void OnUnMuteCommand(BasePlayer sender, Player target)
        {
            if (!target.IsLoggedIn)
            {
                sender.SendClientMessage(Color.Red, Messages.PlayerNotLoggedIn);
                return;
            }

            if (sender == target)
            {
                sender.SendClientMessage(Color.Red, Messages.CommandNotAallowedOnSelf);
                return;
            }

            if (target.Account.Muted < DateTime.Now)
            {
                sender.SendClientMessage(Color.Red, Messages.TargetIsNotMued);
                return;
            }

            var account = target.Account;
            account.Muted = DateTime.Now;

            await new PlayerAccountRepository().UpdateAsync(account);

            target.SendClientMessage(Color.GreenYellow, $"You have been un-muted by {{FFFF00}}{sender.Name}");

            sender.SendClientMessage(Color.GreenYellow, $"You have un-muted {{FFFF00}}{target.Name}");
        }

        [Command("muted", Shortcut = "muted")]
        public static void OnMutedCommand(BasePlayer sender)
        {
            var mutedPlayersDialog = new ListDialog("Muted players:", "Ok");

            foreach (Player player in Player.All)
            {
                if (!player.IsLoggedIn)
                    continue;

                if (player.Account.Muted > DateTime.Now)
                {
                    mutedPlayersDialog.AddItem($"{player.Name} (ID:{player.Id})");
                }
            }

            if (mutedPlayersDialog.Items.Count != 0)
                mutedPlayersDialog.Show(sender);
            else
                sender.SendClientMessage(Color.GreenYellow, Messages.ThereAreNoMutedPlayers);
        }

        [Command("freeze", Shortcut = "freeze")]
        public static void OnFreezeCommand(BasePlayer sender, Player target, int seconds, string reason)
        {
            if (!target.IsLoggedIn)
            {
                sender.SendClientMessage(Color.Red, Messages.PlayerNotLoggedIn);
                return;
            }

            if (sender == target)
            {
                sender.SendClientMessage(Color.Red, Messages.CommandNotAallowedOnSelf);
                return;
            }

            if (seconds < 1)
            {
                sender.SendClientMessage(Color.Red, Messages.ValueNeedToBePositive);
                return;
            }

            if (target.FrozenTime > 0)
            {
                sender.SendClientMessage(Color.Red, Messages.PlayerIsAlreadyFrozen);
                return;
            }

            target.FrozenTime = seconds;
            target.ToggleControllable(false);

            Timer freezeTimer = new Timer(1000, true);
            freezeTimer.Tick += (senderObject, args) =>
            {
                target.FrozenTime--;

                if (target.FrozenTime <= 0)
                {
                    target.ToggleControllable(true);
                    target.FrozenTime = 0;

                    freezeTimer.IsRepeating = false;
                    freezeTimer.IsRunning = false;
                    freezeTimer.Dispose();
                }
            };

            target.SendClientMessage(Color.Red, $"You have been frozen by {{FFFF00}}{sender.Name} {{FF0000}}for {{FFFF00}}{reason}");
            sender.SendClientMessage(Color.GreenYellow, $"You have frozen {{FFFF00}}{target.Name}");
        }

        [Command("unfreeze", Shortcut = "unfreeze")]
        public static void OnUnfreezeCommand(BasePlayer sender, Player target)
        {
            if (!target.IsLoggedIn)
            {
                sender.SendClientMessage(Color.Red, Messages.PlayerNotLoggedIn);
                return;
            }

            if (sender == target)
            {
                sender.SendClientMessage(Color.Red, Messages.CommandNotAallowedOnSelf);
                return;
            }

            if (target.FrozenTime <= 0)
            {
                sender.SendClientMessage(Color.Red, Messages.PlayerIsNotFrozen);
                return;
            }

            target.FrozenTime = 0;

            target.SendClientMessage(Color.Red, $"You have been un-frozen by {{FFFF00}}{sender.Name}");
            sender.SendClientMessage(Color.GreenYellow, $"You have un-frozen {{FFFF00}}{target.Name}");
        }

        [Command("respawnvehicle", Shortcut = "respawnvehicle")]
        public static void OnRespawnVehicleCommand(BasePlayer sender, BaseVehicle vehicle)
        {
            if (vehicle == null)
            {
                sender.SendClientMessage(Color.Red, Messages.VehicleIsNotValid);
                return;
            }

            vehicle.Respawn();
            sender.SendClientMessage($"{{00FF00}}You've respawned vehicle {{FFFF00}}{vehicle.Id}");
        }

        [Command("achat", Shortcut = "achat")]
        public static void OnAchatCommand(BasePlayer sender, string message)
        {
            foreach (Player player in Player.All)
            {
                if (!player.IsLoggedIn)
                    continue;

                if (player.Account.AdminLevel < 1)
                    continue;

                player.SendClientMessage($"{{A0A0A0}}(Admin Chat) {{D0D0D0}}{sender.Name}: {{FFFFFF}}{message}");
            }
        }

        [Command("spec", Shortcut = "spec")]
        public static void OnSpecCommand(Player sender, Player target)
        {
            if (sender == target)
            {
                sender.SendClientMessage(Color.Red, Messages.CommandNotAallowedOnSelf);
                return;
            }

            if (!target.IsLoggedIn)
            {
                sender.SendClientMessage(Color.Red, Messages.PlayerNotLoggedIn);
                return;
            }

            if (target.State == PlayerState.Spectating)
            {
                sender.SendClientMessage(Color.Red, Messages.PlayerIsSpectacting);
                return;
            }

            if (target.State == PlayerState.Wasted || target.State == PlayerState.None)
            {
                sender.SendClientMessage(Color.Red, Messages.PlayerIsDeadOrNotSpawned);
                return;
            }

            if (sender.State != PlayerState.Spectating)
            {
                sender.SpectatePosition = sender.Position;
                sender.IsSpectating = true;
            }

            sender.ToggleSpectating(true);

            if (target.VehicleSeat == -1)
            {
                sender.SpectatePlayer(target);
                sender.Interior = target.Interior;
                sender.SpectatedPlayer = target;
                sender.SpectateType = SpectateTypes.Player;
            }
            else
            {
                sender.SpectateVehicle(target.Vehicle);
                sender.SpectatedPlayer = target;
                sender.SpectatedVehicle = target.Vehicle;
                sender.SpectateType = SpectateTypes.Vehicle;
            }

            sender.SendClientMessage($"{{00FF00}}You're spectating {{FFFF00}}{target.Name}");
        }

        [Command("endspec", Shortcut = "endspec")]
        public static void OnEndSpecCommand(Player sender)
        {
            if (sender.State != PlayerState.Spectating)
            {
                sender.SendClientMessage(Color.Red, Messages.PlayerIsNotSpectacting);
                return;
            }

            sender.ToggleSpectating(false);
            sender.SpectatedPlayer = null;
            sender.SpectatedVehicle = null;
            sender.SpectateType = SpectateTypes.None;
        }
    }
}