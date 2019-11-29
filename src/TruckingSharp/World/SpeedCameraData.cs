using SampSharp.GameMode;
using SampSharp.GameMode.SAMP;
using SampSharp.GameMode.World;

namespace TruckingSharp.World
{
    public class SpeedCameraData
    {
        public static SpeedCameraData[] SpeedCameras = new SpeedCameraData[Configuration.Instance.MaximumSpeedCameras];

        public SpeedCameraData(int camId, Vector3 position, float angle, int maxSpeed)
        {
            Id = camId;
            Position = position;
            FacingAngle = angle;
            Speed = maxSpeed;

            CameraObject = new GlobalObject(18880, Position, new Vector3(0, 0, FacingAngle), 50.0f);
            CameraObject1 = new GlobalObject(18880, Position, new Vector3(0, 0, FacingAngle + 180.0), 50.0f);

            TextLabel = new TextLabel($"CamID: {camId}\nSpeed: {maxSpeed}", Color.White, position + new Vector3(0, 0, 5), 50.0f, 0, false);

            SpeedCameras[camId] = this;
        }

        public GlobalObject CameraObject { get; set; }
        public GlobalObject CameraObject1 { get; set; }
        public float FacingAngle { get; set; }
        public int Id { get; set; }
        public Vector3 Position { get; set; }
        public int Speed { get; set; }
        public TextLabel TextLabel { get; set; }
    }
}