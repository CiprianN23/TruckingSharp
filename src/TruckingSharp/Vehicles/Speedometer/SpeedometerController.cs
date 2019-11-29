using SampSharp.GameMode;
using SampSharp.GameMode.Controllers;
using SampSharp.GameMode.Display;
using SampSharp.GameMode.SAMP;
using System;
using TruckingSharp.Database.Repositories;
using TruckingSharp.World;

namespace TruckingSharp.Vehicles.Speedometer
{
    [Controller]
    public class SpeedometerController : IEventListener
    {
        private PlayerAccountRepository _playerAccountRepository => new PlayerAccountRepository(ConnectionFactory.GetConnection);

        public void RegisterEvents(BaseMode gameMode)
        {
            gameMode.PlayerConnected += Speedometer_PlayerConnected;
            gameMode.PlayerStateChanged += Speedoemeter_PlayerStateChanged;
        }

        private void Speedoemeter_PlayerStateChanged(object sender, SampSharp.GameMode.Events.StateEventArgs e)
        {
            var player = sender as Player;

            if (e.NewState == SampSharp.GameMode.Definitions.PlayerState.Driving || e.NewState == SampSharp.GameMode.Definitions.PlayerState.Passenger)
            {
                player.VehicleNameTextDraw.Show();
                player.SpeedometerTextDraw.Show();
                player.FuelGaugeTextDraw.Show();

                player.Speed = 0;

                player.SpeedometerTimer.IsRunning = true;
            }
            else if (e.OldState == SampSharp.GameMode.Definitions.PlayerState.Driving || e.OldState == SampSharp.GameMode.Definitions.PlayerState.Passenger)
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
            var player = sender as Player;

            player.VehicleNameTextDraw = new PlayerTextDraw(player, new Vector2(500.0, 380.0), " ");
            player.SpeedometerTextDraw = new PlayerTextDraw(player, new Vector2(500.0, 395.0), " ");
            player.FuelGaugeTextDraw = new PlayerTextDraw(player, new Vector2(500.0, 410.0), " ");

            player.SpeedometerTimer = new Timer(TimeSpan.FromMilliseconds(500), true);
            player.SpeedometerTimer.IsRunning = false;
            player.SpeedometerTimer.Tick += (sender, e) => SpeedometerTimer_Tick(sender, e, player);
            player.SpeedometerTimer.Tick += (sender, e) => SpeedCameraController.SpeedometerTimer_Tick(sender, e, player);
        }

        private async void SpeedometerTimer_Tick(object sender, EventArgs e, Player player)
        {
            if (player.Vehicle != null)
            {
                var playerVehicle = (Vehicle)player.Vehicle;
                var playerVehicleVelocity = playerVehicle.Velocity;

                int playerVehicleSpeed = GetPlayerVehicleSpeed(playerVehicleVelocity);

                player.SpeedometerTextDraw.Text = $"~w~Speed: ~b~{playerVehicleSpeed}~w~ kph";

                player.Speed = playerVehicleSpeed;

                var account = player.Account;
                account.MetersDriven = (float)(account.MetersDriven + (playerVehicleSpeed / 7.2));
                await _playerAccountRepository.UpdateAsync(account);

                player.VehicleNameTextDraw.Text = $"{playerVehicle.ModelInfo.Name}";

                if (playerVehicleSpeed > 10 && playerVehicle.Fuel > 0 && playerVehicle.Engine)
                    playerVehicle.Fuel--;

                player.FuelGaugeTextDraw.Text = ConstructFuelGauge(playerVehicle.Fuel);

                if (playerVehicle.Fuel == 0)
                {
                    playerVehicle.Engine = false;
                    playerVehicle.Lights = false;
                }
            }
        }

        private int GetPlayerVehicleSpeed(Vector3 velocity)
        {
            return (int)(Math.Sqrt(Math.Pow(velocity.X, 2) + Math.Pow(velocity.Y, 2) + Math.Pow(velocity.Z, 2)) * Configuration.Instance.KilometersPerHourMultiplier);
        }

        private string ConstructFuelGauge(int fuel)
        {
            int MaxFuel = Configuration.Instance.MaximumFuel;

            if (fuel > 0 && fuel < (MaxFuel / 10))
                return "~g~I~r~IIIIIIIII";
            else if (fuel >= ((MaxFuel / 10) * 1) && fuel < ((MaxFuel / 10) * 2))
                return "~g~II~r~IIIIIIII";
            else if (fuel >= ((MaxFuel / 10) * 2) && fuel < ((MaxFuel / 10) * 3))
                return "~g~III~r~IIIIIII";
            else if (fuel >= ((MaxFuel / 10) * 3) && fuel < ((MaxFuel / 10) * 4))
                return "~g~IIII~r~IIIIII";
            else if (fuel >= ((MaxFuel / 10) * 4) && fuel < ((MaxFuel / 10) * 5))
                return "~g~IIIII~r~IIIII";
            else if (fuel >= ((MaxFuel / 10) * 5) && fuel < ((MaxFuel / 10) * 6))
                return "~g~IIIIII~r~IIII";
            else if (fuel >= ((MaxFuel / 10) * 6) && fuel < ((MaxFuel / 10) * 7))
                return "~g~IIIIIII~r~III";
            else if (fuel >= ((MaxFuel / 10) * 7) && fuel < ((MaxFuel / 10) * 8))
                return "~g~IIIIIIII~r~II";
            else if (fuel >= ((MaxFuel / 10) * 8) && fuel < ((MaxFuel / 10) * 9))
                return "~g~IIIIIIIII~r~I";
            else if (fuel >= ((MaxFuel / 10) * 9) && fuel <= MaxFuel)
                return "~g~IIIIIIIIII";
            else
                return "~r~IIIIIIIIII";
        }
    }
}