using SampSharp.GameMode;
using SampSharp.GameMode.Controllers;
using SampSharp.GameMode.SAMP;

namespace TruckingSharp.Controllers
{
    [Controller]
    public class PlayerCommandsController : IEventListener
    {
        public void RegisterEvents(BaseMode gameMode)
        {
            gameMode.PlayerCommandText += GameMode_PlayerCommandText;
        }

        private void GameMode_PlayerCommandText(object sender, SampSharp.GameMode.Events.CommandTextEventArgs e)
        {
            var player = sender as Player;

            foreach (Player players in Player.All)
            {
                if (!players.IsLoggedIn)
                    continue;

                if (players.Account.AdminLevel > 0)
                    players.SendClientMessage(Color.Gray, $"{player.Name} used: {e.Text}");

                System.Console.WriteLine($"{player.Name} used: {e.Text}");
            }
        }
    }
}