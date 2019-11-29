﻿using SampSharp.GameMode;
using SampSharp.GameMode.Controllers;
using SampSharp.GameMode.SAMP;
using System;
using TruckingSharp.Database.Entities;
using TruckingSharp.Database.Repositories;

namespace TruckingSharp.World
{
    [Controller]
    public class SpeedCameraController : IEventListener
    {
        private static SpeedCameraRepository SpeedCameraRepository => new SpeedCameraRepository(ConnectionFactory.GetConnection);

        public void RegisterEvents(BaseMode gameMode)
        {
            gameMode.Initialized += Speedcamera_GamemodeInitialized;
        }

        private void Speedcamera_GamemodeInitialized(object sender, EventArgs e)
        {
            LoadSpeedCameras();
        }

        public static async void CreateSpeedCamera(Vector3 position, float angle, int maxSpeed)
        {
            for (int camId = 1; camId < SpeedCameraData.SpeedCameras.Length; camId++)
            {
                if (SpeedCameraData.SpeedCameras[camId] == null)
                {
                    new SpeedCameraData(camId, position, angle, maxSpeed);

                    var databaseSpeedCamera = new SpeedCamera()
                    {
                        Id = camId,
                        PositionX = position.X,
                        PositionY = position.Y,
                        PositionZ = position.Z,
                        Angle = angle,
                        Speed = maxSpeed
                    };

                    await SpeedCameraRepository.AddAsync(databaseSpeedCamera);

                    break;
                }
            }
        }

        public static async void LoadSpeedCameras()
        {
            var speedCameras = await SpeedCameraRepository.GetAllAsync();
            int camerasCount = 0;

            foreach (var camera in speedCameras)
            {
                new SpeedCameraData(camera.Id, new Vector3(camera.PositionX, camera.PositionY, camera.PositionZ), camera.Angle, camera.Speed);
                camerasCount++;
            }

            Console.WriteLine($"Speed cameras loaded: {camerasCount}.");
        }

        public static async void RemoveSpeedCamera(int camId)
        {
            if (SpeedCameraData.SpeedCameras[camId] == null)
                return;

            var databaseSpeedCamera = SpeedCameraRepository.Find(camId);
            await SpeedCameraRepository.DeleteAsync(databaseSpeedCamera);

            SpeedCameraData.SpeedCameras[camId].CameraObject.Dispose();
            SpeedCameraData.SpeedCameras[camId].CameraObject1.Dispose();
            SpeedCameraData.SpeedCameras[camId].TextLabel.Dispose();
            SpeedCameraData.SpeedCameras[camId] = null;
        }

        public static void SpeedometerTimer_Tick(object sender, EventArgs e, Player player)
        {
            for (int camId = 1; camId < SpeedCameraData.SpeedCameras.Length; camId++)
            {
                if (SpeedCameraData.SpeedCameras[camId] == null)
                    continue;

                if (player.TimeSincePlayerCaughtSpeedingInSeconds > 0)
                {
                    player.TimeSincePlayerCaughtSpeedingInSeconds--;
                    return;
                }

                if (player.IsInRangeOfPoint(50.0f, SpeedCameraData.SpeedCameras[camId].Position))
                {
                    if (player.Speed > SpeedCameraData.SpeedCameras[camId].Speed)
                    {
                        player.TimeSincePlayerCaughtSpeedingInSeconds = 40;
                        player.SetWantedLevel(player.Account.Wanted + 1);
                        player.SendClientMessage(Color.Red, "You've been caught by a speedtrap, slow down!");

                        // TODO: Inform police
                    }
                }
            }
        }
    }
}