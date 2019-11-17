using SampSharp.GameMode;
using SampSharp.GameMode.Controllers;
using SampSharp.GameMode.Definitions;
using SampSharp.GameMode.SAMP;
using SampSharp.GameMode.World;
using TruckingSharp.Constants;
using TruckingSharp.Database;
using TruckingSharp.Database.Repositories;
using TruckingSharp.Missions.Bonus;
using TruckingSharp.Missions.Trucker;

namespace TruckingSharp.Missions.Convoy
{
    [Controller]
    public class ConvoyController : IController, IEventListener
    {
        private static PlayerAccountRepository _accountRepository => new PlayerAccountRepository(DapperConnection.ConnectionString);

        public static void ConvoyListDialog_Response(object sender, SampSharp.GameMode.Events.DialogResponseEventArgs e)
        {
            if (e.DialogButton == DialogButton.Right)
                return;

            var player = e.Player as Player;
            var convoy = MissionConvoy.Convoys[e.ListItem];

            switch (convoy.Status)
            {
                case ConvoyStatus.Empty:
                    MissionConvoy.MakeLeader(player, convoy);
                    break;

                case ConvoyStatus.Open:
                    MissionConvoy.MakeMember(player, convoy);
                    break;

                case ConvoyStatus.Full:
                    player.SendClientMessage(Color.Red, Messages.MissionConvoyAlreadyFull);
                    break;

                case ConvoyStatus.Closed:
                    player.SendClientMessage(Color.Red, Messages.MissionConvoyAlreadyOnRoute);
                    break;
            }
        }

        public static async void ConvoyTimer_Tick(object sender, System.EventArgs e, MissionConvoy convoy)
        {
            convoy.UpdateTextDraw();

            var leader = convoy.Members[0];

            switch (convoy.MissionStep)
            {
                case 0:
                    bool IsSameTrailer = true;

                    if (leader.IsDoingMission)
                    {
                        convoy.MissionCargo = leader.MissionCargo;
                        convoy.FromLocation = leader.FromLocation;
                        convoy.ToLocation = leader.ToLocation;
                        convoy.ConvoyTrailerModel = leader.Vehicle?.Trailer?.Model ?? 0;

                        if (convoy.ConvoyTrailerModel != 0)
                        {
                            foreach (var member in convoy.Members)
                            {
                                if (member == leader)
                                    continue;

                                if (member.Vehicle?.Trailer?.Model != convoy.ConvoyTrailerModel)
                                {
                                    switch (convoy.ConvoyTrailerModel)
                                    {
                                        case VehicleModelType.ArticleTrailer:
                                        case VehicleModelType.ArticleTrailer3:
                                            member.MissionTextDraw.Text = Messages.MissionConvoyCargoTrailerNeeded;
                                            break;

                                        case VehicleModelType.ArticleTrailer2:
                                            member.MissionTextDraw.Text = Messages.MissionConvoyOreTrailerNeeded;
                                            break;

                                        case VehicleModelType.PetrolTrailer:
                                            member.MissionTextDraw.Text = Messages.MissionConvoyFluidsTrailerNeeded;
                                            break;
                                    }
                                    IsSameTrailer = false;
                                }
                            }
                        }
                        else
                        {
                            foreach (var member in convoy.Members)
                            {
                                if (member == leader)
                                    continue;

                                switch (member.Vehicle?.Model)
                                {
                                    case VehicleModelType.Flatbed:
                                    case VehicleModelType.DFT30:
                                        IsSameTrailer = true;
                                        break;

                                    default:
                                        member.MissionTextDraw.Text = Messages.MissionConvoyNoTrailerVehicleNeeded;
                                        break;
                                }
                            }
                        }

                        if (IsSameTrailer)
                        {
                            leader.SendClientMessage(Color.GreenYellow, Messages.MissionconvoyReadyToGo);

                            foreach (var member in convoy.Members)
                            {
                                if (member == leader)
                                    continue;

                                convoy.StartMemberJob(member);
                            }

                            convoy.MissionStep = 1;
                            convoy.Status = ConvoyStatus.Closed;
                        }
                        else
                        {
                            if (!convoy.IsLeaderInformed)
                            {
                                leader.SendClientMessage(Color.Red, Messages.MissionConvoyCantGo);
                                convoy.IsLeaderInformed = true;
                            }
                        }
                    }
                    break;

                case 1:
                    bool AreAllMemebersLoaded = true;

                    foreach (var member in convoy.Members)
                    {
                        if (member.MissionStep != 2)
                            AreAllMemebersLoaded = false;
                    }

                    if (AreAllMemebersLoaded)
                    {
                        leader.SendClientMessage(Color.GreenYellow, Messages.MissionConvoyMembersLoaded);

                        foreach (var member in convoy.Members)
                        {
                            convoy.UpdateMemberJob(member);
                        }

                        convoy.MissionStep = 2;
                    }
                    break;

                case 2:
                    bool didAllMembersUnlaoded = true;

                    foreach(var member in convoy.Members)
                    {
                        if (member.MissionStep != 4)
                            didAllMembersUnlaoded = false;
                    }

                    if (didAllMembersUnlaoded)
                        convoy.MissionStep = 3;
                    break;

                case 3:
                    int numberOfMembers = convoy.Members.Count;

                    int payment = TruckerController.CalculatePayment(convoy.FromLocation, convoy.ToLocation, convoy.MissionCargo);

                    if (!BonusMission.IsMissionFinished
                        && BonusMission.RandomCargo == convoy.MissionCargo
                        && BonusMission.RandomFromLocation == convoy.FromLocation
                        && BonusMission.RandomToLocation == convoy.ToLocation)
                    {
                        payment *= 2;
                        BonusMission.IsMissionFinished = true;
                        BasePlayer.SendClientMessageToAll($"{{00BBFF}}Convoy with leader {{FFBB00}}{leader.Name}{{00BBFF}} has finished the bonus mission.");
                    }

                    int bonus = (numberOfMembers * 25) + 100;
                    payment = (payment * bonus) / 100;

                    foreach (var member in convoy.Members)
                    {
                        member.Reward(payment, 5);

                        var memberAccount = member.Account;
                        memberAccount.ConvoyJobs++;
                        await _accountRepository.UpdateAsync(memberAccount);

                        MissionsController.ClassEndMission(member);

                        member.SendClientMessage(Color.White, $"{{00FF00}}You finished the convoy and earned ${payment}");

                        if (member != convoy.Members[0])
                            member.MissionTextDraw.Text = Messages.MissionConvoyWaitingForLeader;
                    }

                    convoy.MissionCargo = null;
                    convoy.FromLocation = null;
                    convoy.ToLocation = null;
                    convoy.Status = ConvoyStatus.Open;
                    convoy.MissionStep = 0;
                    convoy.ConvoyTrailerModel = 0;
                    convoy.IsLeaderInformed = false;
                    break;
            }
        }

        public void RegisterEvents(BaseMode gameMode)
        {
            gameMode.PlayerDisconnected += Convoy_PlayerDisconnected;
            gameMode.PlayerDied += Convoy_PlayerDied;
        }

        private void Convoy_PlayerDied(object sender, SampSharp.GameMode.Events.DeathEventArgs e)
        {
            var player = sender as Player;
            MissionConvoy.PlayerLeaveConvoy(player);
        }

        private void Convoy_PlayerDisconnected(object sender, SampSharp.GameMode.Events.DisconnectEventArgs e)
        {
            var player = sender as Player;
            MissionConvoy.PlayerLeaveConvoy(player);
        }
    }
}