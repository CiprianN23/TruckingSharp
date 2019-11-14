using SampSharp.GameMode;

namespace TruckingSharp.PlayerClasses.ClassesSpawn
{
    public class RoadWorkerSpawn
    {
        public Vector3 Position { get; set; }
        public float Angle { get; set; }

        public static RoadWorkerSpawn[] RoadworkerSpawns = {
            new RoadWorkerSpawn { Position = new Vector3(-1866.25, -1715.25, 22.7), Angle = 125.0f }
        };
    }
}