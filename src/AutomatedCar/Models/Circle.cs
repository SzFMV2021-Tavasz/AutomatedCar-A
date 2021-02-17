namespace AutomatedCar.Models
{
    using System;

    /// <summary>This is a dummy object for demonstrating the codebase.</summary>
    public class Circle : WorldObject
    {
        public Circle(int x, int y, string filename, int radius)
            : base(x, y, filename)
        {
            this.Radius = radius;
        }

        public int Radius { get; set; }

        public double CalculateArea()
        {
            return Math.PI * this.Radius * this.Radius;
        }
    }
}