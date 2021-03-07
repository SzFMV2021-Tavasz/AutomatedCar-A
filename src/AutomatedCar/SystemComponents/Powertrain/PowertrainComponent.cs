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
        private IPriorityChecker priorityChecker;
        private IVehicleForces vehicleForcers;

        public PowertrainComponent(IVirtualFunctionBus virtualFunctionBus, IPriorityChecker priorityChecker, IVehicleForces vehicleForces)
           : base(virtualFunctionBus)
        {
            this.powertrainPacket = new PowertrainComponentPacket();
            virtualFunctionBus.PowertrainPacket = this.powertrainPacket;
            this.vehicleForcers = vehicleForces;

            this.priorityChecker = priorityChecker;
            this.priorityChecker.virtualFunctionBus = virtualFunctionBus;
            this.priorityChecker.vehicleForces = vehicleForces;
        }

        public override void Process()
        {
            priorityChecker.AccelerationPriorityCheck();
            priorityChecker.SteeringPriorityCheck();
        }
    }
}
