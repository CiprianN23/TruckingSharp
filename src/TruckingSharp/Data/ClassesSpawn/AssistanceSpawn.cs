using SampSharp.GameMode;

namespace TruckingSharp.Data.ClassesSpawn
{
    public class AssistanceSpawn
    {
        public Vector3 Position { get; set; }
        public float Angle { get; set; }

        public static AssistanceSpawn[] AssistanceSpawns = {
            new AssistanceSpawn { Position = new Vector3(211.25, 24.75, 2.6), Angle = 270.0f }
        };
    }
}