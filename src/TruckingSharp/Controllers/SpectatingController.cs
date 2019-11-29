using SampSharp.GameMode;
using SampSharp.GameMode.Controllers;
using SampSharp.GameMode.Definitions;
using SampSharp.GameMode.Events;
using SampSharp.GameMode.SAMP;
using System;
using TruckingSharp.Data;

namespace TruckingSharp.Controllers
{
    [Controller]
    public class SpectatingController : IEventListener
    {
        public void RegisterEvents(BaseMode gameMode)
        {
            gameMode.PlayerConnected += Spectate_PlayerConnected;
            gameMode.PlayerDisconnected += Spectate_PlayerDisconnected;
        }

        private void Spectate_PlayerDisconnected(object sender, DisconnectEventArgs e)
        {
            if (!(sender is Player player))
                return;

            foreach (var basePlayer in Player.All)
            {
                var serverPlayer = (Player)basePlayer;

                if (!serverPlayer.IsLoggedIn)
                    continue;

                if (serverPlayer.State != PlayerState.Spectating || serverPlayer.SpectatedPlayer != player)
                    continue;

                player.ToggleSpectating(false);
                player.SpectatedPlayer = null;
                player.SpectatedVehicle = null;
                player.SpectateTimer.IsRunning = false;
                player.SendClientMessage(Color.Red, "Target player has logged off, ending spectate mode.");
            }
        }

        private void Spectate_PlayerConnected(object sender, EventArgs e)
        {
            if (!(sender is Player player))
                return;

            player.SpectateTimer = new Timer(TimeSpan.FromMilliseconds(500), true) { IsRunning = false };
            player.SpectateTimer.Tick += (timerSender, ev) => SpectateTimer_Tick(timerSender, ev, player);
        }

        private static void SpectateTimer_Tick(object sender, EventArgs e, Player player)
        {
            if (player.SpectatedPlayer == null)
                return;

            if (player.State != PlayerState.Spectating)
                return;

            player.VirtualWorld = player.SpectatedPlayer.VirtualWorld;
            player.Interior = player.SpectatedPlayer.Interior;

            if (player.SpectateType == SpectateTypes.Player)
            {
                if (player.SpectatedPlayer.VehicleSeat == -1)
                    return;

                player.SpectateVehicle(player.SpectatedPlayer.Vehicle);
                player.SpectatedVehicle = player.SpectatedPlayer.Vehicle;
                player.SpectateType = SpectateTypes.Vehicle;
                player.SendClientMessage(
                    $"{{00FF00}}Player {{FFFF00}}{player.SpectatedPlayer.Name}{{00FF00}} has entered a vehicle, changing spectate mode to match");
            }
            else
            {
                if (player.SpectatedPlayer.VehicleSeat != -1)
                    return;

                player.SpectatePlayer(player.SpectatedPlayer);
                player.SpectateType = SpectateTypes.Player;
                player.SendClientMessage(
                    $"{{00FF00}}Player {{FFFF00}}{player.SpectatedPlayer.Name}{{00FF00}} has exited a vehicle, changing spectate mode to match");
            }
        }
    }
}