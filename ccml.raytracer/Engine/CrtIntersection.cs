using System;
using ccml.raytracer.Core;
using ccml.raytracer.Shapes;

namespace ccml.raytracer.Engine
{
    public class CrtIntersection
    {
        public double T { get; private set; }
        public CrtShape TheObject { get; private set; }

        public double U { get; private set; }
        public double V { get; private set; }

        internal CrtIntersection(double t, CrtShape theObject, double u = 0.0, double v = 0.0)
        {
            T = t;
            TheObject = theObject;
            U = u;
            V = v;
        }

        public static bool operator ==(CrtIntersection c1, CrtIntersection c2) =>
            (c1 is null && c2 is null) 
            || 
            (!(c1 is null) && !(c2 is null) && CrtReal.AreEquals(c1.T, c2.T) && (c1.TheObject == c2.TheObject));
        
        public static bool operator !=(CrtIntersection c1, CrtIntersection c2) =>
            !(c1 == c2);

        protected bool Equals(CrtIntersection other)
        {
            return T.Equals(other.T) && Equals(TheObject, other.TheObject);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((CrtIntersection) obj);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(T, TheObject);
        }
    }
}
