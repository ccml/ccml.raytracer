using System;
using System.Collections.Generic;
using ccml.raytracer.Core;
using ccml.raytracer.Engine;

namespace ccml.raytracer.Shapes
{
    public class CrtCube : CrtShape
    {
        private (double min, double max) CheckAxis(double origin, double direction)
        {
            var tminNumerator = (-1 - origin);
            var tmaxNumerator = (1 - origin);
            
            var tmin = 0.0;
            var tmax = 0.0;
            if (CrtReal.CompareTo(Math.Abs(direction), 0) > 0)
            {
                tmin = tminNumerator / direction;
                tmax = tmaxNumerator / direction;
            }
            else
            {
                tmin = tminNumerator * double.PositiveInfinity;
                tmax = tmaxNumerator * double.PositiveInfinity;
            }

            if (CrtReal.CompareTo(tmin, tmax) > 0)
            {
                var t = tmin;
                tmin = tmax;
                tmax = t;
            }
            return (tmin, tmax);
        }

        public override IList<CrtIntersection> LocalIntersect(CrtRay r)
        {
            var x = CheckAxis(r.Origin.X, r.Direction.X);
            var y = CheckAxis(r.Origin.Y, r.Direction.Y);
            var z = CheckAxis(r.Origin.Z, r.Direction.Z);

            var tmin = Math.Max(Math.Max(x.min, y.min), z.min);
            var tmax = Math.Min(Math.Min(x.max, y.max), z.max);

            if (CrtReal.CompareTo(tmin, tmax) > 0)
            {
                return new List<CrtIntersection>();
            }

            return CrtFactory.EngineFactory.Intersections(
                    CrtFactory.EngineFactory.Intersection(tmin, this),
                    CrtFactory.EngineFactory.Intersection(tmax, this)
            );
        }

        public override CrtVector LocalNormalAt(CrtPoint point)
        {
            var maxc = Math.Max(Math.Max(Math.Abs(point.X), Math.Abs(point.Y)), Math.Abs(point.Z));
            if (CrtReal.AreEquals(maxc, Math.Abs(point.X)))
            {
                return CrtFactory.CoreFactory.Vector(point.X, 0, 0);
            }
            else if (CrtReal.AreEquals(maxc, Math.Abs(point.Y)))
            {
                return CrtFactory.CoreFactory.Vector(0, point.Y, 0);
            }
            return CrtFactory.CoreFactory.Vector(0, 0, point.Z);
        }
    }
}
