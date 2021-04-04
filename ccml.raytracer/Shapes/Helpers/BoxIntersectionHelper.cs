using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ccml.raytracer.Core;
using ccml.raytracer.Engine;

namespace ccml.raytracer.Shapes.Helpers
{
    public static class BoxIntersectionHelper
    {
        private static (double min, double max) CheckAxis(double origin, double direction, double min, double max)
        {
            var tminNumerator = (min - origin);
            var tmaxNumerator = (max - origin);

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

        public static (double tmin, double tmax) IntersectBoundinBox(CrtRay r, CrtBoundingBox boundingBox)
        {
            double tmin = 0;
            double tmax = 0;
            if (boundingBox == null) return (1, 0);

            var x = CheckAxis(r.Origin.X, r.Direction.X, boundingBox.Minimum.X, boundingBox.Maximum.X);
            var y = CheckAxis(r.Origin.Y, r.Direction.Y, boundingBox.Minimum.Y, boundingBox.Maximum.Y);
            var z = CheckAxis(r.Origin.Z, r.Direction.Z, boundingBox.Minimum.Z, boundingBox.Maximum.Z);

            tmin = Math.Max(Math.Max(x.min, y.min), z.min);
            tmax = Math.Min(Math.Min(x.max, y.max), z.max);

            return (tmin, tmax);
        }
    }
}
