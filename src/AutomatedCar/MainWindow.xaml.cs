using AutomatedCar.KeyboardHandling;
using AutomatedCar.Models;
using AutomatedCar.SystemComponents.SystemDebug;
using AutomatedCar.ViewModels;
using Newtonsoft.Json.Linq;
using System;
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
            var dashBoardViewModel = new DashboardViewModel(world.ControlledCar);
            ViewModel.Dashboard = dashBoardViewModel;
                       
            InitializeComponent();
           
            hmiDebug = new HMIDebug();
            keyboardHandler = new KeyboardHandler(tickInterval);
            BindKeysForDashboardFunctions(dashBoardViewModel);

            timer.Interval = TimeSpan.FromMilliseconds(tickInterval);
            timer.Tick += logic;
            timer.Tick += dashBoardViewModel.HandlePackets;
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

            world.AddObject(controlledCar);
            world.ControlledCar = controlledCar;
            controlledCar.Start();
        }

        private void BindKeysForDashboardFunctions(DashboardViewModel dashBoardViewModel)
        {
            BindACCFeatures(dashBoardViewModel);
            BindParkingPilotAndLaneKeepingFeatures(dashBoardViewModel);
            BindDebugFeatures();
            BindCarControls(dashBoardViewModel);
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
