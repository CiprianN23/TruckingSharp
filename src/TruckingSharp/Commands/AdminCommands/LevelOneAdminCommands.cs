using SampSharp.GameMode.Display;
using SampSharp.GameMode.SAMP;
using SampSharp.GameMode.SAMP.Commands;
using SampSharp.GameMode.World;
using TruckingSharp.Commands.Permissions;
using TruckingSharp.Data;

namespace TruckingSharp.Commands.AdminCommands
{
    [CommandGroup("admin", PermissionChecker = typeof(LevelOneAdminPermission))]
    public class LevelOneAdminCommands
    {
        [Command("reports", Shortcut = "reports")]
        public static void OnReportsCommand(BasePlayer sender)
        {
            if (Report.Reports[0].IsEmpty)
            {
                sender.SendClientMessage(Color.Red, "There are no reports.");
                return;
            }

            var reportsDialog = new ListDialog("Reports list:", "Ok");


            foreach (var report in Report.Reports)
            {
                if (!report.IsEmpty)
                    reportsDialog.AddItem($"{report.OffenderName}: {report.Reason}");
            }


            reportsDialog.Show(sender);
        }
    }
}