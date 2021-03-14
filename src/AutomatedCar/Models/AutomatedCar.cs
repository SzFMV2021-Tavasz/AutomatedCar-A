namespace AutomatedCar.Models
{
    using SystemComponents;
    using System.Windows.Shapes;
    using System.Numerics;
    using global::AutomatedCar.SystemComponents.Powertrain;

    public class AutomatedCar : Car
    {
        private VirtualFunctionBus virtualFunctionBus;
        private DummySensor dummySensor;
        private PowertrainComponent powertrain;
        IVehicleForces VehicleForces;
        IVehicleConstants VehicleConstants;
        IIntegrator Integrator;

        public AutomatedCar(int x, int y, string filename)
            : base(x, y, filename)
        {
            this.virtualFunctionBus = new VirtualFunctionBus();
            this.dummySensor = new DummySensor(this.virtualFunctionBus);
            this.VehicleConstants = new VehicleConstants();
            this.VehicleForces = new VehicleForces(VehicleConstants);
            this.Integrator = new Integrator(VehicleConstants);

            this.powertrain = new PowertrainComponent(this.virtualFunctionBus, VehicleForces, VehicleConstants, Integrator);
            CarHeading = 0;
            AngularVelocity = 0;
            CurrentSteering = 0;
            Velocity = Vector2.Zero;
            RPM = 0;
        }

        public VirtualFunctionBus VirtualFunctionBus { get => this.virtualFunctionBus; }

        /// <summary>Starts the automated cor by starting the ticker in the Virtual Function Bus, that cyclically calls the system components.</summary>
        public void Start()
        {
            this.virtualFunctionBus.Start();
        }

        /// <summary>Stops the automated cor by stopping the ticker in the Virtual Function Bus, that cyclically calls the system components.</summary>
        public void Stop()
        {
            this.virtualFunctionBus.Stop();
        }

        public Polyline Geometry { get; set; }
        public Polyline Video { get; set; }
        public Polyline Radar { get; set; }
        public Polyline UltraSonic { get; set; }
        
        public float CarHeading { get; set; }
        public float AngularVelocity { get; set; }
        public Vector2 Velocity { get; set; }
        public float CurrentSteering { get; set; }
        public int RPM { get; set; }
    }
}