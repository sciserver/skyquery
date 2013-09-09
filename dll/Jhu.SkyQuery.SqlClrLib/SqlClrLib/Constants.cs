using System;
using System.Collections.Generic;
using System.Text;

namespace Jhu.SkyQuery.SqlClrLib
{
    static class Constants
    {
        public const double PI = Math.PI;
        public const double HalfPI = Math.PI / 2;
        public const double MinusHalfPI = -Math.PI / 2;
        public const double TwoPI = 2 * Math.PI;
        public const double Degrees = 180.0 / Math.PI;
        public const double Radians = Math.PI / 180.0;

        public static readonly double Tolerance = 2e-8;

        public static readonly double DoublePrecision = Math.Pow(2, -53);
        public static readonly double DoublePrecision2x = 2 * DoublePrecision;
        internal static readonly double DoublePrecision4x = 4 * DoublePrecision;

        internal static readonly double SafeLimit = 1e-7;
        internal static readonly double CosSafe = Math.Cos(SafeLimit);
        internal static readonly double SinSafe = Math.Sin(SafeLimit);

        public static readonly double Degree2Radian = Math.PI / 180;
        public static readonly double Radian2Degree = 1 / Degree2Radian;

        internal static readonly int HashDigit = 15;
        internal static readonly double HashMagic = 2; //29;

        public static readonly double Arcmin2Radian = Degree2Radian / 60;
        public static readonly double Radian2Arcmin = 1 / Arcmin2Radian;
        public static readonly double SquareRadian2SquareDegree = Radian2Degree * Radian2Degree;
    }
}