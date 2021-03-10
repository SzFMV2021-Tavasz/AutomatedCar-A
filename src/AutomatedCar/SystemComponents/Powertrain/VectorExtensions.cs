﻿using System;
using System.Numerics;

namespace AutomatedCar.SystemComponents.Powertrain
{
    public static class VectorExtensions
    {
        public static Vector2 MakeUnitVectorFromRadians(this float radians)
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
    }
}
