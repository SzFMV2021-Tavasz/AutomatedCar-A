using System.Windows;

namespace AutomatedCar.Visualization
{
    /*
     * Sensor triangle points.  
     */
    public interface IDisplaySensor
    {
        public Point x1 { get; set; }
        public Point x2 { get; set; }
        public Point x3 { get; set; }
    }
}
