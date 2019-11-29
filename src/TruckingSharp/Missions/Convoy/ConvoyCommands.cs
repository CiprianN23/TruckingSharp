using SampSharp.GameMode.Display;
using SampSharp.GameMode.SAMP;
using SampSharp.GameMode.SAMP.Commands;
using TruckingSharp.Commands.Permissions;
using TruckingSharp.Constants;
using TruckingSharp.PlayerClasses.Data;

namespace TruckingSharp.Missions.Convoy
{
    [CommandGroup("player", PermissionChecker = typeof(LoggedPermission))]
    public class ConvoyCommands
    {
        [Command("convoy", Shortcut = "convoy")]
        public static void OnConvoyCommand(Player sender)
        {
            if (sender.PlayerClass != PlayerClassType.TruckDriver)
            {
                sender.SendClientMessage(Color.Red, Messages.CommandOnlyAllowedAsTruckDriver);
                return;
            }

            var convoyStatus = string.Empty;
            var convoyListDialog =
                new ListDialog("Select convoy:", Messages.DialogButtonSelect, Messages.DialogButtonCancel);
            foreach (var convoy in MissionConvoy.Convoys)
            {
                string leaderName;
                int memberCount;
                if (convoy.Status == ConvoyStatus.Empty)
                {
                    memberCount = 0;
                    leaderName = "None";
                    convoyStatus = "Empty";
                }
                else
                {
                    memberCount = convoy.Members.Count;
                    leaderName = convoy.Members[0].Name;

                    switch (convoy.Status)
                    {
                        case ConvoyStatus.Open:
                            convoyStatus = "Open";
                            break;

                        case ConvoyStatus.Closed:
                            convoyStatus = "Closed";
                            break;

                        case ConvoyStatus.Full:
                            convoyStatus = "Full";
                            break;
                    }
                }

                convoyListDialog.AddItem(
                    $"Leader: {{00FF00}}{leaderName}{{FFFFFF}}, members: {{FF0000}}{memberCount}{{FFFFFF}}, Status: {{00FF00}}{convoyStatus}{{FFFFFF}}");
            }

            convoyListDialog.Show(sender);
            convoyListDialog.Response += ConvoyController.ConvoyListDialog_Response;
        }

        [Command("convoykick", Shortcut = "convoykick")]
        public static void OnConvoyKickCommand(Player sender, Player target)
        {
            if (!sender.IsInConvoy)
            {
                sender.SendClientMessage(Color.Red, Messages.PlayerNotPartOfAnyConvoy);
                return;
            }

            var convoy = sender.Convoy;
            var leader = convoy.Members[0];

            if (leader != sender)
            {
                sender.SendClientMessage(Color.Red, Messages.CommandOnlyAllowedAsConvoyLeader);
                return;
            }

            if (target == sender)
            {
                sender.SendClientMessage(Color.Red, Messages.CommandNotAllowedOnSelf);
                return;
            }

            if (target.IsInConvoy && target.Convoy == convoy)
            {
                MissionConvoy.PlayerLeaveConvoy(target);

                sender.SendClientMessage(Color.GreenYellow,
                    $"You've kicked {{0000FF}}{target.Name}{{00FF00}} from the convoy.");
                target.SendClientMessage(Color.GreenYellow,
                    $"Leader {{0000FF}}{sender.Name}{{00FF00}} kicked you from the convoy.");
            }
            else
            {
                sender.SendClientMessage(Color.Red, "You cannot kick a player that's not part of your convoy.");
            }
        }

        [Command("convoyleave", Shortcut = "convoyleave")]
        public static void OnConvoyLeaveCommand(Player sender)
        {
            if (!sender.IsInConvoy)
            {
                sender.SendClientMessage(Color.Red, Messages.PlayerNotPartOfAnyConvoy);
                return;
            }

            MissionConvoy.PlayerLeaveConvoy(sender);
            sender.SendClientMessage(Color.GreenYellow, "You left the convoy.");
        }

        [Command("convoycancel", Shortcut = "convoycancel")]
        public static void OnConvoyCancelCommand(Player sender)
        {
            if (!sender.IsInConvoy)
            {
                sender.SendClientMessage(Color.Red, "You are not part of any convoy.");
                return;
            }

            var convoy = sender.Convoy;
            var leader = convoy.Members[0];

            if (leader != sender)
            {
                sender.SendClientMessage(Color.Red, Messages.CommandOnlyAllowedAsConvoyLeader);
                return;
            }

            MissionConvoy.CancelConvoy(convoy);
        }

        [Command("convoymembers", Shortcut = "convoymembers")]
        public static void OnConvoyMembersCommand(Player sender)
        {
            if (!sender.IsInConvoy)
            {
                sender.SendClientMessage(Color.Red, Messages.PlayerNotPartOfAnyConvoy);
                return;
            }

            var convoy = sender.Convoy;
            var membersListDialog = new ListDialog("Convoy members:", Messages.DialogButtonOk);

            foreach (var member in convoy.Members) membersListDialog.AddItem($"{member.Name}\n");

            membersListDialog.Show(sender);
        }
    }
}