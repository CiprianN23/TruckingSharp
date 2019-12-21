using SampSharp.GameMode;
using SampSharp.GameMode.Controllers;
using SampSharp.GameMode.Events;
using SampSharp.GameMode.SAMP;
using SampSharp.GameMode.World;
using System;
using System.Linq;
using TruckingSharp.Constants;
using TruckingSharp.Database.Repositories;
using TruckingSharp.Missions.Assistance;
using TruckingSharp.Missions.Mafia;
using TruckingSharp.Missions.Police;
using TruckingSharp.PlayerClasses.Data;

namespace TruckingSharp.PlayerClasses
{
    [Controller]
    public class ClassController : IEventListener
    {
        public void RegisterEvents(BaseMode gameMode)
        {
            gameMode.PlayerRequestClass += Class_PlayerRequestClass;
            gameMode.PlayerRequestSpawn += Class_PlayerRequestSpawn;
            gameMode.PlayerSpawned += Class_PlayerSpawned;
            gameMode.Initialized += Class_Initialized;
        }

        private void Class_Initialized(object sender, EventArgs e)
        {
            if (!(sender is GameMode gameMode))
                return;

            //Trucker
            gameMode.AddPlayerClass(133, Vector3.Zero, 0.0f);
            gameMode.AddPlayerClass(201, Vector3.Zero, 0.0f);
            gameMode.AddPlayerClass(202, Vector3.Zero, 0.0f);
            gameMode.AddPlayerClass(234, Vector3.Zero, 0.0f);
            gameMode.AddPlayerClass(258, Vector3.Zero, 0.0f);
            gameMode.AddPlayerClass(261, Vector3.Zero, 0.0f);
            gameMode.AddPlayerClass(206, Vector3.Zero, 0.0f);
            gameMode.AddPlayerClass(34, Vector3.Zero, 0.0f);

            //Bus driver
            gameMode.AddPlayerClass(255, Vector3.Zero, 0.0f);
            gameMode.AddPlayerClass(253, Vector3.Zero, 0.0f);

            //Pilot
            gameMode.AddPlayerClass(61, Vector3.Zero, 0.0f);

            //Police
            gameMode.AddPlayerClass(280, Vector3.Zero, 0.0f);
            gameMode.AddPlayerClass(282, Vector3.Zero, 0.0f);
            gameMode.AddPlayerClass(283, Vector3.Zero, 0.0f);

            //Mafia
            gameMode.AddPlayerClass(111, Vector3.Zero, 0.0f);
            gameMode.AddPlayerClass(112, Vector3.Zero, 0.0f);
            gameMode.AddPlayerClass(113, Vector3.Zero, 0.0f);

            //Assistance
            gameMode.AddPlayerClass(50, Vector3.Zero, 0.0f);
        }

        private void Class_PlayerSpawned(object sender, SpawnEventArgs e)
        {
            if (!(sender is Player player))
                return;

            switch (player.PlayerClass)
            {
                case PlayerClassType.TruckDriver:
                    player.Color = PlayerClassColor.TruckerColor;
                    break;

                case PlayerClassType.BusDriver:
                    player.Color = PlayerClassColor.BusDriverColor;
                    break;

                case PlayerClassType.Pilot:
                    player.Color = PlayerClassColor.PilotColor;
                    break;

                case PlayerClassType.Police:
                    player.Color = PlayerClassColor.PoliceColor;

                    player.CheckTimer?.Dispose();
                    player.CheckTimer = new Timer(TimeSpan.FromSeconds(1), true);
                    player.CheckTimer.Tick += (senderObject, ev) => PoliceController.PoliceCheckTimer_Tick(senderObject, ev, player);

                    if (Configuration.Instance.CanPoliceHaveWeapons)
                    {
                        foreach (var weapon in Configuration.PoliceWeapons)
                        {
                            player.GiveWeapon(weapon, Configuration.Instance.PoliceWeaponsAmmo);
                        }
                    }
                    break;

                case PlayerClassType.Mafia:
                    player.Color = PlayerClassColor.MafiaColor;

                    player.CheckTimer?.Dispose();
                    player.CheckTimer = new Timer(TimeSpan.FromSeconds(1), true);
                    player.CheckTimer.Tick += (senderObject, ev) => MafiaController.MafiaCheckTimer_Tick(senderObject, ev, player);
                    break;

                case PlayerClassType.Assistance:
                    player.Color = PlayerClassColor.AssistanceColor;

                    player.CheckTimer?.Dispose();
                    player.CheckTimer = new Timer(TimeSpan.FromSeconds(1), true);
                    player.CheckTimer.Tick += (senderObject, ev) => AssistanceController.AssistanceCheckTimer_Tick(senderObject, ev, player);
                    break;
            }
        }

        private void Class_PlayerRequestSpawn(object sender, RequestSpawnEventArgs e)
        {
            if (!(sender is Player player))
                return;

            var random = new Random();
            var angle = 0.0f;
            var position = Vector3.Zero;

            var spawns = new ClassSpawnRepository(ConnectionFactory.GetConnection).GetAllByClassType((int)player.PlayerClass);
            var index = random.Next(0, spawns.Count());

            switch (player.PlayerClass)
            {
                case PlayerClassType.TruckDriver:
                    position = new Vector3(spawns.ElementAt(index).PositionX, spawns.ElementAt(index).PositionY, spawns.ElementAt(index).PositionZ);
                    angle = spawns.ElementAt(index).Angle;

                    BasePlayer.SendClientMessageToAll(Messages.PlayerJoinedTruckerClass, player.Name);
                    break;

                case PlayerClassType.BusDriver:
                    position = new Vector3(spawns.ElementAt(index).PositionX, spawns.ElementAt(index).PositionY, spawns.ElementAt(index).PositionZ);
                    angle = spawns.ElementAt(index).Angle;

                    BasePlayer.SendClientMessageToAll(Messages.PlayerJoinedBusDriverClass, player.Name);
                    break;

                case PlayerClassType.Pilot:
                    position = new Vector3(spawns.ElementAt(index).PositionX, spawns.ElementAt(index).PositionY, spawns.ElementAt(index).PositionZ);
                    angle = spawns.ElementAt(index).Angle;

                    BasePlayer.SendClientMessageToAll(Messages.PlayerJoinedPilotClass, player.Name);
                    break;

                case PlayerClassType.Police:

                    if (!player.CheckIfPlayerCanJoinPolice())
                    {
                        e.PreventSpawning = true;
                        return;
                    }

                    if (player.Account.Score < 100)
                    {
                        player.GameText("You need 100 score points for police class", 5000, 4);
                        player.SendClientMessage(Color.Red, "You need 100 score points for police class");
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

                    position = new Vector3(spawns.ElementAt(index).PositionX, spawns.ElementAt(index).PositionY, spawns.ElementAt(index).PositionZ);
                    angle = spawns.ElementAt(index).Angle;

                    BasePlayer.SendClientMessageToAll(Messages.PlayerJoinedPoliceClass, player.Name);
                    break;

                case PlayerClassType.Mafia:
                    position = new Vector3(spawns.ElementAt(index).PositionX, spawns.ElementAt(index).PositionY, spawns.ElementAt(index).PositionZ);
                    angle = spawns.ElementAt(index).Angle;

                    BasePlayer.SendClientMessageToAll(Messages.PlayerJoinedMafiaClass, player.Name);
                    break;

                case PlayerClassType.Assistance:
                    position = new Vector3(spawns.ElementAt(index).PositionX, spawns.ElementAt(index).PositionY, spawns.ElementAt(index).PositionZ);
                    angle = spawns.ElementAt(index).Angle;

                    BasePlayer.SendClientMessageToAll(Messages.PlayerJoinedAssistanceClass, player.Name);
                    break;
            }

            player.SetSpawnInfo(0, player.Skin, position, angle);
        }

        private void Class_PlayerRequestClass(object sender, RequestClassEventArgs e)
        {
            if (!(sender is Player player))
                return;

            player.Interior = 14;
            player.Position = new Vector3(258.4893, -41.4008, 1002.0234);
            player.Angle = 270.0f;
            player.CameraPosition = new Vector3(256.0815, -43.0475, 1004.0234);
            player.SetCameraLookAt(new Vector3(258.4893, -41.4008, 1002.0234));

            switch (e.ClassId)
            {
                case 0:
                case 1:
                case 2:
                case 3:
                case 4:
                case 5:
                case 6:
                case 7:
                    player.GameText(Messages.TruckerClass, 3000, 4);
                    player.PlayerClass = PlayerClassType.TruckDriver;
                    break;

                case 8:
                case 9:
                    player.GameText(Messages.BusDriverClass, 3000, 4);
                    player.PlayerClass = PlayerClassType.BusDriver;
                    break;

                case 10:
                    player.GameText(Messages.PilotClass, 3000, 4);
                    player.PlayerClass = PlayerClassType.Pilot;
                    break;

                case 11:
                case 12:
                case 13:
                    player.GameText(Messages.PoliceClass, 3000, 4);
                    player.PlayerClass = PlayerClassType.Police;
                    break;

                case 14:
                case 15:
                case 16:
                    player.GameText(Messages.MafiaClass, 3000, 4);
                    player.PlayerClass = PlayerClassType.Mafia;
                    break;

                case 17:
                    player.GameText(Messages.AssistanceClass, 3000, 4);
                    player.PlayerClass = PlayerClassType.Assistance;
                    break;
            }
        }
    }
}