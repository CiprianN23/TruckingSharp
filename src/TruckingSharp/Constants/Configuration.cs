using SampSharp.GameMode.Definitions;

namespace TruckingSharp.Constants
{
    public static class Configuration
    {
        public const int MaxLogins = 3;

        public const int MaxBans = 3;

        public const bool IsIntrestEnabled = true;
        public const float BankIntrest = 0.001f;

        public const int ExitBuildingTime = 1000; //time in ms

        public const bool CanShowBoughtHouses = true;

        public const int MaxFuel = 5000;
        public const int RefuelMaxPrice = 1000;

        public const int MaxHouses = 2000;
        public const int MaxHousesPerPlayer = 20;
        public const int HouseUpgradePercent = 100;
        public const float ParkRange = 150.0f;

        public const int MaxBusiness = 2000;
        public const int MaxBusinessPerPlayer = 20;

        public const int MaxTollGates = 20;

        public const int MaxSpikeStrips = 10;

        public const int MaxCameras = 120;

        public const int PricePaintJob = 200;
        public const int PriceColorChange = 150;
        public const int PriceRespray = 150;
        public const int PriceGoBase = 200;
        public const int PriceAutoAssist = 2000;

        public const bool CanPoliceGetWeapons = true;
        public static Weapon[] PoliceWeapons = { Weapon.Nitestick, Weapon.Teargas, Weapon.Colt45, Weapon.Shotgun, Weapon.MP5, Weapon.Rifle, Weapon.Spraycan };
        public const int PoliceGunsAmmo = 5000;
        public const int PlayersBeforePolice = 1;

        public const int DefaultJailTime = 120; // time in seconds
        public const int DefaultFinePerStar = 1000;
        public const int DefaultWarnTimeBeforeJail = 60; //time in seconds

        public const int PriceFailedMission = 1000;
        public const int TimeToFailMission = 60;

        public const float CourierMissionRange = 1000.0f;
        public const int PaymentPerPackage = 500;

        public const int UnclampPricePerVehicle = 20000;

        public const int JailWorld = 10254;

        public const bool CanAutoKickAfterWarn = true;
        public const int MaxWarnBeforeKick = 3;
        public const int KickDelay = 50;

        public const int MaximumConvoys = 5;
        public const int MaximumConvoyMembers = 25;

        public const double KilometersPerHourMultiplier = 195.12;
    }
}