using ReactiveUI;

namespace AutomatedCar.SystemComponents
{
    public sealed class CameraPacket : ReactiveObject, IReadOnlyDummyPacket
    {
        private int x;
        private int y;
        public int DistanceX
        {
            get => this.x;
            set => this.RaiseAndSetIfChanged(ref this.x, value);
        }

        public int DistanceY
        {
            get => this.y;
            set => this.RaiseAndSetIfChanged(ref this.y, value);
        }
    }
}