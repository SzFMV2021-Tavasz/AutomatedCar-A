using System;
using System.Numerics;

namespace AutomatedCar.SystemComponents.Powertrain
{
    public class VehicleForces
    {
        private IVehicleConstants vehicleConstants;

        public VehicleForces(IVehicleConstants vehicleConstants)
        {
            this.vehicleConstants = vehicleConstants;
        }

        /// <summary>
        /// Calculates the drag force acting on a particle.
        /// </summary>
        /// <param name="wheelRotation">Rotation of the wheel in the counter-clockwise direction, in radians.</param>
        /// <param name="v">Speed of the particle.</param>
        /// <returns>Drag force.</returns>
        public Vector2 GetDragForce(float wheelRotation, Vector2 v)
        {
            throw new NotImplementedException();
        }
    }
}

