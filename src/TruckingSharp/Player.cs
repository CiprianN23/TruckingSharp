﻿using SampSharp.GameMode;
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
using TruckingSharp.Database.Entities;
using TruckingSharp.Database.Repositories;
using TruckingSharp.Missions.BusDriver;
using TruckingSharp.Missions.Convoy;
using TruckingSharp.Missions.Data;
using TruckingSharp.PlayerClasses.Data;

namespace TruckingSharp
{
    [PooledType]
    public class Player : BasePlayer
    {
        public PlayerClassType PlayerClass;

        private static PlayerAccountRepository AccountRepository =>
            new PlayerAccountRepository(ConnectionFactory.GetConnection);

        public PlayerAccount Account => AccountRepository.Find(Name);

        public PlayerBankAccount BankAccount =>
            new PlayerBankAccountRepository(ConnectionFactory.GetConnection).Find(Account.Id);

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
        public BusRoute BusRoute { get; set; }
        public int BusPassengers { get; set; }

        public PlayerTextDraw VehicleNameTextDraw { get; set; }
        public PlayerTextDraw SpeedometerTextDraw { get; set; }
        public PlayerTextDraw FuelGaugeTextDraw { get; set; }
        public Timer SpeedometerTimer { get; set; }
        public int Speed { get; set; }
        public int TimeSincePlayerCaughtSpeedingInSeconds { get; set; }

        public bool CheckIfPlayerCanJoinPolice()
        {
            if (Configuration.Instance.PlayersBeforePolice <= 0)
                return false;

            var normalPlayers = 0;
            var policePlayers = 0;
            foreach (var basePlayer in All)
            {
                var player = (Player)basePlayer;

                if (player == this)
                    continue;

                if (player.Interior == 14)
                    continue;

                if (!IsLoggedIn)
                    continue;

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

            var canSpawnAsCop = policePlayers < normalPlayers / Configuration.Instance.PlayersBeforePolice;
            if (canSpawnAsCop) return true;
            GameText("Maximum amount of cops already reached", 5000, 4);
            SendClientMessage(Color.Red,
                "The maximum amount of cops has been reached already, please select another class");

            return false;
        }

        public async void Reward(int money, int score = 0)
        {
            var account = Account;

            account.Money += money;
            account.Score += score;

            Money = account.Money;
            Score = account.Score;

            await AccountRepository.UpdateAsync(account).ConfigureAwait(false);
        }

        public async void SetWantedLevel(int wantedLevel)
        {
            var account = Account;
            account.Wanted = wantedLevel;
            WantedLevel = wantedLevel;
            await AccountRepository.UpdateAsync(account).ConfigureAwait(false);
        }

        public override void OnClickPlayer(ClickPlayerEventArgs e)
        {
            base.OnClickPlayer(e);

            // TODO: Show player stats
        }

        public override void OnConnected(EventArgs e)
        {
            ToggleSpectating(true);

            SendClientMessageToAll(Color.LightGray, Messages.PlayerJoinedTheServer, Name, Id);

            base.OnConnected(e);
        }

        public override void OnDeath(DeathEventArgs e)
        {
            base.OnDeath(e);

            // TODO: Wanted for killer
        }

        public override void OnDisconnected(DisconnectEventArgs e)
        {
            SendClientMessageToAll(Color.LightGray, Messages.PlayerLeftTheServer, Name, Id);

            // TODO Destroy rented vehicle

            base.OnDisconnected(e);
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
        }

        public override async void OnRequestClass(RequestClassEventArgs e)
        {
            base.OnRequestClass(e);

            if (IsLoggedIn)
                return;

            SendClientMessage(Color.Red, Messages.FailedToLoginProperly);
            await Task.Delay(Configuration.Instance.KickDelay);
            Kick();
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

            if (!IsSpectating)
                return;

            Position = SpectatePosition;

            SpectatePosition = Vector3.Zero;
            IsSpectating = false;

            // TODO Send player to jail
        }

        public override void OnStateChanged(StateEventArgs e)
        {
            base.OnStateChanged(e);

            if (e.NewState != PlayerState.Driving)
                return;

            if (PlayerClass != PlayerClassType.Police)
                if (MissionVehicles.PoliceJobVehicles.Contains(Vehicle))
                {
                    RemoveFromVehicle();
                    Vehicle.Engine = false;
                    Vehicle.Lights = false;
                    SendClientMessage(Color.Red, "You can't use police vehicles.");
                }

            if (PlayerClass == PlayerClassType.Pilot)
                return;

            if (!MissionVehicles.PilotJobVehicles.Contains(Vehicle))
                return;

            RemoveFromVehicle();
            Vehicle.Engine = false;
            Vehicle.Lights = false;
            SendClientMessage(Color.Red, "You can't use pilot vehicles.");

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
            SendClientMessage(Color.Silver,
                $"Mute time remaining: Minutes: {remainingMuteTime.Minutes}, Seconds: {remainingMuteTime.Seconds}");
        }

        protected override void Dispose(bool disposing)
        {
            SpeedometerTimer?.Dispose();
            SpectateTimer?.Dispose();
            MissionLoadingTimer?.Dispose();

            base.Dispose(disposing);
        }
    }
}