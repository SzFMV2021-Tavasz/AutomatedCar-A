using System;
using System.Numerics;

namespace AutomatedCar.SystemComponents.Powertrain
{
    public class VehicleForces
    {
        private readonly IVehicleConstants vehicleConstants;

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
            if (relativeVelocity.LengthSquared() < float.Epsilon)
            {
                return Vector2.Zero;
            }

            var forceDir = Vector2.Normalize(relativeVelocity);
            var velocitySquared = relativeVelocity.LengthSquared();
            var dragArea = vehicleConstants.DragCoefficient * vehicleConstants.CrossSectionalArea;

            return 0.5f * vehicleConstants.AirDensity * velocitySquared * dragArea * forceDir;
        }

        public Vector2 GetTractiveForce(float gasPedal, Vector2 wheelDirection, bool isReverse, int? gearIdx)
        {
            if(!isReverse && gearIdx == null)
            {
                throw new ArgumentException($"{nameof(gearIdx)} can't be null when {nameof(isReverse)} is false", nameof(gearIdx));
            }

            if(gearIdx != null && gearIdx >= vehicleConstants.NumberOfGears)
            {
                throw new ArgumentException("Gear index is out of bounds.", nameof(gearIdx));
            }

            float gearRatio;
            if(!isReverse)
            {
                gearRatio = vehicleConstants.GearRatios[gearIdx.Value];
            }
            else
            {
                gearRatio = -vehicleConstants.ReverseGearRatio;
            }

            var engineTorque = vehicleConstants.GetEngineTorque(vehicleConstants.GetCrankshaftSpeed(gasPedal));
            var differentialRatio = vehicleConstants.DifferentialRatio;
            var transmissionEfficiency = vehicleConstants.TransmissionEfficiency;
            var wheelRadius = vehicleConstants.OverallWheelRadius;

            return wheelDirection * engineTorque * gearRatio * differentialRatio * transmissionEfficiency / wheelRadius;
        }
    }
}
