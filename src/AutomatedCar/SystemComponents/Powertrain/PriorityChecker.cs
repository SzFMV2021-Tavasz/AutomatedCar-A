using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutomatedCar.SystemComponents.Powertrain
{
    class PriorityChecker : IPriorityChecker
    {
        public IVirtualFunctionBus virtualFunctionBus { get; set; }

        public IVehicleForces vehicleForces { get; set; }

        public void AccelerationPriorityCheck()
        {
        }

        public void SteeringPriorityCheck()
        {
        }
    }
}
