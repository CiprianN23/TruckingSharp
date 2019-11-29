using SampSharp.GameMode.Pools;
using SampSharp.GameMode.World;

namespace TruckingSharp
{
    [PooledType]
    public class Vehicle : BaseVehicle
    {
        public int Fuel { get; set; }
        public bool IsAdminSpawned { get; set; }
        public bool IsWantedByMafia { get; set; }
        public bool IsOwned { get; set; }

        public void RemoveAllPlayersFromVehicle()
        {
            foreach (BasePlayer player in Passengers)
                player.RemoveFromVehicle();

            Driver?.RemoveFromVehicle();
        }

        protected override void Initialize()
        {
            Fuel = Configuration.Instance.MaximumFuel;
            base.Initialize();
        }
    }
}