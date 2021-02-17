using System.Diagnostics;
using System.Security.AccessControl;
using System.Runtime.CompilerServices;
using System.Globalization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

using System.Windows.Threading;
using AutomatedCar.Models;
using AutomatedCar.ViewModels;

namespace AutomatedCar
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindowViewModel ViewModel { get; set; }
        DispatcherTimer timer = new DispatcherTimer();
        World world = World.Instance;
        bool moveLeft, moveRight, moveUp, moveDown;
        
        public MainWindow()
        {
            ViewModel = new MainWindowViewModel(world);
            InitializeComponent();

            timer.Interval = TimeSpan.FromMilliseconds(20);
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
            // controlledCar.Geometry = geom;
            world.AddObject(controlledCar);
            world.ControlledCar = controlledCar;
            controlledCar.Start();
        }

        private void onKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Left)
            {
                moveLeft = true;
            }
            if (e.Key == Key.Right)
            {
                moveRight = true;
            }
             if (e.Key == Key.Up)
            {
                moveUp = true;
            }
            if (e.Key == Key.Down)
            {
                moveDown = true;
            }
        }
        private void onKeyUp(object sender, KeyEventArgs e)
        {
             if (e.Key == Key.Left)
            {
                moveLeft = false;
            }
            if (e.Key == Key.Right)
            {
                moveRight = false;
            }
            if (e.Key == Key.Up)
            {
                moveUp = false;
            }
            if (e.Key == Key.Down)
            {
                moveDown = false;
            }
        }

        private void logic(object sender, EventArgs e)
        {
            if (moveLeft)
            {
                World.Instance.ControlledCar.X -= 5;
            }
            if (moveRight)
            {
                World.Instance.ControlledCar.X += 5;
            }
            if (moveUp)
            {
                World.Instance.ControlledCar.Y -= 5;
            }
            if (moveDown)
            {
                World.Instance.ControlledCar.Y += 5;
            }
        }

    }
}
