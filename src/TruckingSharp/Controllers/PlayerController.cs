using SampSharp.GameMode;
using SampSharp.GameMode.Controllers;
using TruckingSharp.Extensions.PlayersExtensions;
using TruckingSharp.World;

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
            baseGameMode.PlayerRequestClass += (sender, args) => (sender as Player)?.SetClassSelection(args);
            baseGameMode.PlayerRequestSpawn += (sender, args) => (sender as Player)?.SetClassSpawn(args);
            baseGameMode.PlayerSpawned += (sender, args) => (sender as Player)?.SetPlayerClassColor(args);

            base.RegisterEvents(baseGameMode);
        }
    }
}