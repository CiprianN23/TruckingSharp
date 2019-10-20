using TruckingSharp.Constants;
using TruckingSharp.Data;
using TruckingSharp.Data.ClassesSpawn;
using TruckingSharp.World;
using SampSharp.GameMode;
using SampSharp.GameMode.Events;
using SampSharp.GameMode.SAMP;
using SampSharp.GameMode.World;
using System;

namespace TruckingSharp.Extensions.PlayersExtensions
{
    public static class PlayerRequestSpawnExtension
    {
        public static void SetClassSpawn(this Player player, RequestSpawnEventArgs e)
        {
            var randomIndex = new Random();
            int index;
            float angle = 0.0f;
            Vector3 position = Vector3.Zero;

            switch (player.PlayerClass)
            {
                case PlayerClasses.TruckDriver:
                    index = randomIndex.Next(0, TruckerSpawn.TruckerSpawns.Length);

                    position = TruckerSpawn.TruckerSpawns[index].Position;
                    angle = TruckerSpawn.TruckerSpawns[index].Angle;

                    BasePlayer.SendClientMessageToAll(Messages.PlayerJoinedTruckerClass, player.Name);
                    break;

                case PlayerClasses.BusDriver:
                    index = randomIndex.Next(0, BusDriverSpawn.BusDriverSpawns.Length);

                    position = BusDriverSpawn.BusDriverSpawns[index].Position;
                    angle = BusDriverSpawn.BusDriverSpawns[index].Angle;

                    BasePlayer.SendClientMessageToAll(Messages.PlayerJoinedBusDriverClass, player.Name);
                    break;

                case PlayerClasses.Pilot:
                    index = randomIndex.Next(0, PilotSpawn.PilotSpawns.Length);

                    position = PilotSpawn.PilotSpawns[index].Position;
                    angle = PilotSpawn.PilotSpawns[index].Angle;

                    BasePlayer.SendClientMessageToAll(Messages.PlayerJoinedPilotClass, player.Name);
                    break;

                case PlayerClasses.Police:

                    player.CheckIfPlayerCanJoinPolice();

                    if (player.Account.Score < 100)
                    {
                        player.GameText("You need 100 scorepoints for police class", 5000, 4);
                        player.SendClientMessage(Color.Red, "You need 100 scorepoints for police class");
                        e.PreventSpawning = true;
                        return;
                    }

                    if (player.Account.Wanted > 0)
                    {
                        player.GameText("You are not allowed to choose police class when you're wanted", 5000, 4);
                        player.SendClientMessage(Color.Red, "You are not allowed to choose police class when you're wanted");
                        e.PreventSpawning = true;
                        return;
                    }

                    index = randomIndex.Next(0, PoliceSpawn.PoliceSpawns.Length);

                    position = PoliceSpawn.PoliceSpawns[index].Position;
                    angle = PoliceSpawn.PoliceSpawns[index].Angle;

                    BasePlayer.SendClientMessageToAll(Messages.PlayerJoinedPoliceClass, player.Name);
                    break;

                case PlayerClasses.Mafia:
                    index = randomIndex.Next(0, MafiaSpawn.MafiaSpawns.Length);

                    position = MafiaSpawn.MafiaSpawns[index].Position;
                    angle = MafiaSpawn.MafiaSpawns[index].Angle;

                    BasePlayer.SendClientMessageToAll(Messages.PlayerJoinedMafiaClass, player.Name);
                    break;

                case PlayerClasses.Courier:
                    index = randomIndex.Next(0, CourierSpawn.CourierSpawns.Length);

                    position = CourierSpawn.CourierSpawns[index].Position;
                    angle = CourierSpawn.CourierSpawns[index].Angle;

                    BasePlayer.SendClientMessageToAll(Messages.PlayerJoinedCourierClass, player.Name);
                    break;

                case PlayerClasses.Assistance:
                    index = randomIndex.Next(0, AssistanceSpawn.AssistanceSpawns.Length);

                    position = AssistanceSpawn.AssistanceSpawns[index].Position;
                    angle = AssistanceSpawn.AssistanceSpawns[index].Angle;

                    BasePlayer.SendClientMessageToAll(Messages.PlayerJoinedAssistanceClass, player.Name);
                    break;

                case PlayerClasses.RoadWorker:
                    index = randomIndex.Next(0, RoadWorkerSpawn.RoadworkerSpawns.Length);

                    position = RoadWorkerSpawn.RoadworkerSpawns[index].Position;
                    angle = RoadWorkerSpawn.RoadworkerSpawns[index].Angle;

                    BasePlayer.SendClientMessageToAll(Messages.PlayerJoinedRoadWorkerClass, player.Name);
                    break;
            }

            player.SetSpawnInfo(0, player.Skin, position, angle);
        }
    }
}
