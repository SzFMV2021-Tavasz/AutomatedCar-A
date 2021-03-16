using AutomatedCar.SystemComponents.Powertrain;
using Moq;
using System;
using System.Collections.Generic;
using System.Numerics;
using Xunit;

namespace AutomatedCarTest.SystemComponents.Powertrain
{
    public class ParticleIntegratorTests
    {
        private IVehicleConstants vehicleConstants = new VehicleConstants();

        [Fact]
        public void ForceSumming()
        {
            var integrator = new ParticleIntegrator(Vector2.Zero, Vector2.Zero, 0, 0, WheelKind.Front);

            var force0 = new Vector2(7, -5);
            var force1 = new Vector2(-3, 11);
            var force2 = Vector2.Zero;

            integrator.AccumulateForce(force0);
            integrator.AccumulateForce(force1);
            integrator.AccumulateForce(force2);

            var sum = integrator.AccumulatedForce;
            Assert.Equal(sum, force0 + force1);
        }

        [Fact]
        public void MassProperty()
        {
            var massInit = 100f;
            var integrator = new ParticleIntegrator(Vector2.Zero, Vector2.Zero, massInit, 0, WheelKind.Front);

            var mass = integrator.Mass;

            Assert.Equal(massInit, mass);
        }
        public static IEnumerable<object[]> integrationTestCases =>
            new List<object[]>
            {
                new object[] { new Vector2(7, 5), new Vector2(3, -4), 13f, 1f, new Vector2(26, -26), new Vector2(12, -1), new Vector2(5, -6) },
            };

        [Theory]
        [MemberData(nameof(integrationTestCases))]
        public void NextStateCalculatedIsCorrect(Vector2 initialPosition, Vector2 initialVelocity, float mass, float deltaTime, Vector2 netForce, Vector2 expectedPosition, Vector2 expectedVelocity)
        {
            var integrator = new ParticleIntegrator(initialPosition, initialVelocity, mass, deltaTime, WheelKind.Front);

            integrator.AccumulateForce(netForce);
            var (nextPosition, nextVelocity) = integrator.NextState;

            Assert.Equal(expectedPosition, nextPosition);
            Assert.Equal(expectedVelocity, nextVelocity);
        }
    }
}
