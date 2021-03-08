using AutomatedCar.SystemComponents.Powertrain;
using Moq;
using System;
using System.Collections.Generic;
using System.Numerics;
using Xunit;

namespace AutomatedCarTest.SystemComponents.Powertrain
{
    public class VehicleForcesTests
    {
        private readonly Mock<IVehicleConstants> constants = new Mock<IVehicleConstants>();

        public VehicleForcesTests()
        {
            constants.Setup(C => C.AirDensity).Returns(1.184f);
            constants.Setup(C => C.DragCoefficient).Returns(0.280f);
            constants.Setup(C => C.CrossSectionalArea).Returns(2.642f);
            constants.Setup(C => C.NumberOfGears).Returns(2);
            constants.Setup(C => C.GearRatios).Returns(new float[] { 1 / 5.25f, 1 / 3.03f });
            constants.Setup(C => C.ReverseGearRatio).Returns(1 / 4.01f);
            constants.Setup(C => C.OverallWheelRadius).Returns(1);
            constants.Setup(C => C.TransmissionEfficiency).Returns(1);
            constants.Setup(C => C.DifferentialRatio).Returns(1);
            constants.Setup(C => C.BrakingConstant).Returns(80);

            constants.Setup(C => C.GetCrankshaftSpeed(It.IsAny<float>()))
                .Returns((float p) => p * 6000);

            constants.Setup(C => C.GetEngineTorque(It.IsAny<float>()))
                .Returns((float rpm) => rpm * 100);
        }

        [Fact]
        public void DragIsZeroAtZeroVelocity()
        {
            var forceCalculator = new VehicleForces(constants.Object);
            var vel = Vector2.Zero;

            var force = forceCalculator.GetDragForce(vel);

            Assert.True(force.Length() < float.Epsilon);
        }

        public static IEnumerable<object[]> dragProportionalTestVectors =>
            new List<object[]>
            {
                new object[] { Vector2.Zero,        new Vector2(0, 28) },
                new object[] { new Vector2(0, 10),  new Vector2(0, 28) },
            };

        [Theory]
        [MemberData(nameof(dragProportionalTestVectors))]
        public void DragIsProportionalToSpeed(Vector2 vel0, Vector2 vel1)
        {
            var forceCalculator = new VehicleForces(constants.Object);

            var force0 = forceCalculator.GetDragForce(vel0);
            var force1 = forceCalculator.GetDragForce(vel1);

            Assert.True(force0.Length() < force1.Length());
        }

        [Fact]
        public void DragPointsInTheOppositeDirectionOfTheVelocityVector()
        {
            var forceCalculator = new VehicleForces(constants.Object);
            var vel = new Vector2(0, 28);

            var force = forceCalculator.GetDragForce(vel);

            // NOTE(danielm): dot product of two units vectors is -1 if the
            // angle between them is 180deg.
            var dot = Vector2.Dot(Vector2.Normalize(force), Vector2.Normalize(vel));
            var diff = Math.Abs(-1 - dot);

            Assert.True(diff < float.Epsilon);
        }

        [Fact]
        public void TractiveForcePointsInTheDirectionOfTheWheels()
        {
            var forceCalculator = new VehicleForces(constants.Object);
            var gear = 1;
            var wheelDirection = new Vector2(0, 1);
            var gasPedal = 1.0f;

            var force = forceCalculator.GetTractiveForce(gasPedal, wheelDirection, gear);

            // NOTE(danielm): dot product of two unit vectors is 1 if the
            // angle between them is 0 degrees.
            var dot = Vector2.Dot(Vector2.Normalize(force), wheelDirection);
            var diff = Math.Abs(1 - dot);

            Assert.True(diff < float.Epsilon);
        }

        [Theory]
        [InlineData(0, 1)]
        public void TractiveForceIsProportionalToCurrentGear(int gearIdx0, int gearIdx1)
        {
            var forceCalculator = new VehicleForces(constants.Object);
            var wheelDirection = new Vector2(0, 1);
            var gasPedal = 1.0f;

            var force0 = forceCalculator.GetTractiveForce(gasPedal, wheelDirection, gearIdx0);
            var force1 = forceCalculator.GetTractiveForce(gasPedal, wheelDirection, gearIdx1);

            var ratio = force1.Length() / force0.Length();
            Assert.True(ratio >= 1);
        }

        [Fact]
        public void TractiveForcePointsInTheReverseDirectionInReverseGear()
        {
            var forceCalculator = new VehicleForces(constants.Object);
            var wheelDirection = new Vector2(0, 1);
            var gasPedal = 1.0f;

            var force = forceCalculator.GetTractiveForceInReverse(gasPedal, wheelDirection);

            // NOTE(danielm): dot product of two units vectors is -1 if the
            // angle between them is 180deg.
            var dot = Vector2.Dot(Vector2.Normalize(force), Vector2.Normalize(wheelDirection));
            var diff = Math.Abs(-1 - dot);

            Assert.True(diff < float.Epsilon);
        }

        [Fact]
        public void GetTractiveForceThrowsWhenGearIndexIsOutOfBounds()
        {
            var forceCalculator = new VehicleForces(constants.Object);
            var wheelDirection = new Vector2(0, 1);
            var gasPedal = 1.0f;
            var gearIdx = constants.Object.NumberOfGears;

            Assert.Throws<ArgumentException>(
                () => forceCalculator.GetTractiveForce(gasPedal, wheelDirection, gearIdx)
            );
        }

        [Fact]
        public void GetTractiveForceThrowsWhenGearIndexIsLessThanZero()
        {
            var forceCalculator = new VehicleForces(constants.Object);
            var wheelDirection = new Vector2(0, 1);
            var gasPedal = 1.0f;
            var gearIdx = -1;

            Assert.Throws<ArgumentException>(
                () => forceCalculator.GetTractiveForce(gasPedal, wheelDirection, gearIdx)
            );
        }

        [Theory]
        [InlineData(0f, 0.5f)]
        [InlineData(0f, 1.0f)]
        [InlineData(0.5f, 0.5f)]
        public void BrakingForceIsProportionalToBrakePedal(float brakePedal0, float brakePedal1)
        {
            var forceCalculator = new VehicleForces(constants.Object);
            var velocity = new Vector2(0, 28);

            var force0 = forceCalculator.GetBrakingForce(brakePedal0, velocity);
            var force1 = forceCalculator.GetBrakingForce(brakePedal1, velocity);

            var ratio = force1.Length() / force0.Length();
            var ratioIsGreaterThanOne = ratio > 1;

            var ratioEqualsOne = Math.Abs(1 - ratio) < float.Epsilon;
            var brakePedalRatiosAreEqual = Math.Abs(brakePedal1 - brakePedal0) < float.Epsilon;

            // The ratio is either greater than one OR the ratio is one if and
            // only if the brake pedal ratios are equal.
            Assert.True(ratioIsGreaterThanOne || ((ratioEqualsOne && brakePedalRatiosAreEqual) || (!ratioEqualsOne && !brakePedalRatiosAreEqual)));
        }

        [Theory]
        [MemberData(nameof(dragProportionalTestVectors))]
        public void BrakingForceIsProportionalToVelocity(Vector2 velocity0, Vector2 velocity1)
        {
            var forceCalculator = new VehicleForces(constants.Object);
            var brakePedal = 1.0f;

            var force0 = forceCalculator.GetBrakingForce(brakePedal, velocity0);
            var force1 = forceCalculator.GetBrakingForce(brakePedal, velocity1);

            var ratio = force1.Length() / force0.Length();
            Assert.True(ratio >= 1);
        }

        [Theory]
        [InlineData(-10, 0)]
        [InlineData(0, -10)]
        [InlineData(-10, -10)]
        public void BrakingForceIsCorrectForVelocityVectorsWithNegativeComponents(float vx, float vy)
        {
            var forceCalculator = new VehicleForces(constants.Object);
            var brakePedal = 1.0f;
            var velocity = new Vector2(vx, vy);

            var force = forceCalculator.GetBrakingForce(brakePedal, velocity);

            // NOTE(danielm): dot product of two units vectors is -1 if the
            // angle between them is 180deg.
            var dot = Vector2.Dot(Vector2.Normalize(force), Vector2.Normalize(velocity));
            var error = Math.Abs(-1 - dot);

            Assert.True(error < 0.001f);
        }
    }
}
