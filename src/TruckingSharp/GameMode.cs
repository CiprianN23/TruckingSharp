using TruckingSharp.Controllers;
using TruckingSharp.Data;
using SampSharp.GameMode; // Contains BaseMode class
using SampSharp.GameMode.Controllers; // Contains ControllerCollection class
using SampSharp.GameMode.Definitions;
using SampSharp.GameMode.World;
using System;

namespace TruckingSharp
{
    public class GameMode : BaseMode
    {
        #region Overrides of BaseMode

        protected override void OnInitialized(EventArgs e)
        {
            // TODO: Spawn pickups/cars
            CreateClasses();
            SpawnTrucks();
            SpawnBuses();
            SpawnPoliceCars();
            SpawnMafiaCars();
            SpawnPilotPlanes();
            SpawnAssistanceVehicles();
            SpawnCourierVehicles();
            SpawnRoadworkerVehicles();

            ShowPlayerMarkers(PlayerMarkersMode.Global);
            ShowNameTags(true);
            ManualVehicleEngineAndLights();
            EnableStuntBonusForAll(false);
            DisableInteriorEnterExits();
            UsePlayerPedAnimations();

            // TODO: Timers for timed messgaes/random bonus mission/global timer

            base.OnInitialized(e);
        }

        protected override void LoadControllers(ControllerCollection controllers)
        {
            base.LoadControllers(controllers);

            controllers.Override(new PlayerController());
            controllers.Override(new VehicleController());
        }

        #endregion

        private void CreateClasses()
        {
            //Trucker
            AddPlayerClass(133, Vector3.Zero, 0.0f);
            AddPlayerClass(201, Vector3.Zero, 0.0f);
            AddPlayerClass(202, Vector3.Zero, 0.0f);
            AddPlayerClass(234, Vector3.Zero, 0.0f);
            AddPlayerClass(258, Vector3.Zero, 0.0f);
            AddPlayerClass(261, Vector3.Zero, 0.0f);
            AddPlayerClass(206, Vector3.Zero, 0.0f);
            AddPlayerClass(34, Vector3.Zero, 0.0f);

            //Bus driver
            AddPlayerClass(255, Vector3.Zero, 0.0f);
            AddPlayerClass(253, Vector3.Zero, 0.0f);

            //Pilot
            AddPlayerClass(61, Vector3.Zero, 0.0f);

            //Police
            AddPlayerClass(280, Vector3.Zero, 0.0f);
            AddPlayerClass(282, Vector3.Zero, 0.0f);
            AddPlayerClass(283, Vector3.Zero, 0.0f);

            //Mafia
            AddPlayerClass(111, Vector3.Zero, 0.0f);
            AddPlayerClass(112, Vector3.Zero, 0.0f);
            AddPlayerClass(113, Vector3.Zero, 0.0f);

            //Courier
            AddPlayerClass(250, Vector3.Zero, 0.0f);
            AddPlayerClass(193, Vector3.Zero, 0.0f);

            //Assistance
            AddPlayerClass(50, Vector3.Zero, 0.0f);

            //Roadworker
            AddPlayerClass(16, Vector3.Zero, 0.0f);
            AddPlayerClass(27, Vector3.Zero, 0.0f);
            AddPlayerClass(260, Vector3.Zero, 0.0f);
        }

        private void SpawnTrucks()
        {
            #region Fallen Tree Depot

            JobVehicles.TruckerJobVehicles.Add(BaseVehicle.CreateStatic(VehicleModelType.ArticleTrailer, new Vector3(-475.0, -523.0, 26.0), 90.0f, -1, -1, 600));
            JobVehicles.TruckerJobVehicles.Add(BaseVehicle.CreateStatic(VehicleModelType.ArticleTrailer2, new Vector3(-475.0, -518.0, 26.0), 90.0f, -1, -1, 600));
            JobVehicles.TruckerJobVehicles.Add(BaseVehicle.CreateStatic(VehicleModelType.PetrolTrailer, new Vector3(-475.0, -513.0, 26.0), 90.0f, -1, -1, 600));
            JobVehicles.TruckerJobVehicles.Add(BaseVehicle.CreateStatic(VehicleModelType.Linerunner, new Vector3(-475.0, -508.0, 26.0), 90.0f, -1, -1, 600));
            JobVehicles.TruckerJobVehicles.Add(BaseVehicle.CreateStatic(VehicleModelType.Tanker, new Vector3(-475.0, -503.0, 26.0), 90.0f, -1, -1, 600));
            JobVehicles.TruckerJobVehicles.Add(BaseVehicle.CreateStatic(VehicleModelType.Roadtrain, new Vector3(-475.0, -498.0, 26.0), 90.0f, -1, -1, 600));
            JobVehicles.TruckerJobVehicles.Add(BaseVehicle.CreateStatic(VehicleModelType.DFT30, new Vector3(-475.0, -493.0, 26.0), 90.0f, -1, -1, 600));
            JobVehicles.TruckerJobVehicles.Add(BaseVehicle.CreateStatic(VehicleModelType.Flatbed, new Vector3(-475.0, -488.0, 26.0), 90.0f, -1, -1, 600));
            JobVehicles.TruckerJobVehicles.Add(BaseVehicle.CreateStatic(VehicleModelType.ArticleTrailer, new Vector3(-475.0, -475.0, 26.0), 180.0f, -1, -1, 600));
            JobVehicles.TruckerJobVehicles.Add(BaseVehicle.CreateStatic(VehicleModelType.ArticleTrailer2, new Vector3(-480.0, -475.0, 26.0), 180.0f, -1, -1, 600));
            JobVehicles.TruckerJobVehicles.Add(BaseVehicle.CreateStatic(VehicleModelType.PetrolTrailer, new Vector3(-485.0, -475.0, 26.0), 180.0f, -1, -1, 600));
            JobVehicles.TruckerJobVehicles.Add(BaseVehicle.CreateStatic(VehicleModelType.Linerunner, new Vector3(-490.0, -475.0, 26.0), 180.0f, -1, -1, 600));
            JobVehicles.TruckerJobVehicles.Add(BaseVehicle.CreateStatic(VehicleModelType.Tanker, new Vector3(-495.0, -475.0, 26.0), 180.0f, -1, -1, 600));
            JobVehicles.TruckerJobVehicles.Add(BaseVehicle.CreateStatic(VehicleModelType.Roadtrain, new Vector3(-500.0, -475.0, 26.0), 180.0f, -1, -1, 600));
            JobVehicles.TruckerJobVehicles.Add(BaseVehicle.CreateStatic(VehicleModelType.DFT30, new Vector3(-505.0, -475.0, 26.0), 180.0f, -1, -1, 600));
            JobVehicles.TruckerJobVehicles.Add(BaseVehicle.CreateStatic(VehicleModelType.Flatbed, new Vector3(-510.0, -475.0, 26.0), 180.0f, -1, -1, 600));
            JobVehicles.TruckerJobVehicles.Add(BaseVehicle.CreateStatic(VehicleModelType.ArticleTrailer, new Vector3(-515.0, -475.0, 26.0), 180.0f, -1, -1, 600));
            JobVehicles.TruckerJobVehicles.Add(BaseVehicle.CreateStatic(VehicleModelType.ArticleTrailer2, new Vector3(-520.0, -475.0, 26.0), 180.0f, -1, -1, 600));
            JobVehicles.TruckerJobVehicles.Add(BaseVehicle.CreateStatic(VehicleModelType.PetrolTrailer, new Vector3(-525.0, -475.0, 26.0), 180.0f, -1, -1, 600));
            JobVehicles.TruckerJobVehicles.Add(BaseVehicle.CreateStatic(VehicleModelType.Linerunner, new Vector3(-530.0, -475.0, 26.0), 180.0f, -1, -1, 600));
            JobVehicles.TruckerJobVehicles.Add(BaseVehicle.CreateStatic(VehicleModelType.Tanker, new Vector3(-535.0, -475.0, 26.0), 180.0f, -1, -1, 600));
            JobVehicles.TruckerJobVehicles.Add(BaseVehicle.CreateStatic(VehicleModelType.Roadtrain, new Vector3(-540.0, -475.0, 26.0), 180.0f, -1, -1, 600));
            JobVehicles.TruckerJobVehicles.Add(BaseVehicle.CreateStatic(VehicleModelType.DFT30, new Vector3(-545.0, -475.0, 26.0), 180.0f, -1, -1, 600));
            JobVehicles.TruckerJobVehicles.Add(BaseVehicle.CreateStatic(VehicleModelType.Flatbed, new Vector3(-550.0, -475.0, 26.0), 180.0f, -1, -1, 600));

            #endregion

            #region Flint Trucking Depot

            JobVehicles.TruckerJobVehicles.Add(BaseVehicle.CreateStatic(VehicleModelType.DFT30, new Vector3(-77.0, -1109.0, 1.25), 160.0f, -1, -1, 600));
            JobVehicles.TruckerJobVehicles.Add(BaseVehicle.CreateStatic(VehicleModelType.DFT30, new Vector3(-72.0, -1112.0, 1.25), 160.0f, -1, -1, 600));
            JobVehicles.TruckerJobVehicles.Add(BaseVehicle.CreateStatic(VehicleModelType.DFT30, new Vector3(-67.0, -1114.0, 1.25), 160.0f, -1, -1, 600));
            JobVehicles.TruckerJobVehicles.Add(BaseVehicle.CreateStatic(VehicleModelType.Flatbed, new Vector3(-30.0, -1128.0, 1.25), 160.0f, -1, -1, 600));
            JobVehicles.TruckerJobVehicles.Add(BaseVehicle.CreateStatic(VehicleModelType.Flatbed, new Vector3(-41.0, -1152.0, 1.25), 335.0f, -1, -1, 600));
            JobVehicles.TruckerJobVehicles.Add(BaseVehicle.CreateStatic(VehicleModelType.Flatbed, new Vector3(-46.0, -1150.0, 1.25), 335.0f, -1, -1, 600));
            JobVehicles.TruckerJobVehicles.Add(BaseVehicle.CreateStatic(VehicleModelType.Flatbed, new Vector3(-51.0, -1148.0, 1.25), 335.0f, -1, -1, 600));
            JobVehicles.TruckerJobVehicles.Add(BaseVehicle.CreateStatic(VehicleModelType.Flatbed, new Vector3(-56.0, -1146.0, 1.25), 335.0f, -1, -1, 600));
            JobVehicles.TruckerJobVehicles.Add(BaseVehicle.CreateStatic(VehicleModelType.Flatbed, new Vector3(-61.0, -1144.0, 1.25), 335.0f, -1, -1, 600));

            #endregion

            #region LVA Freight Depot

            JobVehicles.TruckerJobVehicles.Add(BaseVehicle.CreateStatic(VehicleModelType.DFT30, new Vector3(1467.0, 975.0, 11.0), 0.0f, -1, -1, 600));
            JobVehicles.TruckerJobVehicles.Add(BaseVehicle.CreateStatic(VehicleModelType.Flatbed, new Vector3(1472.0, 975.0, 11.0), 0.0f, -1, -1, 600));
            JobVehicles.TruckerJobVehicles.Add(BaseVehicle.CreateStatic(VehicleModelType.DFT30, new Vector3(1445.0, 975.0, 11.0), 0.0f, -1, -1, 600));
            JobVehicles.TruckerJobVehicles.Add(BaseVehicle.CreateStatic(VehicleModelType.Flatbed, new Vector3(1440.0, 975.0, 11.0), 0.0f, -1, -1, 600));
            JobVehicles.TruckerJobVehicles.Add(BaseVehicle.CreateStatic(VehicleModelType.DFT30, new Vector3(1435.0, 975.0, 11.0), 0.0f, -1, -1, 600));
            JobVehicles.TruckerJobVehicles.Add(BaseVehicle.CreateStatic(VehicleModelType.Flatbed, new Vector3(1430.0, 975.0, 11.0), 0.0f, -1, -1, 600));
            JobVehicles.TruckerJobVehicles.Add(BaseVehicle.CreateStatic(VehicleModelType.DFT30, new Vector3(1420.0, 975.0, 11.0), 0.0f, -1, -1, 600));
            JobVehicles.TruckerJobVehicles.Add(BaseVehicle.CreateStatic(VehicleModelType.Flatbed, new Vector3(1415.0, 975.0, 11.0), 0.0f, -1, -1, 600));
            JobVehicles.TruckerJobVehicles.Add(BaseVehicle.CreateStatic(VehicleModelType.DFT30, new Vector3(1410.0, 975.0, 11.0), 0.0f, -1, -1, 600));
            JobVehicles.TruckerJobVehicles.Add(BaseVehicle.CreateStatic(VehicleModelType.Flatbed, new Vector3(1405.0, 975.0, 11.0), 0.0f, -1, -1, 600));
            JobVehicles.TruckerJobVehicles.Add(BaseVehicle.CreateStatic(VehicleModelType.ArticleTrailer, new Vector3(1490.0, 1015.0, 11.0), 90.0f, -1, -1, 600));
            JobVehicles.TruckerJobVehicles.Add(BaseVehicle.CreateStatic(VehicleModelType.ArticleTrailer2, new Vector3(1490.0, 1020.0, 11.0), 90.0f, -1, -1, 600));
            JobVehicles.TruckerJobVehicles.Add(BaseVehicle.CreateStatic(VehicleModelType.PetrolTrailer, new Vector3(1490.0, 1025.0, 11.0), 90.0f, -1, -1, 600));
            JobVehicles.TruckerJobVehicles.Add(BaseVehicle.CreateStatic(VehicleModelType.Linerunner, new Vector3(1490.0, 1030.0, 11.0), 90.0f, -1, -1, 600));
            JobVehicles.TruckerJobVehicles.Add(BaseVehicle.CreateStatic(VehicleModelType.Tanker, new Vector3(1490.0, 1035.0, 11.0), 90.0f, -1, -1, 600));
            JobVehicles.TruckerJobVehicles.Add(BaseVehicle.CreateStatic(VehicleModelType.Roadtrain, new Vector3(1490.0, 1040.0, 11.0), 90.0f, -1, -1, 600));
            JobVehicles.TruckerJobVehicles.Add(BaseVehicle.CreateStatic(VehicleModelType.ArticleTrailer, new Vector3(1490.0, 1045.0, 11.0), 90.0f, -1, -1, 600));
            JobVehicles.TruckerJobVehicles.Add(BaseVehicle.CreateStatic(VehicleModelType.ArticleTrailer2, new Vector3(1490.0, 1050.0, 11.0), 90.0f, -1, -1, 600));
            JobVehicles.TruckerJobVehicles.Add(BaseVehicle.CreateStatic(VehicleModelType.PetrolTrailer, new Vector3(1490.0, 1055.0, 11.0), 90.0f, -1, -1, 600));
            JobVehicles.TruckerJobVehicles.Add(BaseVehicle.CreateStatic(VehicleModelType.Linerunner, new Vector3(1490.0, 1060.0, 11.0), 90.0f, -1, -1, 600));
            JobVehicles.TruckerJobVehicles.Add(BaseVehicle.CreateStatic(VehicleModelType.Tanker, new Vector3(1490.0, 1065.0, 11.0), 90.0f, -1, -1, 600));
            JobVehicles.TruckerJobVehicles.Add(BaseVehicle.CreateStatic(VehicleModelType.Roadtrain, new Vector3(1490.0, 1070.0, 11.0), 90.0f, -1, -1, 600));

            #endregion

            #region Doherty Depot

            JobVehicles.TruckerJobVehicles.Add(BaseVehicle.CreateStatic(VehicleModelType.ArticleTrailer, new Vector3(-2105.0, -200.0, 35.5), 90.0f, -1, -1, 600));
            JobVehicles.TruckerJobVehicles.Add(BaseVehicle.CreateStatic(VehicleModelType.ArticleTrailer2, new Vector3(-2105.0, -205.0, 35.5), 90.0f, -1, -1, 600));
            JobVehicles.TruckerJobVehicles.Add(BaseVehicle.CreateStatic(VehicleModelType.PetrolTrailer, new Vector3(-2105.0, -210.0, 35.5), 90.0f, -1, -1, 600));
            JobVehicles.TruckerJobVehicles.Add(BaseVehicle.CreateStatic(VehicleModelType.ArticleTrailer, new Vector3(-2105.0, -215.0, 35.5), 90.0f, -1, -1, 600));
            JobVehicles.TruckerJobVehicles.Add(BaseVehicle.CreateStatic(VehicleModelType.ArticleTrailer2, new Vector3(-2105.0, -220.0, 35.5), 90.0f, -1, -1, 600));
            JobVehicles.TruckerJobVehicles.Add(BaseVehicle.CreateStatic(VehicleModelType.PetrolTrailer, new Vector3(-2105.0, -225.0, 35.5), 90.0f, -1, -1, 600));
            JobVehicles.TruckerJobVehicles.Add(BaseVehicle.CreateStatic(VehicleModelType.ArticleTrailer, new Vector3(-2105.0, -230.0, 35.5), 90.0f, -1, -1, 600));
            JobVehicles.TruckerJobVehicles.Add(BaseVehicle.CreateStatic(VehicleModelType.ArticleTrailer2, new Vector3(-2105.0, -235.0, 35.5), 90.0f, -1, -1, 600));
            JobVehicles.TruckerJobVehicles.Add(BaseVehicle.CreateStatic(VehicleModelType.PetrolTrailer, new Vector3(-2105.0, -240.0, 35.5), 90.0f, -1, -1, 600));
            JobVehicles.TruckerJobVehicles.Add(BaseVehicle.CreateStatic(VehicleModelType.DFT30, new Vector3(-2105.0, -245.0, 35.5), 90.0f, -1, -1, 600));
            JobVehicles.TruckerJobVehicles.Add(BaseVehicle.CreateStatic(VehicleModelType.Flatbed, new Vector3(-2105.0, -250.0, 35.5), 90.0f, -1, -1, 600));
            JobVehicles.TruckerJobVehicles.Add(BaseVehicle.CreateStatic(VehicleModelType.DFT30, new Vector3(-2105.0, -255.0, 35.5), 90.0f, -1, -1, 600));
            JobVehicles.TruckerJobVehicles.Add(BaseVehicle.CreateStatic(VehicleModelType.Flatbed, new Vector3(-2105.0, -260.0, 35.5), 90.0f, -1, -1, 600));
            JobVehicles.TruckerJobVehicles.Add(BaseVehicle.CreateStatic(VehicleModelType.DFT30, new Vector3(-2105.0, -265.0, 35.5), 90.0f, -1, -1, 600));
            JobVehicles.TruckerJobVehicles.Add(BaseVehicle.CreateStatic(VehicleModelType.Flatbed, new Vector3(-2105.0, -270.0, 35.5), 90.0f, -1, -1, 600));
            JobVehicles.TruckerJobVehicles.Add(BaseVehicle.CreateStatic(VehicleModelType.Linerunner, new Vector3(-2144.0, -186.0, 35.5), 270.0f, -1, -1, 600));
            JobVehicles.TruckerJobVehicles.Add(BaseVehicle.CreateStatic(VehicleModelType.Tanker, new Vector3(-2144.0, -191.0, 35.5), 270.0f, -1, -1, 600));
            JobVehicles.TruckerJobVehicles.Add(BaseVehicle.CreateStatic(VehicleModelType.Roadtrain, new Vector3(-2144.0, -196.0, 35.5), 270.0f, -1, -1, 600));
            JobVehicles.TruckerJobVehicles.Add(BaseVehicle.CreateStatic(VehicleModelType.Linerunner, new Vector3(-2144.0, -201.0, 35.5), 270.0f, -1, -1, 600));
            JobVehicles.TruckerJobVehicles.Add(BaseVehicle.CreateStatic(VehicleModelType.Tanker, new Vector3(-2144.0, -206.0, 35.5), 270.0f, -1, -1, 600));
            JobVehicles.TruckerJobVehicles.Add(BaseVehicle.CreateStatic(VehicleModelType.Roadtrain, new Vector3(-2144.0, -211.0, 35.5), 270.0f, -1, -1, 600));

            #endregion

            #region El Corona Depot

            JobVehicles.TruckerJobVehicles.Add(BaseVehicle.CreateStatic(VehicleModelType.ArticleTrailer, new Vector3(1805.0, -2025.0, 14.0), 90.0f, -1, -1, 600));
            JobVehicles.TruckerJobVehicles.Add(BaseVehicle.CreateStatic(VehicleModelType.ArticleTrailer2, new Vector3(1805.0, -2030.0, 14.0), 90.0f, -1, -1, 600));
            JobVehicles.TruckerJobVehicles.Add(BaseVehicle.CreateStatic(VehicleModelType.PetrolTrailer, new Vector3(1805.0, -2035.0, 14.0), 90.0f, -1, -1, 600));
            JobVehicles.TruckerJobVehicles.Add(BaseVehicle.CreateStatic(VehicleModelType.ArticleTrailer, new Vector3(1805.0, -2040.0, 14.0), 90.0f, -1, -1, 600));
            JobVehicles.TruckerJobVehicles.Add(BaseVehicle.CreateStatic(VehicleModelType.ArticleTrailer2, new Vector3(1805.0, -2045.0, 14.0), 90.0f, -1, -1, 600));
            JobVehicles.TruckerJobVehicles.Add(BaseVehicle.CreateStatic(VehicleModelType.PetrolTrailer, new Vector3(1805.0, -2050.0, 14.0), 90.0f, -1, -1, 600));
            JobVehicles.TruckerJobVehicles.Add(BaseVehicle.CreateStatic(VehicleModelType.DFT30, new Vector3(1745.0, -2070.0, 14.0), 0.0f, -1, -1, 600));
            JobVehicles.TruckerJobVehicles.Add(BaseVehicle.CreateStatic(VehicleModelType.Flatbed, new Vector3(1750.0, -2070.0, 14.0), 0.0f, -1, -1, 600));
            JobVehicles.TruckerJobVehicles.Add(BaseVehicle.CreateStatic(VehicleModelType.DFT30, new Vector3(1755.0, -2070.0, 14.0), 0.0f, -1, -1, 600));
            JobVehicles.TruckerJobVehicles.Add(BaseVehicle.CreateStatic(VehicleModelType.Flatbed, new Vector3(1760.0, -2070.0, 14.0), 0.0f, -1, -1, 600));
            JobVehicles.TruckerJobVehicles.Add(BaseVehicle.CreateStatic(VehicleModelType.Linerunner, new Vector3(1765.0, -2070.0, 14.0), 0.0f, -1, -1, 600));
            JobVehicles.TruckerJobVehicles.Add(BaseVehicle.CreateStatic(VehicleModelType.Tanker, new Vector3(1770.0, -2070.0, 14.0), 0.0f, -1, -1, 600));
            JobVehicles.TruckerJobVehicles.Add(BaseVehicle.CreateStatic(VehicleModelType.Roadtrain, new Vector3(1775.0, -2070.0, 14.0), 0.0f, -1, -1, 600));

            #endregion

            #region Las Payasdas Depot

            JobVehicles.TruckerJobVehicles.Add(BaseVehicle.CreateStatic(VehicleModelType.ArticleTrailer, new Vector3(-535.0, 2635.0, 54.0), 270.0f, -1, -1, 600));
            JobVehicles.TruckerJobVehicles.Add(BaseVehicle.CreateStatic(VehicleModelType.ArticleTrailer2, new Vector3(-535.0, 2630.0, 54.0), 270.0f, -1, -1, 600));
            JobVehicles.TruckerJobVehicles.Add(BaseVehicle.CreateStatic(VehicleModelType.PetrolTrailer, new Vector3(-535.0, 2625.0, 54.0), 270.0f, -1, -1, 600));
            JobVehicles.TruckerJobVehicles.Add(BaseVehicle.CreateStatic(VehicleModelType.ArticleTrailer, new Vector3(-535.0, 2620.0, 54.0), 270.0f, -1, -1, 600));
            JobVehicles.TruckerJobVehicles.Add(BaseVehicle.CreateStatic(VehicleModelType.ArticleTrailer2, new Vector3(-535.0, 2615.0, 54.0), 270.0f, -1, -1, 600));
            JobVehicles.TruckerJobVehicles.Add(BaseVehicle.CreateStatic(VehicleModelType.PetrolTrailer, new Vector3(-535.0, 2610.0, 54.0), 270.0f, -1, -1, 600));
            JobVehicles.TruckerJobVehicles.Add(BaseVehicle.CreateStatic(VehicleModelType.ArticleTrailer, new Vector3(-535.0, 2605.0, 54.0), 270.0f, -1, -1, 600));
            JobVehicles.TruckerJobVehicles.Add(BaseVehicle.CreateStatic(VehicleModelType.ArticleTrailer2, new Vector3(-535.0, 2600.0, 54.0), 270.0f, -1, -1, 600));
            JobVehicles.TruckerJobVehicles.Add(BaseVehicle.CreateStatic(VehicleModelType.PetrolTrailer, new Vector3(-535.0, 2595.0, 54.0), 270.0f, -1, -1, 600));
            JobVehicles.TruckerJobVehicles.Add(BaseVehicle.CreateStatic(VehicleModelType.ArticleTrailer, new Vector3(-535.0, 2590.0, 54.0), 270.0f, -1, -1, 600));
            JobVehicles.TruckerJobVehicles.Add(BaseVehicle.CreateStatic(VehicleModelType.ArticleTrailer2, new Vector3(-535.0, 2585.0, 54.0), 270.0f, -1, -1, 600));
            JobVehicles.TruckerJobVehicles.Add(BaseVehicle.CreateStatic(VehicleModelType.PetrolTrailer, new Vector3(-535.0, 2580.0, 54.0), 270.0f, -1, -1, 600));
            JobVehicles.TruckerJobVehicles.Add(BaseVehicle.CreateStatic(VehicleModelType.ArticleTrailer, new Vector3(-535.0, 2575.0, 54.0), 270.0f, -1, -1, 600));
            JobVehicles.TruckerJobVehicles.Add(BaseVehicle.CreateStatic(VehicleModelType.ArticleTrailer2, new Vector3(-535.0, 2570.0, 54.0), 270.0f, -1, -1, 600));
            JobVehicles.TruckerJobVehicles.Add(BaseVehicle.CreateStatic(VehicleModelType.PetrolTrailer, new Vector3(-535.0, 2565.0, 54.0), 270.0f, -1, -1, 600));
            JobVehicles.TruckerJobVehicles.Add(BaseVehicle.CreateStatic(VehicleModelType.ArticleTrailer, new Vector3(-535.0, 2560.0, 54.0), 270.0f, -1, -1, 600));
            JobVehicles.TruckerJobVehicles.Add(BaseVehicle.CreateStatic(VehicleModelType.ArticleTrailer2, new Vector3(-535.0, 2555.0, 54.0), 270.0f, -1, -1, 600));
            JobVehicles.TruckerJobVehicles.Add(BaseVehicle.CreateStatic(VehicleModelType.PetrolTrailer, new Vector3(-535.0, 2550.0, 54.0), 270.0f, -1, -1, 600));
            JobVehicles.TruckerJobVehicles.Add(BaseVehicle.CreateStatic(VehicleModelType.Linerunner, new Vector3(-512.0, 2635.0, 54.0), 90.0f, -1, -1, 600));
            JobVehicles.TruckerJobVehicles.Add(BaseVehicle.CreateStatic(VehicleModelType.Tanker, new Vector3(-512.0, 2630.0, 54.0), 90.0f, -1, -1, 600));
            JobVehicles.TruckerJobVehicles.Add(BaseVehicle.CreateStatic(VehicleModelType.Roadtrain, new Vector3(-512.0, 2625.0, 54.0), 90.0f, -1, -1, 600));
            JobVehicles.TruckerJobVehicles.Add(BaseVehicle.CreateStatic(VehicleModelType.Linerunner, new Vector3(-512.0, 2620.0, 54.0), 90.0f, -1, -1, 600));
            JobVehicles.TruckerJobVehicles.Add(BaseVehicle.CreateStatic(VehicleModelType.Tanker, new Vector3(-512.0, 2615.0, 54.0), 90.0f, -1, -1, 600));
            JobVehicles.TruckerJobVehicles.Add(BaseVehicle.CreateStatic(VehicleModelType.Roadtrain, new Vector3(-512.0, 2610.0, 54.0), 90.0f, -1, -1, 600));
            JobVehicles.TruckerJobVehicles.Add(BaseVehicle.CreateStatic(VehicleModelType.DFT30, new Vector3(-512.0, 2605.0, 54.0), 90.0f, -1, -1, 600));
            JobVehicles.TruckerJobVehicles.Add(BaseVehicle.CreateStatic(VehicleModelType.Flatbed, new Vector3(-512.0, 2600.0, 54.0), 90.0f, -1, -1, 600));
            JobVehicles.TruckerJobVehicles.Add(BaseVehicle.CreateStatic(VehicleModelType.DFT30, new Vector3(-512.0, 2585.0, 54.0), 90.0f, -1, -1, 600));
            JobVehicles.TruckerJobVehicles.Add(BaseVehicle.CreateStatic(VehicleModelType.Flatbed, new Vector3(-512.0, 2580.0, 54.0), 90.0f, -1, -1, 600));
            JobVehicles.TruckerJobVehicles.Add(BaseVehicle.CreateStatic(VehicleModelType.Linerunner, new Vector3(-512.0, 2575.0, 54.0), 90.0f, -1, -1, 600));
            JobVehicles.TruckerJobVehicles.Add(BaseVehicle.CreateStatic(VehicleModelType.Tanker, new Vector3(-512.0, 2570.0, 54.0), 90.0f, -1, -1, 600));
            JobVehicles.TruckerJobVehicles.Add(BaseVehicle.CreateStatic(VehicleModelType.Roadtrain, new Vector3(-512.0, 2565.0, 54.0), 90.0f, -1, -1, 600));
            JobVehicles.TruckerJobVehicles.Add(BaseVehicle.CreateStatic(VehicleModelType.Linerunner, new Vector3(-512.0, 2560.0, 54.0), 90.0f, -1, -1, 600));
            JobVehicles.TruckerJobVehicles.Add(BaseVehicle.CreateStatic(VehicleModelType.Tanker, new Vector3(-512.0, 2555.0, 54.0), 90.0f, -1, -1, 600));
            JobVehicles.TruckerJobVehicles.Add(BaseVehicle.CreateStatic(VehicleModelType.Roadtrain, new Vector3(-512.0, 2550.0, 54.0), 90.0f, -1, -1, 600));

            #endregion

            #region  Quarry Top

            JobVehicles.TruckerJobVehicles.Add(BaseVehicle.CreateStatic(VehicleModelType.CementTruck, new Vector3(340.0, 850.0, 21.0), 0.0f, -1, -1, 600));
            JobVehicles.TruckerJobVehicles.Add(BaseVehicle.CreateStatic(VehicleModelType.CementTruck, new Vector3(335.0, 860.0, 21.0), 0.0f, -1, -1, 600));
            JobVehicles.TruckerJobVehicles.Add(BaseVehicle.CreateStatic(VehicleModelType.CementTruck, new Vector3(330.0, 870.0, 21.0), 0.0f, -1, -1, 600));
            JobVehicles.TruckerJobVehicles.Add(BaseVehicle.CreateStatic(VehicleModelType.CementTruck, new Vector3(325.0, 880.0, 21.0), 0.0f, -1, -1, 600));
            JobVehicles.TruckerJobVehicles.Add(BaseVehicle.CreateStatic(VehicleModelType.CementTruck, new Vector3(320.0, 890.0, 21.0), 0.0f, -1, -1, 600));

            #endregion

            #region Shady Creek Depot

            JobVehicles.TruckerJobVehicles.Add(BaseVehicle.CreateStatic(VehicleModelType.ArticleTrailer, new Vector3(-1545.0, -2737.00, 49.0), 145.0f, -1, -1, 600));
            JobVehicles.TruckerJobVehicles.Add(BaseVehicle.CreateStatic(VehicleModelType.ArticleTrailer2, new Vector3(-1540.9, -2739.87, 49.0), 145.0f, -1, -1, 600));
            JobVehicles.TruckerJobVehicles.Add(BaseVehicle.CreateStatic(VehicleModelType.PetrolTrailer, new Vector3(-1536.8, -2742.74, 49.0), 145.0f, -1, -1, 600));
            JobVehicles.TruckerJobVehicles.Add(BaseVehicle.CreateStatic(VehicleModelType.ArticleTrailer, new Vector3(-1532.7, -2745.61, 49.0), 145.0f, -1, -1, 600));
            JobVehicles.TruckerJobVehicles.Add(BaseVehicle.CreateStatic(VehicleModelType.ArticleTrailer2, new Vector3(-1528.6, -2748.48, 49.0), 145.0f, -1, -1, 600));
            JobVehicles.TruckerJobVehicles.Add(BaseVehicle.CreateStatic(VehicleModelType.PetrolTrailer, new Vector3(-1524.5, -2751.35, 49.0), 145.0f, -1, -1, 600));
            JobVehicles.TruckerJobVehicles.Add(BaseVehicle.CreateStatic(VehicleModelType.Linerunner, new Vector3(-1557.75, -2744.80, 49.0), 145.0f, -1, -1, 600));
            JobVehicles.TruckerJobVehicles.Add(BaseVehicle.CreateStatic(VehicleModelType.Tanker, new Vector3(-1561.85, -2741.93, 49.0), 145.0f, -1, -1, 600));
            JobVehicles.TruckerJobVehicles.Add(BaseVehicle.CreateStatic(VehicleModelType.Roadtrain, new Vector3(-1565.95, -2739.06, 49.0), 145.0f, -1, -1, 600));
            JobVehicles.TruckerJobVehicles.Add(BaseVehicle.CreateStatic(VehicleModelType.Linerunner, new Vector3(-1570.05, -2736.19, 49.0), 145.0f, -1, -1, 600));
            JobVehicles.TruckerJobVehicles.Add(BaseVehicle.CreateStatic(VehicleModelType.Tanker, new Vector3(-1574.15, -2733.32, 49.0), 145.0f, -1, -1, 600));
            JobVehicles.TruckerJobVehicles.Add(BaseVehicle.CreateStatic(VehicleModelType.Roadtrain, new Vector3(-1578.25, -2730.45, 49.0), 145.0f, -1, -1, 600));

            #endregion
        }

        private void SpawnBuses()
        {
            JobVehicles.BusDriverJobVehicles.Add(BaseVehicle.CreateStatic(VehicleModelType.Coach, new Vector3(1782.0, -1888.0, 13.6), 270.0f, -1, -1, 600));
            JobVehicles.BusDriverJobVehicles.Add(BaseVehicle.CreateStatic(VehicleModelType.Coach, new Vector3(1782.0, -1893.0, 13.6), 270.0f, -1, -1, 600));
            JobVehicles.BusDriverJobVehicles.Add(BaseVehicle.CreateStatic(VehicleModelType.Coach, new Vector3(1782.0, -1898.0, 13.6), 270.0f, -1, -1, 600));
            JobVehicles.BusDriverJobVehicles.Add(BaseVehicle.CreateStatic(VehicleModelType.Coach, new Vector3(1782.0, -1903.0, 13.6), 270.0f, -1, -1, 600));
            JobVehicles.BusDriverJobVehicles.Add(BaseVehicle.CreateStatic(VehicleModelType.Coach, new Vector3(1782.0, -1908.0, 13.6), 270.0f, -1, -1, 600));
            JobVehicles.BusDriverJobVehicles.Add(BaseVehicle.CreateStatic(VehicleModelType.Coach, new Vector3(1782.0, -1913.0, 13.6), 270.0f, -1, -1, 600));
            JobVehicles.BusDriverJobVehicles.Add(BaseVehicle.CreateStatic(VehicleModelType.Coach, new Vector3(1782.0, -1918.0, 13.6), 270.0f, -1, -1, 600));

            JobVehicles.BusDriverJobVehicles.Add(BaseVehicle.CreateStatic(VehicleModelType.Coach, new Vector3(-1974.0, 105.0, 27.9), 90.0f, -1, -1, 600));
            JobVehicles.BusDriverJobVehicles.Add(BaseVehicle.CreateStatic(VehicleModelType.Coach, new Vector3(-1974.0, 100.0, 27.9), 90.0f, -1, -1, 600));
            JobVehicles.BusDriverJobVehicles.Add(BaseVehicle.CreateStatic(VehicleModelType.Coach, new Vector3(-1974.0, 95.0, 27.9), 90.0f, -1, -1, 600));
            JobVehicles.BusDriverJobVehicles.Add(BaseVehicle.CreateStatic(VehicleModelType.Coach, new Vector3(-1974.0, 85.0, 27.9), 90.0f, -1, -1, 600));
            JobVehicles.BusDriverJobVehicles.Add(BaseVehicle.CreateStatic(VehicleModelType.Coach, new Vector3(-1974.0, 80.0, 27.9), 90.0f, -1, -1, 600));

            JobVehicles.BusDriverJobVehicles.Add(BaseVehicle.CreateStatic(VehicleModelType.Coach, new Vector3(1085.0, 1230.0, 11.0), 90.0f, -1, -1, 600));
            JobVehicles.BusDriverJobVehicles.Add(BaseVehicle.CreateStatic(VehicleModelType.Coach, new Vector3(1085.0, 1235.0, 11.0), 90.0f, -1, -1, 600));
            JobVehicles.BusDriverJobVehicles.Add(BaseVehicle.CreateStatic(VehicleModelType.Coach, new Vector3(1085.0, 1240.0, 11.0), 90.0f, -1, -1, 600));
            JobVehicles.BusDriverJobVehicles.Add(BaseVehicle.CreateStatic(VehicleModelType.Coach, new Vector3(1085.0, 1245.0, 11.0), 90.0f, -1, -1, 600));
            JobVehicles.BusDriverJobVehicles.Add(BaseVehicle.CreateStatic(VehicleModelType.Coach, new Vector3(1085.0, 1250.0, 11.0), 90.0f, -1, -1, 600));
            JobVehicles.BusDriverJobVehicles.Add(BaseVehicle.CreateStatic(VehicleModelType.Coach, new Vector3(1085.0, 1255.0, 11.0), 90.0f, -1, -1, 600));
            JobVehicles.BusDriverJobVehicles.Add(BaseVehicle.CreateStatic(VehicleModelType.Coach, new Vector3(1085.0, 1260.0, 11.0), 90.0f, -1, -1, 600));
            JobVehicles.BusDriverJobVehicles.Add(BaseVehicle.CreateStatic(VehicleModelType.Coach, new Vector3(1085.0, 1265.0, 11.0), 90.0f, -1, -1, 600));
            JobVehicles.BusDriverJobVehicles.Add(BaseVehicle.CreateStatic(VehicleModelType.Coach, new Vector3(1085.0, 1270.0, 11.0), 90.0f, -1, -1, 600));
            JobVehicles.BusDriverJobVehicles.Add(BaseVehicle.CreateStatic(VehicleModelType.Coach, new Vector3(1085.0, 1275.0, 11.0), 90.0f, -1, -1, 600));
            JobVehicles.BusDriverJobVehicles.Add(BaseVehicle.CreateStatic(VehicleModelType.Coach, new Vector3(1085.0, 1280.0, 11.0), 90.0f, -1, -1, 600));
        }
        private void SpawnPoliceCars()
        {
            #region Los Santos Police Impound
            JobVehicles.PoliceJobVehicles.Add(BaseVehicle.CreateStatic(VehicleModelType.PoliceCarLSPD, new Vector3(1555.0, -1710.75, 6.0), 0.0f, 0, 1, 600));
            JobVehicles.PoliceJobVehicles.Add(BaseVehicle.CreateStatic(VehicleModelType.HPV1000, new Vector3(1560.0, -1710.75, 6.0), 0.0f, 0, 1, 600));
            JobVehicles.PoliceJobVehicles.Add(BaseVehicle.CreateStatic(VehicleModelType.PoliceRanger, new Vector3(1565.0, -1710.75, 6.0), 0.0f, 0, 1, 600));
            JobVehicles.PoliceJobVehicles.Add(BaseVehicle.CreateStatic(VehicleModelType.PoliceCarLSPD, new Vector3(1570.0, -1710.75, 6.0), 0.0f, 0, 1, 600));
            JobVehicles.PoliceJobVehicles.Add(BaseVehicle.CreateStatic(VehicleModelType.HPV1000, new Vector3(1575.0, -1710.75, 6.0), 0.0f, 0, 1, 600));
            JobVehicles.PoliceJobVehicles.Add(BaseVehicle.CreateStatic(VehicleModelType.PoliceRanger, new Vector3(1580.0, -1710.75, 6.0), 0.0f, 0, 1, 600));
            JobVehicles.PoliceJobVehicles.Add(BaseVehicle.CreateStatic(VehicleModelType.PoliceCarLSPD, new Vector3(1585.0, -1710.75, 6.0), 0.0f, 0, 1, 600));
            JobVehicles.PoliceJobVehicles.Add(BaseVehicle.CreateStatic(VehicleModelType.HPV1000, new Vector3(1590.0, -1710.75, 6.0), 0.0f, 0, 1, 600));
            JobVehicles.PoliceJobVehicles.Add(BaseVehicle.CreateStatic(VehicleModelType.PoliceRanger, new Vector3(1595.0, -1710.75, 6.0), 0.0f, 0, 1, 600));
            #endregion

            #region San Fierro Police Impound
            JobVehicles.PoliceJobVehicles.Add(BaseVehicle.CreateStatic(VehicleModelType.PoliceCarSFPD, new Vector3(-1573.0, 701.0, -5.0), 90.0f, 0, 1, 600));
            JobVehicles.PoliceJobVehicles.Add(BaseVehicle.CreateStatic(VehicleModelType.HPV1000, new Vector3(-1573.0, 706.0, -5.0), 90.0f, 0, 1, 600));
            JobVehicles.PoliceJobVehicles.Add(BaseVehicle.CreateStatic(VehicleModelType.PoliceRanger, new Vector3(-1573.0, 711.0, -5.0), 90.0f, 0, 1, 600));
            JobVehicles.PoliceJobVehicles.Add(BaseVehicle.CreateStatic(VehicleModelType.PoliceCarSFPD, new Vector3(-1573.0, 716.0, -5.0), 90.0f, 0, 1, 600));
            JobVehicles.PoliceJobVehicles.Add(BaseVehicle.CreateStatic(VehicleModelType.HPV1000, new Vector3(-1573.0, 721.0, -5.0), 90.0f, 0, 1, 600));
            JobVehicles.PoliceJobVehicles.Add(BaseVehicle.CreateStatic(VehicleModelType.PoliceRanger, new Vector3(-1573.0, 726.0, -5.0), 90.0f, 0, 1, 600));
            JobVehicles.PoliceJobVehicles.Add(BaseVehicle.CreateStatic(VehicleModelType.PoliceCarSFPD, new Vector3(-1573.0, 731.0, -5.0), 90.0f, 0, 1, 600));
            JobVehicles.PoliceJobVehicles.Add(BaseVehicle.CreateStatic(VehicleModelType.HPV1000, new Vector3(-1573.0, 736.0, -5.0), 90.0f, 0, 1, 600));
            JobVehicles.PoliceJobVehicles.Add(BaseVehicle.CreateStatic(VehicleModelType.PoliceRanger, new Vector3(-1573.0, 741.0, -5.0), 90.0f, 0, 1, 600));
            #endregion

            #region Las Venturas Police Impound
            JobVehicles.PoliceJobVehicles.Add(BaseVehicle.CreateStatic(VehicleModelType.PoliceCarLVPD, new Vector3(2282.0, 2477.0, 11.0), 180.0f, 0, 1, 600));
            JobVehicles.PoliceJobVehicles.Add(BaseVehicle.CreateStatic(VehicleModelType.HPV1000, new Vector3(2277.0, 2477.0, 11.0), 180.0f, 0, 1, 600));
            JobVehicles.PoliceJobVehicles.Add(BaseVehicle.CreateStatic(VehicleModelType.PoliceRanger, new Vector3(2272.0, 2477.0, 11.0), 180.0f, 0, 1, 600));
            JobVehicles.PoliceJobVehicles.Add(BaseVehicle.CreateStatic(VehicleModelType.PoliceCarLVPD, new Vector3(2262.0, 2477.0, 11.0), 180.0f, 0, 1, 600));
            JobVehicles.PoliceJobVehicles.Add(BaseVehicle.CreateStatic(VehicleModelType.HPV1000, new Vector3(2257.0, 2477.0, 11.0), 180.0f, 0, 1, 600));
            JobVehicles.PoliceJobVehicles.Add(BaseVehicle.CreateStatic(VehicleModelType.PoliceRanger, new Vector3(2252.0, 2477.0, 11.0), 180.0f, 0, 1, 600));

            JobVehicles.PoliceJobVehicles.Add(BaseVehicle.CreateStatic(VehicleModelType.PoliceCarLVPD, new Vector3(2282.0, 2443.0, 11.0), 0.0f, 0, 1, 600));
            JobVehicles.PoliceJobVehicles.Add(BaseVehicle.CreateStatic(VehicleModelType.HPV1000, new Vector3(2277.0, 2443.0, 11.0), 0.0f, 0, 1, 600));
            JobVehicles.PoliceJobVehicles.Add(BaseVehicle.CreateStatic(VehicleModelType.PoliceRanger, new Vector3(2272.0, 2443.0, 11.0), 0.0f, 0, 1, 600));
            JobVehicles.PoliceJobVehicles.Add(BaseVehicle.CreateStatic(VehicleModelType.PoliceCarLVPD, new Vector3(2262.0, 2443.0, 11.0), 0.0f, 0, 1, 600));
            JobVehicles.PoliceJobVehicles.Add(BaseVehicle.CreateStatic(VehicleModelType.HPV1000, new Vector3(2257.0, 2443.0, 11.0), 0.0f, 0, 1, 600));
            JobVehicles.PoliceJobVehicles.Add(BaseVehicle.CreateStatic(VehicleModelType.PoliceRanger, new Vector3(2252.0, 2443.0, 11.0), 0.0f, 0, 1, 600));
            #endregion

        }
        private void SpawnMafiaCars()
        {
            JobVehicles.MafiaJobVehicles.Add(BaseVehicle.CreateStatic(VehicleModelType.Sandking, new Vector3(2811.0, 900.0, 10.8), 0.0f, 0, 0, 600));
            JobVehicles.MafiaJobVehicles.Add(BaseVehicle.CreateStatic(VehicleModelType.Sandking, new Vector3(2806.0, 900.0, 10.8), 0.0f, 0, 0, 600));
            JobVehicles.MafiaJobVehicles.Add(BaseVehicle.CreateStatic(VehicleModelType.Sandking, new Vector3(2801.0, 900.0, 10.8), 0.0f, 0, 0, 600));
            JobVehicles.MafiaJobVehicles.Add(BaseVehicle.CreateStatic(VehicleModelType.Sandking, new Vector3(2796.0, 900.0, 10.8), 0.0f, 0, 0, 600));
            JobVehicles.MafiaJobVehicles.Add(BaseVehicle.CreateStatic(VehicleModelType.Moonbeam, new Vector3(2833.0, 900.0, 10.8), 0.0f, 0, 0, 600));
            JobVehicles.MafiaJobVehicles.Add(BaseVehicle.CreateStatic(VehicleModelType.Moonbeam, new Vector3(2838.0, 900.0, 10.8), 0.0f, 0, 0, 600));
            JobVehicles.MafiaJobVehicles.Add(BaseVehicle.CreateStatic(VehicleModelType.Moonbeam, new Vector3(2843.0, 900.0, 10.8), 0.0f, 0, 0, 600));
            JobVehicles.MafiaJobVehicles.Add(BaseVehicle.CreateStatic(VehicleModelType.Moonbeam, new Vector3(2848.0, 900.0, 10.8), 0.0f, 0, 0, 600));
            JobVehicles.MafiaJobVehicles.Add(BaseVehicle.CreateStatic(VehicleModelType.Roadtrain, new Vector3(2802, 966.0, 10.8), 180.0f, 0, 0, 600));
            JobVehicles.MafiaJobVehicles.Add(BaseVehicle.CreateStatic(VehicleModelType.Roadtrain, new Vector3(2807, 966.0, 10.8), 180.0f, 0, 0, 600));
            JobVehicles.MafiaJobVehicles.Add(BaseVehicle.CreateStatic(VehicleModelType.Roadtrain, new Vector3(2812, 966.0, 10.8), 180.0f, 0, 0, 600));
            JobVehicles.MafiaJobVehicles.Add(BaseVehicle.CreateStatic(VehicleModelType.Roadtrain, new Vector3(2817, 966.0, 10.8), 180.0f, 0, 0, 600));
            JobVehicles.MafiaJobVehicles.Add(BaseVehicle.CreateStatic(VehicleModelType.Roadtrain, new Vector3(2822, 966.0, 10.8), 180.0f, 0, 0, 600));
            JobVehicles.MafiaJobVehicles.Add(BaseVehicle.CreateStatic(VehicleModelType.Roadtrain, new Vector3(2827, 966.0, 10.8), 180.0f, 0, 0, 600));
        }
        private void SpawnPilotPlanes()
        {
            JobVehicles.PilotJobVehicles.Add(BaseVehicle.CreateStatic(VehicleModelType.Shamal, new Vector3(1990.0, -2295.0, 14.5), 90.0f, -1, -1, 300));
            JobVehicles.PilotJobVehicles.Add(BaseVehicle.CreateStatic(VehicleModelType.Shamal, new Vector3(1990.0, -2320.0, 14.5), 90.0f, -1, -1, 300));
            JobVehicles.PilotJobVehicles.Add(BaseVehicle.CreateStatic(VehicleModelType.Shamal, new Vector3(1990.0, -2345.0, 14.5), 90.0f, -1, -1, 300));
            JobVehicles.PilotJobVehicles.Add(BaseVehicle.CreateStatic(VehicleModelType.Shamal, new Vector3(1990.0, -2370.0, 14.5), 90.0f, -1, -1, 300));
            JobVehicles.PilotJobVehicles.Add(BaseVehicle.CreateStatic(VehicleModelType.Nevada, new Vector3(1920.0, -2265.0, 14.5), 180.0f, -1, -1, 300));
            JobVehicles.PilotJobVehicles.Add(BaseVehicle.CreateStatic(VehicleModelType.Nevada, new Vector3(1890.0, -2295.0, 14.5), 270.0f, -1, -1, 300));
            JobVehicles.PilotJobVehicles.Add(BaseVehicle.CreateStatic(VehicleModelType.Maverick, new Vector3(1940.0, -2265.0, 13.6), 180.0f, -1, -1, 300));
            JobVehicles.PilotJobVehicles.Add(BaseVehicle.CreateStatic(VehicleModelType.Maverick, new Vector3(1955.0, -2265.0, 13.6), 180.0f, -1, -1, 300));
            JobVehicles.PilotJobVehicles.Add(BaseVehicle.CreateStatic(VehicleModelType.Maverick, new Vector3(1970.0, -2265.0, 13.6), 180.0f, -1, -1, 300));
            JobVehicles.PilotJobVehicles.Add(BaseVehicle.CreateStatic(VehicleModelType.Maverick, new Vector3(1985.0, -2265.0, 13.6), 180.0f, -1, -1, 300));

            JobVehicles.PilotJobVehicles.Add(BaseVehicle.CreateStatic(VehicleModelType.Shamal, new Vector3(-1197.0, -153.0, 15.1), 135.0f, -1, -1, 300));
            JobVehicles.PilotJobVehicles.Add(BaseVehicle.CreateStatic(VehicleModelType.Shamal, new Vector3(-1214.0, -137.5, 15.1), 135.0f, -1, -1, 300));
            JobVehicles.PilotJobVehicles.Add(BaseVehicle.CreateStatic(VehicleModelType.Shamal, new Vector3(-1230.0, -120.0, 15.1), 135.0f, -1, -1, 300));
            JobVehicles.PilotJobVehicles.Add(BaseVehicle.CreateStatic(VehicleModelType.Shamal, new Vector3(-1246.0, -103.5, 15.1), 135.0f, -1, -1, 300));
            JobVehicles.PilotJobVehicles.Add(BaseVehicle.CreateStatic(VehicleModelType.Nevada, new Vector3(-1287.0, -52.0, 15.1), 135.0f, -1, -1, 300));
            JobVehicles.PilotJobVehicles.Add(BaseVehicle.CreateStatic(VehicleModelType.Nevada, new Vector3(-1312.0, -27.0, 15.1), 135.0f, -1, -1, 300));
            JobVehicles.PilotJobVehicles.Add(BaseVehicle.CreateStatic(VehicleModelType.Maverick, new Vector3(-1262.0, -88.5, 14.2), 135.0f, -1, -1, 300));
            JobVehicles.PilotJobVehicles.Add(BaseVehicle.CreateStatic(VehicleModelType.Maverick, new Vector3(-1277.0, -74.5, 14.2), 90.0f, -1, -1, 300));
            JobVehicles.PilotJobVehicles.Add(BaseVehicle.CreateStatic(VehicleModelType.Maverick, new Vector3(-1264.0, -61.5, 14.2), 45.0f, -1, -1, 300));

            JobVehicles.PilotJobVehicles.Add(BaseVehicle.CreateStatic(VehicleModelType.Shamal, new Vector3(1615.0, 1630.0, 11.8), 180.0f, -1, -1, 300));
            JobVehicles.PilotJobVehicles.Add(BaseVehicle.CreateStatic(VehicleModelType.Shamal, new Vector3(1590.0, 1630.0, 11.8), 180.0f, -1, -1, 300));
            JobVehicles.PilotJobVehicles.Add(BaseVehicle.CreateStatic(VehicleModelType.Shamal, new Vector3(1565.0, 1630.0, 11.8), 180.0f, -1, -1, 300));
            JobVehicles.PilotJobVehicles.Add(BaseVehicle.CreateStatic(VehicleModelType.Nevada, new Vector3(1545.0, 1640.0, 11.8), 90.0f, -1, -1, 300));
            JobVehicles.PilotJobVehicles.Add(BaseVehicle.CreateStatic(VehicleModelType.Nevada, new Vector3(1545.0, 1675.0, 11.8), 90.0f, -1, -1, 300));
            JobVehicles.PilotJobVehicles.Add(BaseVehicle.CreateStatic(VehicleModelType.Nevada, new Vector3(1545.0, 1710.0, 11.8), 90.0f, -1, -1, 300));
            JobVehicles.PilotJobVehicles.Add(BaseVehicle.CreateStatic(VehicleModelType.Maverick, new Vector3(1650.0, 1557, 10.9), 90.0f, -1, -1, 300));
            JobVehicles.PilotJobVehicles.Add(BaseVehicle.CreateStatic(VehicleModelType.Maverick, new Vector3(1650.0, 1542, 10.9), 90.0f, -1, -1, 300));
            JobVehicles.PilotJobVehicles.Add(BaseVehicle.CreateStatic(VehicleModelType.Maverick, new Vector3(1635.0, 1534, 10.9), 0.0f, -1, -1, 300));
        }
        private void SpawnAssistanceVehicles()
        {
            JobVehicles.AssistanceJobVehicles.Add(BaseVehicle.CreateStatic(VehicleModelType.Towtruck, new Vector3(220.0, 34.0, 2.6), 270.0f, -1, -1, 300));
            JobVehicles.AssistanceJobVehicles.Add(BaseVehicle.CreateStatic(VehicleModelType.Towtruck, new Vector3(220.0, 29.0, 2.6), 270.0f, -1, -1, 300));
            JobVehicles.AssistanceJobVehicles.Add(BaseVehicle.CreateStatic(VehicleModelType.Towtruck, new Vector3(220.0, 24.0, 2.6), 270.0f, -1, -1, 300));
            JobVehicles.AssistanceJobVehicles.Add(BaseVehicle.CreateStatic(VehicleModelType.Towtruck, new Vector3(220.0, 19.0, 2.6), 270.0f, -1, -1, 300));
            JobVehicles.AssistanceJobVehicles.Add(BaseVehicle.CreateStatic(VehicleModelType.Towtruck, new Vector3(220.0, 14.0, 2.6), 270.0f, -1, -1, 300));
            JobVehicles.AssistanceJobVehicles.Add(BaseVehicle.CreateStatic(VehicleModelType.Towtruck, new Vector3(220.0, 9.0, 2.6), 270.0f, -1, -1, 300));
            JobVehicles.AssistanceJobVehicles.Add(BaseVehicle.CreateStatic(VehicleModelType.Towtruck, new Vector3(220.0, 4.0, 2.6), 270.0f, -1, -1, 300));
            JobVehicles.AssistanceJobVehicles.Add(BaseVehicle.CreateStatic(VehicleModelType.Towtruck, new Vector3(220.0, -1.0, 2.6), 270.0f, -1, -1, 300));
        }
        private void SpawnCourierVehicles()
        {
            JobVehicles.CourierJobVehicles.Add(BaseVehicle.CreateStatic(VehicleModelType.Burrito, new Vector3(784.0, -610.0, 16.4), 0.0f, -1, -1, 300));
            JobVehicles.CourierJobVehicles.Add(BaseVehicle.CreateStatic(VehicleModelType.Burrito, new Vector3(794.0, -610.0, 16.4), 0.0f, -1, -1, 300));
            JobVehicles.CourierJobVehicles.Add(BaseVehicle.CreateStatic(VehicleModelType.Burrito, new Vector3(804.0, -610.0, 16.4), 0.0f, -1, -1, 300));
            JobVehicles.CourierJobVehicles.Add(BaseVehicle.CreateStatic(VehicleModelType.Burrito, new Vector3(814.0, -610.0, 16.4), 0.0f, -1, -1, 300));
            JobVehicles.CourierJobVehicles.Add(BaseVehicle.CreateStatic(VehicleModelType.Burrito, new Vector3(824.0, -610.0, 16.4), 0.0f, -1, -1, 300));
            JobVehicles.CourierJobVehicles.Add(BaseVehicle.CreateStatic(VehicleModelType.Faggio, new Vector3(789.0, -610.0, 16.4), 0.0f, -1, -1, 300));
            JobVehicles.CourierJobVehicles.Add(BaseVehicle.CreateStatic(VehicleModelType.Faggio, new Vector3(799.0, -610.0, 16.4), 0.0f, -1, -1, 300));
            JobVehicles.CourierJobVehicles.Add(BaseVehicle.CreateStatic(VehicleModelType.Faggio, new Vector3(809.0, -610.0, 16.4), 0.0f, -1, -1, 300));
            JobVehicles.CourierJobVehicles.Add(BaseVehicle.CreateStatic(VehicleModelType.Faggio, new Vector3(819.0, -610.0, 16.4), 0.0f, -1, -1, 300));
            JobVehicles.CourierJobVehicles.Add(BaseVehicle.CreateStatic(VehicleModelType.Faggio, new Vector3(829.0, -610.0, 16.4), 0.0f, -1, -1, 300));

            JobVehicles.CourierJobVehicles.Add(BaseVehicle.CreateStatic(VehicleModelType.Burrito, new Vector3(-1851.0, -142.5, 12.0), 0.0f, -1, -1, 300));
            JobVehicles.CourierJobVehicles.Add(BaseVehicle.CreateStatic(VehicleModelType.Burrito, new Vector3(-1856.0, -142.5, 12.0), 0.0f, -1, -1, 300));
            JobVehicles.CourierJobVehicles.Add(BaseVehicle.CreateStatic(VehicleModelType.Burrito, new Vector3(-1861.0, -142.5, 12.0), 0.0f, -1, -1, 300));
            JobVehicles.CourierJobVehicles.Add(BaseVehicle.CreateStatic(VehicleModelType.Burrito, new Vector3(-1866.0, -142.5, 12.0), 0.0f, -1, -1, 300));
            JobVehicles.CourierJobVehicles.Add(BaseVehicle.CreateStatic(VehicleModelType.Burrito, new Vector3(-1871.0, -142.5, 12.0), 0.0f, -1, -1, 300));
            JobVehicles.CourierJobVehicles.Add(BaseVehicle.CreateStatic(VehicleModelType.Faggio, new Vector3(-1851.0, -129.5, 12.0), 180.0f, -1, -1, 300));
            JobVehicles.CourierJobVehicles.Add(BaseVehicle.CreateStatic(VehicleModelType.Faggio, new Vector3(-1856.0, -129.5, 12.0), 180.0f, -1, -1, 300));
            JobVehicles.CourierJobVehicles.Add(BaseVehicle.CreateStatic(VehicleModelType.Faggio, new Vector3(-1861.0, -129.5, 12.0), 180.0f, -1, -1, 300));
            JobVehicles.CourierJobVehicles.Add(BaseVehicle.CreateStatic(VehicleModelType.Faggio, new Vector3(-1866.0, -129.5, 12.0), 180.0f, -1, -1, 300));

            JobVehicles.CourierJobVehicles.Add(BaseVehicle.CreateStatic(VehicleModelType.Burrito, new Vector3(1052.5, 1915.25, 10.9), 0.0f, -1, -1, 300));
            JobVehicles.CourierJobVehicles.Add(BaseVehicle.CreateStatic(VehicleModelType.Burrito, new Vector3(1062.5, 1915.25, 10.9), 0.0f, -1, -1, 300));
            JobVehicles.CourierJobVehicles.Add(BaseVehicle.CreateStatic(VehicleModelType.Burrito, new Vector3(1072.5, 1915.25, 10.9), 0.0f, -1, -1, 300));
            JobVehicles.CourierJobVehicles.Add(BaseVehicle.CreateStatic(VehicleModelType.Burrito, new Vector3(1082.5, 1915.25, 10.9), 0.0f, -1, -1, 300));
            JobVehicles.CourierJobVehicles.Add(BaseVehicle.CreateStatic(VehicleModelType.Burrito, new Vector3(1092.5, 1915.25, 10.9), 0.0f, -1, -1, 300));
            JobVehicles.CourierJobVehicles.Add(BaseVehicle.CreateStatic(VehicleModelType.Faggio, new Vector3(1057.5, 1915.25, 10.9), 0.0f, -1, -1, 300));
            JobVehicles.CourierJobVehicles.Add(BaseVehicle.CreateStatic(VehicleModelType.Faggio, new Vector3(1067.5, 1915.25, 10.9), 0.0f, -1, -1, 300));
            JobVehicles.CourierJobVehicles.Add(BaseVehicle.CreateStatic(VehicleModelType.Faggio, new Vector3(1077.5, 1915.25, 10.9), 0.0f, -1, -1, 300));
            JobVehicles.CourierJobVehicles.Add(BaseVehicle.CreateStatic(VehicleModelType.Faggio, new Vector3(1087.5, 1915.25, 10.9), 0.0f, -1, -1, 300));
        }
        private void SpawnRoadworkerVehicles()
        {
            JobVehicles.RoadworkerJobVehicles.Add(BaseVehicle.CreateStatic(VehicleModelType.UtilityVan, new Vector3(-1895.0, -1705.0, 21.5), 180.0f, -1, -1, 300));
            JobVehicles.RoadworkerJobVehicles.Add(BaseVehicle.CreateStatic(VehicleModelType.UtilityVan, new Vector3(-1900.0, -1705.0, 21.5), 180.0f, -1, -1, 300));
            JobVehicles.RoadworkerJobVehicles.Add(BaseVehicle.CreateStatic(VehicleModelType.UtilityVan, new Vector3(-1905.0, -1705.0, 21.5), 180.0f, -1, -1, 300));
            JobVehicles.RoadworkerJobVehicles.Add(BaseVehicle.CreateStatic(VehicleModelType.UtilityVan, new Vector3(-1910.0, -1705.0, 21.5), 180.0f, -1, -1, 300));
            JobVehicles.RoadworkerJobVehicles.Add(BaseVehicle.CreateStatic(VehicleModelType.UtilityVan, new Vector3(-1915.0, -1705.0, 21.5), 180.0f, -1, -1, 300));
            JobVehicles.RoadworkerJobVehicles.Add(BaseVehicle.CreateStatic(VehicleModelType.UtilityVan, new Vector3(-1920.0, -1705.0, 21.5), 180.0f, -1, -1, 300));
            JobVehicles.RoadworkerJobVehicles.Add(BaseVehicle.CreateStatic(VehicleModelType.UtilityTrailer, new Vector3(-1895.0, -1700.0, 21.5), 180.0f, -1, -1, 300));
            JobVehicles.RoadworkerJobVehicles.Add(BaseVehicle.CreateStatic(VehicleModelType.UtilityTrailer, new Vector3(-1900.0, -1700.0, 21.5), 180.0f, -1, -1, 300));
            JobVehicles.RoadworkerJobVehicles.Add(BaseVehicle.CreateStatic(VehicleModelType.UtilityTrailer, new Vector3(-1905.0, -1700.0, 21.5), 180.0f, -1, -1, 300));
            JobVehicles.RoadworkerJobVehicles.Add(BaseVehicle.CreateStatic(VehicleModelType.UtilityTrailer, new Vector3(-1910.0, -1700.0, 21.5), 180.0f, -1, -1, 300));
            JobVehicles.RoadworkerJobVehicles.Add(BaseVehicle.CreateStatic(VehicleModelType.UtilityTrailer, new Vector3(-1915.0, -1700.0, 21.5), 180.0f, -1, -1, 300));
            JobVehicles.RoadworkerJobVehicles.Add(BaseVehicle.CreateStatic(VehicleModelType.UtilityTrailer, new Vector3(-1920.0, -1700.0, 21.5), 180.0f, -1, -1, 300));
            JobVehicles.RoadworkerJobVehicles.Add(BaseVehicle.CreateStatic(VehicleModelType.Towtruck, new Vector3(-1866.5, -1731.75, 21.7), 30.0f, -1, -1, 300));
            JobVehicles.RoadworkerJobVehicles.Add(BaseVehicle.CreateStatic(VehicleModelType.Towtruck, new Vector3(-1870.5, -1734.25, 21.7), 30.0f, -1, -1, 300));
            JobVehicles.RoadworkerJobVehicles.Add(BaseVehicle.CreateStatic(VehicleModelType.Towtruck, new Vector3(-1874.5, -1736.75, 21.7), 30.0f, -1, -1, 300));
            JobVehicles.RoadworkerJobVehicles.Add(BaseVehicle.CreateStatic(VehicleModelType.Towtruck, new Vector3(-1878.5, -1739.25, 21.7), 30.0f, -1, -1, 300));
            JobVehicles.RoadworkerJobVehicles.Add(BaseVehicle.CreateStatic(VehicleModelType.Towtruck, new Vector3(-1882.5, -1741.75, 21.7), 30.0f, -1, -1, 300));
            JobVehicles.RoadworkerJobVehicles.Add(BaseVehicle.CreateStatic(VehicleModelType.Towtruck, new Vector3(-1886.5, -1744.25, 21.7), 30.0f, -1, -1, 300));
        }
    }
}