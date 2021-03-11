using System;

namespace BaseModel
{
    public class Triangle: Tuple<Tuple<int, int>, Tuple<int, int>, Tuple<int, int>>
    {
        public Triangle(Tuple<int, int> item1, Tuple<int, int> item2, Tuple<int, int> item3) : base(item1, item2, item3)
        {
        }

        public Triangle(int p0X, int p0Y, int p1X, int p1Y, int p2X, int p2Y) : base(new Tuple<int, int>(p0X, p0Y),
            new Tuple<int, int>(p1X, p1Y), new Tuple<int, int>(p2X, p2Y))
        {
        }
    }
}