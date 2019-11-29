using SampSharp.GameMode;

namespace TruckingSharp.PlayerClasses.ClassesSpawn
{
    public class RoadWorkerSpawn
    {
        public static readonly RoadWorkerSpawn[] RoadWorkerSpawns =
        {
            new RoadWorkerSpawn {Position = new Vector3(-1866.25, -1715.25, 22.7), Angle = 125.0f}
        };

        public Vector3 Position { get; private set; }
        public float Angle { get; private set; }
    }
}