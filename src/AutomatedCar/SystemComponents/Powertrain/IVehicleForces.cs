using System.Numerics;

namespace AutomatedCar.SystemComponents.Powertrain
{
    public interface IVehicleForces
    {
        Vector2 GetDragForce(Vector2 velocity);
        Vector2 GetTractiveForce(float gasPedal, Vector2 wheelDirection, int gearIdx);
        Vector2 GetTractiveForceInReverse(float gasPedal, Vector2 wheelDirection);
        Vector2 GetBrakingForce(float brakePedal, Vector2 currentVelocity);
    }
}