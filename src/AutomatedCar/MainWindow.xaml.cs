using AutomatedCar.KeyboardHandling;
using AutomatedCar.Models;
using AutomatedCar.SystemComponents.SystemDebug;
using AutomatedCar.ViewModels;
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
        private readonly double TickInterval = 20;
        private readonly Key ControlsInfoKey = Key.F1;

        public MainWindowViewModel ViewModel { get; set; }

        DispatcherTimer timer = new DispatcherTimer();
        World world = World.Instance;

        private KeyboardHandler keyboardHandler;
        private ControlsDisplayer controlsDisplayer;
        private HMIDebug hmiDebug;

        public MainWindow()
        {
            ViewModel = new MainWindowViewModel(world);
                      
            InitializeComponent();
           
            hmiDebug = new HMIDebug();
            keyboardHandler = new KeyboardHandler(TickInterval);
            timer.Interval = TimeSpan.FromMilliseconds(TickInterval);
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

            var video = new Polyline();
            video.Points.Add(new Point(54,120));
            video.Points.Add(new Point(0,-120));
            video.Points.Add(new Point(108, -120));
            controlledCar.Video = video;

            var radar = new Polyline();
            radar.Points.Add(new Point(54, 120));
            radar.Points.Add(new Point(0, -300));
            radar.Points.Add(new Point(108, -300));
            controlledCar.Radar = radar;

            var sonic = new Polyline();
            sonic.Points.Add(new Point(6, 20));
            sonic.Points.Add(new Point(-144, -159));
            sonic.Points.Add(new Point(-144, 199));
            controlledCar.UltraSonic = sonic;

            world.AddObject(controlledCar);
            world.ControlledCar = controlledCar;
            controlledCar.Start();
            var dashBoardViewModel = new DashboardViewModel(world.ControlledCar, ControlsInfoKey.ToString());
            ViewModel.Dashboard = dashBoardViewModel;
            BindKeysForDashboardFunctions(dashBoardViewModel);
            timer.Tick += dashBoardViewModel.HandlePackets;

            List<InputKey> inputKeys = new List<InputKey>();
            inputKeys.AddRange(keyboardHandler.HoldableKeys);
            inputKeys.AddRange(keyboardHandler.PressableKeys);
            controlsDisplayer = new ControlsDisplayer(inputKeys);
        }

        private void BindKeysForDashboardFunctions(DashboardViewModel dashBoardViewModel)
        {
            BindCarControls(dashBoardViewModel);
            BindShiftingAndSignaling(dashBoardViewModel);
            BindACCFeatures(dashBoardViewModel);
            BindParkingPilotAndLaneKeepingFeatures(dashBoardViewModel);
            BindDebugFeatures();
            BindControlsDisplay();
        }

        private void BindShiftingAndSignaling(DashboardViewModel dashBoardViewModel)
        {
            keyboardHandler.PressableKeys.Add(new PressableKey(Key.M, "Indicator", "Toggle Right Indicator", () => dashBoardViewModel.ToggleRightIndicator()));
            keyboardHandler.PressableKeys.Add(new PressableKey(Key.N, "Indicator", "Toggle Left Indicator", () => dashBoardViewModel.ToggleLeftIndicator()));
            keyboardHandler.PressableKeys.Add(new PressableKey(Key.G, "Gear", "Shift Gear Up", () => dashBoardViewModel.ShiftUp()));
            keyboardHandler.PressableKeys.Add(new PressableKey(Key.F, "Gear", "Shift Gear Down", () => dashBoardViewModel.ShiftDown()));
        }

        private void BindParkingPilotAndLaneKeepingFeatures(DashboardViewModel dashBoardViewModel)
        {
            keyboardHandler.PressableKeys.Add(new PressableKey(Key.L, "Driving Assistance Extras", "Toggle Lane Keeping", () => dashBoardViewModel.ToggleLaneKeeping()));
            keyboardHandler.PressableKeys.Add(new PressableKey(Key.P, "Driving Assistance Extras", "Toggle Parking Pilot", () => dashBoardViewModel.ToggleParkingPilot()));
        }

        private void BindDebugFeatures()
        {
            keyboardHandler.PressableKeys.Add(new PressableKey(Key.D1, "Debug Actions", "Toggle Debug Action 1", () => hmiDebug.OnDebugAction(1)));
            keyboardHandler.PressableKeys.Add(new PressableKey(Key.D2, "Debug Actions", "Toggle Debug Action 2", () => hmiDebug.OnDebugAction(2)));
            keyboardHandler.PressableKeys.Add(new PressableKey(Key.D3, "Debug Actions", "Toggle Debug Action 3", () => hmiDebug.OnDebugAction(3)));
            keyboardHandler.PressableKeys.Add(new PressableKey(Key.D4, "Debug Actions", "Toggle Debug Action 4", () => hmiDebug.OnDebugAction(4)));
        }

        private void BindACCFeatures(DashboardViewModel dashBoardViewModel)
        {
            keyboardHandler.PressableKeys.Add(new PressableKey(Key.RightCtrl, "ACC", "Toggle ACC", () => dashBoardViewModel.ToggleACC()));
            keyboardHandler.PressableKeys.Add(new PressableKey(Key.Add, "ACC", "Increase ACC Desired Speed", () => dashBoardViewModel.IncreaseACCDesiredSpeed()));
            keyboardHandler.PressableKeys.Add(new PressableKey(Key.Subtract, "ACC", "Decrease ACC Desired Speed", () => dashBoardViewModel.DecreaseACCDesiredSpeed()));
            keyboardHandler.PressableKeys.Add(new PressableKey(Key.T, "ACC", "Set To Next ACC Desired Distance", () => dashBoardViewModel.SetToNextACCDesiredDistance()));
        }

        private void BindCarControls(DashboardViewModel dashBoardViewModel)
        {
            keyboardHandler.HoldableKeys.Add(new HoldableKey(Key.W, "Car Controls", "Gas Pedal", (duration) => dashBoardViewModel.MoveGasPedalDown(duration), (duration) => dashBoardViewModel.MoveGasPedalUp(duration)));
            keyboardHandler.HoldableKeys.Add(new HoldableKey(Key.S, "Car Controls", "Brake Pedal", (duration) => dashBoardViewModel.MoveBrakePedalDown(duration), (duration) => dashBoardViewModel.MoveBrakePedalUp(duration)));
            keyboardHandler.HoldableKeys.Add(new HoldableKey(Key.A, "Car Controls", "Steer Left", (duration) => dashBoardViewModel.SteerLeft(duration), (duration) => dashBoardViewModel.SteerRightToIdle(duration)));
            keyboardHandler.HoldableKeys.Add(new HoldableKey(Key.D, "Car Controls", "Steer Right", (duration) => dashBoardViewModel.SteerRight(duration), (duration) => dashBoardViewModel.SteerLeftToIdle(duration)));
        }

        private void BindControlsDisplay()
        {
            keyboardHandler.PressableKeys.Add(new PressableKey(ControlsInfoKey, "Other", "Display Controls", () =>
            {
                onFocusLost();
                controlsDisplayer.DisplayControls();
            }));
        }

        private void onKeyDown(object sender, KeyEventArgs e)
        {
            keyboardHandler.OnKeyDown(e.Key);
        }
        private void onKeyUp(object sender, KeyEventArgs e)
        {
            keyboardHandler.OnKeyUp(e.Key);
        }

        private void onFocusLost()
        {
            keyboardHandler.HoldableKeys.ForEach(holdableKey => keyboardHandler.OnKeyUp(holdableKey.Key));
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
