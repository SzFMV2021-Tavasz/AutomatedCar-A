using AutomatedCar.Visualization;
using System.Windows;

namespace AutomatedCar.SystemComponents
{
    public class CameraSensor : SystemComponent,IDisplaySensor
    {

        private CameraPacket cameraPacket;

        public CameraSensor(VirtualFunctionBus virtualFunctionBus)
          : base(virtualFunctionBus)
        {
            this.virtualFunctionBus = virtualFunctionBus;
            virtualFunctionBus.RegisterComponent(this);

            this.cameraPacket = new CameraPacket();
            virtualFunctionBus.CameraPacket = this.cameraPacket;
        }

        public Point x1 { get; set; }
        public Point x2 { get; set; }
        public Point x3 { get; set; }

        public override void Process()
        {
            throw new System.NotImplementedException();
        }
    }
}
