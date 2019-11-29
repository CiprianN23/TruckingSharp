namespace TruckingSharp.Constants
{
    public static class Messages
    {
        public const string PlayerJoinedTheServer = "Player {0}({1}) has joined the server.";
        public const string PlayerLeftTheServer = "Player {0}({1}) has left the server.";

        public const string DialogButtonSelect = "Select";
        public const string DialogButtonCancel = "Cancel";
        public const string DialogButtonYes = "Yes";
        public const string DialogButtonNo = "No";
        public const string DialogButtonToggle = "Toggle";
        public const string DialogButtonBuy = "Buy";
        public const string DialogButtonSpawn = "Spawn";
        public const string DialogButtonOk = "Ok";

        public const string FailedToLoginProperly = "You failed to login properly, you're kicked";
        public const string RulesNotYetAccepted = "You haven't accepted the {FFFF00}/rules{FF0000} yet";
        public const string MessageTooLongWithLimit = "Your message is too long. Please shorten it to {0} characters.";
        public const string NoPrivateMessageToYou = "You can not send a private message to yourself.";
        public const string PlayerNotLoggedIn = "That player has not logged in yet.";
        public const string PrivateMessageTo = "[PM To] {0} ({1}): {2}";
        public const string PrivateMessageFrom = "[PM From] {0} ({1}): {2}";

        public const string CommandNotAllowedInsideBuilding =
            "You can not use this command when you are inside a building.";

        public const string CommandAllowedOnlyAsDriver =
            "You can only use this command when you are driving a vehicle.";

        public const string CommandOnlyAllowedAsTruckDriver = "Command can be used only by truck drivers.";
        public const string CommandOnlyAllowedAsConvoyLeader = "You need to be the leader of a convoy.";
        public const string NoTrailerAttached = "There is no trailer attached to this vehicle.";
        public const string TrailerDetached = "You have detached the trailer.";
        public const string VehicleFlipped = "You have flipped the vehicle.";
        public const string MustBeOnFoot = "You must be on foot to use this command.";
        public const string MustBeInnocent = "You must be innocent (no wanted level) to use this command.";
        public const string CommandOnlyIfNotDoingAJob = "You can not use this command while doing a job.";
        public const string CommandOnlyIfNotJailed = "You can not use this command when you are in jail.";
        public const string CommandNotAllowedOnSelf = "You can not use this command on yourself.";
        public const string NoAdminOnline = "No admin online.";
        public const string PlayerNotInVehicle = "That player is not in your vehicle.";
        public const string VehicleEngineTurnedOn = "You have turned on the engine.";
        public const string VehicleEngineTurnedOff = "You have turned off the engine.";
        public const string ReasonCanNotBeEmptyOrNull = "Reason can't be empty or null.";
        public const string PasswordCanNotBeEmptyOrNull = "Reason can't be empty or null.";
        public const string PasswordCanNotBeAsTheOldOne = "The password can't be same as old one.";
        public const string PasswordsDontMatch = "The password doesn't match.";
        public const string InvalidPasswordInputted = "You inputed an invalid password. Try again.";

        public const string InvalidPasswordLength =
            "Invalid password lenght. Password must be between {0} and {1} characters.";

        public const string PasswordChangedSuccessfully = "Your password was changed successfully.";
        public const string ValueNeedToBePositive = "Value need to be a positive number.";
        public const string NotEnoughMoney = "You don't have enough money.";
        public const string BankAccountLoggedInSuccessfully = "You have successfully logged in your bank account.";
        public const string BankAccountCreatedSuccessfully = "Bank account created successfully.";
        public const string NoReports = "There are no reports to be shown.";
        public const string AlreadyInAVehicle = "You are already in a vehicle.";
        public const string PlayerOneNotLoggedIn = "Player 1 is not logged in.";
        public const string PlayerTwoNotLoggedIn = "Player 2 is not logged in.";
        public const string PlayersMustBeDifferent = "Players must be different.";
        public const string VehicleIsNotValid = "That vehicle is not valid.";
        public const string VehicleHasBeenRefuelled = "Your vehicle has been refuelled.";
        public const string TargetIsInJailCommandNotAllowed = "That player is in jail. Command is not allowed.";
        public const string NosHasBeenAddedToTheVehicle = "Nos has been added to your vehicle.";
        public const string TargetIsAlreadyMuted = "That player is already muted.";
        public const string TargetIsNotMuted = "That player is not muted.";
        public const string MuteDurationMustBeInRange = "Mute duration must be between 1 and 60 minutes.";
        public const string ThereAreNoMutedPlayers = "There are no muted players.";
        public const string PlayerIsAlreadyFrozen = "Player is already frozen.";
        public const string PlayerIsNotFrozen = "Player is not frozen.";
        public const string PlayerIsSpectating = "Player is spectacting.";
        public const string PlayerIsDeadOrNotSpawned = "That player is dead or not spawned yet.";
        public const string PlayerIsNotSpectating = "You are not spectacting.";
        public const string PlayerNotPartOfAnyConvoy = "You are not part of any convoy.";

        public const string TruckerClass = "Truck Driver";
        public const string BusDriverClass = "Bus Driver";
        public const string PilotClass = "Pilot";
        public const string PoliceClass = "Police";
        public const string MafiaClass = "Mafia";
        public const string CourierClass = "Courier";
        public const string AssistanceClass = "Assistance";
        public const string RoadWorkerClass = "Road Worker";

        public const string MissionNeedVehicleToProceed = "{{FF0000}}You need to be in your vehicle to proceed.";

        public const string MissionNeedTrailerToProceed =
            "{{FF0000}}You need to have your trailer attached to proceed.";

        public const string MissionConvoyWaitingMembersToLoadCargo = "Waiting for all members to load their cargo.";
        public const string MissionConvoyWaitingMembersToUnLoadCargo = "Waiting for all members to unload their cargo.";
        public const string MissionConvoyAlreadyFull = "This convoy is already full.";

        public const string MissionConvoyAlreadyOnRoute =
            "This convoy is en-route to it's destination, you can not join it.";

        public const string MissionConvoyCargoTrailerNeeded = "You need a cargo trailer.";
        public const string MissionConvoyOreTrailerNeeded = "You need a ore trailer.";
        public const string MissionConvoyFluidsTrailerNeeded = "You need a fluids trailer.";
        public const string MissionConvoyNoTrailerVehicleNeeded = "You need a Flatbed or DFT-30.";
        public const string MissionConvoyReadyToGo = "All members have the same trailer, convoy is ready to go.";
        public const string MissionConvoyCantGo = "Not all members have the same trailer, convoy cannot start yet.";

        public const string MissionConvoyMembersLoaded =
            "All members have loaded their cargo, convoy is ready to proceed to the unloading-point.";

        public const string MissionConvoyWaitingForLeader = "Waiting for the leader to start a job.";

        public const string MissionTruckerLoading = "~r~Loading Your Truck... ~w~Please Wait";
        public const string MissionTruckerUnLoading = "~r~Unloading Your Truck... ~w~Please Wait";
        public const string MissionTruckerHaulingToPickupCargo = "~w~Hauling ~b~{0}~w~ from ~r~{1}~w~ to {2}";
        public const string MissionTruckerHaulingToDeliverCargo = "~w~Hauling ~b~{0}~w~ from {1} to ~r~{2}~w~";
        public const string MissionTruckerOverloaded = "{{FF0000}}You have been overloaded! Avoid the police!";
        public const string MissionTruckerMafiaInterested = "~r~The mafia is interested in your load~w~";
        public const string MissionTruckerDeliverFrom = "{{00FF00}}Pickup the {0} at {1}";
        public const string MissionTruckerDeliverTo = "{{00FF00}}Deliver the {0} to {1}.";

        public const string MissionTruckerCompleted =
            "Trucker {{FF00FF}}{0}{{FFFFFF}} succesfully transported {{0000FF}}{1}";

        public const string MissionTruckerCompletedInfo = "from {{00FF00}}{0}{{FFFFFF}} to {{00FF00}}{1}";

        public const string MissionTruckerBonusOverload =
            "{{00FF00}}You also earned a bonus for being overloaded: ${0}";

        public const string MissionTruckerBonusMafia =
            "{{00FF00}}You also earned a bonus for delivering a mafia-load: ${0}";

        public const string MissionTruckerBonusOwnedVehicle =
            "{{00FF00}}You also earned a bonus for using your own truck: ${0}";

        public const string MissionTruckerSelectMissionMethod = "Select method:";
        public const string MissionTruckerDialogSelectLoad = "Select load:";
        public const string MissionTruckerSelectStartingLocation = "Select loading point:";
        public const string MissionTruckerSelectEndingLocation = "Select unloading point:";
        public const string MissionTruckerTrailerNeeded = "You need a trailer to start a job.";
        public const string MissionTruckerMustEnterVehicle = "You must enter your vehicle or re-attach your trailer";

        public const string MissionBusDriverMustEnterVehicle = "You must enter your bus";

        public const string MissionMafiaMustEnterVehicle = "You must enter your vehicle";

        public const string MissionCourierMustEnterVehicle = "You must enter your vehicle";

        public const string MissionRoadWorkerMustEnterVehicle = "You must enter your vehicle or re-attach your trailer";

        public const string MissionReward = "{{00FF00}}You finished the mission and earned ${0}";
        public const string AlreadyDoingAMission = "You are already doing a mission.";
        public const string MissionNotDoingAMission = "You are not doing any mission.";
        public const string MissionFailed = "~w~You ~r~failed~w~ your mission. You lost ~y~${0}~w~ to cover expenses.";

        public const string MissionConvoyOpened =
            "Player {{00FF00}}{0}{{FFFFFF}} wants to start a {{00FF00}}convoy{{FFFFFF}}, join him by entering \"/convoy\"";

        public const string PlayerJoinedTruckerClass =
            "{{00FF00}}Player {{FFFF00}}{0}{{00FF00}} joined {{FFFF00}}Trucker class";

        public const string PlayerJoinedBusDriverClass =
            "{{00FF00}}Player {{FFFF00}}{0}{{00FF00}} joined {{FFFF00}}Busdriver class";

        public const string PlayerJoinedPilotClass =
            "{{00FF00}}Player {{FFFF00}}{0}{{00FF00}} joined {{FFFF00}}Pilot class";

        public const string PlayerJoinedPoliceClass =
            "{{00FF00}}Player {{FFFF00}}{0}{{00FF00}} joined {{FFFF00}}Police class";

        public const string PlayerJoinedMafiaClass =
            "{{00FF00}}Player {{FFFF00}}{0}{{00FF00}} joined {{FFFF00}}Mafia class";

        public const string PlayerJoinedCourierClass =
            "{{00FF00}}Player {{FFFF00}}{0}{{00FF00}} joined {{FFFF00}}Courier class";

        public const string PlayerJoinedAssistanceClass =
            "{{00FF00}}Player {{FFFF00}}{0}{{00FF00}} joined {{FFFF00}}Assistance class";

        public const string PlayerJoinedRoadWorkerClass =
            "{{00FF00}}Player {{FFFF00}}{0}{{00FF00}} joined {{FFFF00}}Roadworker class";

        public const string NoMissionText =
            "No mission at the moment. Enter \"~y~/startmission~w~\" to start a mission.";

        public const string NoMissionTextPolice =
            "~r~'RMB'~w~ fines a player (on foot), ~r~'LCTRL'~w~ warns the player (in vehicle).";

        public const string NoMissionTextMafia =
            "Hijack a ~r~marked~w~ vehicle or enter \"~y~/startmission~w~\" to start a mission.";

        public const string NoMissionTextAssistance =
            "~r~'RMB'~w~ repairs/refuels a vehicle (on foot), ~r~'LCTRL'~w~ fixes your own vehicle.";
    }
}