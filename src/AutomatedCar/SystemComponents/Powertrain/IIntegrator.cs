using System.Numerics;

namespace AutomatedCar.SystemComponents.Powertrain
{
    public interface IIntegrator
    {
        VehicleTransform NextVehicleTransform { get; }
        void Reset(VehicleTransform vehicleTransform, float deltaTime);
        void AccumulateForce(WheelKind wheel, Vector2 force);
    }
}
