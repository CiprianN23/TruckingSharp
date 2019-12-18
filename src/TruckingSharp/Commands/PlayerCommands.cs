using SampSharp.GameMode;
using SampSharp.GameMode.Definitions;
using SampSharp.GameMode.Display;
using SampSharp.GameMode.Events;
using SampSharp.GameMode.SAMP;
using SampSharp.GameMode.SAMP.Commands;
using SampSharp.GameMode.World;
using System.Linq;
using System.Text;
using TruckingSharp.Commands.Permissions;
using TruckingSharp.Constants;
using TruckingSharp.Data;
using TruckingSharp.Database.Entities;
using TruckingSharp.Database.Repositories;
using TruckingSharp.Extensions.PlayersExtensions;
using TruckingSharp.PlayerClasses.Data;
using TruckingSharp.Services;

namespace TruckingSharp.Commands
{
    [CommandGroup("player", PermissionChecker = typeof(LoggedPermission))]
    public class PlayerCommands
    {
        [Command("assist", Shortcut = "assist")]
        public static async void OnAssistCommandAsync(Player sender)
        {
            if (!sender.IsDriving())
            {
                sender.SendClientMessage(Color.Red, Messages.CommandAllowedOnlyAsDriver);
                return;
            }

            bool assistPlayerOnline = false;

            foreach (var basePlayer in BasePlayer.All)
            {
                var serverPlayer = (Player)basePlayer;

                if (!serverPlayer.IsLoggedIn)
                    continue;

                if (serverPlayer.PlayerClass == PlayerClassType.Assistance)
                {
                    assistPlayerOnline = true;
                    serverPlayer.SendClientMessage(Color.GreenYellow, $"Player {{FFFF00}}{sender.Name}{{00FF00}} needs assistance, go help him.");
                }
            }

            if (assistPlayerOnline)
            {
                sender.AssistanaceNeeded = true;
                sender.SendClientMessage(Color.GreenYellow, "You called for assistance.");
            }
            else
            {
                if (sender.IsDriving())
                {
                    var senderVehicle = (Vehicle)sender.Vehicle;
                    senderVehicle.Repair();
                    senderVehicle.Fuel = Configuration.Instance.MaximumFuel;

                    sender.SendClientMessage(Color.GreenYellow, $"Your vehicle has been auto-repaired and refuelled for {{FFFF00}}${Configuration.Instance.AutoAssistPrice} {{00FF00}}as there is no assistance player online.");

                    await sender.RewardAsync(-Configuration.Instance.AutoAssistPrice);
                }
            }
        }

        [Command("cmds", Shortcut = "cmds")]
        public static void OnCmdsCommand(Player sender)
        {
            var commandList = new ListDialog("Commands:", "Select", "Cancel");

            commandList.AddItem("Player\nAdmin level 1\nAdmin level 2\nAdmin level 3");
            commandList.Show(sender);
            commandList.Response += (senderObject, e) =>
            {
                if (e.DialogButton != DialogButton.Left)
                    return;

                var commandMessageDialog = new MessageDialog("Commands:", "", "Back", "Cancel");
                switch (e.ListItem)
                {
                    case 0:
                        var playerCommands = new StringBuilder();

                        playerCommands.AppendLine("/admins - Displays all online admins");
                        playerCommands.AppendLine("/assist - Call for assistance to repair/refuel your vehicle");
                        playerCommands.AppendLine("/bank - Register/login/manage your bank account");
                        playerCommands.AppendLine("/bonus - Shows the current bonus mission (only for trucker class)");
                        playerCommands.AppendLine("/changepassword - Allows the player to change his login password");
                        playerCommands.AppendLine("/convoy - Lets you start or join a convoy");
                        playerCommands.AppendLine("/convoycancel - Cancels the convoy you're in");
                        playerCommands.AppendLine("/convoykick <Player> - Kicks a member from the convoy");
                        playerCommands.AppendLine("/convoyleave - You leave the convoy");
                        playerCommands.AppendLine("/convoymembers - Displays all convoy members");
                        playerCommands.AppendLine("/detach - Detaches your trailer");
                        playerCommands.AppendLine("/eject <Player> - Ejects a player from your vehicle");
                        playerCommands.AppendLine("/engine - Toggle engine-status on or off");
                        playerCommands.AppendLine("/flip - Flips your vehicle back onto it's wheels");
                        playerCommands.AppendLine("/givecash <Player> <Amount> - Give money to a player");
                        playerCommands.AppendLine("/gobase - Teleports you to a location where your class starts");
                        playerCommands.AppendLine("/me <Action> - Just a silly command to repeat your text");
                        playerCommands.AppendLine("/overload - Lets a trucker to overload his vehicle");
                        playerCommands.AppendLine("/pm <Player> <Message> - Send a private message to a player");
                        playerCommands.AppendLine("/reclass - Choose another class");
                        playerCommands.AppendLine("/report <Player> <Message> - Report a player for breaking the rules");
                        playerCommands.AppendLine("/rules - Display the rules and get a small gift (only once)");
                        playerCommands.AppendLine("/radio <Message> - Say something to players that have same class as you");
                        playerCommands.AppendLine("/stats - Displays statistics about yourself");
                        playerCommands.AppendLine("/stopwork - Ends the current job");
                        playerCommands.AppendLine("/work - Starts a job");

                        commandMessageDialog.Message = playerCommands.ToString();
                        break;

                    case 1:
                        var adminLevel1Commands = new StringBuilder();

                        adminLevel1Commands.AppendLine("/achat - Admin-chat (this allows admins to chat privately)");
                        adminLevel1Commands.AppendLine("/spawnvehicle <Vehicle> - Spawn the specified vehicle");
                        adminLevel1Commands.AppendLine("/endspec - Stop spectating a player");
                        adminLevel1Commands.AppendLine("/freeze <Player> <Duration> <Reason> - Freeze a player for a certain time in seconds");
                        adminLevel1Commands.AppendLine("/unfreeze <Player> - Unfreeze a player");
                        adminLevel1Commands.AppendLine("/fuel - Refuels your vehicle for free");
                        adminLevel1Commands.AppendLine("/get <Player> - Teleports a player to your location");
                        adminLevel1Commands.AppendLine("/jail <Player> <Duration> <Reason> - Jails a player for a certain time in seconds");
                        adminLevel1Commands.AppendLine("/unjail <Player> - Unjail a player");
                        adminLevel1Commands.AppendLine("/kick <Player> <Reason> - Kick a player");
                        adminLevel1Commands.AppendLine("/mute <Player> <Duration> <Reason> - Mute a player for x minutes");
                        adminLevel1Commands.AppendLine("/muted - Display muted players");
                        adminLevel1Commands.AppendLine("/nos - Adds nitro to your vehicle");
                        adminLevel1Commands.AppendLine("/port <Player> - Teleport yourself to a player");
                        adminLevel1Commands.AppendLine("/portloc <x> <y> <z> - Teleport yourself to the given coordinates");
                        adminLevel1Commands.AppendLine("/portvehicle <Vehicle ID> - Teleport yourself to a vehicle");
                        adminLevel1Commands.AppendLine("/reports - Show the last 50 reports in a dialog");
                        adminLevel1Commands.AppendLine("/repair - Repairs your vehicle for free");
                        adminLevel1Commands.AppendLine("/respawnvehicle <Vehicle ID> - Forces a vehicle to respawn");
                        adminLevel1Commands.AppendLine("/spec <Player> - Spectate a player");
                        adminLevel1Commands.AppendLine("/tele <Player 1> <Player 2> - Teleport a player to an other player");
                        adminLevel1Commands.AppendLine("/unmute <Player> - Un-mutes a player");
                        adminLevel1Commands.AppendLine("/warn <Player> <Reason> - Warn a player");
                        adminLevel1Commands.AppendLine("/announce <Style (0-6)> <Duration> <Message> - Shows a message on player's screen");

                        commandMessageDialog.Message = adminLevel1Commands.ToString();
                        break;

                    case 2:
                        var adminLevel2Commands = new StringBuilder();

                        adminLevel2Commands.AppendLine("/setwanted <Player> <Stars (0-6)> - Set a player's wanted level");
                        adminLevel2Commands.AppendLine("/ban <Player> <Days> <Reason> - Ban a player for a certain time");
                        adminLevel2Commands.AppendLine("/caroption - Changes some options for your vehicle");
                        adminLevel2Commands.AppendLine("/cleanupallvehicles - Removes all admin spawned vehicles from the map");
                        adminLevel2Commands.AppendLine("/cleanupvehicle <Vehicle ID> - Deletes a vehicle spawned by an admin");
                        adminLevel2Commands.AppendLine("/fly - Equips yourself with a jetpack");
                        adminLevel2Commands.AppendLine("/fuelall - Fuel all vehicles for free");
                        adminLevel2Commands.AppendLine("/givelicense <Player> - Gives a free trucker license to a player");
                        adminLevel2Commands.AppendLine("/healall - Heals all the players");
                        adminLevel2Commands.AppendLine("/ipban <Player> <Reason> - Ban a player's IP");
                        adminLevel2Commands.AppendLine("/loc - Display your current location");
                        adminLevel2Commands.AppendLine("/rangeban <Player> <Reason> - Ban a player's entire IP-range");
                        adminLevel2Commands.AppendLine("/repairall - Repair all vehicles for free");
                        adminLevel2Commands.AppendLine("/setscore <Player> <Amount> - Sets a player's score to the given value");
                        adminLevel2Commands.AppendLine("/unban <Player> - Unban a player");
                        adminLevel2Commands.AppendLine("/setskin <Player> <Skin ID (0-311)> - Changes a player's skin-id");
                        adminLevel2Commands.AppendLine("/weather - Changes the weather");

                        commandMessageDialog.Message = adminLevel2Commands.ToString();
                        break;

                    case 3:
                        var adminLevel3Comamnds = new StringBuilder();

                        adminLevel3Comamnds.AppendLine("/createcamera <Max Speed> - Create s speedcamera at your location");
                        adminLevel3Comamnds.AppendLine("/deletecamera - Delete a speedcamera");
                        adminLevel3Comamnds.AppendLine("/resetplayer <Player> <Money (0/1)> <Score (0/1)> <Stats (0/1)> <Reason> - Reset a player's money, score, stats");
                        adminLevel3Comamnds.AppendLine("/setadmin <Player> <Admin Level (0-3)> - Changes a player's admin-level");

                        commandMessageDialog.Message = adminLevel3Comamnds.ToString();
                        break;
                }

                commandMessageDialog.Show(sender);
                commandMessageDialog.Response += (objectSender, ev) =>
                {
                    if (ev.DialogButton == DialogButton.Left)
                        commandList.Show(sender);
                };
            };
        }

        [Command("admins", Shortcut = "admins")]
        public static void OnAdminsCommand(BasePlayer sender)
        {
            var dialogAdmins = new ListDialog("Online admins:", "Close");

            foreach (var basePlayer in Player.All)
            {
                var player = (Player)basePlayer;

                if (!player.IsLoggedIn)
                    continue;

                if (player.IsAdmin)
                {
                    dialogAdmins.AddItem(
                        $"{AdminRanks.AdminLevelNames[player.Account.AdminLevel]}: {player.Name} (id: {player.Id}), admin-level: {player.Account.AdminLevel} (RCON admin)");
                    continue;
                }

                if (player.Account.AdminLevel > 0)
                    dialogAdmins.AddItem(
                        $"{AdminRanks.AdminLevelNames[player.Account.AdminLevel]}: {player.Name} (id: {player.Id}), admin-level: {player.Account.AdminLevel}");
            }

            if (dialogAdmins.Items.Count > 0)
                dialogAdmins.Show(sender);
            else
                sender.SendClientMessage(Color.Red, Messages.NoAdminOnline);
        }

        [Command("bank", Shortcut = "bank")]
        public static void OnBankCommand(Player sender)
        {
            if (!sender.IsLoggedInBankAccount)
            {
                if (sender.BankAccount == null)
                {
                    var registerNewBankAccountDialog = new InputDialog("Enter a password",
                        "Please enter a password to register your bank account:", true, "Accept", "Cancel");
                    registerNewBankAccountDialog.Show(sender);
                    registerNewBankAccountDialog.Response += async (senderObject, e) =>
                    {
                        if (e.DialogButton != DialogButton.Left)
                            return;

                        if (e.InputText.Length < 1 || e.InputText.Length > 20)
                        {
                            sender.SendClientMessage(Color.Red, Messages.InvalidPasswordLength, 1, 20);
                            registerNewBankAccountDialog.Show(sender);
                            return;
                        }

                        var hash = PasswordHashingService.GetPasswordHash(e.InputText);
                        var newBankAccount = new PlayerBankAccount { Password = hash, PlayerId = sender.Account.Id };
                        await new PlayerBankAccountRepository(ConnectionFactory.GetConnection).AddAsync(newBankAccount);

                        sender.SendClientMessage(Color.GreenYellow, Messages.BankAccountCreatedSuccessfully);
                    };
                }
                else
                {
                    var loginBankAccount = new InputDialog("Enter a password",
                        "Enter a password to login to your bank account:", true, "Accept", "Cancel");
                    loginBankAccount.Show(sender);
                    loginBankAccount.Response += (senderObject, e) =>
                    {
                        if (e.InputText.Length < 1 || e.InputText.Length > 20)
                        {
                            sender.SendClientMessage(Color.Red, Messages.InvalidPasswordLength, 1, 20);
                            loginBankAccount.Show(sender);
                            return;
                        }

                        if (!PasswordHashingService.VerifyPasswordHash(e.InputText, sender.BankAccount.Password))
                        {
                            sender.SendClientMessage(Color.Red, Messages.InvalidPasswordInputted);
                            loginBankAccount.Show(sender);
                            return;
                        }

                        sender.IsLoggedInBankAccount = true;
                        sender.SendClientMessage(Color.GreenYellow, Messages.BankAccountLoggedInSuccessfully);
                        sender.ShowBankAccountOptions();
                    };
                }
            }
            else
            {
                sender.ShowBankAccountOptions();
            }
        }

        [Command("changepassword", Shortcut = "changepassword")]
        public static void OnChangedPasswordCommand(Player sender)
        {
            var oldPasswordDialog = new InputDialog("Enter your old password", "Enter your old password:", true, "Next", "Close");
            oldPasswordDialog.Show(sender);
            oldPasswordDialog.Response += (senderObject, ev) =>
            {
                if (ev.DialogButton != DialogButton.Left)
                    return;

                if (!PasswordHashingService.VerifyPasswordHash(ev.InputText, sender.Account.Password))
                {
                    sender.SendClientMessage(Color.Red, Messages.PasswordsDontMatch);
                    oldPasswordDialog.Show(sender);
                    return;
                }

                var newPasswordDialog = new InputDialog("Enter your new password", "Enter your new password:", true, "Next", "Close");
                newPasswordDialog.Show(sender);
                newPasswordDialog.Response += (objectSender, e) =>
                {
                    if (e.DialogButton != DialogButton.Left)
                        return;

                    if (e.InputText.Length < 1)
                    {
                        sender.SendClientMessage(Color.Red, Messages.PasswordCanNotBeEmptyOrNull);
                        newPasswordDialog.Show(sender);
                        return;
                    }

                    if (PasswordHashingService.VerifyPasswordHash(e.InputText, sender.Account.Password))
                    {
                        sender.SendClientMessage(Color.Red, Messages.PasswordCanNotBeAsTheOldOne);
                        newPasswordDialog.Show(sender);
                        return;
                    }

                    var confirmPasswordDialog = new MessageDialog("Confirm password change",
                        "Are you sure you want to change your password?", "Yes", "No");
                    confirmPasswordDialog.Show(sender);
                    confirmPasswordDialog.Response += async (objectSender1, evv) =>
                    {
                        if (evv.DialogButton != DialogButton.Left)
                            return;

                        var account = sender.Account;
                        account.Password = PasswordHashingService.GetPasswordHash(e.InputText);
                        await new PlayerAccountRepository(ConnectionFactory.GetConnection).UpdateAsync(account);

                        sender.SendClientMessage(Color.GreenYellow, Messages.PasswordChangedSuccessfully);
                    };
                };
            };
        }

        [Command("detach", Shortcut = "detach")]
        public static void OnDetachCommand(BasePlayer sender)
        {
            if (sender.IsInBuilding())
            {
                sender.SendClientMessage(Color.Red, Messages.CommandNotAllowedInsideBuilding);
                return;
            }

            if (!sender.IsDriving())
            {
                sender.SendClientMessage(Color.Red, Messages.CommandAllowedOnlyAsDriver);
                return;
            }

            var playerVehicle = sender.Vehicle;

            if (!playerVehicle.HasTrailer)
            {
                sender.SendClientMessage(Color.Red, Messages.NoTrailerAttached);
                return;
            }

            playerVehicle.Trailer = null;

            sender.SendClientMessage(Color.Blue, Messages.TrailerDetached);
        }

        [Command("eject", Shortcut = "eject")]
        public static void OnEjectCommand(BasePlayer sender, Player target)
        {
            if (sender == target)
            {
                sender.SendClientMessage(Color.Red, Messages.CommandNotAllowedOnSelf);
                return;
            }

            if (!target.IsLoggedIn)
            {
                sender.SendClientMessage(Color.Red, Messages.PlayerNotLoggedIn);
                return;
            }

            if (sender.VehicleSeat != 0)
            {
                sender.SendClientMessage(Color.Red, Messages.CommandAllowedOnlyAsDriver);
                return;
            }

            if (target.Vehicle != sender.Vehicle)
            {
                sender.SendClientMessage(Color.Red, Messages.PlayerNotInVehicle);
                return;
            }

            target.RemoveFromVehicle();
            target.SendClientMessage(Color.Red, $"You have been ejected from vehicle by {{FFFF00}} {sender.Name}");

            sender.SendClientMessage(Color.White,
                $"You have ejected {{FFFF00}}{target.Name}{{00FF00}} from your vehicle.");
        }

        [Command("engine", Shortcut = "engine")]
        public static void OnEngineCommand(BasePlayer sender)
        {
            if (!sender.IsDriving())
            {
                sender.SendClientMessage(Color.Red, Messages.CommandAllowedOnlyAsDriver);
                return;
            }

            if (sender.Vehicle.Engine)
            {
                sender.SendClientMessage(Color.Green, Messages.VehicleEngineTurnedOff);
                sender.Vehicle.Engine = false;
            }
            else
            {
                sender.SendClientMessage(Color.Green, Messages.VehicleEngineTurnedOn);
                sender.Vehicle.Engine = true;
            }
        }

        [Command("flip", Shortcut = "flip")]
        public static void OnFlipCommand(BasePlayer sender)
        {
            if (sender.IsInBuilding())
            {
                sender.SendClientMessage(Color.Red, Messages.CommandNotAllowedInsideBuilding);
                return;
            }

            if (!sender.IsDriving())
            {
                sender.SendClientMessage(Color.Red, Messages.CommandAllowedOnlyAsDriver);
                return;
            }

            sender.PutCameraBehindPlayer();
            sender.Vehicle.Position = sender.Position;
            sender.Vehicle.Angle = 0.0f;

            sender.SendClientMessage(Color.Blue, Messages.VehicleFlipped);
        }

        [Command("gobase", Shortcut = "gobase")]
        public static void OnGoBaseCommand(Player sender)
        {
            if (sender.State != PlayerState.OnFoot)
            {
                sender.SendClientMessage(Color.Red, Messages.MustBeOnFoot);
                return;
            }

            if (sender.Account.Wanted != 0)
            {
                sender.SendClientMessage(Color.Red, Messages.MustBeInnocent);
                return;
            }

            if (sender.IsDoingMission)
            {
                sender.SendClientMessage(Color.Red, Messages.CommandOnlyIfNotDoingAJob);
                return;
            }

            if (sender.Account.Jailed != 0)
            {
                sender.SendClientMessage(Color.Red, Messages.CommandOnlyIfNotJailed);
                return;
            }

            var dialogSpawns = new ListDialog("Select your spawn", "Select", "Cancel");

            var spawns = new ClassSpawnRepository(ConnectionFactory.GetConnection).GetAllByClassType((int)sender.PlayerClass);

            switch (sender.PlayerClass)
            {
                default:
                    foreach (var spawn in spawns)
                    {
                        dialogSpawns.AddItem($"{spawn.Name}");
                    }

                    dialogSpawns.Response += (senderObject, e) => DialogSpawns_Response(senderObject, e, sender.PlayerClass);
                    break;
            }

            dialogSpawns.Show(sender);
        }

        [Command("me", Shortcut = "me")]
        public static void OnMeCommand(BasePlayer sender, string action)
        {
            if (action.Length > 101)
            {
                sender.SendClientMessage(Color.Silver, Messages.MessageTooLongWithLimit, 101);
                return;
            }

            BasePlayer.SendClientMessageToAll(Color.LightYellow, $"* {sender.Name} {action} {{808080}}(/me)");
        }

        [Command("pm", Shortcut = "pm")]
        public static void OnPmCommand(BasePlayer sender, Player target, string message)
        {
            if (sender == target)
            {
                sender.SendClientMessage(Color.Red, Messages.NoPrivateMessageToYou);
                return;
            }

            if (!target.IsLoggedIn)
            {
                sender.SendClientMessage(Color.Orange, Messages.PlayerNotLoggedIn);
                return;
            }

            if (message.Length > 102)
            {
                sender.SendClientMessage(Color.Silver, Messages.MessageTooLongWithLimit, 102);
                return;
            }

            sender.SendClientMessage(Color.LightGoldenrodYellow, Messages.PrivateMessageTo, target.Name, target.Id, message);
            target.SendClientMessage(Color.LightGoldenrodYellow, Messages.PrivateMessageFrom, sender.Name, sender.Id, message);

            target.PlaySound(1085, Vector3.Zero);
        }

        [Command("radio", Shortcut = "radio")]
        public static void OnRadioCommand(Player sender, string message)
        {
            var className = string.Empty;

            switch (sender.PlayerClass)
            {
                case PlayerClassType.TruckDriver:
                    className = Messages.TruckerClass;
                    break;

                case PlayerClassType.BusDriver:
                    className = Messages.BusDriverClass;
                    break;

                case PlayerClassType.Pilot:
                    className = Messages.PilotClass;
                    break;

                case PlayerClassType.Police:
                    className = Messages.PoliceClass;
                    break;

                case PlayerClassType.Mafia:
                    className = Messages.MafiaClass;
                    break;

                case PlayerClassType.Courier:
                    className = Messages.CourierClass;
                    break;

                case PlayerClassType.Assistance:
                    className = Messages.AssistanceClass;
                    break;

                case PlayerClassType.RoadWorker:
                    className = Messages.RoadWorkerClass;
                    break;
            }

            foreach (var basePlayer in Player.All)
            {
                var player = (Player)basePlayer;

                if (!player.IsLoggedIn)
                    continue;

                if (player.PlayerClass == sender.PlayerClass)
                    player.SendClientMessage(Color.Gray, $"({className} chat) {{D0D0D0}}{sender.Name}: {{FFFFFF}}{message}");
            }
        }

        [Command("reclass", Shortcut = "reclass")]
        public static void OnReclassCommand(Player sender)
        {
            if (sender.IsInBuilding())
            {
                sender.SendClientMessage(Color.Red, Messages.CommandNotAllowedInsideBuilding);
                return;
            }

            if (sender.State != PlayerState.OnFoot)
            {
                sender.SendClientMessage(Color.Red, Messages.MustBeOnFoot);
                return;
            }

            if (sender.Account.Wanted != 0)
            {
                sender.SendClientMessage(Color.Red, Messages.MustBeInnocent);
                return;
            }

            sender.ForceClassSelection();
            sender.Health = 0.0f;
        }

        [Command("report", Shortcut = "report")]
        public static void OnReportCommand(BasePlayer sender, Player target, string reason)
        {
            if (sender == target)
            {
                sender.SendClientMessage(Color.Red, Messages.CommandNotAllowedOnSelf);
                return;
            }

            if (string.IsNullOrEmpty(reason))
            {
                sender.SendClientMessage(Color.Red, Messages.ReasonCanNotBeEmptyOrNull);
                return;
            }

            if (!target.IsLoggedIn)
            {
                sender.SendClientMessage(Color.Red, Messages.PlayerNotLoggedIn);
                return;
            }

            Report.SendReportToAdmins(target, reason);

            sender.SendClientMessage(Color.GreenYellow, $"You have reported {{FFFF00}}{target.Name}");
        }

        [Command("rules", Shortcut = "rules")]
        public static async void OnRulesCommandAsync(Player sender)
        {
            var rules = new StringBuilder();
            rules.AppendLine("1. Always drive on the right side of the road");
            rules.AppendLine("2. No flaming or disrespecting other players");
            rules.AppendLine("3. Main language is english");
            rules.AppendLine("4. Don't use any hacks, you'll be banned");
            rules.AppendLine("5. No spamming on the chat");
            rules.AppendLine("6. No car-jacking allowed");

            var dialogRules = new MessageDialog("Rules of the server:", rules.ToString(), "Accept", "Cancel");

            await dialogRules.ShowAsync(sender);

            dialogRules.Response += async (senderObject, e) =>
            {
                if (e.DialogButton != DialogButton.Left ||
                    sender.Account.RulesRead != 0)
                {
                    return;
                }

                await sender.RewardAsync(5000);

                var account = sender.Account;
                account.RulesRead = 1;

                await new PlayerAccountRepository(ConnectionFactory.GetConnection).UpdateAsync(account);

                sender.SendClientMessage(Color.FromInteger(65280, ColorFormat.RGB), "You have earned {FFFF00}$5000{00FF00} for accepting the rules");
            };
        }

        [Command("givecash", Shortcut = "givecash")]
        public static async void OnGiveCashCommandAsync(Player sender, Player target, int money)
        {
            if (sender == target)
            {
                sender.SendClientMessage(Color.Red, Messages.CommandNotAllowedOnSelf);
                return;
            }

            if (!target.IsLoggedIn)
            {
                sender.SendClientMessage(Color.Red, Messages.PlayerNotLoggedIn);
                return;
            }

            if (money <= 0)
            {
                sender.SendClientMessage(Color.Red, Messages.ValueNeedToBePositive);
                return;
            }

            if (sender.Account.Money < money)
            {
                sender.SendClientMessage(Color.Red, Messages.NotEnoughMoney);
                return;
            }

            await sender.RewardAsync(-money);
            await target.RewardAsync(money);

            target.SendClientMessage($"{{00FF00}}You have received {{FFFF00}}${money}{{00FF00}} from {{FFFF00}}{sender.Name}");
            sender.SendClientMessage($"{{00FF00}}You gave {{FFFF00}}${money}{{00FF00}} to {{FFFF00}}{target.Name}");
        }

        private static void DialogSpawns_Response(object sender, DialogResponseEventArgs e, PlayerClassType classId)
        {
            if (e.DialogButton != DialogButton.Left)
                return;

            if (!(e.Player is Player player))
                return;

            var spawns = new ClassSpawnRepository(ConnectionFactory.GetConnection).GetAllByClassType((int)classId);

            switch (classId)
            {
                default:
                    player.Position = new Vector3(spawns.ElementAt(e.ListItem).PositionX, spawns.ElementAt(e.ListItem).PositionY, spawns.ElementAt(e.ListItem).PositionZ);
                    break;
            }
        }
    }
}