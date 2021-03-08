using System.Numerics;

namespace AutomatedCar.SystemComponents.Powertrain
{
    public record VehicleTransform(Vector2 Position, float AngularDisplacement, Vector2 Velocity, float AngularVelocity);
}
