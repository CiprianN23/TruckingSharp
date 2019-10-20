using sampgamemode.World;
using SampSharp.GameMode.Controllers;

namespace sampgamemode.Controllers
{
    class VehicleController : BaseVehicleController
    {
        public override void RegisterTypes()
        {
            Vehicle.Register<Vehicle>();
        }
    }
}
