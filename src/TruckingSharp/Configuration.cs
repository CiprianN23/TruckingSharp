using SampSharp.GameMode.Definitions;
using Serilog;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;

namespace TruckingSharp
{
    public class Configuration
    {
        public const int MaximumConvoys = 5;

        public static List<Weapon> PoliceWeapons = new List<Weapon> { Weapon.Nitestick, Weapon.Teargas, Weapon.Colt45, Weapon.Shotgun, Weapon.MP5, Weapon.Rifle, Weapon.Spraycan };

        private Configuration()
        {
        }

        public static Configuration Instance { get; set; } = new Configuration();

        public int AutoAssistPrice { get; set; }

        public float BankInterest { get; set; }

        public bool CanAutoKickAfterWarn { get; set; }

        public bool CanPoliceHaveWeapons { get; set; }

        public bool CanShowBoughtHouses { get; set; }

        public int ColorChangePrice { get; set; }

        public float CourierMissionRange { get; set; }

        public int DefaultJailSeconds { get; set; }

        public int ExitBuildingMilliseconds { get; set; }

        public int FailedMissionPrice { get; set; }

        public int FailMissionSeconds { get; set; }

        public int FinePerWantedLevel { get; set; }

        public int GoBasePrice { get; set; }

        public int HouseUpgradePercent { get; set; }

        public bool IsInterestEnabled { get; set; }

        public int JailWorld { get; set; }

        public int KickDelay { get; set; }

        public double KilometersPerHourMultiplier { get; set; }

        public int MaximumBans { get; set; }
        public int MaximumBusiness { get; set; }
        public int MaximumBusinessPerPlayer { get; set; }
        public int MaximumConvoyMembers { get; set; }
        public int MaximumFuel { get; set; }
        public int MaximumHouses { get; set; }

        public int MaximumHousesPerPlayer { get; set; }

        public int MaximumLogins { get; set; }

        public int MaximumSpeedCameras { get; set; }

        public int MaximumSpikeStrips { get; set; }

        public int MaximumTollGates { get; set; }

        public int MaximumWarnsBeforeKick { get; set; }

        public int PaintJobPrice { get; set; }

        public float ParkingRange { get; set; }

        public int PaymentPerPackage { get; set; }

        public int PlayersBeforePolice { get; set; }
        public int PoliceWeaponsAmmo { get; set; }

        public int RefuelPrice { get; set; }

        public int ResprayPrice { get; set; }

        public int VehicleUnclampPrice { get; set; }

        public int WarnSecondsBeforeJail { get; set; }

        public static async void LoadConfigurationFromFileAsync()
        {
            try
            {
                using (var file = File.OpenRead(@"scriptfiles\serverconfig.json"))
                {
                    Instance = await JsonSerializer.DeserializeAsync<Configuration>(file);
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Failed to load configuration file.");
                throw;
            }
        }

        public static async void SaveConfigurationToFileAsync()
        {
            try
            {
                using (var file = File.Create(@"scriptfiles\serverconfig.json"))
                {
                    await JsonSerializer.SerializeAsync(file, Instance);
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Failed to save configuration file.");
                throw;
            }
        }

        public static async void LoadDefaultConfigurationFromFileAsync()
        {
            try
            {
                using (var file = File.OpenRead(@"scriptfiles\defaultserverconfig.json"))
                {
                    Instance = await JsonSerializer.DeserializeAsync<Configuration>(file);
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Failed to load default configuration file.");
                throw;
            }
        }
    }
}