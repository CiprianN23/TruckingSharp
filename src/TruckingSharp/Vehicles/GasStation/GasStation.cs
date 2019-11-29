using SampSharp.GameMode;
using SampSharp.GameMode.Definitions;
using SampSharp.GameMode.SAMP;
using SampSharp.GameMode.World;
using SampSharp.Streamer.World;

namespace TruckingSharp.Vehicles.GasStation
{
    public class GasStation
    {
        private readonly DynamicMapIcon _mapIcon;

        private readonly Pickup _pickup;

        private readonly TextLabel _textLabel;

        public GasStation(Vector3 position)
        {
            Position = position;
            _pickup = Pickup.Create(ObjectModel.Petrolpump, PickupType.ScriptedActionsOnlyEveryFewSeconds, position);
            _textLabel = new TextLabel("Honk the horn\nto refuel your vehicle", Color.Teal, position, 30.0f);
            _mapIcon = new DynamicMapIcon(position, 56);
        }

        public Vector3 Position { get; }
    }
}