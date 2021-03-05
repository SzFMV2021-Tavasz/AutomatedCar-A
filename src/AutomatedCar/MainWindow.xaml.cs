using System;
using System.Windows;
using System.Windows.Input;
using System.Windows.Shapes;

using System.Windows.Threading;
using AutomatedCar.Models;
using AutomatedCar.ViewModels;
using Newtonsoft.Json.Linq;
using AutomatedCar.KeyboardHandling;

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

        public MainWindow()
        {
            ViewModel = new MainWindowViewModel(world);
            InitializeComponent();

            keyboardHandler = new KeyboardHandler(tickInterval);
            keyboardHandler.HoldableKeys.Add(new HoldableKey(Key.Left, (duration) => World.Instance.ControlledCar.X -= 5, null));
            keyboardHandler.HoldableKeys.Add(new HoldableKey(Key.Right, (duration) => World.Instance.ControlledCar.X += 5, null));
            keyboardHandler.HoldableKeys.Add(new HoldableKey(Key.Up, (duration) => World.Instance.ControlledCar.Y -= 5, null));
            keyboardHandler.HoldableKeys.Add(new HoldableKey(Key.Down, (duration) => World.Instance.ControlledCar.Y += 5, null));

            timer.Interval = TimeSpan.FromMilliseconds(tickInterval);
            timer.Tick += logic;
            timer.Start();
            // make my dockpanel focus of this game
            MainDockPanel.Focus();


            world.Width = 2000;
            world.Height = 1000;

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
            keyboardHandler.Tick();
        }

    }
}
