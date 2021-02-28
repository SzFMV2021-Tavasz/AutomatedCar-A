using System;
using System.Numerics;

namespace AutomatedCar.SystemComponents.Powertrain
{
    public class VehicleForces
    {
        private readonly IVehicleConstants vehicleConstants;
        private readonly float epsilon = 0.001f;

        public VehicleForces(IVehicleConstants vehicleConstants)
        {
            this.vehicleConstants = vehicleConstants;
        }

        /// <summary>
        /// Calculates the drag force acting on a particle.
        /// </summary>
        /// <param name="velocity">Speed of the particle.</param>
        /// <returns>Drag force.</returns>
        public Vector2 GetDragForce(Vector2 velocity)
        {
            var relativeVelocity = Vector2.Zero - velocity;

            // If the relative velocity is (near) zero we pretend that there is
            // no drag force present, since a zero vector cannot be normalized.
            if (relativeVelocity.LengthSquared() < epsilon)
            {
                return Vector2.Zero;
            }

            var forceDir = Vector2.Normalize(relativeVelocity);
            var velocitySquared = relativeVelocity.LengthSquared();
            var dragArea = vehicleConstants.DragCoefficient * vehicleConstants.CrossSectionalArea;

            return 0.5f * vehicleConstants.AirDensity * velocitySquared * dragArea * forceDir;
        }
    }
}

