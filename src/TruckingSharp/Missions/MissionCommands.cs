using SampSharp.GameMode.Definitions;
using SampSharp.GameMode.Display;
using SampSharp.GameMode.SAMP;
using SampSharp.GameMode.SAMP.Commands;
using System;
using TruckingSharp.Commands.Permissions;
using TruckingSharp.Constants;
using TruckingSharp.Extensions.PlayersExtensions;
using TruckingSharp.Missions.Bonus;
using TruckingSharp.Missions.BusDriver;
using TruckingSharp.Missions.Convoy;
using TruckingSharp.Missions.Pilot;
using TruckingSharp.Missions.Trucker;
using TruckingSharp.PlayerClasses.Data;

namespace TruckingSharp.Missions
{
    [CommandGroup("player", PermissionChecker = typeof(LoggedPermission))]
    public class MissionCommands
    {
        [Command("startmission", Shortcut = "startmission")]
        public static void OnStartMissionCommand(Player sender)
        {
            if (sender.IsDoingMission)
            {
                sender.SendClientMessage(Color.Red, Messages.AlreadyDoingAMission);
                return;
            }

            if (!sender.IsDriving())
            {
                sender.SendClientMessage(Color.Red, Messages.CommandAllowedOnlyAsDriver);
                return;
            }

            var playerVehicleModel = sender.Vehicle?.Model;

            switch (sender.PlayerClass)
            {
                case PlayerClassType.TruckDriver:

                    var convoy = sender.Convoy;

                    if (sender.IsInConvoy && convoy?.Members[0] != sender)
                    {
                        sender.SendClientMessage(Color.Red,
                            "You are not the leader of the convoy, you can not start a job.");
                        return;
                    }

                    var dialogTruckerMission = new ListDialog(Messages.MissionTruckerSelectMissionMethod,
                        Messages.DialogButtonSelect, Messages.DialogButtonCancel);
                    dialogTruckerMission.AddItem("Setup your own load and route\r\nAuto assigned load");

                    switch (playerVehicleModel)
                    {
                        case VehicleModelType.Flatbed:
                        case VehicleModelType.DFT30:
                        case VehicleModelType.CementTruck:
                            if (sender.Account.TruckerLicense == 1)
                                dialogTruckerMission.Show(sender);
                            else
                                TruckerController.StartRandomMission(sender);
                            break;

                        case VehicleModelType.Roadtrain:
                        case VehicleModelType.Linerunner:
                        case VehicleModelType.Tanker:
                            if (sender.Vehicle.Trailer != null)
                            {
                                if (sender.Account.TruckerLicense == 1)
                                    dialogTruckerMission.Show(sender);
                                else
                                    TruckerController.StartRandomMission(sender);
                            }
                            else
                            {
                                sender.SendClientMessage(Color.Red, Messages.MissionTruckerTrailerNeeded);
                                return;
                            }

                            break;
                        default:
                            sender.SendClientMessage(Color.Red, "You need to be the driver of a trucking vehicle to start a mission.");
                            break;
                    }

                    dialogTruckerMission.Response += TruckerController.MissionDialogResponse;

                    break;

                case PlayerClassType.BusDriver:
                    var dialogBusDriverMission = new ListDialog(Messages.MissionTruckerSelectMissionMethod,
                        Messages.DialogButtonSelect, Messages.DialogButtonCancel);
                    dialogBusDriverMission.AddItem("Choose your own busroute\r\nAuto assigned busroute");

                    var random = new Random();

                    switch(playerVehicleModel)
                    {
                        case VehicleModelType.Coach:
                        case VehicleModelType.Bus:
                            if (sender.Account.BusLicense == 1)
                                dialogBusDriverMission.Show(sender);
                            else
                                BusDriverController.StartMission(sender, BusRoute.BusRoutes[random.Next(BusRoute.BusRoutes.Count)]);
                            break;
                        default:
                            sender.SendClientMessage(Color.Red, "You need to be the driver of a bus to start a mission.");
                            break;
                    }

                    dialogBusDriverMission.Response += BusDriverController.DialogBusDriverMission_Response;
                    break;
                case PlayerClassType.Pilot:
                    switch (playerVehicleModel)
                    {
                        case VehicleModelType.Nevada:
                        case VehicleModelType.Shamal:
                        case VehicleModelType.Maverick:
                        case VehicleModelType.Cargobob:
                            PilotController.StartRandomMission(sender);
                            break;
                    }

                    break;
            }
        }

        [Command("endmission", Shortcut = "endmission")]
        public static void OnEndMissionCommand(Player sender)
        {
            if (!sender.IsDoingMission)
            {
                sender.SendClientMessage(Color.Red, Messages.MissionNotDoingAMission);
                return;
            }

            if (sender.IsInConvoy) MissionConvoy.PlayerLeaveConvoy(sender);

            if (sender.MissionStep == 1)
            {
                sender.SendClientMessage(Color.GreenYellow, "You ended your mission.");
                MissionsController.ClassEndMission(sender);
                return;
            }

            MissionsController.ClassEndMission(sender);

            sender.SendClientMessage(Color.Red,
                $"You {{FF0000}}failed{{FFFFFF}} your mission. You lost {{FFFF00}}${Configuration.Instance.FailedMissionPrice}{{FFFFFF}} to cover expenses.");
            sender.Reward(-Configuration.Instance.FailedMissionPrice);
        }

        [Command("overload", Shortcut = "overload")]
        public static void OnOverLoadCommand(Player sender)
        {
            if (sender.PlayerClass != PlayerClassType.TruckDriver)
            {
                sender.SendClientMessage(Color.Red, "You are not a truck driver!");
                return;
            }

            if (!sender.IsDoingMission)
            {
                sender.SendClientMessage(Color.Red, "You are not doing any mission!");
                return;
            }

            if (sender.MissionStep != 2)
            {
                sender.SendClientMessage(Color.Red, "You must load your truck first!");
                return;
            }

            if (sender.IsOverloaded)
            {
                sender.SendClientMessage(Color.Red, "You are already overloaded!");
                return;
            }

            if (!sender.IsInRangeOfPoint(25.0f, sender.FromLocation.Position))
            {
                sender.SendClientMessage(Color.Red, "You must be near the loading area!");
                return;
            }

            var playerVehicleModel = sender.MissionVehicle?.Model;
            var playerVehicleTrailerModel = sender.MissionTrailer?.Model;
            var isValidOverload = false;

            switch (playerVehicleModel)
            {
                case VehicleModelType.Flatbed:
                case VehicleModelType.DFT30:
                    isValidOverload = true;
                    break;

                case VehicleModelType.Roadtrain:
                case VehicleModelType.Tanker:
                case VehicleModelType.Linerunner:
                    switch (playerVehicleTrailerModel)
                    {
                        case VehicleModelType.ArticleTrailer:
                        case VehicleModelType.ArticleTrailer2:
                        case VehicleModelType.ArticleTrailer3:
                            isValidOverload = true;
                            break;
                    }

                    break;
            }

            if (!isValidOverload)
                return;

            sender.IsOverloaded = true;
            sender.SetWantedLevel(sender.Account.Wanted + 2);
            sender.SendClientMessage(Color.Yellow, "You have overloaded your truck, watch out for the police.");
            // TODO: Send message to police
        }

        [Command("bonus", Shortcut = "bonus")]
        public static void OnBonuscommand(Player sender)
        {
            if (sender.PlayerClass != PlayerClassType.TruckDriver)
            {
                sender.SendClientMessage(Color.Red, "You need to be Truck Driver to use this command.");
                return;
            }

            BonusMissionController.Timer.OnTick(EventArgs.Empty);
        }
    }
}