using SampSharp.GameMode;

namespace TruckingSharp.Data.ClassesSpawn
{
    public class CourierSpawn
    {
        public Vector3 Position { get; set; }
        public float Angle { get; set; }

        public static CourierSpawn[] CourierSpawns = {
            new CourierSpawn { Position = new Vector3(798.0, -618.75, 16.4), Angle = 0.0f },
            new CourierSpawn { Position = new Vector3(-1849.25, -135.0, 12.0), Angle = 90.0f },
            new CourierSpawn { Position = new Vector3(1050.5, 1931.0, 10.9), Angle = 270.0f }
        };
    }
}