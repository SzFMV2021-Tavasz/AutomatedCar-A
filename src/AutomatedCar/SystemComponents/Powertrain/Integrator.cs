using System;
using System.Linq;
using System.Collections.Generic;
using System.Numerics;
using AutomatedCar.Models.Enums;

namespace AutomatedCar.SystemComponents.Powertrain
{
    public class Integrator : IIntegrator
    {
        private readonly IVehicleConstants vehicleConstants;
        private readonly Dictionary<WheelKind, ParticleIntegrator> particles;
        private float? deltaTime = null;
        private VehicleTransform? currentTransform = null;
        private bool isParkingMode = false;

        public VehicleTransform NextVehicleTransform { get => CalculateNextVehicleTransform(); }

        public Integrator(IVehicleConstants vehicleConstants)
        {
            this.vehicleConstants = vehicleConstants;

            particles = new Dictionary<WheelKind, ParticleIntegrator>();
        }

        public void Reset(VehicleTransform vehicleTransform, float deltaTime, Gear currentGear)
        {
            var wheelKinds = Enum.GetValues<WheelKind>();
            var perParticleMass = vehicleConstants.CurbWeight / wheelKinds.Length;

            foreach (var wheelKind in wheelKinds)
            {
                var position = CalculatePositionOfWheel(wheelKind, vehicleTransform.Position, vehicleTransform.AngularDisplacement);
                var velocity = vehicleTransform.Velocity;
                particles[wheelKind] = new ParticleIntegrator(position, velocity, perParticleMass, deltaTime, wheelKind);
            }

            this.currentTransform = vehicleTransform;
            this.deltaTime = deltaTime;

            this.isParkingMode = currentGear == Gear.P;
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
            var direction = carHeading.MakeUnitVectorFromRadians();
            var particlePosition = carPosition + (multiplier * distanceFromCenterOfMass * direction);

            return particlePosition;
        }

        private VehicleTransform CalculateNextVehicleTransform()
        {
            if(isParkingMode)
            {
                return currentTransform with { Velocity = Vector2.Zero, AngularVelocity = 0f };
            }

            var distanceFromCenterOfMass = vehicleConstants.WheelBase / 2;
            var particleStates = particles.Values.Select(x => (x.WheelKind, x.NextState, x.Mass, x.AccumulatedForce)).ToArray();

            var (positionSum, velocitySum, massSum) = particleStates.Aggregate(
                (PositionSum: Vector2.Zero, VelocitySum: Vector2.Zero, MassSum: 0f),
                (acc, P) =>
                {
                    var (x, v) = P.NextState;
                    var m = P.Mass;
                    return (acc.PositionSum + (m * x), acc.VelocitySum + (m * v), acc.MassSum + m);
                }
            );

            var centerOfMass = positionSum / massSum;

            var angularAccelerationSum = particleStates.Aggregate(
                0f,
                (acc, P) =>
                {
                    var (position, velocity) = P.NextState;
                    var m = P.Mass;
                    var F = P.AccumulatedForce;

                    // No force acting on this particle
                    if (F.Length() < float.Epsilon)
                    {
                        return acc;
                    }

                    var relativePosition = centerOfMass - position;
                    var torque = CalculateTorqueProducedByForceAtPosition(relativePosition, F);

                    var inertia = m * (distanceFromCenterOfMass * distanceFromCenterOfMass);

                    var angularAcceleration = torque / inertia;
                    return acc + angularAcceleration;
                }
            );

            // HACK: some angular acceleration that slows down the angular velocity in the absence of torque
            angularAccelerationSum += 32 * -(currentTransform.AngularVelocity * currentTransform.AngularVelocity);

            var steerability = CalculateSteerability(currentTransform.Velocity);

            var nextAngularVelocity = currentTransform.AngularVelocity + (steerability * deltaTime.Value * angularAccelerationSum);
            var nextAngularDisplacement = currentTransform.AngularDisplacement + (deltaTime.Value * nextAngularVelocity);

            var nextPosition = centerOfMass;
            var nextVelocity = velocitySum / massSum;

            // HACK: force heading to point in the direction of movement
            if(nextVelocity.Length() > float.Epsilon)
            {
                var nextVelocityNormalized = Vector2.Normalize(nextVelocity);
                nextAngularDisplacement = (float)Math.Atan2(nextVelocityNormalized.Y, nextVelocityNormalized.X);
            }

            return new VehicleTransform(nextPosition, nextAngularDisplacement, nextVelocity, 0);
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
            // sin(theta) = sin(acos[dot(normalized[relativePosition], normalized[force])])
            //
            // as the dot product of the two vectors is the cosine of the
            // angle between them.

            var relativePositionMagnitude = relativePosition.Length();
            var relativePositionNormalized = Vector2.Normalize(relativePosition);
            var forceMagnitude = force.Length();
            var forceNormalized = Vector2.Normalize(force);

            var cosineOfAngle = Vector2.Dot(relativePositionNormalized, forceNormalized);
            // Vector2.Dot sometimes returns values greater than 1, even though
            // the vectors are of unit length.
            // This clamp guards against that.
            var cosineOfAngleSafe = Math.Clamp(cosineOfAngle, -1, 1);

            var angle = Math.Acos(cosineOfAngleSafe);
            var sineOfAngle = Math.Sin(angle);

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
