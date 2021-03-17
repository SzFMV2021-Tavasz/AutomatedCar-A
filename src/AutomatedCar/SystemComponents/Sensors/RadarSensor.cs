using AutomatedCar.Visualization;
using System.Windows;

namespace AutomatedCar.SystemComponents.Sensores
{
    public class RadarSensor : SystemComponent,IDisplaySensor
    {

        private RadarPacket radarPacket;

        public RadarSensor(VirtualFunctionBus virtualFunctionBus)
          : base(virtualFunctionBus)
        {
            this.virtualFunctionBus = virtualFunctionBus;
            virtualFunctionBus.RegisterComponent(this);

            this.radarPacket = new RadarPacket();
            virtualFunctionBus.RadarPacket = this.radarPacket;

            x1 = new Point(54, 120);
            x2 = new Point(0, -300);
            x3 = new Point(108, -300);
        }

        public Point x1 { get; set; }
        public Point x2 { get; set; }
        public Point x3 { get; set; }

        public override void Process()
        {
            //throw new System.NotImplementedException();
        }
    }
}
