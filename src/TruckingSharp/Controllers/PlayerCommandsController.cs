using SampSharp.GameMode;
using SampSharp.GameMode.Controllers;
using SampSharp.GameMode.Events;
using SampSharp.GameMode.SAMP;
using System;

namespace TruckingSharp.Controllers
{
    [Controller]
    public class PlayerCommandsController : IEventListener
    {
        public void RegisterEvents(BaseMode gameMode)
        {
            gameMode.PlayerCommandText += GameMode_PlayerCommandText;
        }

        private void GameMode_PlayerCommandText(object sender, CommandTextEventArgs e)
        {
            var player = sender as Player;

            foreach (var basePlayer in Player.All)
            {
                var players = (Player)basePlayer;

                if (!players.IsLoggedIn)
                    continue;

                if (players.Account.AdminLevel > 0)
                    players.SendClientMessage(Color.Gray, $"{player?.Name} used: {e.Text}");

                Console.WriteLine($"{player?.Name} used: {e.Text}");
            }
        }
    }
}