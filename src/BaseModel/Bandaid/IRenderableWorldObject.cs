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
        float M11 { get; set; }
        float M12 { get; set; }
        float M21 { get; set; }
        float M22 { get; set; }
        bool IsHighLighted { get; set; }
    }
}