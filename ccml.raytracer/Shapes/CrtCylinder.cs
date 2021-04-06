using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using ccml.raytracer.Core;
using ccml.raytracer.Engine;

namespace ccml.raytracer.Shapes
{
    public class CrtCylinder : CrtShape
    {
        private static readonly CrtVector NormalY = CrtFactory.CoreFactory.Vector(0, 1, 0);
        private static readonly CrtVector NegNormalY = CrtFactory.CoreFactory.Vector(0, -1, 0);

        public double Minimum { get; set; } = double.MinValue;
        public double Maximum { get; set; } = double.MaxValue;
        public bool MinimumClosed { get; set; } = false;
        public bool MaximumClosed { get; set; } = false;

        public override IList<CrtIntersection> LocalIntersect(CrtRay r)
        {
            var a = r.Direction.X * r.Direction.X + r.Direction.Z * r.Direction.Z;

            // ray is parallel to the y axis
            if (!MinimumClosed && !MaximumClosed && (CrtReal.CompareTo(a, 0) == 0))
            {
                return new List<CrtIntersection>();
            }

            var b = 2 * r.Origin.X * r.Direction.X + 2 * r.Origin.Z * r.Direction.Z;
            var c = r.Origin.X * r.Origin.X + r.Origin.Z * r.Origin.Z - 1;
            var disc = b * b - 4 * a * c;

            //  ray does not intersect the cylinder
            if (CrtReal.CompareTo(disc, 0) < 0)
            {
                return new List<CrtIntersection>();
            }

            var t0 = (-b - Math.Sqrt(disc)) / (2 * a);
            var t1 = (-b + Math.Sqrt(disc)) / (2 * a);

            if (CrtReal.CompareTo(t0, t1) > 0)
            {
                var t = t0;
                t0 = t1;
                t1 = t;
            }

            var xs = new List<CrtIntersection>();

            var y0 = r.Origin.Y + t0 * r.Direction.Y;
            if((CrtReal.CompareTo(Minimum, y0) < 0) && (CrtReal.CompareTo(y0, Maximum) < 0))
            {
                xs.Add(CrtFactory.EngineFactory.Intersection(t0, this));
            }

            var y1 = r.Origin.Y + t1 * r.Direction.Y;
            if ((CrtReal.CompareTo(Minimum, y1) < 0) && (CrtReal.CompareTo(y1, Maximum) < 0))
            {
                xs.Add(CrtFactory.EngineFactory.Intersection(t1, this));
            }

            IntersectCaps(r, xs);

            return xs;
        }

        public override CrtVector LocalNormalAt(CrtPoint point, CrtIntersection intersection = null)
        {
            var dist = point.X * point.X + point.Z * point.Z;

            if (
                (CrtReal.CompareTo(dist, 1) < 0)
                &&
                (CrtReal.CompareTo(point.Y, Maximum - CrtReal.EPSILON) >= 0)
            )
            {
                return NormalY;
            }

            if (
                (CrtReal.CompareTo(dist, 1) < 0)
                &&
                (CrtReal.CompareTo(point.Y, Minimum + CrtReal.EPSILON) <= 0)
            )
            {
                return NegNormalY;
            }

            return CrtFactory.CoreFactory.Vector(point.X, 0, point.Z);
        }

        #region caps

        public bool CheckCap(CrtRay r, double t)
        {
            var x = r.Origin.X + t * r.Direction.X;
            var z = r.Origin.Z + t * r.Direction.Z;
            return CrtReal.CompareTo(x * x + z * z, 1) <= 0;
        }

        public void IntersectCaps(CrtRay r, IList<CrtIntersection> xs)
        {
            // caps only matter if the cylinder is closed, and might possibly be
            // intersected by the ray.
            if (MinimumClosed)
            {
                // check for an intersection with the lower end cap by intersecting
                // the ray with the plane at y=cyl.minimum
                var t = (Minimum - r.Origin.Y) / r.Direction.Y;
                if (CheckCap(r, t))
                {
                    xs.Add(CrtFactory.EngineFactory.Intersection(t, this));
                }
            }

            if (MaximumClosed)
            {
                // check for an intersection with the upper end cap by intersecting
                // the ray with the plane at y=cyl.maximum
                var t = (Maximum - r.Origin.Y) / r.Direction.Y;
                if (CheckCap(r, t))
                {
                    xs.Add(CrtFactory.EngineFactory.Intersection(t, this));
                }
            }
        }

        #endregion

        public override CrtBoundingBox ObjectBounds()
        {
            if (!(MinimumClosed && MaximumClosed)) return null;
            return new CrtBoundingBox()
            {
                Minimum = CrtFactory.CoreFactory.Point(-1, Minimum, -1),
                Maximum = CrtFactory.CoreFactory.Point(1, Maximum, 1)
            };
        }

        #region fluent mode

        public CrtCylinder WithMinimum(double m)
        {
            Minimum = m;
            return this;
        }

        public CrtCylinder WithMaximum(double m)
        {
            Maximum = m;
            return this;
        }

        public CrtCylinder WithMinimumClosed()
        {
            MinimumClosed = true;
            return this;
        }

        public CrtCylinder WithMaximumClosed()
        {
            MaximumClosed = true;
            return this;
        }

        #endregion
    }
}
