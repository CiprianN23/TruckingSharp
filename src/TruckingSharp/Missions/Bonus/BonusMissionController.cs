using SampSharp.GameMode;
using SampSharp.GameMode.Controllers;
using SampSharp.GameMode.SAMP;
using System;
using TruckingSharp.Missions.Data;
using TruckingSharp.PlayerClasses.Data;

namespace TruckingSharp.Missions.Bonus
{
    [Controller]
    public class BonusMissionController : IEventListener
    {
        public static Timer Timer;

        public void RegisterEvents(BaseMode gameMode)
        {
            gameMode.Initialized += Bonus_GamemodeInitialized;
        }

        private void Bonus_GamemodeInitialized(object sender, EventArgs e)
        {
            Timer = new Timer(TimeSpan.FromMinutes(5), true);
            Timer.Tick += Timer_Tick;
        }

        private static void Timer_Tick(object sender, EventArgs e)
        {
            var isMissionSet = false;
            var random = new Random();

            if (BonusMission.RandomCargo == null || BonusMission.IsMissionFinished)
            {
                while (!isMissionSet)
                {
                    var index = random.Next(1, MissionCargo.MissionCargoes.Count);
                    BonusMission.RandomCargo = MissionCargo.MissionCargoes[index];

                    switch (BonusMission.RandomCargo.JobCargoVehicleType)
                    {
                        case MissionCargoVehicleType.CargoTrailer:
                        case MissionCargoVehicleType.FluidsTrailer:
                        case MissionCargoVehicleType.OreTrailer:
                        case MissionCargoVehicleType.CementTruck:
                        case MissionCargoVehicleType.NoTrailer:
                            isMissionSet = true;
                            break;
                    }
                }

                BonusMission.RandomFromLocation = MissionCargo.GetRandomStartLocation(BonusMission.RandomCargo);
                BonusMission.RandomToLocation = MissionCargo.GetRandomEndLocation(BonusMission.RandomCargo);
                BonusMission.IsMissionFinished = false;
            }

            var truckName = string.Empty;

            switch (BonusMission.RandomCargo.JobCargoVehicleType)
            {
                case MissionCargoVehicleType.CargoTrailer:
                    truckName = "truck with cargo trailer";
                    break;

                case MissionCargoVehicleType.FluidsTrailer:
                    truckName = "truck with fluids trailer";
                    break;

                case MissionCargoVehicleType.OreTrailer:
                    truckName = "truck with ore trailer";
                    break;

                case MissionCargoVehicleType.CementTruck:
                    truckName = "cement truck";
                    break;

                case MissionCargoVehicleType.NoTrailer:
                    truckName = "Flatbed or DFT-30";
                    break;
            }

            foreach (var basePlayer in Player.All)
            {
                var player = (Player)basePlayer;

                if (player.PlayerClass != PlayerClassType.TruckDriver)
                    continue;

                player.SendClientMessage(Color.White,
                    $"{{00BBFF}}Bonus mission: transport {{FFBB00}}{BonusMission.RandomCargo.Name}");
                player.SendClientMessage(Color.White,
                    $"{{00BBFF}}from {{FFBB00}}{BonusMission.RandomFromLocation.Name}{{00BBFF}} to {{FFBB00}}{BonusMission.RandomToLocation.Name}");
                player.SendClientMessage(Color.White,
                    $"{{00BBFF}}You'll need a {{FFBB00}}{truckName}{{00BBFF}} to complete this mission");
            }
        }
    }
}