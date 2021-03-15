using System;
using System.Collections.Generic;
using System.Windows;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaseModel.Sensors
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
