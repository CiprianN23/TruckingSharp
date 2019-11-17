using SampSharp.GameMode;
using SampSharp.GameMode.Definitions;
using SampSharp.GameMode.Display;
using SampSharp.GameMode.Events;
using SampSharp.GameMode.Pools;
using SampSharp.GameMode.SAMP;
using SampSharp.GameMode.World;
using System;
using System.Threading.Tasks;
using TruckingSharp.Constants;
using TruckingSharp.Data;
using TruckingSharp.Database;
using TruckingSharp.Database.Entities;
using TruckingSharp.Database.Repositories;
using TruckingSharp.Missions.Convoy;
using TruckingSharp.Missions.Data;
using TruckingSharp.PlayerClasses.Data;

namespace TruckingSharp
{
    [PooledType]
    public class Player : BasePlayer
    {
        private PlayerAccountRepository _accountRepository => new PlayerAccountRepository(DapperConnection.ConnectionString);

        public PlayerClassType PlayerClass;

        public PlayerAccount Account
        {
            get
            {
                return _accountRepository.Find(Name);
            }
        }

        public PlayerBankAccount BankAccount
        {
            get
            {
                return new PlayerBankAccountRepository().Find(Account.Id);
            }
        }

        public bool IsLoggedIn { get; set; }
        public bool IsLoggedInBankAccount { get; set; }
        public int LoginTries { get; set; }
        public int Warnings { get; set; }
        public int FrozenTime { get; set; }
        public Vector3 SpectatePosition { get; set; }
        public bool IsSpectating { get; set; }
        public SpectateTypes SpectateType { get; set; }
        public Player SpectatedPlayer { get; set; }
        public BaseVehicle SpectatedVehicle { get; set; }
        public Timer SpectateTimer { get; set; }

        public bool IsDoingMission { get; set; }
        public bool IsInConvoy { get; set; }
        public bool IsOverloaded { get; set; }
        public bool IsMafiaLoaded { get; set; }
        public byte MissionStep { get; set; }
        public Vehicle MissionVehicle { get; set; }
        public Vehicle MissionTrailer { get; set; }
        public MissionCargo MissionCargo { get; set; }
        public MissionLocation FromLocation { get; set; }
        public MissionLocation ToLocation { get; set; }
        public Timer MissionLoadingTimer { get; set; }
        public int MissionVehicleTime { get; set; }
        public PlayerTextDraw MissionTextDraw { get; set; }
        public MissionConvoy Convoy { get; set; }

        public bool CheckIfPlayerCanJoinPolice()
        {
            if (Configuration.PlayersBeforePolice > 0)
            {
                int normalPlayers = 0;
                int policePlayers = 0;
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
                            case PlayerClassType.Police:
                                policePlayers++;
                                break;

                            default:
                                normalPlayers++;
                                break;
                        }
                    }
                }

                bool CanSpawnAsCop = policePlayers < (normalPlayers / Configuration.PlayersBeforePolice);
                if (!CanSpawnAsCop)
                {
                    GameText("Maximum amount of cops already reached", 5000, 4);
                    SendClientMessage(Color.Red, "The maximum amount of cops has been reached already, please select another class");
                }

                return CanSpawnAsCop;
            }
        }

        public async void Reward(int money, int score = 0)
        {
            var account = Account;

            account.Money += money;
            account.Score += score;

            Money = account.Money;
            Score = account.Score;

            await _accountRepository.UpdateAsync(account).ConfigureAwait(false);
        }

        public async void SetWantedLevel(int wantedLevel)
        {
            var account = Account;
            account.Wanted = wantedLevel;
            WantedLevel = wantedLevel;
            await _accountRepository.UpdateAsync(account);
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

            await Task.Delay(100);
            SendClientMessageToAll(Color.Blue, Messages.PlayerJoinedTheServer, Name, Id);

            SpectateTimer = new Timer(500, true);
            SpectateTimer.Tick += SpectateTimer_Tick;
        }

        private void SpectateTimer_Tick(object sender, EventArgs e)
        {
            if (SpectatedPlayer == null)
                return;

            if (State == PlayerState.Spectating)
            {
                VirtualWorld = SpectatedPlayer.VirtualWorld;
                Interior = SpectatedPlayer.Interior;

                if (SpectateType == SpectateTypes.Player)
                {
                    if (SpectatedPlayer.VehicleSeat != -1)
                    {
                        SpectateVehicle(SpectatedPlayer.Vehicle);
                        SpectatedVehicle = SpectatedPlayer.Vehicle;
                        SpectateType = SpectateTypes.Vehicle;
                        SendClientMessage($"{{00FF00}}Player {{FFFF00}}{SpectatedPlayer.Name}{{00FF00}} has entered a vehicle, changing spectate mode to match");
                    }
                }
                else
                {
                    if (SpectatedPlayer.VehicleSeat == -1)
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

            // TODO: Leave convoy
            // TODO: Wanted for killer
        }

        public override void OnDisconnected(DisconnectEventArgs e)
        {
            SendClientMessageToAll(Color.Blue, Messages.PlayerLeftTheServer, Name, Id);

            foreach (Player player in All)
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

            // TODO Destroy rented vehicle

            base.OnDisconnected(e);
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
                await Task.Delay(Configuration.KickDelay);
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

            if (IsSpectating)
            {
                Position = SpectatePosition;

                SpectatePosition = Vector3.Zero;
                IsSpectating = false;
            }

            // TODO Send player to jail
        }

        public override void OnStateChanged(StateEventArgs e)
        {
            base.OnStateChanged(e);

            if (e.NewState == PlayerState.Driving)
            {
                if (PlayerClass != PlayerClassType.Police)
                {
                    if (MissionVehicles.PoliceJobVehicles.Contains(Vehicle))
                    {
                        RemoveFromVehicle();
                        Vehicle.Engine = false;
                        Vehicle.Lights = false;
                        SendClientMessage(Color.Red, "You can't use police vehicles.");
                    }
                }

                if (PlayerClass != PlayerClassType.Pilot)
                {
                    if (MissionVehicles.PilotJobVehicles.Contains(Vehicle))
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

        public void ShowRemainingMuteTime()
        {
            var remainingMuteTime = Account.Muted - DateTime.Now;
            SendClientMessage(Color.Silver, $"Mute time remaining: Minutes: {remainingMuteTime.Minutes}, Seconds: {remainingMuteTime.Seconds}");
        }
    }
}