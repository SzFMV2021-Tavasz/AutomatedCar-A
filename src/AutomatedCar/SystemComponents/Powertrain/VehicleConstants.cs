using System;

namespace AutomatedCar.SystemComponents.Powertrain
{
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
            // y = 470 - (x - 4000)**2 / 50000
            var torque = 470 - (float)Math.Pow(rpm - 4000, 2) / 50000;
            // torque should be atleast 350 Nm
            return Math.Min(350, torque);
        }
    }
}
