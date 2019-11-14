using SampSharp.GameMode.Events;
using SampSharp.GameMode.SAMP;

namespace TruckingSharp.Extensions.PlayersExtensions
{
    public static class PlayerCommandsExtension
    {
        public static void OnPlayerCommandTextSendToAdmins(this Player player, CommandTextEventArgs args)
        {
            foreach (Player players in Player.All)
            {
                if (!players.IsLoggedIn)
                    continue;

                if (players.Account.AdminLevel > 0)
                    players.SendClientMessage(Color.Gray, $"{player.Name} used: {args.Text}");
            }
        }
    }
}