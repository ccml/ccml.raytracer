namespace ccml.raytracer.engine.core
{
    /// <summary>
    /// A point in a 3D space.
    /// 
    /// NB) A point is a tuple with W = 1.0
    /// </summary>
    public class CrtPoint : CrtTuple
    {
        public CrtPoint(double x, double y, double z) : base(x, y, z, 1.0)
        {
        }

        public static CrtPoint operator +(CrtPoint p, CrtVector v)
        {
            return (((CrtTuple)p) + ((CrtTuple)v)) as CrtPoint;
        }
        
        public static CrtVector operator -(CrtPoint p1, CrtPoint p2)
        {
            return (((CrtTuple)p1) - ((CrtTuple)p2)) as CrtVector;
        }

        public static CrtPoint operator -(CrtPoint p, CrtVector v)
        {
            return (((CrtTuple)p) - ((CrtTuple)v)) as CrtPoint;
        }

    }
}
