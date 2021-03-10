namespace AutomatedCar.Models
{
    using BaseModel.Interfaces;
    using ReactiveUI;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;

    public class World : ReactiveObject
    {
        private BaseModel.World _world;
        private AutomatedCar _controlledCar;

        public static World Instance { get; } = new World();

        public ObservableCollection<RenderableWorldObject> WorldObjects { get; } = new ObservableCollection<RenderableWorldObject>();

        public List<IRenderableWorldObject> Renderables { get; private set; }

        public AutomatedCar ControlledCar
        {
            get => this._controlledCar;
            set => this.RaiseAndSetIfChanged(ref this._controlledCar, value);
        }

        public int Width => _world.Width;

        public int Height => _world.Height;

        public void AddObject(RenderableWorldObject worldObject)
        {
            this.WorldObjects.Add(worldObject);
        }

        public void LoadFromJSON(string worldJsonPath, string worldObjectPolygonsJsonPath)
        {
            _world = BaseModel.World.FromJSON(worldJsonPath, worldObjectPolygonsJsonPath);
            Renderables = new List<IRenderableWorldObject>();
            _world.objects.ForEach(o => Renderables.Add(o));
        }
    }
}