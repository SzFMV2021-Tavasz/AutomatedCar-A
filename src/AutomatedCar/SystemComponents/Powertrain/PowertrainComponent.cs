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
        ICarUpdater CarUpdater;
        IVehicleForces VehicleForces;
        IVehicleConstants VehicleConstants;
        IIntegrator Integrator;

        public PowertrainComponent(IVirtualFunctionBus virtualFunctionBus, IVehicleForces vehicleForces, IVehicleConstants vehicleConstants,IIntegrator integrator)
           : base(virtualFunctionBus)
        {
            this.powertrainPacket = new PowertrainComponentPacket();
            virtualFunctionBus.PowertrainPacket = this.powertrainPacket;

            this.VehicleForces = vehicleForces;
            this.VehicleConstants = vehicleConstants;
            this.Integrator = integrator;
            var forceMultiplier = 8.0f;
            this.CarUpdater = new CarUpdater(this.virtualFunctionBus, this.VehicleForces, this.Integrator, powertrainPacket, this.VehicleConstants, forceMultiplier);
        }

        public override void Process()
        {
            CarUpdater.Calculate();
            CarUpdater.SetCurrentTransform();
            CarUpdater.UpdateWorldObject();
            CarUpdater.UpdatePacket();
        }
    }
}
