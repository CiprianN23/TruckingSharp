using SampSharp.GameMode;
using SampSharp.GameMode.Controllers;
using SampSharp.GameMode.Definitions;
using SampSharp.GameMode.Display;
using SampSharp.GameMode.Events;
using SampSharp.GameMode.SAMP;
using System;
using TruckingSharp.Database;
using TruckingSharp.Database.Repositories;
using TruckingSharp.World;

namespace TruckingSharp.Vehicles.Speedometer
{
    [Controller]
    public class SpeedometerController : IEventListener
    {
        private PlayerAccountRepository _playerAccountRepository => RepositoriesInstances.AccountRepository;

        public void RegisterEvents(BaseMode gameMode)
        {
            gameMode.PlayerConnected += Speedometer_PlayerConnected;
            gameMode.PlayerStateChanged += Speedometer_PlayerStateChanged;
        }

        private void Speedometer_PlayerStateChanged(object sender, StateEventArgs e)
        {
            if (!(sender is Player player))
                return;

            if (e.NewState == PlayerState.Driving || e.NewState == PlayerState.Passenger)
            {
                player.VehicleNameTextDraw.Show();
                player.SpeedometerTextDraw.Show();
                player.FuelGaugeTextDraw.Show();

                player.Speed = 0;

                player.SpeedometerTimer.IsRunning = true;
            }
            else if (e.OldState == PlayerState.Driving || e.OldState == PlayerState.Passenger)
            {
                player.VehicleNameTextDraw.Hide();
                player.SpeedometerTextDraw.Hide();
                player.FuelGaugeTextDraw.Hide();

                player.Speed = 0;

                player.SpeedometerTimer.IsRunning = false;
            }
        }

        private void Speedometer_PlayerConnected(object sender, EventArgs e)
        {
            if (!(sender is Player player))
                return;

            player.VehicleNameTextDraw = new PlayerTextDraw(player, new Vector2(500.0, 380.0), " ");
            player.SpeedometerTextDraw = new PlayerTextDraw(player, new Vector2(500.0, 395.0), " ");
            player.FuelGaugeTextDraw = new PlayerTextDraw(player, new Vector2(500.0, 410.0), " ");

            player.SpeedometerTimer = new Timer(TimeSpan.FromMilliseconds(500), true) { IsRunning = false };

            player.SpeedometerTimer.Tick += (senderObject, ev) => SpeedometerTimer_Tick(senderObject, ev, player);
            player.SpeedometerTimer.Tick += (senderObject, ev) => SpeedCameraController.SpeedometerTimer_Tick(senderObject, ev, player);
        }

        private async void SpeedometerTimer_Tick(object sender, EventArgs e, Player player)
        {
            if (player.Vehicle == null)
                return;

            var playerVehicle = (Vehicle)player.Vehicle;
            var playerVehicleVelocity = playerVehicle.Velocity;

            var playerVehicleSpeed = GetPlayerVehicleSpeed(playerVehicleVelocity);

            player.SpeedometerTextDraw.Text = $"~w~Speed: ~b~{playerVehicleSpeed}~w~ kph";

            player.Speed = playerVehicleSpeed;

            var account = player.Account;
            account.MetersDriven = (float)(account.MetersDriven + playerVehicleSpeed / 7.2);
            await _playerAccountRepository.UpdateAsync(account);

            if (!player.VehicleNameTextDraw.IsDisposed)
                player.VehicleNameTextDraw.Text = $"{playerVehicle.ModelInfo.Name}";

            if (playerVehicleSpeed > 10 && playerVehicle.Fuel > 0 && playerVehicle.Engine)
                playerVehicle.Fuel--;

            player.FuelGaugeTextDraw.Text = ConstructFuelGauge(playerVehicle.Fuel);

            if (playerVehicle.Fuel != 0)
                return;

            playerVehicle.Engine = false;
            playerVehicle.Lights = false;
        }

        private static int GetPlayerVehicleSpeed(Vector3 velocity)
        {
            return (int)(Math.Sqrt(Math.Pow(velocity.X, 2) + Math.Pow(velocity.Y, 2) + Math.Pow(velocity.Z, 2)) *
                          Configuration.Instance.KilometersPerHourMultiplier);
        }

        private static string ConstructFuelGauge(int fuel)
        {
            var maxFuel = Configuration.Instance.MaximumFuel;

            if (fuel > 0 && fuel < maxFuel / 10)
                return "~g~I~r~IIIIIIIII";
            if (fuel >= maxFuel / 10 * 1 && fuel < maxFuel / 10 * 2)
                return "~g~II~r~IIIIIIII";
            if (fuel >= maxFuel / 10 * 2 && fuel < maxFuel / 10 * 3)
                return "~g~III~r~IIIIIII";
            if (fuel >= maxFuel / 10 * 3 && fuel < maxFuel / 10 * 4)
                return "~g~IIII~r~IIIIII";
            if (fuel >= maxFuel / 10 * 4 && fuel < maxFuel / 10 * 5)
                return "~g~IIIII~r~IIIII";
            if (fuel >= maxFuel / 10 * 5 && fuel < maxFuel / 10 * 6)
                return "~g~IIIIII~r~IIII";
            if (fuel >= maxFuel / 10 * 6 && fuel < maxFuel / 10 * 7)
                return "~g~IIIIIII~r~III";
            if (fuel >= maxFuel / 10 * 7 && fuel < maxFuel / 10 * 8)
                return "~g~IIIIIIII~r~II";
            if (fuel >= maxFuel / 10 * 8 && fuel < maxFuel / 10 * 9)
                return "~g~IIIIIIIII~r~I";
            if (fuel >= maxFuel / 10 * 9 && fuel <= maxFuel)
                return "~g~IIIIIIIIII";
            return "~r~IIIIIIIIII";
        }
    }
}