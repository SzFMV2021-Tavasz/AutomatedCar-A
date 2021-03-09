using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutomatedCar.SystemComponents.Powertrain
{
    public interface ICarUpdater
    {
        IVirtualFunctionBus VirtualFunctionBus { get; }
        IVehicleForces VehicleForces { get; }
        IIntegrator Integrator { get; }
        void Calculate();
        void UpdateWorldObject();
        void UpdatePacket();
        void SetCurrentTransform();
    }
}
