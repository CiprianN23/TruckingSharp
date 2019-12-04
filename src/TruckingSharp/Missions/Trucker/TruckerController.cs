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
using TruckingSharp.Database;
using TruckingSharp.Database.Repositories;
using TruckingSharp.Missions.Bonus;
using TruckingSharp.Missions.Data;
using TruckingSharp.Missions.Police;
using TruckingSharp.PlayerClasses.Data;

namespace TruckingSharp.Missions.Trucker
{
    [Controller]
    public class TruckerController : IEventListener
    {
        private static PlayerAccountRepository AccountRepository => RepositoriesInstances.AccountRepository;

        public void RegisterEvents(BaseMode gameMode)
        {
            gameMode.PlayerEnterCheckpoint += Trucker_PlayerEnterCheckpoint;
        }

        public static void EndMission(Player player)
        {
            if (!player.IsDoingMission)
                return;

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

            if (player.Account.Wanted >= 2)
                player.SetWantedLevel(player.Account.Wanted - 2);
            else
                player.SetWantedLevel(0);

            if (!player.IsOverloaded)
                return;

            player.IsOverloaded = false;
        }

        public static void MissionDialogResponse(object sender, DialogResponseEventArgs e)
        {
            if (e.DialogButton != DialogButton.Left)
                return;

            var playerVehicleModel = e.Player.Vehicle?.Model;
            var playerVehicleTrailerModel = e.Player.Vehicle?.Trailer?.Model;

            switch (e.ListItem)
            {
                case 0:
                    var cargoList = GetMissionCargoes(playerVehicleModel, playerVehicleTrailerModel);

                    if (cargoList == null) return;

                    var dialogCargoSelect = new ListDialog(Messages.MissionTruckerDialogSelectLoad,
                        Messages.DialogButtonSelect, Messages.DialogButtonCancel);

                    foreach (var cargo in cargoList) dialogCargoSelect.AddItem(cargo.Name);

                    dialogCargoSelect.Show(e.Player);
                    dialogCargoSelect.Response += DialogCargoSelect_Response;

                    break;

                case 1:
                    StartRandomMission((Player)e.Player);
                    break;
            }
        }

        public static void StartRandomMission(Player player)
        {
            if (!SetRandomMission(player))
                return;

            player.IsDoingMission = true;

            player.MissionVehicleTime = Configuration.Instance.FailMissionSeconds;

            player.MissionStep = 1;

            player.MissionTextDraw.Text = string.Format(Messages.MissionTruckerHaulingToPickupCargo,
                player.MissionCargo.Name, player.FromLocation.Name, player.ToLocation.Name);

            player.SetCheckpoint(player.FromLocation.Position, 7);

            player.SendClientMessage(Messages.MissionTruckerDeliverFrom, player.MissionCargo.Name,
                player.FromLocation.Name);
        }

        private static void DialogCargoSelect_Response(object sender, DialogResponseEventArgs e)
        {
            if (e.DialogButton != DialogButton.Left)
                return;

            var playerVehicleModel = e.Player.Vehicle?.Model;
            var playerVehicleTrailerModel = e.Player.Vehicle?.Trailer?.Model;

            if (!(e.Player is Player player))
                return;

            var cargoList = GetMissionCargoes(playerVehicleModel, playerVehicleTrailerModel);

            var dialogSelectStartLocation = new ListDialog(Messages.MissionTruckerSelectStartingLocation,
                Messages.DialogButtonSelect, Messages.DialogButtonCancel);

            player.MissionCargo = cargoList[e.ListItem];

            foreach (var startLocationIndex in player.MissionCargo.FromLocations)
                if (startLocationIndex != 0)
                    dialogSelectStartLocation.AddItem(MissionLocation.MissionLocations[startLocationIndex].Name);
                else
                    break;

            dialogSelectStartLocation.Show(e.Player);
            dialogSelectStartLocation.Response += DialogSelectStartLocation_Response;
        }

        private static void DialogSelectEndLocation_Response(object sender, DialogResponseEventArgs e)
        {
            if (!(e.Player is Player player))
                return;

            var cargo = player.MissionCargo;
            var endLocationIndex = player.MissionCargo.ToLocations[e.ListItem];
            player.ToLocation = MissionLocation.MissionLocations[endLocationIndex];
            player.IsDoingMission = true;
            player.MissionVehicle = (Vehicle)player.Vehicle;
            player.MissionTrailer = (Vehicle)player.Vehicle?.Trailer;
            player.MissionStep = 1;

            player.MissionTextDraw.Text = string.Format(Messages.MissionTruckerHaulingToPickupCargo,
                player.MissionCargo.Name, player.FromLocation.Name, player.ToLocation.Name);

            player.SetCheckpoint(player.FromLocation.Position, 7);
            player.MissionVehicleTime = Configuration.Instance.FailMissionSeconds;

            player.SendClientMessage(Messages.MissionTruckerDeliverFrom, cargo.Name, player.FromLocation.Name);
        }

        private static void DialogSelectStartLocation_Response(object sender, DialogResponseEventArgs e)
        {
            if (e.DialogButton != DialogButton.Left)
                return;

            if (!(e.Player is Player player))
                return;

            var cargo = player.MissionCargo;
            var startLocationIndex = player.MissionCargo.FromLocations[e.ListItem];
            player.FromLocation = MissionLocation.MissionLocations[startLocationIndex];

            var dialogSelectEndLocation = new ListDialog(Messages.MissionTruckerSelectEndingLocation,
                Messages.DialogButtonSelect, Messages.DialogButtonCancel);

            for (var i = 0; i < cargo.ToLocations.Length; i++)
            {
                var endLocationIndex = player.MissionCargo.ToLocations[i];

                if (endLocationIndex != 0)
                    dialogSelectEndLocation.AddItem(MissionLocation.MissionLocations[endLocationIndex].Name);
                else
                    break;
            }

            dialogSelectEndLocation.Show(e.Player);
            dialogSelectEndLocation.Response += DialogSelectEndLocation_Response;
        }

        private static List<MissionCargo> GetMissionCargoes(VehicleModelType? playerVehicleModel,
            VehicleModelType? playerVehicleTrailerModel)
        {
            var numberOfCargoes = 0;
            switch (playerVehicleModel)
            {
                case VehicleModelType.Flatbed:
                case VehicleModelType.DFT30:
                    return MissionCargo.GetCargoList(MissionCargoVehicleType.NoTrailer, ref numberOfCargoes);

                case VehicleModelType.CementTruck:
                    return MissionCargo.GetCargoList(MissionCargoVehicleType.CementTruck, ref numberOfCargoes);

                case VehicleModelType.Linerunner:
                case VehicleModelType.Tanker:
                case VehicleModelType.Roadtrain:
                    switch (playerVehicleTrailerModel)
                    {
                        case VehicleModelType.ArticleTrailer:
                        case VehicleModelType.ArticleTrailer3:
                            return MissionCargo.GetCargoList(MissionCargoVehicleType.CargoTrailer, ref numberOfCargoes);

                        case VehicleModelType.ArticleTrailer2:
                            return MissionCargo.GetCargoList(MissionCargoVehicleType.OreTrailer, ref numberOfCargoes);

                        case VehicleModelType.PetrolTrailer:
                            return MissionCargo.GetCargoList(MissionCargoVehicleType.FluidsTrailer,
                                ref numberOfCargoes);
                    }

                    break;
            }

            return null;
        }

        private static bool SetRandomMission(Player player)
        {
            if (player.State != PlayerState.Driving)
                return false;

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

        private void SetRandomOverload(Player player)
        {
            var playerTrailerModel = player.Vehicle?.Trailer?.Model;

            if (playerTrailerModel != VehicleModelType.ArticleTrailer &&
                playerTrailerModel != VehicleModelType.ArticleTrailer2)
            {
                return;
            }

            var random = new Random();
            var chance = random.Next(100);

            if (chance > 15)
                return;

            player.IsOverloaded = true;
            player.SetWantedLevel(player.Account.Wanted + 2);

            PoliceController.SendMessage(Color.GreenYellow ,$"Trucker {{FFFF00}}{player.Name}{{00FF00}} is overloaded, pursue and fine him.");
        }

        private void Trucker_PlayerEnterCheckpoint(object sender, EventArgs e)
        {
            if (!(sender is Player player))
                return;

            if (player.PlayerClass != PlayerClassType.TruckDriver)
                return;

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
            player.MissionLoadingTimer.Tick += (senderObject, ev) => TruckerLoadUnload_Tick(senderObject, ev, player);
        }

        private async void TruckerLoadUnload_Tick(object sender, EventArgs e, Player player)
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

                    var routeText = string.Format(Messages.MissionTruckerHaulingToDeliverCargo,
                        player.MissionCargo.Name, player.FromLocation.Name, player.ToLocation.Name);

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
                    player.SendClientMessage(Messages.MissionTruckerDeliverTo, player.MissionCargo.Name,
                        player.ToLocation.Name);
                    break;

                case 2:
                    BasePlayer.SendClientMessageToAll(Messages.MissionTruckerCompleted, player.Name,
                        player.MissionCargo.Name);
                    BasePlayer.SendClientMessageToAll(Messages.MissionTruckerCompletedInfo, player.FromLocation.Name,
                        player.ToLocation.Name);

                    var payment = MissionsController.CalculatePayment(player.FromLocation, player.ToLocation, player.MissionCargo);

                    if (!BonusMission.IsMissionFinished
                        && BonusMission.RandomCargo == player.MissionCargo
                        && BonusMission.RandomFromLocation == player.FromLocation
                        && BonusMission.RandomToLocation == player.ToLocation)
                    {
                        payment *= 2;
                        BonusMission.IsMissionFinished = true;
                        BasePlayer.SendClientMessageToAll(
                            $"{{00BBFF}}Player {{FFBB00}}{player.Name}{{00BBFF}} has finished the bonus mission.");
                    }

                    player.Reward(payment);
                    player.SendClientMessage(Messages.MissionReward, payment);

                    if (player.IsOverloaded)
                    {
                        var bonus = payment * 25 / 100;
                        player.Reward(bonus);
                        player.SendClientMessage(Messages.MissionTruckerBonusOverload, bonus);
                    }

                    if (player.IsMafiaLoaded)
                    {
                        var bonus = payment * 50 / 100;
                        player.Reward(bonus);
                        player.SendClientMessage(Messages.MissionTruckerBonusMafia, bonus);
                    }

                    if (player.MissionVehicle.IsOwned)
                    {
                        var bonus = payment * 10 / 100;
                        player.Reward(bonus);
                        player.SendClientMessage(Messages.MissionTruckerBonusOwnedVehicle, bonus);
                    }

                    player.Reward(0, MissionsController.GetDistance(player.FromLocation, player.ToLocation) > 3000.0f ? 2 : 1);

                    var account = player.Account;
                    account.TruckerJobs++;

                    await AccountRepository.UpdateAsync(account);

                    EndMission(player);

                    player.ToggleControllable(true);
                    break;
            }
        }
    }
}