using SampSharp.GameMode;

namespace TruckingSharp.PlayerClasses.ClassesSpawn
{
    public class BusDriverSpawn
    {
        public static BusDriverSpawn[] BusDriverSpawns =
        {
            new BusDriverSpawn {Position = new Vector3(1809.0, -1905.0, 13.6), Angle = 90.0f},
            new BusDriverSpawn {Position = new Vector3(1060.0, 1260.0, 11.0), Angle = 270.0f},
            new BusDriverSpawn {Position = new Vector3(-1983.0, 110.0, 27.7), Angle = 180.0f}
        };

        public Vector3 Position { get; set; }

        public float Angle { get; set; }
    }
}