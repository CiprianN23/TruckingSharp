using SampSharp.GameMode.Controllers;
using TruckingSharp.World;

namespace TruckingSharp.Controllers
{
    internal class VehicleController : BaseVehicleController
    {
        public override void RegisterTypes()
        {
            Vehicle.Register<Vehicle>();
        }
    }
}