using SampSharp.GameMode.SAMP.Commands.PermissionCheckers;
using SampSharp.GameMode.World;

namespace TruckingSharp.Commands.Permissions
{
    public class LevelOneAdminPermission : IPermissionChecker
    {
        public string Message => "You are not a level 1 admin.";

        public bool Check(BasePlayer player)
        {
            return player is Player playerData && playerData.IsLoggedIn && playerData.Account.AdminLevel >= 1;
        }
    }
}