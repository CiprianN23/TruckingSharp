using SampSharp.GameMode;
using SampSharp.GameMode.Controllers;
using SampSharp.GameMode.SAMP;
using System;
using TruckingSharp.Missions.Data;

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

        public static void Timer_Tick(object sender, EventArgs e)
        {
            bool isMissionSet = false;
            Random random = new Random();

            if (BonusMission.RandomCargo == null || BonusMission.IsMissionFinished)
            {
                while (!isMissionSet)
                {
                    int index = random.Next(1, MissionCargo.MissionCargos.Count);
                    BonusMission.RandomCargo = MissionCargo.MissionCargos[index];

                    switch (BonusMission.RandomCargo.JobCargoVehicleType)
                    {
                        case MissionCargoVehicleType.CargoTrailer:
                        case MissionCargoVehicleType.FluidsTrailer:
                        case MissionCargoVehicleType.OreTrailer:
                        case MissionCargoVehicleType.CementTruck:
                        case MissionCargoVehicleType.NoTrailer:
                            isMissionSet = true;
                            break;

                        default:
                            isMissionSet = false;
                            break;
                    }
                }

                BonusMission.RandomFromLocation = MissionCargo.GetRandomStartLocation(BonusMission.RandomCargo);
                BonusMission.RandomToLocation = MissionCargo.GetRandomEndLocation(BonusMission.RandomCargo);
                BonusMission.IsMissionFinished = false;
            }

            string truckName = string.Empty;

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

            foreach (Player player in Player.All)
            {
                if (player.PlayerClass == PlayerClasses.Data.PlayerClassType.TruckDriver)
                {
                    player.SendClientMessage(Color.White, $"{{00BBFF}}Bonus mission: transport {{FFBB00}}{BonusMission.RandomCargo.Name}");
                    player.SendClientMessage(Color.White, $"{{00BBFF}}from {{FFBB00}}{BonusMission.RandomFromLocation.Name}{{00BBFF}} to {{FFBB00}}{BonusMission.RandomToLocation.Name}");
                    player.SendClientMessage(Color.White, $"{{00BBFF}}You'll need a {{FFBB00}}{truckName}{{00BBFF}} to complete this mission");
                }
            }
        }
    }
}