using SampSharp.GameMode.SAMP.Commands.PermissionCheckers;
using SampSharp.GameMode.World;

namespace TruckingSharp.Commands.Permissions
{
    public class LevelTwoAdminPermission : IPermissionChecker
    {
        public string Message => "You are not a level 2 admin.";

        public bool Check(BasePlayer player)
        {
            return player is Player playerData && playerData.IsLoggedIn && playerData.Account.AdminLevel >= 2;
        }
    }
}