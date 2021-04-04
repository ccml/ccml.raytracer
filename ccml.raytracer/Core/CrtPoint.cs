namespace ccml.raytracer.Core
{
    /// <summary>
    /// A point in a 3D space.
    /// 
    /// NB) A point is a tuple with W = 1.0
    /// </summary>
    public class CrtPoint : CrtTuple
    {
        internal CrtPoint(double x, double y, double z) : base(x, y, z, 1.0)
        {
        }

        public static CrtPoint operator +(CrtPoint p, CrtVector v)
        {
            var result = (((CrtTuple) p) + ((CrtTuple) v));
            return CrtFactory.CoreFactory.Point(result.X, result.Y, result.Z);
        }
        
        public static CrtVector operator -(CrtPoint p1, CrtPoint p2)
        {
            var result = (((CrtTuple) p1) - ((CrtTuple) p2));
            return CrtFactory.CoreFactory.Vector(result.X, result.Y, result.Z);
        }

        public static CrtPoint operator -(CrtPoint p, CrtVector v)
        {
            var result = (((CrtTuple) p) - ((CrtTuple) v));
            return CrtFactory.CoreFactory.Point(result.X, result.Y, result.Z);
        }

    }
}
