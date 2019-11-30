using System;
using System.Collections.Generic;

namespace TruckingSharp.Missions.Data
{
    public class MissionCargo
    {
        public static readonly List<MissionCargo> MissionCargoes = new List<MissionCargo>
        {
            new MissionCargo("Dummy", false, 0, MissionCargoVehicleType.None, new[] {0}, new[] {0}),

            new MissionCargo("Gravel", false, 1.0, MissionCargoVehicleType.OreTrailer, new[] {11}, new[] {1, 2, 3}),
            new MissionCargo("Sand", false, 1.0, MissionCargoVehicleType.OreTrailer, new[] {12}, new[] {1, 2, 3, 4, 5}),
            new MissionCargo("Rocks", false, 1.0, MissionCargoVehicleType.OreTrailer, new[] {11}, new[] {1, 2, 3, 4, 5}),
            new MissionCargo("Coal", false, 1.0, MissionCargoVehicleType.OreTrailer, new[] {12}, new[] {6, 7, 8}),
            new MissionCargo("Ore", false, 1.0, MissionCargoVehicleType.OreTrailer, new[] {12}, new[] {6, 9}),
            new MissionCargo("Logs", false, 1.0, MissionCargoVehicleType.OreTrailer, new[] {13}, new[] {7, 10}),
            new MissionCargo("Woodchips", false, 1.0, MissionCargoVehicleType.OreTrailer, new[] {10}, new[] {14}),
            new MissionCargo("Dry Waste", false, 1.0, MissionCargoVehicleType.OreTrailer, new[] {6, 15, 16}, new[] {17, 18, 19}),
            new MissionCargo("Debris", false, 1.0, MissionCargoVehicleType.OreTrailer, new[] {17, 19}, new[] {6, 18, 20}),
            new MissionCargo("Wheat", false, 1.0, MissionCargoVehicleType.OreTrailer, new[] {21, 22}, new[] {18, 23, 24}),

            new MissionCargo("Unleaded Fuel", true, 1.0, MissionCargoVehicleType.FluidsTrailer, new[] {8, 25, 26}, new[] {27, 28, 29, 30, 31, 32, 33, 34}),
            new MissionCargo("Diesel Fuel", true, 1.0, MissionCargoVehicleType.FluidsTrailer, new[] {8, 25, 26}, new[] {27, 28, 29, 30, 31, 32, 33, 34}),
            new MissionCargo("Aviation Fuel", true, 1.0, MissionCargoVehicleType.FluidsTrailer, new[] {8, 25, 26}, new[] {35, 36}),
            new MissionCargo("Crude Oil", false, 1.0, MissionCargoVehicleType.FluidsTrailer, new[] {8, 25, 26}, new[] {6}),
            new MissionCargo("Liquid Nitrogen", false, 1.0, MissionCargoVehicleType.FluidsTrailer, new[] {37, 38}, new[] {39, 40}),
            new MissionCargo("Chemicals", false, 1.0, MissionCargoVehicleType.FluidsTrailer, new[] {37}, new[] {6, 15, 39, 40}),
            new MissionCargo("Pure Water", false, 1.0, MissionCargoVehicleType.FluidsTrailer, new[] {37}, new[] {15, 40, 41}),
            new MissionCargo("Grease", false, 1.0, MissionCargoVehicleType.FluidsTrailer, new[] {8, 25}, new[] {6, 10, 14, 42}),
            new MissionCargo("Fertilizer", false, 1.0, MissionCargoVehicleType.FluidsTrailer, new[] {37, 38}, new[] {21, 22}),
            new MissionCargo("Milk", false, 1.0, MissionCargoVehicleType.FluidsTrailer, new[] {21, 22}, new[] {43, 44, 45}),
            new MissionCargo("Beer", false, 1.0, MissionCargoVehicleType.FluidsTrailer, new[] {41}, new[] {44, 46}),
            new MissionCargo("Ethanol", false, 1.0, MissionCargoVehicleType.FluidsTrailer, new[] {37, 41}, new[] {26, 39, 40}),

            new MissionCargo("Food", false, 1.0, MissionCargoVehicleType.CargoTrailer, new[] {7, 44, 47, 48}, new[] {49, 50, 51, 52, 53}),
            new MissionCargo("Drinks", false, 1.0, MissionCargoVehicleType.CargoTrailer, new[] {15, 54, 55}, new[] {49, 50, 51, 52, 53}),
            new MissionCargo("Bottled Beer", true, 1.0, MissionCargoVehicleType.CargoTrailer, new[] {56}, new[] {57, 58, 59, 60, 61}),
            new MissionCargo("Luxury Goods", false, 1.0, MissionCargoVehicleType.CargoTrailer, new[] {24, 55}, new[] {47, 48, 50}),
            new MissionCargo("Electronics", true, 1.0, MissionCargoVehicleType.CargoTrailer, new[] {7, 24, 48}, new[] {50, 51, 62, 63}),
            new MissionCargo("Sport Equipment", false, 1.0, MissionCargoVehicleType.CargoTrailer, new[] {47, 48}, new[] {50, 53, 57}),
            new MissionCargo("Boards", false, 1.0, MissionCargoVehicleType.CargoTrailer, new[] {10}, new[] {4, 7, 18, 22}),
            new MissionCargo("Building Materials", false, 1.0, MissionCargoVehicleType.CargoTrailer, new[] {4}, new[] {1, 2, 3, 5}),
            new MissionCargo("LiveStock", false, 1.0, MissionCargoVehicleType.CargoTrailer, new[] {21, 22}, new[] {23, 43, 47}),
            new MissionCargo("Meat", false, 1.0, MissionCargoVehicleType.CargoTrailer, new[] {43},new[] {44, 49, 50, 51, 52, 53}),
            new MissionCargo("Paper", false, 1.0, MissionCargoVehicleType.CargoTrailer, new[] {14}, new[] {9, 62}),

            new MissionCargo("Cement", false, 1.0, MissionCargoVehicleType.CementTruck, new[] {12}, new[] {1, 2, 3, 4}),

            new MissionCargo("Food", false, 0.6, MissionCargoVehicleType.NoTrailer, new[] {7, 44, 47, 48}, new[] {49, 50, 51, 52, 53}),
            new MissionCargo("Drinks", false, 0.6, MissionCargoVehicleType.NoTrailer, new[] {15, 54, 55}, new[] {49, 50, 51, 52, 53}),
            new MissionCargo("Bottled Beer", true, 0.6, MissionCargoVehicleType.NoTrailer, new[] {56}, new[] {57, 58, 59, 60, 61}),
            new MissionCargo("Luxury Goods", false, 0.6, MissionCargoVehicleType.NoTrailer, new[] {24, 55}, new[] {47, 48, 50}),
            new MissionCargo("Electronics", true, 0.6, MissionCargoVehicleType.NoTrailer, new[] {7, 24, 48}, new[] {50, 51, 62, 63}),
            new MissionCargo("Sport Equipment", false, 0.6, MissionCargoVehicleType.NoTrailer, new[] {47, 48}, new[] {50, 53, 57}),
            new MissionCargo("Boards", false, 0.6, MissionCargoVehicleType.NoTrailer, new[] {10}, new[] {4, 7, 18, 22}),
            new MissionCargo("Building Materials", false, 0.6, MissionCargoVehicleType.NoTrailer, new[] {4}, new[] {1, 2, 3, 5}),
            new MissionCargo("LiveStock", false, 0.6, MissionCargoVehicleType.NoTrailer, new[] {21, 22}, new[] {23, 43, 47}),
            new MissionCargo("Meat", false, 0.6, MissionCargoVehicleType.NoTrailer, new[] {43}, new[] {44, 49, 50, 51, 52, 53}),
            new MissionCargo("Paper", false, 0.6, MissionCargoVehicleType.NoTrailer, new[] {14}, new[] {9, 62})
        };

        private static readonly Random Random = new Random();

        public readonly int[] FromLocations;
        public readonly int[] ToLocations;

        private MissionCargo(string cargoName, bool isWantedByMafia, double payPerUnit,
            MissionCargoVehicleType jobCargoVehicleType, int[] fromLocations, int[] toLocations)
        {
            Name = cargoName;
            IsWantedByMafia = isWantedByMafia;
            PayPerUnit = payPerUnit;
            JobCargoVehicleType = jobCargoVehicleType;
            FromLocations = fromLocations;
            ToLocations = toLocations;
        }

        public string Name { get; }
        public bool IsWantedByMafia { get; }
        public MissionCargoVehicleType JobCargoVehicleType { get; }
        public double PayPerUnit { get; }

        public static List<MissionCargo> GetCargoList(MissionCargoVehicleType jobCargoVehicleType, ref int numProducts)
        {
            var cargoList = new List<MissionCargo>();
            foreach (var cargo in MissionCargoes)
            {
                if (numProducts >= 50)
                    continue;

                if (cargo.JobCargoVehicleType != jobCargoVehicleType)
                    continue;

                cargoList.Add(cargo);
                numProducts++;
            }

            return cargoList;
        }

        public static MissionCargo GetRandomCargo(MissionCargoVehicleType jobCargoVehicleType)
        {
            var numProducts = 0;

            var randomCargo = GetCargoList(jobCargoVehicleType, ref numProducts);

            return randomCargo[Random.Next(numProducts)];
        }

        public static MissionLocation GetRandomStartLocation(MissionCargo missionCargo)
        {
            var numProducts = 0;

            foreach (var location in missionCargo.FromLocations)
            {
                if (location != 0)
                {
                    numProducts++;
                }
                else
                {
                    break;
                }
            }

            var index = missionCargo.FromLocations[Random.Next(numProducts)];

            return MissionLocation.MissionLocations[index];
        }

        public static MissionLocation GetRandomEndLocation(MissionCargo missionCargo)
        {
            var numProducts = 0;

            foreach (var location in missionCargo.ToLocations)
            {
                if (location != 0)
                {
                    numProducts++;
                }
                else
                {
                    break;
                }
            }

            var index = missionCargo.ToLocations[Random.Next(numProducts)];

            return MissionLocation.MissionLocations[index];
        }
    }
}