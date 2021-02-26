using System;

namespace ccml.raytracer.engine.core
{
    /// <summary>
    /// Utility class to manage double numbers
    /// </summary>
    public static class CrtReal
    {
        /// <summary>
        /// If absolute value of difference between 2 reals is less than EPSILON they are considered as equals
        /// </summary>
        public const double EPSILON = 0.00001;

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
    }
}
