using System;

namespace ccml.raytracer.Core
{
    /// <summary>
    /// A vector in a 3D space.
    /// 
    /// NB) A vector is a tuple with W = 0.0
    /// </summary>
    public class CrtVector : CrtTuple
    {
        internal CrtVector(double x, double y, double z) : base(x, y, z, 0.0)
        {
        }

        public static CrtPoint operator +(CrtVector v, CrtPoint p)
        {
            var result = (((CrtTuple) v) + ((CrtTuple) p));
            return CrtFactory.CoreFactory.Point(result.X, result.Y, result.Z);
        }

        public static CrtVector operator +(CrtVector v1, CrtVector v2)
        {
            var result = (((CrtTuple) v1) + ((CrtTuple) v2));
            return CrtFactory.CoreFactory.Vector(result.X, result.Y, result.Z);
        }

        public static CrtVector operator -(CrtVector v1, CrtVector v2)
        {
            var result = (((CrtTuple) v1) - ((CrtTuple) v2));
            return CrtFactory.CoreFactory.Vector(result.X, result.Y, result.Z);
        }

        public static CrtVector operator *(CrtVector v, double scalar)
        {
            var result = (((CrtTuple) v) * scalar);
            return CrtFactory.CoreFactory.Vector(result.X, result.Y, result.Z);
        }

        public static CrtVector operator *(double scalar, CrtVector v)
        {
            var result = (scalar * ((CrtTuple) v));
            return CrtFactory.CoreFactory.Vector(result.X, result.Y, result.Z);
        }

        public static CrtVector operator -(CrtVector v)
        {
            if (v is null) throw new ArgumentException();
            var result = -((CrtTuple) v);
            return CrtFactory.CoreFactory.Vector(result.X, result.Y, result.Z);
        }

        /// <summary>
        /// Normalize vector operator
        /// </summary>
        /// <param name="v">A vector</param>
        /// <returns>The corresponding normalized vector</returns>
        public static CrtVector operator ~(CrtVector v)
        {
            if (v is null) throw new ArgumentException();
            return (CrtVector)(v / !v);
        }

        /// <summary>
        /// Magnitude of a vector
        /// </summary>
        /// <param name="v">A vector</param>
        /// <returns>Magnitude of the vector</returns>
        public static double operator !(CrtVector v)
        {
            if (v is null) throw new ArgumentException();
            return Math.Sqrt(v.X * v.X + v.Y * v.Y + v.Z * v.Z + v.W * v.W);
        }

        /// <summary>
        /// Dot product (scalar product or inner product) of 2 vectors
        /// </summary>
        /// <param name="v1">vector 1</param>
        /// <param name="v2">vector 2</param>
        /// <returns>Dot product of the 2 vectors</returns>
        public static double operator *(CrtVector v1, CrtVector v2)
        {
            if (v1 is null) throw new ArgumentException();
            if (v2 is null) throw new ArgumentException();
            return
                v1.X * v2.X +
                v1.Y * v2.Y +
                v1.Z * v2.Z +
                v1.W * v2.W;
        }

        /// <summary>
        /// Cross product of 2 vectors
        /// </summary>
        /// <param name="v1">vector 1</param>
        /// <param name="v2">vector 2</param>
        /// <returns>Cross product of the 2 vectors</returns>
        public static CrtVector operator ^(CrtVector v1, CrtVector v2)
        {
            if (v1 is null) throw new ArgumentException();
            if (v2 is null) throw new ArgumentException();
            return CrtFactory.CoreFactory.Vector(
                v1.Y * v2.Z - v1.Z * v2.Y,
                v1.Z * v2.X - v1.X * v2.Z,
                v1.X * v2.Y - v1.Y * v2.X
            );
        }

        /// <summary>
        /// return the vector resulting of the reflection of the current vector around the normal vector
        /// </summary>
        /// <param name="normal">the normal</param>
        /// <returns>the reflected vector</returns>
        public CrtVector ReflectBy(CrtVector normal)
        {
            return this - normal * 2 * (this * normal);
        }
    }
}
