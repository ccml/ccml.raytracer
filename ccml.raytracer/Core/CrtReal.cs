using System;

namespace ccml.raytracer.Core
{
    /// <summary>
    /// Utility class to manage double numbers
    /// </summary>
    public static class CrtReal
    {
        /// <summary>
        /// If absolute value of difference between 2 reals is less than EPSILON they are considered as equals
        /// </summary>
        public const double EPSILON = 1e-5;

        /// <summary>
        /// Compare 2 reals and if absolute value of their difference is less than EPSILON they are considered as equals
        /// </summary>
        /// <param name="f1"></param>
        /// <param name="f2"></param>
        /// <returns></returns>
        public static bool AreEquals(double f1, double f2)
        {
            return Math.Abs(f2 - f1) < EPSILON;
        }

        /// <summary>
        /// Compare 2 reals and if absolute value of their difference is less than a specified precision they are considered as equals
        /// </summary>
        /// <param name="f1"></param>
        /// <param name="f2"></param>
        /// <param name="precision"></param>
        /// <returns></returns>
        public static bool AreEquals(double f1, double f2, double precision)
        {
            return Math.Abs(f2 - f1) < precision;
        }

        public static int CompareTo(double f1, double f2)
        {
            if (AreEquals(f1, f2)) return 0;
            return f1.CompareTo(f2);
        }
    }
}
