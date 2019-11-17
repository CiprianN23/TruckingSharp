using SampSharp.GameMode;
using SampSharp.GameMode.Controllers;
using SampSharp.GameMode.Display;
using SampSharp.GameMode.SAMP;
using System;
using TruckingSharp.Constants;
using TruckingSharp.Database;
using TruckingSharp.Database.Repositories;

namespace TruckingSharp.Speedometer
{
    [Controller]
    public class SpeedometerController : IEventListener
    {
        private PlayerAccountRepository _playerAccountRepository => new PlayerAccountRepository(DapperConnection.ConnectionString);

        public void RegisterEvents(BaseMode gameMode)
        {
            gameMode.PlayerConnected += Speedometer_PlayerConnected;
        }

        private void Speedometer_PlayerConnected(object sender, EventArgs e)
        {
            var player = sender as Player;

            player.VehicleNameTextDraw = new PlayerTextDraw(player, new Vector2(500.0, 380.0), " ");
            player.SpeedometerTextDraw = new PlayerTextDraw(player, new Vector2(500.0, 395.0), " ");
            player.FuelGaugeTextDraw = new PlayerTextDraw(player, new Vector2(500.0, 410.0), " ");

            player.SpeedometerTimer = new Timer(TimeSpan.FromMilliseconds(500), true);
            player.SpeedometerTimer.Tick += (sender, e) => SpeedometerTimer_Tick(sender, e, player);
        }

        private async void SpeedometerTimer_Tick(object sender, EventArgs e, Player player)
        {
            if (player.Vehicle != null)
            {
                player.VehicleNameTextDraw.Show();
                player.SpeedometerTextDraw.Show();
                player.FuelGaugeTextDraw.Show();

                var playerVehicle = (Vehicle)player.Vehicle;
                var playerVehicleVelocity = playerVehicle.Velocity;

                int finalSpeed = (int)(Math.Sqrt(Math.Pow(playerVehicleVelocity.X, 2) + Math.Pow(playerVehicleVelocity.Y, 2) + Math.Pow(playerVehicleVelocity.Z, 2))
                    * Configuration.KilometersPerHourMultiplier);

                player.SpeedometerTextDraw.Text = $"~w~Speed: ~b~{finalSpeed}~w~ kph";

                var account = player.Account;
                account.MetersDriven = (float)(account.MetersDriven + (finalSpeed / 7.2));
                await _playerAccountRepository.UpdateAsync(account);

                player.VehicleNameTextDraw.Text = $"{playerVehicle.ModelInfo.Name}";

                if (finalSpeed > 10 && playerVehicle.Fuel > 0)
                    playerVehicle.Fuel--;

                player.FuelGaugeTextDraw.Text = ConstructFuelGauge(playerVehicle.Fuel);

                if (playerVehicle.Fuel == 0)
                {
                    playerVehicle.Engine = false;
                    playerVehicle.Lights = false;
                }
            }
            else
            {
                player.VehicleNameTextDraw.Hide();
                player.SpeedometerTextDraw.Hide();
                player.FuelGaugeTextDraw.Hide();
            }
        }

        private string ConstructFuelGauge(int fuel)
        {
            int MaxFuel = Configuration.MaxFuel;

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