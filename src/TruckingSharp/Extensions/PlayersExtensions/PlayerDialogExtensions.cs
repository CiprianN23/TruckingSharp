using SampSharp.GameMode.Display;
using SampSharp.GameMode.Events;
using SampSharp.GameMode.World;
using System;

namespace TruckingSharp.Extensions.PlayersExtensions
{
    public static class PlayerDialogExtensions
    {
        public static InputDialog ShowPlayerInputDialog(this BasePlayer player, string caption, string message, bool isPassword, string button1, string button2, Action<object, DialogResponseEventArgs> response)
        {
            var dialog = new InputDialog(caption, message, isPassword, button1, button2);
            dialog.Show(player);
            dialog.Response += (s, e) => response(s, e);
            return dialog;
        }
    }
}