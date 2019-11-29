using SampSharp.GameMode;
using SampSharp.GameMode.Controllers;
using SampSharp.GameMode.Definitions;
using SampSharp.GameMode.Events;
using SampSharp.GameMode.SAMP;
using SampSharp.GameMode.World;
using System;
using TruckingSharp.Constants;
using TruckingSharp.Database.Repositories;
using TruckingSharp.Missions.Bonus;
using TruckingSharp.Missions.Trucker;

namespace TruckingSharp.Missions.Convoy
{
    [Controller]
    public class ConvoyController : IEventListener
    {
        private static PlayerAccountRepository AccountRepository =>
            new PlayerAccountRepository(ConnectionFactory.GetConnection);

        public void RegisterEvents(BaseMode gameMode)
        {
            gameMode.PlayerDisconnected += Convoy_PlayerDisconnected;
            gameMode.PlayerDied += Convoy_PlayerDied;
        }

        public static void ConvoyListDialog_Response(object sender, DialogResponseEventArgs e)
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
                    player?.SendClientMessage(Color.Red, Messages.MissionConvoyAlreadyFull);
                    break;

                case ConvoyStatus.Closed:
                    player?.SendClientMessage(Color.Red, Messages.MissionConvoyAlreadyOnRoute);
                    break;
            }
        }

        public static async void ConvoyTimer_Tick(object sender, EventArgs e, MissionConvoy convoy)
        {
            convoy.UpdateTextDraw();

            var leader = convoy.Members[0];

            switch (convoy.MissionStep)
            {
                case 0:
                    var isSameTrailer = true;

                    if (leader.IsDoingMission)
                    {
                        convoy.MissionCargo = leader.MissionCargo;
                        convoy.FromLocation = leader.FromLocation;
                        convoy.ToLocation = leader.ToLocation;
                        convoy.ConvoyTrailerModel = leader.Vehicle?.Trailer?.Model ?? 0;

                        if (convoy.ConvoyTrailerModel != 0)
                            foreach (var member in convoy.Members)
                            {
                                if (member == leader)
                                    continue;

                                if (member.Vehicle?.Trailer?.Model == convoy.ConvoyTrailerModel)
                                    continue;

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

                                isSameTrailer = false;
                            }
                        else
                            foreach (var member in convoy.Members)
                            {
                                if (member == leader)
                                    continue;

                                switch (member.Vehicle?.Model)
                                {
                                    case VehicleModelType.Flatbed:
                                    case VehicleModelType.DFT30:
                                        isSameTrailer = true;
                                        break;

                                    default:
                                        member.MissionTextDraw.Text = Messages.MissionConvoyNoTrailerVehicleNeeded;
                                        break;
                                }
                            }

                        if (isSameTrailer)
                        {
                            leader.SendClientMessage(Color.GreenYellow, Messages.MissionConvoyReadyToGo);

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
                    var areAllMembersLoaded = true;

                    foreach (var member in convoy.Members)
                        if (member.MissionStep != 2)
                            areAllMembersLoaded = false;

                    if (areAllMembersLoaded)
                    {
                        leader.SendClientMessage(Color.GreenYellow, Messages.MissionConvoyMembersLoaded);

                        foreach (var member in convoy.Members) convoy.UpdateMemberJob(member);

                        convoy.MissionStep = 2;
                    }

                    break;

                case 2:
                    var didAllMembersUnloaded = true;

                    foreach (var member in convoy.Members)
                        if (member.MissionStep != 4)
                            didAllMembersUnloaded = false;

                    if (didAllMembersUnloaded)
                        convoy.MissionStep = 3;
                    break;

                case 3:
                    var numberOfMembers = convoy.Members.Count;

                    var payment = TruckerController.CalculatePayment(convoy.FromLocation, convoy.ToLocation,
                        convoy.MissionCargo);

                    if (!BonusMission.IsMissionFinished
                        && BonusMission.RandomCargo == convoy.MissionCargo
                        && BonusMission.RandomFromLocation == convoy.FromLocation
                        && BonusMission.RandomToLocation == convoy.ToLocation)
                    {
                        payment *= 2;
                        BonusMission.IsMissionFinished = true;
                        BasePlayer.SendClientMessageToAll(
                            $"{{00BBFF}}Convoy with leader {{FFBB00}}{leader.Name}{{00BBFF}} has finished the bonus mission.");
                    }

                    var bonus = numberOfMembers * 25 + 100;
                    payment = payment * bonus / 100;

                    foreach (var member in convoy.Members)
                    {
                        member.Reward(payment, 5);

                        var memberAccount = member.Account;
                        memberAccount.ConvoyJobs++;
                        await AccountRepository.UpdateAsync(memberAccount);

                        MissionsController.ClassEndMission(member);

                        member.SendClientMessage(Color.White,
                            $"{{00FF00}}You finished the convoy and earned ${payment}");

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

        private void Convoy_PlayerDied(object sender, DeathEventArgs e)
        {
            var player = sender as Player;
            MissionConvoy.PlayerLeaveConvoy(player);
        }

        private void Convoy_PlayerDisconnected(object sender, DisconnectEventArgs e)
        {
            var player = sender as Player;
            MissionConvoy.PlayerLeaveConvoy(player);
        }
    }
}