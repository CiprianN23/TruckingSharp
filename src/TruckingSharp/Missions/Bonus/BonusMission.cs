using TruckingSharp.Missions.Data;

namespace TruckingSharp.Missions.Bonus
{
    public static class BonusMission
    {
        public static MissionCargo RandomCargo { get; set; }
        public static MissionLocation RandomFromLocation { get; set; }
        public static MissionLocation RandomToLocation { get; set; }
        public static bool IsMissionFinished { get; set; }
    }
}