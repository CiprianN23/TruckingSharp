using sampgamemode.Constants;
using SampSharp.GameMode.Pools;
using SampSharp.GameMode.World;

namespace sampgamemode.World
{
    [PooledType]
    public class Vehicle : BaseVehicle
    {
        public int Fuel { get; set; }
        public bool IsAdminSpawned { get; set; }

        protected override void Initialize()
        {
            base.Initialize();

            Fuel = Configuration.MaxFuel;
        }
    }
}
