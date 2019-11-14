using SampSharp.GameMode;

namespace TruckingSharp.PlayerClasses.ClassesSpawn
{
    public class TruckerSpawn
    {
        public Vector3 Position { get; set; }
        public float Angle { get; set; }

        public static TruckerSpawn[] TruckerSpawns = {
            new TruckerSpawn {Position = new Vector3(-525.0, -502.0, 26.0), Angle = 0.0f },
            new TruckerSpawn {Position = new Vector3(-74.7, -1137.5, 4.5), Angle = 0.0f },
            new TruckerSpawn {Position = new Vector3(1457.0, 975.5, 11.0), Angle = 0.0f },
            new TruckerSpawn {Position = new Vector3(-2136.0, -247.5, 36.5), Angle = 270.0f },
            new TruckerSpawn {Position = new Vector3(1766.5, -2040.7, 14.0), Angle = 270.0f },
            new TruckerSpawn {Position = new Vector3( -546.0, 2594.0, 54.0), Angle = 270.0f },
            new TruckerSpawn {Position = new Vector3( 332.0, 900.0, 25.0), Angle = 205.0f },
            new TruckerSpawn {Position = new Vector3( -1575.0, -2724.0, 49.0f), Angle = 145.0f }
        };
    }
}