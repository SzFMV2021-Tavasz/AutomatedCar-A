namespace AutomatedCar.SystemComponents.Powertrain
{
    public interface IVehicleConstants
    {
        /// <summary>
        /// An array of gear ratios.
        /// The ratio of the 1st gear is at index 0 and so on.
        /// </summary>
        float[] GearRatios { get; }

        /// <summary>
        /// The number of gears the transmission has.
        /// </summary>
        int NumberOfGears { get; }

        /// <summary>
        /// Reverse ratio.
        /// </summary>
        float ReverseGearRatio { get; }

        /// <summary>
        /// Final drive axle ratio.
        /// </summary>
        float DifferentialRatio { get; }

        /// <summary>
        /// Transmission efficiency, in 0..1.
        /// </summary>
        float TransmissionEfficiency { get; }

        /// <summary>
        /// Gets the rotating force produced by the engine at a given RPM.
        /// </summary>
        /// <param name="rpm">Revolutions per minute</param>
        /// <returns>Engine torque in Newton-meters.</returns>
        float GetEngineTorque(float rpm);

        /// <summary>
        /// Gets the speed of the crankshaft as a function of the gas pedal state.
        /// </summary>
        /// <param name="gasPedal">Accelerator state, in 0..1.</param>
        /// <returns>The crankshaft speed in revolutions per minute.</returns>
        float GetCrankshaftSpeed(float gasPedal);

        /// <summary>
        /// Overall radius of the wheel, including both the wheel and the tire, in meters.
        /// </summary>
        float OverallWheelRadius { get; }

        /// <summary>
        /// Density of the air in kilograms per cubic meter.
        /// </summary>
        float AirDensity { get; }

        /// <summary>
        /// Drag coefficient of the vehicle.
        /// </summary>
        float DragCoefficient { get; }

        /// <summary>
        /// Cross-sectional area of the car in sq meters.
        /// </summary>
        float CrossSectionalArea { get; }

        /// <summary>
        /// Constant used when calculating the braking force; kilogram over meters.
        /// </summary>
        float BrakingConstant { get; }

        /// <summary>
        /// Total mass of the vehicle; kilograms.
        /// </summary>
        float CurbWeight { get; }

        /// <summary>
        /// Distance between the front and rear wheels; meters.
        /// </summary>
        float WheelBase { get; }
    }
}
