using SampSharp.GameMode.Controllers;

namespace TruckingSharp.Controllers
{
    public class PlayerController : BasePlayerController
    {
        public override void RegisterTypes()
        {
            Player.Register<Player>();
        }
    }
}