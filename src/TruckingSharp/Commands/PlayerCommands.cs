using SampSharp.GameMode;
using SampSharp.GameMode.Display;
using SampSharp.GameMode.SAMP;
using SampSharp.GameMode.SAMP.Commands;
using SampSharp.GameMode.World;
using System.Text;
using TruckingSharp.Commands.Permissions;
using TruckingSharp.Constants;
using TruckingSharp.Data;
using TruckingSharp.Database.Entities;
using TruckingSharp.Database.Repositories;
using TruckingSharp.Extensions.PlayersExtensions;
using TruckingSharp.PlayerClasses.ClassesSpawn;
using TruckingSharp.PlayerClasses.Data;
using TruckingSharp.Services;

namespace TruckingSharp.Commands
{
    [CommandGroup("player", PermissionChecker = typeof(LoggedPermission))]
    public class PlayerCommands
    {
        [Command("admins", Shortcut = "admins")]
        public static void OnAdminsCommand(BasePlayer sender)
        {
            var dialogAdmins = new ListDialog("Online admins:", "Close");

            foreach (Player player in Player.All)
            {
                if (!player.IsLoggedIn)
                    continue;

                if (player.IsAdmin)
                {
                    dialogAdmins.AddItem($"{AdminRanks.AdminLevelNames[player.Account.AdminLevel]}: {player.Name} (id: {player.Id}), admin-level: {player.Account.AdminLevel} (RCON admin)");
                    continue;
                }

                if (player.Account.AdminLevel > 0)
                {
                    dialogAdmins.AddItem($"{AdminRanks.AdminLevelNames[player.Account.AdminLevel]}: {player.Name} (id: {player.Id}), admin-level: {player.Account.AdminLevel}");
                }
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
                    var registerNewBankAccountDialog = new InputDialog("Enter a password", "Please enter a password to register your bank account:", true, "Accept", "Cancel");
                    registerNewBankAccountDialog.Show(sender);
                    registerNewBankAccountDialog.Response += async (senderObject, e) =>
                    {
                        if (e.DialogButton == SampSharp.GameMode.Definitions.DialogButton.Left)
                        {
                            if (e.InputText.Length < 1 || e.InputText.Length > 20)
                            {
                                sender.SendClientMessage(Color.Red, Messages.InvalidPasswordLenght, 1, 20);
                                registerNewBankAccountDialog.Show(sender);
                                return;
                            }

                            var hash = PasswordHashingService.GetPasswordHash(e.InputText);
                            var newBankAccount = new PlayerBankAccount { Password = hash, PlayerId = sender.Account.Id };
                            await new PlayerBankAccountRepository(ConnectionFactory.GetConnection).AddAsync(newBankAccount);

                            sender.SendClientMessage(Color.GreenYellow, Messages.BankAccountCreatedSuccessfully);
                        }
                    };
                }
                else
                {
                    var loginBankAccount = new InputDialog("Enter a password", "Enter a password to login to your bank account:", true, "Accept", "Cancel");
                    loginBankAccount.Show(sender);
                    loginBankAccount.Response += (senderObject, e) =>
                    {
                        if (e.InputText.Length < 1 || e.InputText.Length > 20)
                        {
                            sender.SendClientMessage(Color.Red, Messages.InvalidPasswordLenght, 1, 20);
                            loginBankAccount.Show(sender);
                            return;
                        }

                        if (!PasswordHashingService.VerifyPasswordHash(e.InputText, sender.BankAccount.Password))
                        {
                            sender.SendClientMessage(Color.Red, Messages.InvalidPasswordInputed);
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
                if (ev.DialogButton == SampSharp.GameMode.Definitions.DialogButton.Left)
                {
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
                        if (e.DialogButton == SampSharp.GameMode.Definitions.DialogButton.Left)
                        {
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

                            var confirmPasswordDialog = new MessageDialog("Confirm password change", "Are you sure you want to change your password?", "Yes", "No");
                            confirmPasswordDialog.Show(sender);
                            confirmPasswordDialog.Response += async (objectSender1, evv) =>
                            {
                                if (evv.DialogButton == SampSharp.GameMode.Definitions.DialogButton.Left)
                                {
                                    var account = sender.Account;
                                    account.Password = PasswordHashingService.GetPasswordHash(e.InputText);
                                    await new PlayerAccountRepository(ConnectionFactory.GetConnection).UpdateAsync(account);

                                    sender.SendClientMessage(Color.GreenYellow, Messages.PasswordChangedSuccessfully);
                                }
                            };
                        }
                    };
                }
            };
        }

        [Command("detach", Shortcut = "detach")]
        public static void OnDetachCommand(BasePlayer sender)
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

            sender.SendClientMessage(Color.White, $"You have ejected {{FFFF00}}{target.Name}{{00FF00}} from your vehicle.");
        }

        [Command("engine", Shortcut = "engine")]
        public static void OnEngineCommand(BasePlayer sender)
        {
            if (!sender.IsPlayerDriving())
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

            sender.PutCameraBehindPlayer();
            sender.Vehicle.Position = sender.Position;
            sender.Vehicle.Angle = 0.0f;

            sender.SendClientMessage(Color.Blue, Messages.VehicleFlipped);
        }

        [Command("gobase", Shortcut = "gobase")]
        public static void OnGobaseCommand(Player sender)
        {
            if (sender.State != SampSharp.GameMode.Definitions.PlayerState.OnFoot)
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

            switch (sender.PlayerClass)
            {
                case PlayerClassType.TruckDriver:
                    dialogSpawns.AddItem("Fallen Tree Depot");
                    dialogSpawns.AddItem("Flint Trucking Depot");
                    dialogSpawns.AddItem("LVA Freight Depot");
                    dialogSpawns.AddItem("Doherty Depot");
                    dialogSpawns.AddItem("El Corona Depot");
                    dialogSpawns.AddItem("Las Payasdas Depot");
                    dialogSpawns.AddItem("Quarry Top");
                    dialogSpawns.AddItem("Shady Creek Depot");

                    dialogSpawns.Response += (sender, e) => DialogSpawns_Response(sender, e, PlayerClassType.TruckDriver);
                    break;

                case PlayerClassType.BusDriver:
                    dialogSpawns.AddItem("Los Santos");
                    dialogSpawns.AddItem("San Fierro");
                    dialogSpawns.AddItem("Las Venturas");

                    dialogSpawns.Response += (sender, e) => DialogSpawns_Response(sender, e, PlayerClassType.BusDriver);
                    break;

                case PlayerClassType.Pilot:
                    dialogSpawns.AddItem("Los Santos");
                    dialogSpawns.AddItem("San Fierro");
                    dialogSpawns.AddItem("Las Venturas");

                    dialogSpawns.Response += (sender, e) => DialogSpawns_Response(sender, e, PlayerClassType.Pilot);
                    break;

                case PlayerClassType.Police:
                    dialogSpawns.AddItem("Los Santos");
                    dialogSpawns.AddItem("San Fierro");
                    dialogSpawns.AddItem("Las Venturas");

                    dialogSpawns.Response += (sender, e) => DialogSpawns_Response(sender, e, PlayerClassType.Police);
                    break;

                case PlayerClassType.Courier:
                    dialogSpawns.AddItem("Los Santos");
                    dialogSpawns.AddItem("San Fierro");
                    dialogSpawns.AddItem("Las Venturas");

                    dialogSpawns.Response += (sender, e) => DialogSpawns_Response(sender, e, PlayerClassType.Courier);
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
                sender.SendClientMessage(Color.Red, Messages.NoPrivMessageToYou);
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

            sender.SendClientMessage(Color.LightGoldenrodYellow, Messages.PrivMessageTo, target.Name, target.Id, message);
            target.SendClientMessage(Color.LightGoldenrodYellow, Messages.PrivMessageFrom, sender.Name, sender.Id, message);

            target.PlaySound(1085, Vector3.Zero);
        }

        [Command("radio", Shortcut = "radio")]
        public static void OnRadioCommand(Player sender, string message)
        {
            string className = string.Empty;

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

            foreach (Player player in Player.All)
            {
                if (!player.IsLoggedIn)
                    continue;

                if (player.PlayerClass == sender.PlayerClass)
                {
                    player.SendClientMessage(Color.Gray, $"({className} chat) {{D0D0D0}}{sender.Name}: {{FFFFFF}}{message}");
                }
            }
        }

        [Command("reclass", Shortcut = "reclass")]
        public static void OnReclassCommand(Player sender)
        {
            if (sender.IsPlayerInBuilding())
            {
                sender.SendClientMessage(Color.Red, Messages.CommandNotAllowedInsideBuilding);
                return;
            }

            if (sender.State != SampSharp.GameMode.Definitions.PlayerState.OnFoot)
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
            StringBuilder Rules = new StringBuilder();
            Rules.AppendLine("1. Always drive on the right side of the road");
            Rules.AppendLine("2. No flaming or disrespecting other players");
            Rules.AppendLine("3. Main language is english");
            Rules.AppendLine("4. Don't use any hacks, you'll be banned");
            Rules.AppendLine("5. No spamming on the chat");
            Rules.AppendLine("6. No car-jacking allowed");

            var dialogRules = new MessageDialog("Rules of the server:", Rules.ToString(), "Accept", "Cancel");

            await dialogRules.ShowAsync(sender);

            dialogRules.Response += async (senderObject, e) =>
            {
                if (e.DialogButton == SampSharp.GameMode.Definitions.DialogButton.Left && sender.Account.RulesRead == 0)
                {
                    sender.Reward(5000);

                    var account = sender.Account;
                    account.RulesRead = 1;

                    await new PlayerAccountRepository(ConnectionFactory.GetConnection).UpdateAsync(account);

                    sender.SendClientMessage(Color.FromInteger(65280, ColorFormat.RGB), "You have earned {FFFF00}$5000{00FF00} for accepting the rules");
                }
            };
        }

        [Command("givecash", Shortcut = "givecash")]
        public static void OnGiveCashCommand(Player sender, Player target, int money)
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

            sender.Reward(-money);
            target.Reward(money);

            target.SendClientMessage($"{{00FF00}}You have received {{FFFF00}}${money}{{00FF00}} from {{FFFF00}}{sender.Name}");
            sender.SendClientMessage($"{{00FF00}}You gave {{FFFF00}}${money}{{00FF00}} to {{FFFF00}}{target.Name}");
        }

        private static void DialogSpawns_Response(object sender, SampSharp.GameMode.Events.DialogResponseEventArgs e, PlayerClassType classId)
        {
            if (e.DialogButton == SampSharp.GameMode.Definitions.DialogButton.Left)
            {
                var player = e.Player;

                switch (classId)
                {
                    case PlayerClassType.TruckDriver:
                        player.Position = e.ListItem switch
                        {
                            _ => TruckerSpawn.TruckerSpawns[e.ListItem].Position,
                        };
                        break;

                    case PlayerClassType.BusDriver:
                        player.Position = e.ListItem switch
                        {
                            _ => BusDriverSpawn.BusDriverSpawns[e.ListItem].Position,
                        };
                        break;

                    case PlayerClassType.Pilot:
                        player.Position = e.ListItem switch
                        {
                            _ => PilotSpawn.PilotSpawns[e.ListItem].Position,
                        };
                        break;

                    case PlayerClassType.Police:
                        player.Position = e.ListItem switch
                        {
                            _ => PoliceSpawn.PoliceSpawns[e.ListItem].Position,
                        };
                        break;

                    case PlayerClassType.Courier:
                        player.Position = e.ListItem switch
                        {
                            _ => CourierSpawn.CourierSpawns[e.ListItem].Position,
                        };
                        break;
                }
            }
        }
    }
}