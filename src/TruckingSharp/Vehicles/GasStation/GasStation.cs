using SampSharp.GameMode;
using SampSharp.GameMode.Definitions;
using SampSharp.GameMode.SAMP;
using SampSharp.GameMode.World;
using SampSharp.Streamer.World;

namespace TruckingSharp.Vehicles.GasStation
{
    public class GasStation
    {
        private readonly DynamicMapIcon MapIcon;

        private readonly Pickup Pickup;

        private readonly TextLabel TextLabel;

        public GasStation(Vector3 positon)
        {
            Position = positon;
            Pickup = Pickup.Create(ObjectModel.Petrolpump, PickupType.ScriptedActionsOnlyEveryFewSeconds, positon);
            TextLabel = new TextLabel("Honk the horn\nto refuel your vehicle", Color.Teal, positon, 30.0f);
            MapIcon = new DynamicMapIcon(positon, 56);
        }

        public Vector3 Position { get; set; }
    }
}