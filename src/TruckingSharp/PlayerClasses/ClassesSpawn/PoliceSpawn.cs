using SampSharp.GameMode;

namespace TruckingSharp.PlayerClasses.ClassesSpawn
{
    public class PoliceSpawn
    {
        public static PoliceSpawn[] PoliceSpawns =
        {
            new PoliceSpawn {Position = new Vector3(1568.5, -1693.5, 6.0), Angle = 180.0f},
            new PoliceSpawn {Position = new Vector3(-1590.0, 716.25, -5.0), Angle = 270.0f},
            new PoliceSpawn {Position = new Vector3(2275.0, 2460.0, 10.9), Angle = 270.0f}
        };

        public Vector3 Position { get; set; }
        public float Angle { get; set; }
    }
}