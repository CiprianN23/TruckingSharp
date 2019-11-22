using SampSharp.GameMode;
using SampSharp.GameMode.Controllers;
using SampSharp.GameMode.Definitions;
using SampSharp.GameMode.Display;
using SampSharp.GameMode.Events;
using SampSharp.GameMode.SAMP;
using SampSharp.GameMode.World;
using System;
using System.Collections.Generic;
using TruckingSharp.Constants;
using TruckingSharp.Database.Repositories;
using TruckingSharp.Missions.Bonus;
using TruckingSharp.Missions.Data;
using TruckingSharp.PlayerClasses.Data;

namespace TruckingSharp.Missions.Trucker
{
    [Controller]
    public class TruckerController : IController, IEventListener
    {
        private PlayerAccountRepository _accountRepository => new PlayerAccountRepository(ConnectionFactory.GetConnection);

        public static void EndMission(Player player)
        {
            if (player.IsDoingMission)
            {
                player.MissionLoadingTimer?.Dispose();

                if (player.MissionTrailer == null)
                    player.MissionVehicle.IsWantedByMafia = false;
                else
                    player.MissionTrailer.IsWantedByMafia = false;

                player.IsDoingMission = false;
                player.MissionStep = 0;
                player.MissionVehicleTime = 0;
                player.MissionCargo = null;
                player.MissionVehicle = null;
                player.MissionTrailer = null;
                player.FromLocation = null;
                player.ToLocation = null;
                player.IsMafiaLoaded = false;

                player.DisableCheckpoint();
                player.MissionTextDraw.Text = Messages.NoMissionText;

                if (player.IsOverloaded)
                {
                    player.IsOverloaded = false;

                    if (player.Account.Wanted >= 2)
                        player.SetWantedLevel(player.Account.Wanted - 2);
                    else
                        player.SetWantedLevel(0);
                }
            }
        }

        public static void MissionDialogResponse(object sender, DialogResponseEventArgs e)
        {
            if (e.DialogButton == DialogButton.Left)
            {
                var playerVehicleModel = e.Player.Vehicle?.Model;
                var playerVehiclTrailereModel = e.Player.Vehicle?.Trailer?.Model;
                List<MissionCargo> CargoList = new List<MissionCargo>();

                switch (e.ListItem)
                {
                    case 0:
                        CargoList = GetMissionCargos(playerVehicleModel, playerVehiclTrailereModel);

                        if (CargoList == null)
                        {
                            return;
                        }

                        var dialogCargoSelect = new ListDialog(Messages.MissionTruckerDialogSelectLoad, Messages.DialogButtonSelect, Messages.DialogButtonCancel);

                        foreach (var cargo in CargoList)
                        {
                            dialogCargoSelect.AddItem(cargo.Name);
                        }

                        dialogCargoSelect.Show(e.Player);
                        dialogCargoSelect.Response += DialogCargoSelect_Response;

                        break;

                    case 1:
                        StartRandomMission((Player)e.Player);
                        break;
                }
            }
        }

        public static void StartRandomMission(Player player)
        {
            if (SetRandomMission(player))
            {
                player.IsDoingMission = true;

                player.MissionVehicleTime = Configuration.TimeToFailMission;

                player.MissionStep = 1;

                player.MissionTextDraw.Text = string.Format(Messages.MissionTruckerHaulingToPickupCargo, player.MissionCargo.Name, player.FromLocation.Name, player.ToLocation.Name);

                player.SetCheckpoint(player.FromLocation.Position, 7);

                player.SendClientMessage(Messages.MissionTruckerDeliverFrom, player.MissionCargo.Name, player.FromLocation.Name);
            }
        }

        public void RegisterEvents(BaseMode gameMode)
        {
            gameMode.PlayerEnterCheckpoint += Trucker_PlayerEnterCheckpoint;
        }

        private static void DialogCargoSelect_Response(object sender, DialogResponseEventArgs e)
        {
            if (e.DialogButton == DialogButton.Left)
            {
                var playerVehicleModel = e.Player.Vehicle?.Model;
                var playerVehiclTrailereModel = e.Player.Vehicle?.Trailer?.Model;
                var player = e.Player as Player;
                List<MissionCargo> CargoList = new List<MissionCargo>();

                CargoList = GetMissionCargos(playerVehicleModel, playerVehiclTrailereModel);

                var dialogSelectStartLocation = new ListDialog(Messages.MissionTruckerSelectStartingLocation, Messages.DialogButtonSelect, Messages.DialogButtonCancel);

                player.MissionCargo = CargoList[e.ListItem];

                for (int i = 0; i < player.MissionCargo.FromLocations.Length; i++)
                {
                    int startLocationIndex = player.MissionCargo.FromLocations[i];

                    if (startLocationIndex != 0)
                        dialogSelectStartLocation.AddItem(MissionLocation.MissionLocations[startLocationIndex].Name);
                    else
                        break;
                }

                dialogSelectStartLocation.Show(e.Player);
                dialogSelectStartLocation.Response += DialogSelectStartLocation_Response;
            }
        }

        private static void DialogSelectEndLocation_Response(object sender, DialogResponseEventArgs e)
        {
            var player = e.Player as Player;
            var cargo = player.MissionCargo;
            int endLocationIndex = player.MissionCargo.ToLocations[e.ListItem];
            player.ToLocation = MissionLocation.MissionLocations[endLocationIndex];
            player.IsDoingMission = true;
            player.MissionVehicle = (Vehicle)player.Vehicle;
            player.MissionTrailer = (Vehicle)player.Vehicle?.Trailer;
            player.MissionStep = 1;

            player.MissionTextDraw.Text = string.Format(Messages.MissionTruckerHaulingToPickupCargo, player.MissionCargo.Name, player.FromLocation.Name, player.ToLocation.Name);

            player.SetCheckpoint(player.FromLocation.Position, 7);
            player.MissionVehicleTime = Configuration.TimeToFailMission;

            player.SendClientMessage(Messages.MissionTruckerDeliverFrom, cargo.Name, player.FromLocation.Name);
        }

        private static void DialogSelectStartLocation_Response(object sender, DialogResponseEventArgs e)
        {
            if (e.DialogButton == DialogButton.Left)
            {
                var player = e.Player as Player;

                var cargo = player.MissionCargo;
                int startLocationIndex = player.MissionCargo.FromLocations[e.ListItem];
                player.FromLocation = MissionLocation.MissionLocations[startLocationIndex];

                var dialogSelectEndLocation = new ListDialog(Messages.MissionTruckerSelectEndingLocation, Messages.DialogButtonSelect, Messages.DialogButtonCancel);

                for (int i = 0; i < cargo.ToLocations.Length; i++)
                {
                    int endLcoationIndex = player.MissionCargo.ToLocations[i];

                    if (endLcoationIndex != 0)
                        dialogSelectEndLocation.AddItem(MissionLocation.MissionLocations[endLcoationIndex].Name);
                    else
                        break;
                }

                dialogSelectEndLocation.Show(e.Player);
                dialogSelectEndLocation.Response += DialogSelectEndLocation_Response;
            }
        }

        private static List<MissionCargo> GetMissionCargos(VehicleModelType? playerVehicleModel, VehicleModelType? playerVehicleTrailerModel)
        {
            int numberOfCargos = 0;
            switch (playerVehicleModel)
            {
                case VehicleModelType.Flatbed:
                case VehicleModelType.DFT30:
                    return MissionCargo.GetCargoList(MissionCargoVehicleType.NoTrailer, ref numberOfCargos);

                case VehicleModelType.CementTruck:
                    return MissionCargo.GetCargoList(MissionCargoVehicleType.CementTruck, ref numberOfCargos);

                case VehicleModelType.Linerunner:
                case VehicleModelType.Tanker:
                case VehicleModelType.Roadtrain:
                    switch (playerVehicleTrailerModel)
                    {
                        case VehicleModelType.ArticleTrailer:
                        case VehicleModelType.ArticleTrailer3:
                            return MissionCargo.GetCargoList(MissionCargoVehicleType.CargoTrailer, ref numberOfCargos);

                        case VehicleModelType.ArticleTrailer2:
                            return MissionCargo.GetCargoList(MissionCargoVehicleType.OreTrailer, ref numberOfCargos);

                        case VehicleModelType.PetrolTrailer:
                            return MissionCargo.GetCargoList(MissionCargoVehicleType.FluidsTrailer, ref numberOfCargos);
                    }
                    break;
            }

            return null;
        }

        private static bool SetRandomMission(Player player)
        {
            if (player.State == PlayerState.Driving)
            {
                var playerVehicleModel = player.Vehicle?.Model;
                var playerTrailerModel = player.Vehicle?.Trailer?.Model;

                switch (playerVehicleModel)
                {
                    case VehicleModelType.Flatbed:
                    case VehicleModelType.DFT30:
                        return SetRandomMissionData(player, MissionCargoVehicleType.NoTrailer);

                    case VehicleModelType.CementTruck:
                        return SetRandomMissionData(player, MissionCargoVehicleType.CementTruck);

                    case VehicleModelType.Linerunner:
                    case VehicleModelType.Roadtrain:
                    case VehicleModelType.Tanker:
                        {
                            switch (playerTrailerModel)
                            {
                                case VehicleModelType.ArticleTrailer3:
                                case VehicleModelType.ArticleTrailer:
                                    return SetRandomMissionData(player, MissionCargoVehicleType.CargoTrailer);

                                case VehicleModelType.ArticleTrailer2:
                                    return SetRandomMissionData(player, MissionCargoVehicleType.OreTrailer);

                                case VehicleModelType.PetrolTrailer:
                                    return SetRandomMissionData(player, MissionCargoVehicleType.FluidsTrailer);
                            }
                        }
                        break;
                }
            }

            return false;
        }

        private static bool SetRandomMissionData(Player player, MissionCargoVehicleType missionCargoVehicleType)
        {
            player.MissionCargo = MissionCargo.GetRandomCargo(missionCargoVehicleType);
            player.FromLocation = MissionCargo.GetRandomStartLocation(player.MissionCargo);
            player.ToLocation = MissionCargo.GetRandomEndLocation(player.MissionCargo);

            player.MissionVehicle = (Vehicle)player.Vehicle;
            player.MissionTrailer = (Vehicle)player.Vehicle.Trailer;

            return true;
        }

        public static int CalculatePayment(MissionLocation fromLocation, MissionLocation toLocation, MissionCargo missionCargo)
        {
            var distance = GetDistance(fromLocation, toLocation);
            return (int)Math.Floor(distance * missionCargo.PayPerUnit);
        }

        public static double GetDistance(MissionLocation fromLocation, MissionLocation toLocation)
        {
            return Math.Sqrt(Math.Pow(toLocation.Position.X - fromLocation.Position.X, 2) + Math.Pow(toLocation.Position.Y - fromLocation.Position.Y, 2));
        }

        private void SetRandomOverload(Player player)
        {
            var playerTrailerModel = player.Vehicle?.Trailer?.Model;

            if (playerTrailerModel == VehicleModelType.ArticleTrailer || playerTrailerModel == VehicleModelType.ArticleTrailer2)
            {
                Random random = new Random();
                int chance = random.Next(100);

                if (chance <= 15)
                {
                    player.IsOverloaded = true;
                    player.SetWantedLevel(player.Account.Wanted + 2);

                    // TODO: Inform police about it
                }
            }
        }

        private void Trucker_PlayerEnterCheckpoint(object sender, System.EventArgs e)
        {
            Player player = sender as Player;

            if (player.PlayerClass != PlayerClassType.TruckDriver)
            {
                return;
            }

            if (player.Vehicle != player.MissionVehicle)
            {
                player.SendClientMessage(Messages.MissionNeedVehicleToProceed);
                return;
            }

            if (player.Vehicle.Trailer != player.MissionTrailer)
            {
                player.SendClientMessage(Messages.MissionNeedTrailerToProceed);
                return;
            }

            switch (player.MissionStep)
            {
                case 1:
                    player.GameText(Messages.MissionTruckerLoading, 5000, 4);
                    break;

                case 2:
                case 3:
                    player.GameText(Messages.MissionTruckerUnLoading, 5000, 4);
                    break;
            }

            player.ToggleControllable(false);

            player.MissionLoadingTimer = new Timer(TimeSpan.FromSeconds(5), false);
            player.MissionLoadingTimer.Tick += (sender, e) => TruckerLoadUnlaod_Tick(sender, e, player);
        }

        private async void TruckerLoadUnlaod_Tick(object sender, System.EventArgs e, Player player)
        {
            if (player.IsInConvoy)
            {
                if (player.MissionStep == 1)
                {
                    player.MissionStep = 2;
                    player.MissionTextDraw.Text = Messages.MissionConvoyWaitingMembersToLoadCargo;
                }

                if (player.MissionStep == 3)
                {
                    player.MissionStep = 4;
                    player.MissionTextDraw.Text = Messages.MissionConvoyWaitingMembersToUnLoadCargo;
                }

                player.DisableCheckpoint();
                player.ToggleControllable(true);

                return;
            }

            switch (player.MissionStep)
            {
                case 1:
                    player.MissionStep = 2;
                    player.DisableCheckpoint();

                    SetRandomOverload(player);

                    string routeText = string.Format(Messages.MissionTruckerHaulingToDeliverCargo, player.MissionCargo.Name, player.FromLocation.Name, player.ToLocation.Name);

                    if (player.IsOverloaded)
                    {
                        _ = string.Concat(routeText, "~r~(OL)~w~");

                        player.SendClientMessage(Messages.MissionTruckerOverloaded);
                    }

                    if (player.MissionCargo.IsWantedByMafia)
                    {
                        _ = string.Concat(routeText, "~r~(ML)~w~");
                        player.GameText(Messages.MissionTruckerMafiaInterested, 5000, 4);
                        player.IsMafiaLoaded = true;

                        if (player.MissionTrailer == null)
                            player.MissionVehicle.IsWantedByMafia = true;
                        else
                            player.MissionTrailer.IsWantedByMafia = true;
                    }

                    player.MissionTextDraw.Text = routeText;
                    player.SetCheckpoint(player.ToLocation.Position, 7.0f);
                    player.ToggleControllable(true);
                    player.SendClientMessage(Messages.MissionTruckerDeliverTo, player.MissionCargo.Name, player.ToLocation.Name);
                    break;

                case 2:
                    BasePlayer.SendClientMessageToAll(Messages.MissionTruckerCopletedJob, player.Name, player.MissionCargo.Name);
                    BasePlayer.SendClientMessageToAll(Messages.MissionTruckerCopletedJobInfo, player.FromLocation.Name, player.ToLocation.Name);

                    int payment = CalculatePayment(player.FromLocation, player.ToLocation, player.MissionCargo);

                    if (!BonusMission.IsMissionFinished
                        && BonusMission.RandomCargo == player.MissionCargo
                        && BonusMission.RandomFromLocation == player.FromLocation
                        && BonusMission.RandomToLocation == player.ToLocation)
                    {
                        payment *= 2;
                        BonusMission.IsMissionFinished = true;
                        BasePlayer.SendClientMessageToAll($"{{00BBFF}}Player {{FFBB00}}{player.Name}{{00BBFF}} has finished the bonus mission.");
                    }

                    player.Reward(payment);
                    player.SendClientMessage(Messages.MissionReward, payment);

                    if (player.IsOverloaded)
                    {
                        int bonus = (payment * 25) / 100;
                        player.Reward(bonus);
                        player.SendClientMessage(Messages.MissionTruckerBonusdOverload, bonus);
                    }

                    if (player.IsMafiaLoaded)
                    {
                        int bonus = (payment * 50) / 100;
                        player.Reward(bonus);
                        player.SendClientMessage(Messages.MissionTruckerBonusMafia, bonus);
                    }

                    if (player.MissionVehicle.IsOwned)
                    {
                        int bonus = (payment * 10) / 100;
                        player.Reward(bonus);
                        player.SendClientMessage(Messages.MissionTruckerBonusOwnedVehicle, bonus);
                    }

                    if (GetDistance(player.FromLocation, player.ToLocation) > 3000.0f)
                        player.Reward(0, 2);
                    else
                        player.Reward(0, 1);

                    var account = player.Account;
                    account.TruckerJobs++;

                    await _accountRepository.UpdateAsync(account);

                    EndMission(player);

                    player.ToggleControllable(true);
                    break;
            }
        }
    }
}