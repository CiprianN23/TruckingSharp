namespace TruckingSharp.Database.Entities
{
    public class ClassSpawn
    {
        public int Id { get; set; }
        public float PositionX { get; set; }
        public float PositionY { get; set; }
        public float PositionZ { get; set; }
        public float Angle { get; set; }
        public int ClassType { get; set; }
        public string Name { get; set; }
    }
}