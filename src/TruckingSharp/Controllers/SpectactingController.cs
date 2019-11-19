using SampSharp.GameMode;
using SampSharp.GameMode.Controllers;
using System;
using SampSharp.GameMode.SAMP;
using SampSharp.GameMode.Definitions;
using TruckingSharp.Data;

namespace TruckingSharp.Controllers
{
    [Controller]
    public class SpectactingController : IEventListener
    {
        public void RegisterEvents(BaseMode gameMode)
        {
            gameMode.PlayerConnected += Spectate_PlayerConnected;
            gameMode.PlayerDisconnected += Spectate_PlayerDisconnected;
        }

        private void Spectate_PlayerDisconnected(object sender, SampSharp.GameMode.Events.DisconnectEventArgs e)
        {
            var player = sender as Player;

            foreach (Player serverPlayer in Player.All)
            {
                if (!serverPlayer.IsLoggedIn)
                    continue;

                if (serverPlayer.State == PlayerState.Spectating && serverPlayer.SpectatedPlayer == player)
                {
                    player.ToggleSpectating(false);
                    player.SpectatedPlayer = null;
                    player.SpectatedVehicle = null;
                    player.SpectateTimer.IsRunning = false;
                    player.SendClientMessage(Color.Red, "Target player has logged off, ending specate mode.");
                }
            }
        }

        private void Spectate_PlayerConnected(object sender, EventArgs e)
        {
            var player = sender as Player;

            player.SpectateTimer = new Timer(TimeSpan.FromMilliseconds(500), true);
            player.SpectateTimer.IsRunning = false;
            player.SpectateTimer.Tick += (timerSender, e) => SpectateTimer_Tick(timerSender, e, player);
        }

        private static void SpectateTimer_Tick(object sender, EventArgs e, Player player)
        {
            if (player.SpectatedPlayer == null)
                return;

            if (player.State == PlayerState.Spectating)
            {
                player.VirtualWorld = player.SpectatedPlayer.VirtualWorld;
                player.Interior = player.SpectatedPlayer.Interior;

                if (player.SpectateType == SpectateTypes.Player)
                {
                    if (player.SpectatedPlayer.VehicleSeat != -1)
                    {
                        player.SpectateVehicle(player.SpectatedPlayer.Vehicle);
                        player.SpectatedVehicle = player.SpectatedPlayer.Vehicle;
                        player.SpectateType = SpectateTypes.Vehicle;
                        player.SendClientMessage($"{{00FF00}}Player {{FFFF00}}{player.SpectatedPlayer.Name}{{00FF00}} has entered a vehicle, changing spectate mode to match");
                    }
                }
                else
                {
                    if (player.SpectatedPlayer.VehicleSeat == -1)
                    {
                        player.SpectatePlayer(player.SpectatedPlayer);
                        player.SpectateType = SpectateTypes.Player;
                        player.SendClientMessage($"{{00FF00}}Player {{FFFF00}}{player.SpectatedPlayer.Name}{{00FF00}} has exited a vehicle, changing spectate mode to match");
                    }
                }
            }
        }
    }
}