using System;
using System.Linq;
using System.Collections.Generic;
using System.Numerics;

namespace AutomatedCar.SystemComponents.Powertrain
{
    public class Integrator : IIntegrator
    {
        private readonly IVehicleConstants vehicleConstants;
        private readonly Dictionary<WheelKind, ParticleIntegrator> particles;
        private float? deltaTime = null;
        private VehicleTransform? currentTransform = null;

        public VehicleTransform NextVehicleTransform { get => CalculateNextVehicleTransform(); }

        public Integrator(IVehicleConstants vehicleConstants)
        {
            this.vehicleConstants = vehicleConstants;

            particles = new Dictionary<WheelKind, ParticleIntegrator>();
        }

        public void Reset(VehicleTransform vehicleTransform, float deltaTime)
        {
            var wheelKinds = Enum.GetValues<WheelKind>();
            var perParticleMass = vehicleConstants.CurbWeight / wheelKinds.Length;

            foreach (var wheelKind in wheelKinds)
            {
                var position = CalculatePositionOfWheel(wheelKind, vehicleTransform.Position, vehicleTransform.AngularDisplacement);
                var velocity = vehicleTransform.Velocity;
                particles[wheelKind] = new ParticleIntegrator(position, velocity, perParticleMass, deltaTime);
            }

            this.currentTransform = vehicleTransform;
            this.deltaTime = deltaTime;
        }

        public void AccumulateForce(WheelKind wheel, Vector2 force)
        {
            particles[wheel].AccumulateForce(force);
        }

        private Vector2 CalculatePositionOfWheel(WheelKind wheel, Vector2 carPosition, float carHeading)
        {
            float multiplier;
            switch (wheel)
            {
                case WheelKind.Front:
                    multiplier = +1;
                    break;
                case WheelKind.Back:
                    multiplier = -1;
                    break;
                default:
                    throw new ArgumentException($"{nameof(wheel)} is out-of-range or wasn't handled.");
            }

            var distanceFromCenterOfMass = vehicleConstants.WheelBase / 2;
            var direction = GetDirectionVector(carHeading);
            var particlePosition = carPosition + multiplier * distanceFromCenterOfMass * direction;

            return particlePosition;
        }

        private Vector2 GetDirectionVector(float carHeading)
        {
            var x = (float)Math.Cos(carHeading);
            var y = (float)Math.Sin(carHeading);

            return new Vector2(x, y);
        }

        private VehicleTransform CalculateNextVehicleTransform()
        {
            var distanceFromCenterOfMass = vehicleConstants.WheelBase / 2;

            var (positionSum, velocitySum, massSum) = particles.Values.Aggregate(
                (PositionSum: Vector2.Zero, VelocitySum: Vector2.Zero, MassSum: 0f),
                (acc, P) =>
                {
                    var (x, v) = P.NextState;
                    var m = P.Mass;
                    return (acc.PositionSum + m * x, acc.VelocitySum + m * v, acc.MassSum + m);
                }
            );

            var centerOfMass = positionSum / massSum;

            var angularAccelerationSum = particles.Values.Aggregate(
                0f,
                (acc, P) =>
                {
                    var (position, velocity) = P.NextState;
                    var m = P.Mass;
                    var F = P.AccumulatedForce;

                    // No force acting on this particle
                    if (F.Length() < float.Epsilon)
                    {
                        return 0f;
                    }

                    var relativePosition = centerOfMass - position;
                    var torque = CalculateTorqueProducedByForceAtPosition(relativePosition, F);

                    var inertia = m * (distanceFromCenterOfMass * distanceFromCenterOfMass);

                    var angularAcceleration = torque / inertia;
                    return angularAcceleration;
                }
            );

            var steerability = CalculateSteerability(currentTransform.Velocity);

            var nextAngularVelocity = currentTransform.AngularVelocity + steerability * deltaTime.Value * angularAccelerationSum;
            var nextAngularDisplacement = currentTransform.AngularDisplacement + deltaTime.Value * nextAngularVelocity;

            var nextPosition = centerOfMass;
            var nextVelocity = velocitySum / massSum;

            return new VehicleTransform(nextPosition, nextAngularDisplacement, nextVelocity, nextAngularVelocity);
        }

        private float CalculateTorqueProducedByForceAtPosition(Vector2 relativePosition, Vector2 force)
        {
            // Magnitude of torque produced by the force at the particle's position is
            //
            // torque = [magnitude of relativePosition] * [magnitude of force] * sin(theta)
            //
            // where theta is the angle between the two vectors.
            // sin(theta) can be calculated by
            //
            // sin(theta) = sqrt(1 - cos**2(theta))
            //
            // and cos(theta) is simply the dot product of the two vectors
            // after normalization.

            var relativePositionMagnitude = relativePosition.Length();
            var relativePositionNormalized = relativePosition / relativePositionMagnitude;
            var forceMagnitude = force.Length();
            var forceNormalized = force / forceMagnitude;

            var cosineOfAngle = Vector2.Dot(relativePositionNormalized, forceNormalized);
            var sineOfAngle = Math.Sqrt(1 - cosineOfAngle * cosineOfAngle);

            var torque = relativePositionMagnitude * forceMagnitude * sineOfAngle;

            return (float)torque;
        }

        private float CalculateSteerability(Vector2 vehicleVelocity)
        {
            // Steerability is a function of speed:
            // s = 1 - (v / v_max) ** 2
            // Initially it's close to a 100%, but starts to fall off at high
            // velocities (non-linearly).
            var maxVelocity = 128;
            return (float)(1.0 - Math.Pow(vehicleVelocity.Length() / maxVelocity, 2));
        }
    }
}
