namespace AutomatedCar.Models
{
    public interface IRenderableWorldObject
    {
        string Filename { get; set; }
        int Height { get; set; }
        int Width { get; set; }
        int X { get; set; }
        int Y { get; set; }
        int ZIndex { get; set; }
        double M11 { get; set; }
        double M12 { get; set; }
        double M21 { get; set; }
        double M22 { get; set; }
    }
}