namespace BaseModel.Interfaces
{
    public interface IRenderableWorldObject
    {
        string Filename { get; set; }
        int Height { get; set; }
        int Width { get; set; }
        int X { get; set; }
        int Y { get; set; }
        int ZIndex { get; set; }
    }
}