using BaseModel.WorldObjects;

namespace AutomatedCar.Models
{
    using ReactiveUI;
    using System.Windows;

    public abstract class RenderableWorldObject : ReactiveObject, IRenderableWorldObject
    {
        private int _x;
        private int _y;

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

        public string Filename { get; set; }
    }
}