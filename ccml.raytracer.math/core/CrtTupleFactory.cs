using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace ccml.raytracer.math.core
{
    public static class CrtTupleFactory
    {
        /// <summary>
        /// Create a tuple
        /// </summary>
        /// <param name="x">coordinate on x axis</param>
        /// <param name="y">coordinate on y axis</param>
        /// <param name="z">coordinate on z axis</param>
        /// <param name="w">1.0 for points and 0.0 for vectors</param>
        /// <returns></returns>
        public static CrtTuple Tuple(double x, double y, double z, double w)
        {
            return 
                CrtReal.AreEquals(w, 1.0) ? 
                    Point(x, y, z) 
                    : CrtReal.AreEquals(w, 0.0) ?
                        Vector(x, y, z)
                        : 
                        new CrtTuple(x, y, z, w);
        }

        /// <summary>
        /// Create a 3D point
        /// </summary>
        /// <param name="x">coordinate on x axis</param>
        /// <param name="y">coordinate on y axis</param>
        /// <param name="z">coordinate on z axis</param>
        /// <returns></returns>
        public static CrtPoint Point(double x, double y, double z) => new CrtPoint(x, y, z);

        /// <summary>
        /// Create a 3D vector
        /// </summary>
        /// <param name="x">coordinate on x axis</param>
        /// <param name="y">coordinate on y axis</param>
        /// <param name="z">coordinate on z axis</param>
        /// <returns></returns>
        public static CrtVector Vector(double x, double y, double z) => new CrtVector(x, y, z);
    }
}
