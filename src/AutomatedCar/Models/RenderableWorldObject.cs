using BaseModel.WorldObjects;

namespace AutomatedCar.Models
{
    using ReactiveUI;

    public abstract class RenderableWorldObject : ReactiveObject
    {
        private int _x;
        private int _y;

        private double _m11 = 1.0;
        private double _m12 = 0.0;
        private double _m21 = 0.0;
        private double _m22 = 1.0;

        WorldObject wo = new Tree();

        public RenderableWorldObject(int x, int y, string filename)
        {
            this.X = x;
            this.Y = y;
            this.Filename = filename;
            this.ZIndex = 1;
        }

        public int ZIndex { get; set; }

        public int Width { get; set; }

        public int Height { get; set; }

        public int X
        {
            get => this._x;
            set => this.RaiseAndSetIfChanged(ref this._x, value);
        }

        public int Y
        {
            get => this._y;
            set => this.RaiseAndSetIfChanged(ref this._y, value);
        }

        public double M11
        {
            get => this._m11;
            set => this.RaiseAndSetIfChanged(ref this._m11, value);
        }

        public double M12
        {
            get => this._m12;
            set => this.RaiseAndSetIfChanged(ref this._m12, value);
        }

        public double M21
        {
            get => this._m21;
            set => this.RaiseAndSetIfChanged(ref this._m21, value);
        }

        public double M22
        {
            get => this._m22;
            set => this.RaiseAndSetIfChanged(ref this._m22, value);
        }

        public string Filename { get; set; }
    }
}