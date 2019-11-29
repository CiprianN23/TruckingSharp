using SampSharp.GameMode.SAMP;
using SampSharp.GameMode.World;
using TruckingSharp.Helpers;

namespace TruckingSharp.Data
{
    public class Report
    {
        public static readonly Report[] Reports = ArrayHelper.InitializeArray<Report>(50);
        private bool _isUsed;
        public bool IsEmpty => !_isUsed && string.IsNullOrEmpty(OffenderName) && string.IsNullOrEmpty(Reason);
        public string OffenderName { get; private set; }
        public string Reason { get; private set; }

        public static void AddReport(string offenderName, string reason)
        {
            var reportId = -1;

            for (var i = 0; i < 50; i++)
            {
                if (Reports[i]._isUsed)
                    continue;

                reportId = i;
                break;
            }

            if (reportId == -1)
            {
                for (var i = 1; i < 50; i++)
                {
                    Reports[i - 1]._isUsed = Reports[i]._isUsed;
                    Reports[i - 1].OffenderName = $"{Reports[i].OffenderName}";
                    Reports[i - 1].Reason = $"{Reports[i].Reason}";
                }

                reportId = 49;
            }

            Reports[reportId]._isUsed = true;
            Reports[reportId].OffenderName = $"{offenderName}";
            Reports[reportId].Reason = $"{reason}";
        }

        public static void SendReportToAdmins(BasePlayer offender, string reason, bool autoReport = false)
        {
            string message;
            string gameTextMessage;
            string totalReason;

            if (!autoReport)
            {
                message = $"*** Report: {offender.Name} ({offender.Id}): {reason}";
                gameTextMessage = $"Report:~n~~g~{offender.Name} ({offender.Id})~n~~r~{reason}";
                totalReason = reason;
            }
            else
            {
                message = $"*** Auto-Report: {offender.Name} ({offender.Id}): {reason}";
                gameTextMessage = $"Auto-Report:~n~~g~{offender.Name} ({offender.Id})~n~~r~{reason}";
                totalReason = $"{reason} (by AntiHack)";
            }

            foreach (var basePlayer in Player.All)
            {
                var admin = (Player)basePlayer;

                if (admin.Account.AdminLevel <= 0)
                    continue;

                admin.SendClientMessage(Color.Cyan, message);
                admin.GameText(gameTextMessage, 10000, 4);
            }

            AddReport(offender.Name, totalReason);
        }
    }
}