using SampSharp.GameMode.SAMP.Commands.PermissionCheckers;
using SampSharp.GameMode.World;

namespace TruckingSharp.Commands.Permissions
{
    internal class LoggedPermission : IPermissionChecker
    {
        public string Message => "You are not logged in.";

        public bool Check(BasePlayer player)
        {
            return player is Player playerData && playerData.IsLoggedIn;
        }
    }
}