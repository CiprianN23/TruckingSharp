using SampSharp.GameMode.Pools;
using SampSharp.GameMode.World;
using TruckingSharp.Constants;

namespace TruckingSharp
{
    [PooledType]
    public class Vehicle : BaseVehicle
    {
        public int Fuel { get; set; }
        public bool IsAdminSpawned { get; set; }
        public bool IsWantedByMafia { get; set; }
        public bool IsOwned { get; set; }

        protected override void Initialize()
        {
            base.Initialize();

            Fuel = Configuration.MaxFuel;
        }
    }
}