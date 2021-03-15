using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace BaseModel.Sensors
{
    public class CameraSensor : IDisplaySensor
    {
        public Point x1 { get ; set; }
        public Point x2 { get; set; }
        public Point x3 { get ; set; }
    }
}
