using sampgamemode.Extensions.PlayersExtensions;
using sampgamemode.World;
using SampSharp.GameMode;
using SampSharp.GameMode.Controllers;

namespace sampgamemode.Controllers
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
            baseGameMode.PlayerRequestClass += (sender, args) => (sender as Player)?.SetClassSelection(args);
            baseGameMode.PlayerRequestSpawn += (sender, args) => (sender as Player)?.SetClassSpawn(args);

            base.RegisterEvents(baseGameMode);
        }
    }
}
