using SampSharp.GameMode.Controllers;

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