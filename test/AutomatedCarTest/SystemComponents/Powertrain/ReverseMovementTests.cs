using AutomatedCar.SystemComponents.Powertrain;
using System;
using System.Numerics;
using NUnit.Framework;

namespace AutomatedCarTest.SystemComponents.Powertrain
{
    public class ReverseMovementTests
    {
        private readonly ReverseMovement reverseMovement = new ReverseMovement();

        [Test]
        public void IdentityProperty()
        {
            var deltaTime = 1f;
            var initPosition = Vector2.Zero;
            var initOrientation = 0f;
            var initVelocity = Vector2.Zero;
            var initAngularVelocity = 0f;
            var transform = new VehicleTransform(initPosition, initOrientation, initVelocity, initAngularVelocity);

            reverseMovement.Reset(transform, deltaTime);
            reverseMovement.Accelerator = 0f;
            reverseMovement.Braking = 0f;
            reverseMovement.SteeringWheel = initOrientation.MakeUnitVectorFromRadians();

            var nextTransform = reverseMovement.NextTransform;

            Assert.AreEqual(transform, nextTransform);
        }

        [Test]
        public void MovementDirection()
        {
            var deltaTime = 1f;
            var initPosition = Vector2.Zero;
            var initOrientation = 0f;
            var initVelocity = Vector2.Zero;
            var initAngularVelocity = 0f;
            var transform = new VehicleTransform(initPosition, initOrientation, initVelocity, initAngularVelocity);

            reverseMovement.Reset(transform, deltaTime);
            reverseMovement.Accelerator = 1f;
            reverseMovement.Braking = 0f;
            reverseMovement.SteeringWheel = initOrientation.MakeUnitVectorFromRadians();

            var nextTransform = reverseMovement.NextTransform;

            var expectedMovementDirection = ((float)(initOrientation - Math.PI)).MakeUnitVectorFromRadians();
            var movementDirection = Vector2.Normalize(nextTransform.Velocity);
            var cosineOfAngle = Vector2.Dot(expectedMovementDirection, movementDirection);

            Assert.AreEqual(1f, cosineOfAngle, 0.005f);
        }

        [Test]
        public void ChangeOfOrientation()
        {
            var deltaTime = 1f;
            var initPosition = Vector2.Zero;
            var initOrientation = 0f;
            var initVelocity = Vector2.Zero;
            var initAngularVelocity = 0f;
            var transform = new VehicleTransform(initPosition, initOrientation, initVelocity, initAngularVelocity);
            var steeringWheelAngle = 45.0;

            reverseMovement.Reset(transform, deltaTime);
            reverseMovement.Accelerator = 1f;
            reverseMovement.Braking = 0f;
            reverseMovement.SteeringWheel = steeringWheelAngle.DegreesToRadians().MakeUnitVectorFromRadians();

            var nextTransform = reverseMovement.NextTransform;

            var changeOfOrientation = nextTransform.AngularDisplacement - transform.AngularDisplacement;
            // changeOfOrientation and steeringWheelAngle have the same sign, that is
            // they point in the same direction
            Assert.GreaterOrEqual(changeOfOrientation * steeringWheelAngle, 0);
        }

        [Test]
        public void VelocityIsIntegrated()
        {
            var deltaTime = 1f;
            var initPosition = Vector2.Zero;
            var initOrientation = 0f;
            var initVelocity = new Vector2(10, -20);
            var initAngularVelocity = 0f;
            var transform = new VehicleTransform(initPosition, initOrientation, initVelocity, initAngularVelocity);

            reverseMovement.Reset(transform, deltaTime);
            reverseMovement.Accelerator = 0f;
            reverseMovement.Braking = 0f;
            reverseMovement.SteeringWheel = Vector2.UnitX;

            var nextTransform = reverseMovement.NextTransform;

            Assert.AreEqual(nextTransform.Position, deltaTime * initVelocity);
        }
    }
}
