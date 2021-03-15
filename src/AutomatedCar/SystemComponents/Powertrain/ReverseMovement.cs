using System;
using System.Numerics;

namespace AutomatedCar.SystemComponents.Powertrain
{
    public class ReverseMovement
    {
        private float deltaTime;
        private VehicleTransform transform;

        public float Braking { get; set; }
        public float Accelerator { get; set; }
        public Vector2 SteeringWheel { get; set; }
        public VehicleTransform NextTransform { get => CalculateNextTransform(); }

        public void Reset(VehicleTransform currentTransform, float deltaTime)
        {
            this.transform = currentTransform;
            this.deltaTime = deltaTime;
        }

        private VehicleTransform CalculateNextTransform()
        {
            var movementDirection = Vector2.Normalize(-SteeringWheel);
            var steeringWheelAngle = Math.Acos(SteeringWheel.X);
            var rotationDirection = steeringWheelAngle - transform.AngularDisplacement;

            var nextVelocity = (transform.Velocity + Accelerator * deltaTime * movementDirection) * (1f - Braking);
            var nextPosition = transform.Position + deltaTime * nextVelocity;

            var nextOrientation = transform.AngularDisplacement;
            if (nextVelocity.Length() > 0)
            {
                nextOrientation = (float)(transform.AngularDisplacement + deltaTime * rotationDirection);
            }

            return new VehicleTransform(nextPosition, nextOrientation, nextVelocity, 0);
        }
    }
}
