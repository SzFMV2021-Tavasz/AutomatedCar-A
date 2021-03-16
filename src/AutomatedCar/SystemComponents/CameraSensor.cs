using AutomatedCar.Visualization;
using System.Windows;

namespace AutomatedCar.SystemComponents
{
    public class CameraSensor : IDisplaySensor
   {
        public Point x1 { get; set; }
        public Point x2 { get; set; }
        public Point x3 { get; set; }
    }
}
