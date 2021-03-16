using AutomatedCar.Models.Enums;
using System.Numerics;

namespace AutomatedCar.SystemComponents.Powertrain
{
    public interface IIntegrator
    {
        VehicleTransform NextVehicleTransform { get; }
        void Reset(VehicleTransform vehicleTransform, float deltaTime, Gear currentGear);
        void AccumulateForce(WheelKind wheel, Vector2 force);
    }
}
