namespace AutomatedCar.SystemComponents
{
    using AutomatedCar.Models;
    using AutomatedCar.SystemComponents.Packets;

    /// <summary>This is a dummy sensor for demonstrating the codebase.
    /// It calculates distance per coordinate between the controlled car and the dummy Circle object</summary>
    public class DummySensor : SystemComponent
    {
        private DummyPacket dummyPacket;

        public DummySensor(VirtualFunctionBus virtualFunctionBus)
            : base(virtualFunctionBus)
        {
            this.virtualFunctionBus = virtualFunctionBus;
            virtualFunctionBus.RegisterComponent(this);

            this.dummyPacket = new DummyPacket();
            virtualFunctionBus.DummyPacket = this.dummyPacket;
        }

        public override void Process()
        {
            this.dummyPacket.DistanceX = World.Instance.WorldObjects[0].X - World.Instance.ControlledCar.X;
            this.dummyPacket.DistanceY = World.Instance.WorldObjects[0].Y - World.Instance.ControlledCar.Y;
        }
    }
}