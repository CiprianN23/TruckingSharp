using SampSharp.GameMode;
using SampSharp.GameMode.Controllers;
using TruckingSharp.Extensions.PlayersExtensions;

namespace TruckingSharp.Controllers
{
    public class PlayerController : BasePlayerController
    {
        public override void RegisterTypes()
        {
            Player.Register<Player>();
        }

        public override void RegisterEvents(BaseMode gameMode)
        {
            var baseGameMode = gameMode as GameMode;

            baseGameMode.PlayerCommandText += (sender, args) => (sender as Player)?.OnPlayerCommandTextSendToAdmins(args);

            base.RegisterEvents(baseGameMode);
        }
    }
}