using sampgamemode.World;
using SampSharp.GameMode.SAMP.Commands.PermissionCheckers;
using SampSharp.GameMode.World;

namespace sampgamemode.Commands.Permissions
{
    class LoggedPermission : IPermissionChecker
    {
        public string Message => "You are not logged in";

        public bool Check(BasePlayer player)
        {
            return player is Player playerData && playerData.IsLoggedIn;
        }
    }
}
