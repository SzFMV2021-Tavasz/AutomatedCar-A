using BaseModel.WorldObjects;

namespace AutomatedCar.Models
{
    using ReactiveUI;

    public abstract class RenderableWorldObject : ReactiveObject, IRenderableWorldObject
    {
        private int _x;
        private int _y;

        private float _m11;
        private float _m12;
        private float _m21;
        private float _m22;

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
        public float M11
        {
            get => this._m11;
            set => this.RaiseAndSetIfChanged(ref this._m11, value);
        }

        public float M12
        {
            get => this._m12;
            set => this.RaiseAndSetIfChanged(ref this._m12, value);
        }

        public float M21
        {
            get => this._m21;
            set => this.RaiseAndSetIfChanged(ref this._m21, value);
        }

        public float M22
        {
            get => this._m22;
            set => this.RaiseAndSetIfChanged(ref this._m22, value);
        }

        public string Filename { get; set; }
    }
}