using Newtonsoft.Json;
using System.IO;

namespace TruckingSharp
{
    public class Configuration
    {
        public const int MaximumConvoys = 5;

        public int[] PoliceWeapons;

        private Configuration()
        {
        }

        public static Configuration Instance { get; set; }
        public int AutoAssistPrice { get; set; }

        public float BankIntrest { get; set; }

        public bool CanAutoKickAfterWarn { get; set; }

        public bool CanPoliceHaveWeapons { get; set; }

        public bool CanShowBoughtHouses { get; set; }

        public int ColorChangePrice { get; set; }

        public float CourierMissionRange { get; set; }

        public int DefaultJailSeconds { get; set; }

        public int ExitBuildingMiliseconds { get; set; }

        public int FailedMissionPrice { get; set; }

        public int FailMissionSeconds { get; set; }

        public int FinePerWantedLevel { get; set; }

        public int GoBasePrice { get; set; }

        public int HouseUpgradePercent { get; set; }

        public bool IsIntrestEnabled { get; set; }

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

        public static void LoadConfigurationFromFile()
        {
            using StreamReader file = File.OpenText(@"scriptfiles\serverconfig.json");
            JsonSerializer serializer = new JsonSerializer();
            Instance = (Configuration)serializer.Deserialize(file, typeof(Configuration));
        }

        public static void SaveConfigurationToFile()
        {
            using StreamWriter file = File.CreateText(@"scriptfiles\serverconfig.json");
            JsonSerializer serializer = new JsonSerializer();
            serializer.Serialize(file, Instance);
        }

        public static void LoadDefaultConfigurationFromFile()
        {
            using StreamReader file = File.OpenText(@"scriptfiles\defaultserverconfig.json");
            JsonSerializer serializer = new JsonSerializer();
            Instance = (Configuration)serializer.Deserialize(file, typeof(Configuration));
        }
    }
}