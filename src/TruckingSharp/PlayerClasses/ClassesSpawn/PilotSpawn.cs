using SampSharp.GameMode;

namespace TruckingSharp.PlayerClasses.ClassesSpawn
{
    public class PilotSpawn
    {
        public Vector3 Position { get; set; }
        public float Angle { get; set; }

        public static PilotSpawn[] PilotSpawns = {
            new PilotSpawn { Position = new Vector3(2010.0, -2345.0, 13.6), Angle = 90.0f },
            new PilotSpawn { Position = new Vector3(-1211.0, -105.0, 14.2), Angle = 135.0f },
            new PilotSpawn { Position = new Vector3(1630.0, 1615.0, 10.9), Angle = 90.0f }
        };
    }
}