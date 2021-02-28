namespace AutomatedCar.SystemComponents.Powertrain
{
    public interface IVehicleConstants
    {
        /// <summary>
        /// An array of gear ratios.
        /// The ratio of the 1st gear is at index 0 and so on.
        /// </summary>
        public float[] GearRatios { get; }

        /// <summary>
        /// The number of gears the transmission has.
        /// </summary>
        public int NumberOfGears { get; }

        /// <summary>
        /// Reverse ratio.
        /// </summary>
        public float ReverseGearRatio { get; }

        /// <summary>
        /// Final drive axle ratio.
        /// </summary>
        public float DifferentialRatio { get; }

        /// <summary>
        /// Transmission efficiency, in 0..1.
        /// </summary>
        public float TransmissionEfficiency { get; }

        /// <summary>
        /// Gets the rotating force produced by the engine at a given RPM.
        /// </summary>
        /// <param name="rpm">Revolutions per minute</param>
        /// <returns>Engine torque in Newton-meters.</returns>
        public float GetEngineTorque(float rpm);

        /// <summary>
        /// Gets the speed of the crankshaft as a function of the gas pedal state.
        /// </summary>
        /// <param name="gasPedal">Accelerator state, in 0..1.</param>
        /// <returns>The crankshaft speed in revolutions per minute.</returns>
        public float GetCrankshaftSpeed(float gasPedal);

        /// <summary>
        /// Overall radius of the wheel, including both the wheel and the tire, in meters.
        /// </summary>
        public float OverallWheelRadius { get; }

        /// <summary>
        /// Density of the air in kilograms per cubic meter.
        /// </summary>
        public float AirDensity { get; }

        /// <summary>
        /// Drag coefficient of the vehicle.
        /// </summary>
        public float DragCoefficient { get; }

        /// <summary>
        /// Cross-sectional area of the car in sq meters.
        /// </summary>
        public float CrossSectionalArea { get; }

        /// <summary>
        /// Constant used when calculating the braking force; kilograms.
        /// </summary>
        public float BrakingConstant { get; }

        /// <summary>
        /// Total mass of the vehicle; kilograms.
        /// </summary>
        public float CurbWeight { get; }

        /// <summary>
        /// Distance between the front and rear wheels; meters.
        /// </summary>
        public float WheelBase { get; }
    }
}
