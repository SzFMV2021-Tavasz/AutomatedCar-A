using AutomatedCar.SystemComponents.Packets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutomatedCar.SystemComponents.Powertrain
{
    public class PowertrainComponent : SystemComponent
    {
        private PowertrainComponentPacket powertrainPacket;

        public PowertrainComponent(IVirtualFunctionBus virtualFunctionBus)
           : base(virtualFunctionBus)
        {
            this.powertrainPacket = new PowertrainComponentPacket();
            virtualFunctionBus.PowertrainPacket = this.powertrainPacket;
        }

        public override void Process()
        {
        }
    }
}
