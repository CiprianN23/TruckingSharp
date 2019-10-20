using TruckingSharp.World;
using SampSharp.GameMode.Controllers;

namespace TruckingSharp.Controllers
{
    class VehicleController : BaseVehicleController
    {
        public override void RegisterTypes()
        {
            Vehicle.Register<Vehicle>();
        }
    }
}
