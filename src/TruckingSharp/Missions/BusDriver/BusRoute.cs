using System.Collections.Generic;
using TruckingSharp.Missions.Data;

namespace TruckingSharp.Missions.BusDriver
{
    public class BusRoute
    {
        public static readonly List<BusRoute> BusRoutes = new List<BusRoute>
        {
            new BusRoute(64, 101, 2, "SomeStart1 - SomeStop1", new[] { 65, 67, 94, 99, 69, 75, 76, 64, -1}),
            new BusRoute(64, 102, 1, "SomeStart2 - SomeStop2", new[] { 94, 96, 99, 100, 71, 94, -1}),
            new BusRoute(64, 201, 3, "SomeStart3 - SomeStop3", new[] { 92, 93, 91, 89, 88, 86, 84, 85, 79, 73, 72, 92, -1}),
            new BusRoute(64, 301, 2, "Las Barrancas - Bayside", new[] { 108, 107, 105, 103, 104, 108, -1}),
            new BusRoute(64, 302, 2, "Las Barrancas - Bayside", new[] { 103, 104, 106, 109, 105, 103, -1}),
            new BusRoute(64, 303, 2, "Fort Carson - El Quebrados", new[] { 112, 108, 105, 106, 108, 111, 112, -1}),
            new BusRoute(64, 401, 2, "Palomino Creek - Dillimore", new[] { 116, 117, 120, 118, 115, 116, -1}),
        };

        public int[] Locations;

        public BusRoute(int homeDepot, int lineNumber, int score, string description, int[] locations)
        {
            HomeDepot = homeDepot;
            LineNumber = lineNumber;
            Score = score;
            Description = description;
            Locations = locations;
        }

        public static MissionLocation GetLocation(int index)
        {
            return MissionLocation.MissionLocations[index];
        }

        public string Description { get; set; }
        public int HomeDepot { get; set; }
        public int LineNumber { get; set; }
        public int Score { get; set; }
    }
}