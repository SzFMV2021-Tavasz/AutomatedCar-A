namespace AutomatedCar.Models
{
    using SystemComponents;
    using System.Windows.Shapes;
    using System.Numerics;

    public class AutomatedCar : Car
    {
        private VirtualFunctionBus virtualFunctionBus;
        private DummySensor dummySensor;

        public AutomatedCar(int x, int y, string filename)
            : base(x, y, filename)
        {
            this.virtualFunctionBus = new VirtualFunctionBus();
            this.dummySensor = new DummySensor(this.virtualFunctionBus);

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
        public float CarHeading { get; set; }
        public float AngularVelocity { get; set; }
        public Vector2 Velocity { get; set; }
        public float CurrentSteering { get; set; }
        public int RPM { get; set; }
    }
}