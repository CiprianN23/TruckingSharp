using SampSharp.GameMode;
using SampSharp.GameMode.Definitions;
using SampSharp.GameMode.Display;
using SampSharp.GameMode.Events;
using SampSharp.GameMode.Pools;
using SampSharp.GameMode.SAMP;
using SampSharp.GameMode.World;
using System;
using System.Security.Cryptography;
using System.Threading.Tasks;
using TruckingSharp.Constants;
using TruckingSharp.Data;
using TruckingSharp.Database;
using TruckingSharp.Database.Entities;
using TruckingSharp.Database.UnitsOfWork;
using TruckingSharp.Extensions.PlayersExtensions;
using TruckingSharp.Services;

namespace TruckingSharp.World
{
    [PooledType]
    public class Player : BasePlayer
    {
        public PlayerClasses PlayerClass;

        public PlayerAccount Account
        {
            get
            {
                using var uow = new UnitOfWork(DapperConnection.ConnectionString);
                return uow.PlayerAccountRepository.Find(Name);
            }
        }

        public PlayerBankAccount BankAccount
        {
            get
            {
                using var uow = new UnitOfWork(DapperConnection.ConnectionString);
                return uow.PlayerBankAccountRepository.Find(uow.PlayerBankAccountRepository.GetPlayerId(Name));
            }
        }

        public bool IsDoingJob { get; set; }
        public bool IsLoggedIn { get; set; }
        public bool IsLoggedInBankAccount { get; set; }
        public int LoginTries { get; private set; }
        public int Warnings { get; set; }
        public int FrozenTime { get; set; }
        public Vector3 SpectatePosition { get; set; }
        public bool IsSpectating { get; set; }
        public SpectateTypes SpectateType { get; set; }
        public Player SpectatedPlayer { get; set; }
        public BaseVehicle SpectatedVehicle { get; set; }
        public Timer SpectateTimer { get; set; }

        public void CheckIfPlayerCanJoinPolice()
        {
            if (Configuration.PlayersBeforePolice > 0)
            {
                int normalPlayers;
                int policePlayers;
                foreach (Player player in All)
                {
                    if (player == this)
                        continue;

                    if (player.Interior == 14)
                        continue;

                    if (IsLoggedIn)
                    {
                        switch (player.PlayerClass)
                        {
                            case PlayerClasses.Police:
                                policePlayers++;
                                break;

                            default:
                                normalPlayers++;
                                break;
                        }
                    }
                }

                bool CanSpawnAsCop;
                if (policePlayers < (normalPlayers / Configuration.PlayersBeforePolice))
                    CanSpawnAsCop = true;
                else
                    CanSpawnAsCop = false;

                if (!CanSpawnAsCop)
                {
                    GameText("Maximum amount of cops already reached", 5000, 4);
                    SendClientMessage(Color.Red, "The maximum amount of cops has been reached already, please select another class");
                }
            }
        }

        public override void GiveMoney(int money)
        {
            var account = Account;

            account.Money += money;

            Money = account.Money;

            using var uow = new UnitOfWork(DapperConnection.ConnectionString);
            uow.PlayerAccountRepository.Update(account);
            uow.Commit();
        }

        public override void OnClickPlayer(ClickPlayerEventArgs e)
        {
            base.OnClickPlayer(e);

            // TODO: Show player stats
        }

        public override async void OnConnected(EventArgs e)
        {
            ToggleSpectating(true);

            base.OnConnected(e);

            await Task.Delay(100).ConfigureAwait(false);
            SendClientMessageToAll(Color.Blue, Messages.PlayerJoinedTheServer, Name, Id);

            if (Account == null)
                RegisterPlayer();
            else
                LoginPlayer();

            SpectateTimer = new Timer(500, true);
            SpectateTimer.Tick += SpectateTimer_Tick;
        }

        private void SpectateTimer_Tick(object sender, EventArgs e)
        {
            if (SpectatedPlayer == null)
                return;

            if(State == PlayerState.Spectating)
            {
                VirtualWorld = SpectatedPlayer.VirtualWorld;
                Interior = SpectatedPlayer.Interior;

                if(SpectateType == SpectateTypes.Player)
                {
                    if(SpectatedPlayer.VehicleSeat != -1)
                    {
                        SpectateVehicle(SpectatedPlayer.Vehicle);
                        SpectatedVehicle = SpectatedPlayer.Vehicle;
                        SpectateType = SpectateTypes.Vehicle;
                        SendClientMessage($"{{00FF00}}Player {{FFFF00}}{SpectatedPlayer.Name}{{00FF00}} has entered a vehicle, changing spectate mode to match");
                    }
                }
                else
                {
                    if(SpectatedPlayer.VehicleSeat == -1)
                    {
                        SpectatePlayer(SpectatedPlayer);
                        SpectateType = SpectateTypes.Player;
                        SendClientMessage($"{{00FF00}}Player {{FFFF00}}{SpectatedPlayer.Name}{{00FF00}} has exited a vehicle, changing spectate mode to match");
                    }
                }
            }
        }

        public override void OnDeath(DeathEventArgs e)
        {
            base.OnDeath(e);

            // TODO: Cancel jobs
            // TODO: Leave convoy
            // TODO: Wanted for killer
        }

        public override void OnDisconnected(DisconnectEventArgs e)
        {
            base.OnDisconnected(e);

            SendClientMessageToAll(Color.Blue, Messages.PlayerLeftTheServer, Name, Id);

            foreach(Player player in All)
            {
                if (!player.IsLoggedIn)
                    continue;

                if (player.State == PlayerState.Spectating && player.SpectatedPlayer == this)
                {
                    player.ToggleSpectating(false);
                    player.SpectatedPlayer = null;
                    player.SpectatedVehicle = null;
                    player.SendClientMessage(Color.Red, "Target player has logged off, ending specate mode.");
                }
            }

            // TODO: Cancel jobs/leave convoy
            // TODO Destroy rented  vehicle
        }

        public override void OnEnterVehicle(EnterVehicleEventArgs e)
        {
            base.OnEnterVehicle(e);

            // TODO: Check fuel/no engine
        }

        public override void OnExitVehicle(PlayerVehicleEventArgs e)
        {
            base.OnExitVehicle(e);

            e.Vehicle.Engine = false;
            e.Vehicle.Lights = false;

            // TODO: Cancel pilot job
        }

        public override void OnKeyStateChanged(KeyStateChangedEventArgs e)
        {
            base.OnKeyStateChanged(e);

            // TODO: Police fine/warn to stop
            // TODO: Assistance repair vehicle/own vehicle
            // TODO: Tow vehicle with tow truck
            // TODO: Refuel with horn key
        }

        public override async void OnRequestClass(RequestClassEventArgs e)
        {
            base.OnRequestClass(e);

            if (!IsLoggedIn)
            {
                SendClientMessage(Color.Red, Messages.FailedToLoginProperly);
                await Task.Delay(Configuration.KickDelay).ConfigureAwait(false);
                Kick();
            }
        }

        public override void OnSpawned(SpawnEventArgs e)
        {
            base.OnSpawned(e);

            VirtualWorld = 0;
            Interior = 0;
            ToggleClock(false);
            ResetWeapons();

            if (Account.RulesRead == 0)
                SendClientMessage(Color.Red, Messages.RulesNotYetAccepted);

            if(IsSpectating)
            {
                Position = SpectatePosition;

                SpectatePosition = Vector3.Zero;
                IsSpectating = false;
            }

            // TODO: No-job textdraw
            // TODO Send player to jail
        }

        public override void OnStateChanged(StateEventArgs e)
        {
            base.OnStateChanged(e);

            if (e.NewState == PlayerState.Driving)
            {
                if (PlayerClass != PlayerClasses.Police)
                {
                    if (JobVehicles.PoliceJobVehicles.Contains(Vehicle))
                    {
                        RemoveFromVehicle();
                        Vehicle.Engine = false;
                        Vehicle.Lights = false;
                        SendClientMessage(Color.Red, "You can't use police vehicles.");
                    }
                }

                if (PlayerClass != PlayerClasses.Pilot)
                {
                    if (JobVehicles.PilotJobVehicles.Contains(Vehicle))
                    {
                        RemoveFromVehicle();
                        Vehicle.Engine = false;
                        Vehicle.Lights = false;
                        SendClientMessage(Color.Red, "You can't use pilot vehicles.");
                    }
                }
            }

            // TODO: Kick player out of vehicle if vehicle is owned by other/clmaped
        }

        public override void OnText(TextEventArgs e)
        {
            if (!IsLoggedIn)
            {
                e.SendToPlayers = false;
                return;
            }

            if (Account.Muted > DateTime.Now)
            {
                e.SendToPlayers = false;
                ShowRemainingMuteTime();
                return;
            }

            base.OnText(e);
        }

        private void LoginPlayer()
        {
            var message = $"Insert your password. Tries left: {LoginTries}/{Configuration.MaxLogins}";
            var dialog = new InputDialog("Login", message, true, "Login", "Cancel");
            dialog.Show(this);
            dialog.Response += async (sender, ev) =>
            {
                if (ev.DialogButton == DialogButton.Left)
                {
                    if (LoginTries >= Configuration.MaxLogins)
                    {
                        SendClientMessage(Color.OrangeRed, "You exceed maximum login tries. You have been kicked!");
                        await Task.Delay(Configuration.KickDelay).ConfigureAwait(false);
                        Kick();
                    }
                    else if (PasswordHashingService.VerifyPasswordHash(ev.InputText, Account.Password))
                    {
                        ToggleSpectating(false);
                        IsLoggedIn = true;

                        base.Money = Account.Money;
                    }
                    else
                    {
                        LoginTries++;
                        SendClientMessage(Color.Red, "Wrong password");

                        dialog.Message = $"Wrong password! Retype your password! Tries left: {LoginTries}/{Configuration.MaxLogins}";

                        LoginPlayer();
                    }
                }
                else
                {
                    Kick();
                }
            };
        }

        private void RegisterPlayer()
        {
            this.ShowPlayerInputDialog("Register", "Insert your password", true, "Submit", "Cancel", (senderPlayer, ev) =>
            {
                if (ev.DialogButton == DialogButton.Left)
                {

                    var hash = PasswordHashingService.GetPasswordHash(ev.InputText);

                    using var uow = new UnitOfWork(DapperConnection.ConnectionString);

                    var newAccount = new PlayerAccount { Name = Name, Password = hash };

                    uow.PlayerAccountRepository.Add(newAccount);

                    uow.Commit();

                    LoginPlayer();
                }
                else
                {
                    Kick();
                }
            });
        }

        public void ShowRemainingMuteTime()
        {
            var remainingMuteTime = Account.Muted - DateTime.Now;
            SendClientMessage(Color.Silver, $"Mute time remaining: Minutes: {remainingMuteTime.Minutes}, Seconds: {remainingMuteTime.Seconds}");
        }
    }
}