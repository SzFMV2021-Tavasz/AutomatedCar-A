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

        public Vector2 GetTractiveForceInReverse(float gasPedal, Vector2 wheelDirection)
        {
            var gearRatio = -vehicleConstants.ReverseGearRatio;
            return CalculateTractiveForce(gasPedal, wheelDirection, gearRatio);
        }

        public Vector2 GetTractiveForce(float gasPedal, Vector2 wheelDirection, int gearIdx)
        {
            if(gearIdx < 0 || vehicleConstants.NumberOfGears <= gearIdx)
            {
                throw new ArgumentException("Gear index is out of bounds.", nameof(gearIdx));
            }

            var gearRatio = vehicleConstants.GearRatios[gearIdx];
            return CalculateTractiveForce(gasPedal, wheelDirection, gearRatio);
        }

        private Vector2 CalculateTractiveForce(float gasPedal, Vector2 wheelDirection, float gearRatio)
        {
            var engineTorque = vehicleConstants.GetEngineTorque(vehicleConstants.GetCrankshaftSpeed(gasPedal));
            var differentialRatio = vehicleConstants.DifferentialRatio;
            var transmissionEfficiency = vehicleConstants.TransmissionEfficiency;
            var wheelRadius = vehicleConstants.OverallWheelRadius;

            return wheelDirection * engineTorque * gearRatio * differentialRatio * transmissionEfficiency / wheelRadius;
        }
    }
}
