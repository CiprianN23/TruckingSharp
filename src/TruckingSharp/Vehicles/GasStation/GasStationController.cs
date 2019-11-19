using SampSharp.GameMode;
using SampSharp.GameMode.Controllers;
using SampSharp.GameMode.SAMP;
using SampSharp.GameMode.Tools;
using System;
using System.Collections.Generic;
using TruckingSharp.Constants;
using TruckingSharp.Extensions.PlayersExtensions;

namespace TruckingSharp.Vehicles.GasStation
{
    [Controller]
    public class GasStationController : IEventListener
    {
        public static List<GasStation> GasStations = new List<GasStation>();

        public void RegisterEvents(BaseMode gameMode)
        {
            gameMode.Initialized += GasStation_GamemodeInitialized;
            gameMode.PlayerKeyStateChanged += GasStation_PlayerKeyStateChange;
        }

        private void GasStation_PlayerKeyStateChange(object sender, SampSharp.GameMode.Events.KeyStateChangedEventArgs e)
        {
            var player = sender as Player;

            if (KeyUtils.HasPressed(e.NewKeys, e.OldKeys, SampSharp.GameMode.Definitions.Keys.Crouch))
            {
                if (!player.IsPlayerDriving())
                    return;

                foreach (var gasStation in GasStations)
                {
                    if (player.IsInRangeOfPoint(2.5f, gasStation.Position))
                    {
                        player.GameText("~g~Refuelling...", 3000, 4);
                        player.ToggleControllable(false);

                        Timer refuelTimer = new Timer(TimeSpan.FromSeconds(5), false);
                        refuelTimer.Tick += (sender, e) => RefuelVehicle(sender, e, player);
                        break;
                    }
                }
            }
        }

        private void RefuelVehicle(object sender, EventArgs e, Player player)
        {
            var playerVehicle = (Vehicle)player.Vehicle;
            int fuelAmount = Configuration.MaxFuel - playerVehicle.Fuel;
            int refuelPrice = (fuelAmount / Configuration.RefuelMaxPrice) / Configuration.MaxFuel;

            if (player.Account.Money < refuelPrice)
            {
                player.SendClientMessage(Color.Red, "You don't have enough cash to refuel your vehicle.");
                player.ToggleControllable(true);
                return;
            }

            playerVehicle.Fuel = Configuration.MaxFuel;
            player.Reward(-refuelPrice);
            player.SendClientMessage(Color.GreenYellow, $"You refuelled your vehicle for ${refuelPrice}.");
            player.ToggleControllable(true);
        }

        private void GasStation_GamemodeInitialized(object sender, EventArgs e)
        {
            GasStations.Add(new GasStation(new Vector3(-1471.5, 1863.75, 32.7)));
            GasStations.Add(new GasStation(new Vector3(-1326.5, 2677.5, 50.1)));
            GasStations.Add(new GasStation(new Vector3(611.5, 1694.5, 7.0)));
            GasStations.Add(new GasStation(new Vector3(-2249.25, -2559.0, 32.0)));
            GasStations.Add(new GasStation(new Vector3(-1606.5, -2714.0, 48.6)));
            GasStations.Add(new GasStation(new Vector3(-93.5, -1175.0, 2.3)));
            GasStations.Add(new GasStation(new Vector3(1377.5, 457.0, 19.9)));
            GasStations.Add(new GasStation(new Vector3(651.5, -565.5, 16.4)));
            GasStations.Add(new GasStation(new Vector3(-1675.75, 412.75, 7.2)));
            GasStations.Add(new GasStation(new Vector3(-2405.50, 976.25, 45.3)));
            GasStations.Add(new GasStation(new Vector3(-2023.25, 156.75, 28.9)));
            GasStations.Add(new GasStation(new Vector3(-1131.75, -204.25, 14.2)));
            GasStations.Add(new GasStation(new Vector3(66.50, 1220.50, 18.9)));
            GasStations.Add(new GasStation(new Vector3(350.50, 2537.50, 16.8)));
            GasStations.Add(new GasStation(new Vector3(2147.00, 2747.75, 10.9)));
            GasStations.Add(new GasStation(new Vector3(2639.75, 1106.00, 10.9)));
            GasStations.Add(new GasStation(new Vector3(2115.00, 920.00, 10.9)));
            GasStations.Add(new GasStation(new Vector3(2202.00, 2475.00, 10.9)));
            GasStations.Add(new GasStation(new Vector3(1596.50, 2199.75, 10.9)));
            GasStations.Add(new GasStation(new Vector3(1584.25, 1448.25, 10.9)));
            GasStations.Add(new GasStation(new Vector3(1004.25, -940.50, 42.2)));
            GasStations.Add(new GasStation(new Vector3(1935.00, -1772.75, 13.4)));
        }
    }
}