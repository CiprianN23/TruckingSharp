using SampSharp.GameMode.Pools;
using SampSharp.GameMode.World;
using System.Threading.Tasks;

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
            foreach (var player in Passengers)
                player.RemoveFromVehicle();

            Driver?.RemoveFromVehicle();
        }

        protected override async void Initialize()
        {
            await Task.Delay(100);
            Fuel = Configuration.Instance.MaximumFuel;
            base.Initialize();
        }
    }
}