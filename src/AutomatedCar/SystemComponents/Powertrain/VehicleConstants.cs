using System;

namespace AutomatedCar.SystemComponents.Powertrain
{
    /// <summary>
    /// Constants for the in-game car.
    /// </summary>
    public class VehicleConstants : IVehicleConstants
    {
        public float[] GearRatios => new float[] {
            1 / 5.25f,
            1 / 3.03f,
            1 / 1.95f,
            1 / 1.46f,
            1 / 1.22f,
            1 / 1.00f,
            1 / 0.81f,
            1 / 0.67f
        };

        public int NumberOfGears => 8;

        public float ReverseGearRatio => 1 / 4.01f;

        public float DifferentialRatio => 1 / 3.08f;

        public float TransmissionEfficiency => 0.70f;

        public float OverallWheelRadius => 0.334391f;

        public float AirDensity => 1.184f;

        public float DragCoefficient => 0.28f;

        public float CrossSectionalArea => 2.64285714f;

        public float BrakingConstant => 80;

        public float CurbWeight => 1800;

        public float WheelBase => 2.872f;

        public float GetCrankshaftSpeed(float gasPedal)
        {
            return gasPedal * 6000;
        }

        public float GetEngineTorque(float rpm)
        {
            // A real-valued function approximating the torque function:
            // y = -1/40000 * rpm**2 + 0.225 * rpm
            var torque = -1 / 40000 * rpm * rpm + 0.225 * rpm;
            // torque should be atleast 0 Nm
            return (float)Math.Max(0, torque);
        }
    }
}
