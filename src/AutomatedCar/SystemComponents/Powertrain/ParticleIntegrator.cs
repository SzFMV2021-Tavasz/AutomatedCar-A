using System.Numerics;

namespace AutomatedCar.SystemComponents.Powertrain
{
    public class ParticleIntegrator
    {
        private Vector2 forceAccumulator;
        private readonly Vector2 position;
        private readonly Vector2 velocity;
        private readonly float mass;
        private readonly float deltaTime;

        public (Vector2 Position, Vector2 Velocity) NextState => CalculateNextState();
        public float Mass { get => mass; }
        public Vector2 AccumulatedForce { get => forceAccumulator; }

        public ParticleIntegrator(Vector2 position, Vector2 velocity, float mass, float deltaTime)
        {
            this.forceAccumulator = Vector2.Zero;
            this.position = position;
            this.velocity = velocity;
            this.mass = mass;
            this.deltaTime = deltaTime;
        }

        public void AccumulateForce(Vector2 force)
        {
            this.forceAccumulator += force;
        }

        private (Vector2 Position, Vector2 Velocity) CalculateNextState()
        {
            var position0 = this.position;
            var velocity0 = this.velocity;

            var acceleration = this.forceAccumulator / mass;

            var velocity1 = velocity0 + deltaTime * acceleration;
            var position1 = position0 + deltaTime * velocity1;

            return (position1, velocity1);
        }
    }
}
