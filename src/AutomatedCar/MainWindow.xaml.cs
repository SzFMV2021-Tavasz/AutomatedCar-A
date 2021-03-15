using AutomatedCar.KeyboardHandling;
using AutomatedCar.Models;
using AutomatedCar.SystemComponents.SystemDebug;
using AutomatedCar.ViewModels;
using BaseModel.Sensors;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Windows;
using System.Windows.Input;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace AutomatedCar
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly double tickInterval = 20;

        public MainWindowViewModel ViewModel { get; set; }

        DispatcherTimer timer = new DispatcherTimer();
        World world = World.Instance;

        private KeyboardHandler keyboardHandler;
        private HMIDebug hmiDebug;

        public MainWindow()
        {
            ViewModel = new MainWindowViewModel(world);
                      
            InitializeComponent();
           
            hmiDebug = new HMIDebug();
            keyboardHandler = new KeyboardHandler(tickInterval);          
            timer.Interval = TimeSpan.FromMilliseconds(tickInterval);
            timer.Tick += logic;
            timer.Start();
            // make my dockpanel focus of this game
            MainDockPanel.Focus();

            //world.Width = 2000;
            //world.Height = 1000;
            var path = $"{Directory.GetCurrentDirectory()}/Assets";
            var worldPath = $"{path}/test_world.json";
            var polygonsPath = $"{path}/worldobject_polygons.json";
            world.LoadFromJSON(worldPath, polygonsPath);

            var circle = new Circle(400, 200, "circle.png", 20);
            circle.Width = 40;
            circle.Height = 40;
            circle.ZIndex = 2;
            world.AddObject(circle);

            var controlledCar = new Models.AutomatedCar(50, 50, "car_1_white.png");
            controlledCar.Width = 108;
            controlledCar.Height = 240;
            controlledCar.IsHighLighted = true;

            // read the world object polygons, get the one for the car in a primitive way
            // this is just a sample, the proecssing shold be much more general
            string json_text = System.IO.File.ReadAllText($"Assets/worldobject_polygons.json");
            dynamic stuff = JObject.Parse(json_text);
            var geom = new Polyline();
            // get the points from the json and add to the polyline
            foreach (var i in stuff["objects"][0]["polys"][0]["points"])
            {
                geom.Points.Add(new Point(i[0].ToObject<int>(), i[1].ToObject<int>()));
            }
            // add polyline to the car
            controlledCar.Geometry = geom;


            List<IDisplaySensor> cameraSensores = new List<IDisplaySensor>();
            cameraSensores.Add(new CameraSensor() { x1 = new Point(54, 120), x2 = new Point(0, -120), x3= new Point(108, -120) });
            controlledCar.Video = cameraSensores;

            List<IDisplaySensor> radarSensores = new List<IDisplaySensor>();
            //Radar points example, clear in  #127 issue
            var radar = new Polyline();
            radar.Points.Add(new Point(54, 120));
            radar.Points.Add(new Point(0, -300));
            radar.Points.Add(new Point(108, -300));
            //controlledCar.Radar = radar;
            controlledCar.Radar = null;

            List<IDisplaySensor> sonicSensores = new List<IDisplaySensor>();
            //Sonic points example, clear in  #127 issue
            var sonic = new Polyline();
            sonic.Points.Add(new Point(6, 20));
            sonic.Points.Add(new Point(-144, -159));
            sonic.Points.Add(new Point(-144, 199));
            //controlledCar.UltraSonic = sonic;
            controlledCar.UltraSonic = null;

            world.AddObject(controlledCar);
            world.ControlledCar = controlledCar;
            controlledCar.Start();
            var dashBoardViewModel = new DashboardViewModel(world.ControlledCar);
            ViewModel.Dashboard = dashBoardViewModel;
            BindKeysForDashboardFunctions(dashBoardViewModel);
            timer.Tick += dashBoardViewModel.HandlePackets;
        }

        private void BindKeysForDashboardFunctions(DashboardViewModel dashBoardViewModel)
        {
            BindACCFeatures(dashBoardViewModel);
            BindParkingPilotAndLaneKeepingFeatures(dashBoardViewModel);
            BindDebugFeatures();
            BindCarControls(dashBoardViewModel);
            BindShiftingAndSignaling(dashBoardViewModel);
        }

        private void BindShiftingAndSignaling(DashboardViewModel dashBoardViewModel)
        {
            keyboardHandler.PressableKeys.Add(new PressableKey(Key.M, () => dashBoardViewModel.ToggleRightIndicator()));
            keyboardHandler.PressableKeys.Add(new PressableKey(Key.N, () => dashBoardViewModel.ToggleLeftIndicator()));
            keyboardHandler.PressableKeys.Add(new PressableKey(Key.G, () => dashBoardViewModel.ShiftUp()));
            keyboardHandler.PressableKeys.Add(new PressableKey(Key.F, () => dashBoardViewModel.ShiftDown()));
        }

        private void BindParkingPilotAndLaneKeepingFeatures(DashboardViewModel dashBoardViewModel)
        {
            keyboardHandler.PressableKeys.Add(new PressableKey(Key.L, () => dashBoardViewModel.ToggleLaneKeeping()));
            keyboardHandler.PressableKeys.Add(new PressableKey(Key.P, () => dashBoardViewModel.ToggleParkingPilot()));
        }

        private void BindDebugFeatures()
        {
            keyboardHandler.PressableKeys.Add(new PressableKey(Key.D1, () => hmiDebug.OnDebugAction(1)));
            keyboardHandler.PressableKeys.Add(new PressableKey(Key.D2, () => hmiDebug.OnDebugAction(2)));
            keyboardHandler.PressableKeys.Add(new PressableKey(Key.D3, () => hmiDebug.OnDebugAction(3)));
            keyboardHandler.PressableKeys.Add(new PressableKey(Key.D4, () => hmiDebug.OnDebugAction(4)));
        }

        private void BindACCFeatures(DashboardViewModel dashBoardViewModel)
        {
            keyboardHandler.PressableKeys.Add(new PressableKey(Key.Add, () => dashBoardViewModel.IncreaseACCDesiredSpeed()));
            keyboardHandler.PressableKeys.Add(new PressableKey(Key.Subtract, () => dashBoardViewModel.DecreaseACCDesiredSpeed()));
            keyboardHandler.PressableKeys.Add(new PressableKey(Key.T, () => dashBoardViewModel.SetToNextACCDesiredDistance()));
            keyboardHandler.PressableKeys.Add(new PressableKey(Key.RightCtrl, () => dashBoardViewModel.ToggleACC()));
        }

        private void BindCarControls(DashboardViewModel dashBoardViewModel)
        {
            keyboardHandler.HoldableKeys.Add(new HoldableKey(Key.W, (duration) => dashBoardViewModel.MoveGasPedalDown(duration), (duration) => dashBoardViewModel.MoveGasPedalUp(duration)));
            keyboardHandler.HoldableKeys.Add(new HoldableKey(Key.S, (duration) => dashBoardViewModel.MoveBrakePedalDown(duration), (duration) => dashBoardViewModel.MoveBrakePedalUp(duration)));
            keyboardHandler.HoldableKeys.Add(new HoldableKey(Key.A, (duration) => dashBoardViewModel.SteerLeft(duration), (duration) => dashBoardViewModel.SteerRightToIdle(duration)));
            keyboardHandler.HoldableKeys.Add(new HoldableKey(Key.D, (duration) => dashBoardViewModel.SteerRight(duration), (duration) => dashBoardViewModel.SteerLeftToIdle(duration)));
        }

        private void onKeyDown(object sender, KeyEventArgs e)
        {
            keyboardHandler.OnKeyDown(e.Key);
        }
        private void onKeyUp(object sender, KeyEventArgs e)
        {
            keyboardHandler.OnKeyUp(e.Key);
        }

        private void logic(object sender, EventArgs e)
        {
            HandleMovement();
            CourseDisplay.InvalidateVisual();
        }

        private void HandleMovement()
        {
            keyboardHandler.Tick();
        }

    }
}
