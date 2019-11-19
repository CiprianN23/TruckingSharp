using SampSharp.GameMode;
using SampSharp.GameMode.Definitions;
using SampSharp.GameMode.Display;
using SampSharp.GameMode.SAMP;
using System;
using System.Collections.Generic;
using TruckingSharp.Constants;
using TruckingSharp.Missions.Data;

namespace TruckingSharp.Missions.Convoy
{
    public class MissionConvoy
    {
        public static MissionConvoy[] Convoys = new MissionConvoy[Configuration.MaximumConvoys]
        {
            new MissionConvoy(),
            new MissionConvoy(),
            new MissionConvoy(),
            new MissionConvoy(),
            new MissionConvoy(),
        };

        public List<Player> Members = new List<Player>();

        public MissionConvoy()
        {
            Status = ConvoyStatus.Empty;

            LeaderText = new TextDraw(new Vector2(320.0, 1.0), string.Empty)
            {
                Shadow = 1,
                Alignment = TextDrawAlignment.Center,
                UseBox = true,
                BoxColor = Color.Black
            };

            MemberText = new TextDraw(new Vector2(320.0, 1.0), string.Empty)
            {
                Shadow = 1,
                Alignment = TextDrawAlignment.Center,
                UseBox = true,
                BoxColor = Color.Black
            };
        }

        public VehicleModelType ConvoyTrailerModel { get; set; }
        public MissionLocation FromLocation { get; set; }
        public bool IsLeaderInformed { get; set; }
        public TextDraw LeaderText { get; set; }
        public TextDraw MemberText { get; set; }

        public MissionCargo MissionCargo { get; set; }

        public int MissionStep { get; set; }

        public ConvoyStatus Status { get; set; }

        public Timer Timer { get; set; }

        public MissionLocation ToLocation { get; set; }

        public static bool IasPlayerAllowedToJoin(Player player)
        {
            if (!player.IsInConvoy)
            {
                if (!player.IsDoingMission)
                    return true;
                else
                    player.SendClientMessage(Color.White, "{FF0000}You already started a job, you cannot create or join a convoy.");
            }
            else
            {
                player.SendClientMessage(Color.White, "{FF0000}You already joined a convoy.");
            }

            return false;
        }

        public static void MakeLeader(Player leader, MissionConvoy convoy)
        {
            if (IasPlayerAllowedToJoin(leader))
            {
                convoy.Status = ConvoyStatus.Open;
                convoy.Members.Add(leader);

                leader.IsInConvoy = true;
                leader.Convoy = convoy;

                convoy.Timer = new Timer(TimeSpan.FromSeconds(1), true);
                convoy.Timer.Tick += (sender, e) => ConvoyController.ConvoyTimer_Tick(sender, e, convoy);

                foreach (Player player in Player.All)
                {
                    if (player.PlayerClass == PlayerClasses.Data.PlayerClassType.TruckDriver)
                        player.SendClientMessage(Color.White, Messages.MissionConvoyOpened, leader.Name);
                }
            }
        }

        public static void MakeMember(Player member, MissionConvoy convoy)
        {
            if (IasPlayerAllowedToJoin(member))
            {
                if (convoy.Members.Count < Configuration.MaximumConvoyMembers)
                {
                    convoy.SendMessage($"Player {{00FF00}}{member.Name}{{FFFFFF}} has joined the convoy.");

                    member.SendClientMessage(Color.White, "{00FF00}You have joined the convoy");

                    convoy.Members.Add(member);

                    member.IsInConvoy = true;
                    member.Convoy = convoy;

                    if (convoy.Members.Count == Configuration.MaximumConvoyMembers)
                        convoy.Status = ConvoyStatus.Full;

                    convoy.MemberText.Text = "Waiting for the leader to start a job.";
                }
            }
        }

        public static void PlayerLeaveConvoy(Player player)
        {
            var convoy = player.Convoy;

            if (convoy == null)
                return;

            if (convoy.Members.Count == 1)
            {
                CancelConvoy(convoy);
                return;
            }

            player.IsInConvoy = false;
            player.Convoy = null;

            convoy.MemberText.Hide(player);
            convoy.LeaderText.Hide(player);

            if (!player.MissionTextDraw.IsDisposed)
                player.MissionTextDraw.Text = Messages.NoMissionText;

            if (convoy.Members[0] == player)
            {
                convoy.Members.Remove(player);

                var newLeader = convoy.Members[0];
                convoy.MemberText.Hide(newLeader);
            }
            else
            {
                convoy.Members.Remove(player);
                MissionsController.ClassEndMission(player);
            }
        }

        public void SendMessage(string message)
        {
            foreach (var member in Members)
            {
                member.SendClientMessage(Color.White, message);
            }
        }

        public void StartMemberJob(Player member)
        {
            member.IsDoingMission = true;
            member.MissionCargo = MissionCargo;
            member.FromLocation = FromLocation;
            member.ToLocation = ToLocation;

            member.MissionVehicle = (Vehicle)member.Vehicle;
            member.MissionTrailer = (Vehicle)member.Vehicle.Trailer;

            member.MissionStep = 1;
            member.MissionVehicleTime = Configuration.TimeToFailMission;
            member.MissionTextDraw.Text = string.Format(Messages.MissionTruckerHaulingToPickupCargo, MissionCargo.Name, FromLocation.Name, ToLocation.Name);
            member.SetCheckpoint(FromLocation.Position, 7.0f);
            member.SendClientMessage(Color.White, Messages.MissionTruckerDeliverFrom, MissionCargo.Name, FromLocation.Name);
            member.SendClientMessage(Color.White, "{00FF00}Meet the other members of the convoy at the loading point.");
        }

        public void UpdateMemberJob(Player member)
        {
            member.MissionStep = 3;
            member.MissionTextDraw.Text = string.Format(Messages.MissionTruckerHaulingToDeliverCargo, MissionCargo.Name, FromLocation.Name, ToLocation.Name);

            member.SetCheckpoint(ToLocation.Position, 7.0f);
            member.SendClientMessage(Color.White, Messages.MissionTruckerDeliverTo, MissionCargo.Name, ToLocation.Name);
        }

        public void UpdateTextDraw()
        {
            float distanceFromLeader;
            var leader = Members[0];
            var numberOfMembers = Members.Count;
            string furthestMemberName;

            if (Members.Count > 1)
            {
                var furthestMember = GetFurthestMember();
                furthestMemberName = furthestMember.Name;
                distanceFromLeader = GetDistanceBetweenMembers(leader, furthestMember);
            }
            else
            {
                furthestMemberName = " - ";
                distanceFromLeader = 0.0f;
            }

            LeaderText.Text = $"Members: ~g~{numberOfMembers}~w~, Furthest member: ~g~{furthestMemberName}~w~, Distance: ~r~{distanceFromLeader}~w~";
            LeaderText.Show(leader);

            foreach (var member in Members)
            {
                if (member == Members[0])
                    continue;

                distanceFromLeader = GetDistanceBetweenMembers(leader, member);
                MemberText.Text = $"Leader: ~r~{leader.Name}~w~, distance: ~r~{distanceFromLeader}~w~, members: ~r~{numberOfMembers}~w~";
                MemberText.Show(member);
            }
        }

        public static void CancelConvoy(MissionConvoy convoy)
        {
            foreach (var member in convoy.Members)
            {
                member.Convoy = null;
                member.IsInConvoy = false;

                convoy.MemberText.Hide(member);
                convoy.LeaderText.Hide(member);

                MissionsController.ClassEndMission(member);

                member.SendClientMessage(Color.White, "{FF0000}The leader cancelled the convoy.");
            }

            convoy.MissionCargo = null;
            convoy.FromLocation = null;
            convoy.ToLocation = null;
            convoy.Status = ConvoyStatus.Empty;
            convoy.MissionStep = 0;
            convoy.ConvoyTrailerModel = 0;
            convoy.IsLeaderInformed = false;
            convoy.Members.Clear();
            convoy.Timer.Dispose();
        }

        private float GetDistanceBetweenMembers(Player member1, Player member2)
        {
            return (float)Math.Sqrt(Math.Pow(member1.Position.X - member2.Position.X, 2) + Math.Pow(member1.Position.Y - member2.Position.Y, 2));
        }

        private Player GetFurthestMember()
        {
            float oldDistance = 0, newDistance = 0;
            var leader = Members[0];

            foreach (var member in Members)
            {
                if (member == Members[0])
                    continue;

                newDistance = GetDistanceBetweenMembers(leader, member);

                if (newDistance > oldDistance)
                {
                    oldDistance = newDistance;
                    return member;
                }
            }

            return null;
        }
    }
}