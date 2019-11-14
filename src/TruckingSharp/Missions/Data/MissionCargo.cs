using System;
using System.Collections.Generic;

namespace TruckingSharp.Missions.Data
{
    public class MissionCargo
    {
        public static List<MissionCargo> MissionCargos = new List<MissionCargo>()
        {
            new MissionCargo("Dummy", false, 0, MissionCargoVehicleType.None, new int[] {0}, new int[] {0}),

            new MissionCargo("Gravel", false, 1.0, MissionCargoVehicleType.OreTrailer, new int[] {11}, new int[] {1, 2, 3}),
            new MissionCargo("Sand", false, 1.0, MissionCargoVehicleType.OreTrailer, new int[] {12}, new int[] {1, 2, 3, 4, 5}),
            new MissionCargo("Rocks", false, 1.0, MissionCargoVehicleType.OreTrailer, new int[] {11}, new int[] {1, 2, 3, 4, 5}),
            new MissionCargo("Coal", false, 1.0, MissionCargoVehicleType.OreTrailer, new int[] {12}, new int[] {6, 7, 8}),
            new MissionCargo("Ore", false, 1.0, MissionCargoVehicleType.OreTrailer, new int[] {12}, new int[] {6, 9}),
            new MissionCargo("Logs", false, 1.0, MissionCargoVehicleType.OreTrailer, new int[] {13}, new int[] {7, 10}),
            new MissionCargo("Woodchips", false, 1.0, MissionCargoVehicleType.OreTrailer, new int[] {10}, new int[] {14}),
            new MissionCargo("Dry Waste", false, 1.0, MissionCargoVehicleType.OreTrailer, new int[] {6, 15, 16}, new int[] {17, 18, 19}),
            new MissionCargo("Debris", false, 1.0, MissionCargoVehicleType.OreTrailer, new int[] {17, 19}, new int[] {6, 18, 20}),
            new MissionCargo("Wheat", false, 1.0, MissionCargoVehicleType.OreTrailer, new int[] {21, 22}, new int[] {18, 23, 24}),

            new MissionCargo("Unleaded Fuel", true, 1.0, MissionCargoVehicleType.FluidsTrailer, new int[] {8, 25, 26}, new int[] {27, 28, 29, 30, 31, 32, 33, 34}),
            new MissionCargo("Diesel Fuel", true, 1.0, MissionCargoVehicleType.FluidsTrailer, new int[] {8, 25, 26}, new int[] {27, 28, 29, 30, 31, 32, 33, 34}),
            new MissionCargo("Aviation Fuel", true, 1.0, MissionCargoVehicleType.FluidsTrailer, new int[] {8, 25, 26}, new int[] {35, 36}),
            new MissionCargo("Crude Oil", false, 1.0, MissionCargoVehicleType.FluidsTrailer, new int[] {8, 25, 26}, new int[] {6}),
            new MissionCargo("Liquid Nitrogen", false, 1.0, MissionCargoVehicleType.FluidsTrailer, new int[] {37, 38}, new int[] {39, 40}),
            new MissionCargo("Chemicals", false, 1.0, MissionCargoVehicleType.FluidsTrailer, new int[] {37}, new int[] {6, 15, 39, 40}),
            new MissionCargo("Pure Water", false, 1.0, MissionCargoVehicleType.FluidsTrailer, new int[] {37}, new int[] {15, 40, 41}),
            new MissionCargo("Grease", false, 1.0, MissionCargoVehicleType.FluidsTrailer, new int[] {8, 25}, new int[] {6, 10, 14, 42}),
            new MissionCargo("Fertilizer", false, 1.0, MissionCargoVehicleType.FluidsTrailer, new int[] {37, 38}, new int[] {21, 22}),
            new MissionCargo("Milk", false, 1.0, MissionCargoVehicleType.FluidsTrailer, new int[] {21, 22}, new int[] {43, 44, 45}),
            new MissionCargo("Beer", false, 1.0, MissionCargoVehicleType.FluidsTrailer, new int[] {41}, new int[] {44, 46}),
            new MissionCargo("Ethanol", false, 1.0, MissionCargoVehicleType.FluidsTrailer, new int[] {37, 41}, new int[] {26, 39, 40}),

            new MissionCargo("Food", false, 1.0, MissionCargoVehicleType.CargoTrailer, new int[] {7, 44, 47, 48}, new int[] {49, 50, 51, 52, 53}),
            new MissionCargo("Drinks", false, 1.0, MissionCargoVehicleType.CargoTrailer, new int[] {15, 54, 55}, new int[] {49, 50, 51, 52, 53}),
            new MissionCargo("Bottled Beer", true, 1.0, MissionCargoVehicleType.CargoTrailer, new int[] {56}, new int[] {57, 58, 59, 60, 61}),
            new MissionCargo("Luxury Goods", false, 1.0, MissionCargoVehicleType.CargoTrailer, new int[] {24, 55}, new int[] {47, 48, 50}),
            new MissionCargo("Electronics", true, 1.0, MissionCargoVehicleType.CargoTrailer, new int[] {7, 24, 48}, new int[] {50, 51, 62, 63}),
            new MissionCargo("Sport Equipment", false, 1.0, MissionCargoVehicleType.CargoTrailer, new int[] {47, 48}, new int[] {50, 53, 57}),
            new MissionCargo("Boards", false, 1.0, MissionCargoVehicleType.CargoTrailer, new int[] {10}, new int[] {4, 7, 18, 22}),
            new MissionCargo("Building Materials", false, 1.0, MissionCargoVehicleType.CargoTrailer, new int[] {4}, new int[] {1, 2, 3, 5}),
            new MissionCargo("LiveStock", false, 1.0, MissionCargoVehicleType.CargoTrailer, new int[] {21, 22}, new int[] {23, 43, 47}),
            new MissionCargo("Meat", false, 1.0, MissionCargoVehicleType.CargoTrailer, new int[] {43}, new int[] {44, 49, 50, 51, 52, 53 }),
            new MissionCargo("Paper", false, 1.0, MissionCargoVehicleType.CargoTrailer, new int[] {14}, new int[] {9, 62 }),

            new MissionCargo("Cement", false, 1.0, MissionCargoVehicleType.CementTruck, new int[] {12}, new int[] {1, 2, 3, 4}),

            new MissionCargo("Food", false, 0.6, MissionCargoVehicleType.NoTrailer, new int[] {7, 44, 47, 48}, new int[] {49, 50, 51, 52, 53}),
            new MissionCargo("Drinks", false, 0.6, MissionCargoVehicleType.NoTrailer, new int[] {15, 54, 55}, new int[] {49, 50, 51, 52, 53}),
            new MissionCargo("Bottled Beer", true, 0.6, MissionCargoVehicleType.NoTrailer, new int[] {56}, new int[] {57, 58, 59, 60, 61}),
            new MissionCargo("Luxury Goods", false, 0.6, MissionCargoVehicleType.NoTrailer, new int[] {24, 55}, new int[] {47, 48, 50}),
            new MissionCargo("Electronics", true, 0.6, MissionCargoVehicleType.NoTrailer, new int[] {7, 24, 48}, new int[] {50, 51, 62, 63}),
            new MissionCargo("Sport Equipment", false, 0.6, MissionCargoVehicleType.NoTrailer, new int[] {47, 48}, new int[] {50, 53, 57}),
            new MissionCargo("Boards", false, 0.6, MissionCargoVehicleType.NoTrailer, new int[] {10}, new int[] {4, 7, 18, 22}),
            new MissionCargo("Building Materials", false, 0.6, MissionCargoVehicleType.NoTrailer, new int[] {4}, new int[] {1, 2, 3, 5}),
            new MissionCargo("LiveStock", false, 0.6, MissionCargoVehicleType.NoTrailer, new int[] {21, 22}, new int[] {23, 43, 47}),
            new MissionCargo("Meat", false, 0.6, MissionCargoVehicleType.NoTrailer, new int[] {43}, new int[] {44, 49, 50, 51, 52, 53 }),
            new MissionCargo("Paper", false, 0.6, MissionCargoVehicleType.NoTrailer, new int[] {14}, new int[] {9, 62 }),
        };

        public int[] FromLocations = new int[30];
        public int[] ToLocations = new int[30];

        public MissionCargo(string cargoName, bool isWantedByMafia, double payPerUnit, MissionCargoVehicleType jobCargoVehicleType, int[] fromLocations, int[] toLocations)
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

        private static readonly Random _random = new Random();

        public static List<MissionCargo> GetCargoList(MissionCargoVehicleType jobCargoVehicleType, ref int numProducts)
        {
            List<MissionCargo> cargoList = new List<MissionCargo>();
            foreach (var cargo in MissionCargos)
            {
                if (numProducts < 50)
                {
                    if (cargo.JobCargoVehicleType == jobCargoVehicleType)
                    {
                        cargoList.Add(cargo);
                        numProducts++;
                    }
                }
            }

            return cargoList;
        }

        public static MissionCargo GetRandomCargo(MissionCargoVehicleType jobCargoVehicleType)
        {
            List<MissionCargo> randomCargo = new List<MissionCargo>();
            int numProducts = 0;

            randomCargo = GetCargoList(jobCargoVehicleType, ref numProducts);

            return randomCargo[_random.Next(numProducts)];
        }

        public static MissionLocation GetRandomStartLocation(MissionCargo missionCargo)
        {
            int numProducts = 0;

            for (int i = 0; i < missionCargo.FromLocations.Length; i++)
            {
                if (missionCargo.FromLocations[i] != 0)
                    numProducts++;
                else
                    break;
            }

            int index = missionCargo.FromLocations[_random.Next(numProducts)];

            return MissionLocation.MissionLocations[index];
        }

        public static MissionLocation GetRandomEndLocation(MissionCargo missionCargo)
        {
            int numProducts = 0;

            for (int i = 0; i < missionCargo.ToLocations.Length; i++)
            {
                if (missionCargo.ToLocations[i] != 0)
                    numProducts++;
                else
                    break;
            }

            int index = missionCargo.ToLocations[_random.Next(numProducts)];

            return MissionLocation.MissionLocations[index];
        }
    }
}