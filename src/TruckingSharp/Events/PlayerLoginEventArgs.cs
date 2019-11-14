using System;

namespace TruckingSharp.Events
{
    public class PlayerLoginEventArgs : EventArgs
    {
        public PlayerLoginEventArgs()
        {
        }

        public PlayerLoginEventArgs(Player player)
        {
            Player = player;
        }

        public Player Player { get; }
        public bool Success { get; set; }
    }
}