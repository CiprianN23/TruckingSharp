namespace TruckingSharp.Constants
{
    public static class Messages
    {
        public const string PlayerJoinedTheServer = "Player '{{FFFF00}}{0}{{0000FF}}' (id: {{FFFF00}}{1}{{0000FF}}) has joined the server";
        public const string PlayerLeftTheServer = "Player '{{FFFF00}}{0}{{0000FF}}' (id: {{FFFF00}}{1}{{0000FF}}) has left the server";

        public const string FailedToLoginProperly = "You failed to login properly, you're kicked";
        public const string RulesNotYetAccepted = "You haven't accepted the {FFFF00}/rules{FF0000} yet";
        public const string MessageTooLongWithLimit = "Your message is too long. Please shorten it to {0} characters.";
        public const string NoPrivMessageToYou = "You can not send a private message to yourself.";
        public const string PlayerNotLoggedIn = "That player has not logged in yet.";
        public const string PrivMessageTo = "[PM To] {0} ({1}): {2}";
        public const string PrivMessageFrom = "[PM From] {0} ({1}): {2}";
        public const string CommandNotAllowedInsideBuilding = "You can not use this command when you are inside a building.";
        public const string CommandOnlyAvailableAsDriver = "You can only use this command when you are driving a vehicle.";
        public const string NoTrailerAttached = "There is no trailer attached to this vehicle.";
        public const string TrailerDetached = "You have detached the trailer.";
        public const string VehicleFlipped = "You have flipped the vehicle.";
        public const string MustBeOnFoot = "You must be on foot to use this command.";
        public const string MustBeInnocent = "You must be innocent (no wanted level) to use this command.";
        public const string CommandOnlyIfNotDoingAJob = "You can not use this command while doing a job.";
        public const string CommandOnlyIfNotJailed = "You can not use this command when you are in jail.";

        public const string TruckerClass = "Truck Driver";
        public const string BusDriverClass = "Bus Driver";
        public const string PilotClass = "Pilot";
        public const string PoliceClass = "Police";
        public const string MafiaClass = "Mafia";
        public const string CourierClass = "Courier";
        public const string AssistanceClass = "Assistance";
        public const string RoadWorkerClass = "Road Worker";

        public const string PlayerJoinedTruckerClass = "{{00FF00}}Player {{FFFF00}}{0}{{00FF00}} joined {{FFFF00}}Trucker class";
        public const string PlayerJoinedBusDriverClass = "{{00FF00}}Player {{FFFF00}}{0}{{00FF00}} joined {{FFFF00}}Busdriver class";
        public const string PlayerJoinedPilotClass = "{{00FF00}}Player {{FFFF00}}{0}{{00FF00}} joined {{FFFF00}}Pilot class";
        public const string PlayerJoinedPoliceClass = "{{00FF00}}Player {{FFFF00}}{0}{{00FF00}} joined {{FFFF00}}Police class";
        public const string PlayerJoinedMafiaClass = "{{00FF00}}Player {{FFFF00}}{0}{{00FF00}} joined {{FFFF00}}Mafia class";
        public const string PlayerJoinedCourierClass = "{{00FF00}}Player {{FFFF00}}{0}{{00FF00}} joined {{FFFF00}}Courier class";
        public const string PlayerJoinedAssistanceClass = "{{00FF00}}Player {{FFFF00}}{0}{{00FF00}} joined {{FFFF00}}Assistance class";
        public const string PlayerJoinedRoadWorkerClass = "{{00FF00}}Player {{FFFF00}}{0}{{00FF00}} joined {{FFFF00}}Roadworker class";
    }
}