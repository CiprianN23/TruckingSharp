using sampgamemode.World;
using System.Linq;

namespace sampgamemode.Extensions.PlayersExtensions
{
    public static class PlayerChatExtensions
    {
        public static void SendPublicMessage(this Player player, string message)
        {
            Player.
                All.
                Cast<Player>().
                Where(x => x.IsLoggedIn).
                ToList().
                ForEach(x => x.SendClientMessage($"{player.Color.ToString()}{player.Name} {{FFFFFF}}({player.Id}): {message}"));
        }
    }
}
