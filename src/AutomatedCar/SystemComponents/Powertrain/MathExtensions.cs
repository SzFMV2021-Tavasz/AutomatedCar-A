using System;
using System.Numerics;

namespace AutomatedCar.SystemComponents.Powertrain
{
    public static class MathExtensions
    {
        public static Vector2 MakeUnitVectorFromRadians(this float radians)
        {
            return new Vector2((float)Math.Cos(radians), (float)Math.Sin(radians));
        }

        public static Vector2 MakeUnitVectorFromRadians(this double radians)
        {
            return new Vector2((float)Math.Cos(radians), (float)Math.Sin(radians));
        }

        public static float RadiansToDegrees(this float radians)
        {
            return (float)(radians / Math.PI * 180);
        }

        public static double DegreesToRadians(this double degrees)
        {
            return degrees * Math.PI / 180;
        }

        public static void MakeUnitVectorFromRadians(this float radians, out float X, out float Y)
        {
            X = (float)Math.Cos(radians);
            Y = (float)Math.Sin(radians);
        }

        public static double NormalizeRadians(this double radians)
        {
            var ret = radians;

            while (ret < 0)
            {
                ret += 2 * Math.PI;
            }

            while (ret > 2 * Math.PI)
            {
                ret -= 2 * Math.PI;
            }

            return ret;
        }
    }
}
