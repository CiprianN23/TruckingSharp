using SampSharp.GameMode;

namespace sampgamemode.Data.ClassesSpawn
{
    public class MafiaSpawn
    {
        public Vector3 Position { get; set; }
        public float Angle { get; set; }

        public static MafiaSpawn[] MafiaSpawns = {
            new MafiaSpawn { Position = new Vector3(2822.5, 898.5, 10.8), Angle = 0.0f }
        };
    }
}
