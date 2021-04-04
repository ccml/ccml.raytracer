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
    public class CrtCone : CrtShape
    {
        private static readonly CrtVector NormalY = CrtFactory.CoreFactory.Vector(0, 1, 0);
        private static readonly CrtVector NegNormalY = CrtFactory.CoreFactory.Vector(0, -1, 0);

        public double Minimum { get; set; } = double.MinValue;
        public double Maximum { get; set; } = double.MaxValue;
        public bool MinimumClosed { get; set; } = false;
        public bool MaximumClosed { get; set; } = false;

        public override IList<CrtIntersection> LocalIntersect(CrtRay r)
        {
            var a = r.Direction.X * r.Direction.X - r.Direction.Y * r.Direction.Y + r.Direction.Z * r.Direction.Z;
            var b = 2 * r.Origin.X * r.Direction.X - 2 * r.Origin.Y * r.Direction.Y + 2 * r.Origin.Z * r.Direction.Z;
            var c = r.Origin.X * r.Origin.X - r.Origin.Y * r.Origin.Y + r.Origin.Z * r.Origin.Z;

            var xs = new List<CrtIntersection>();

            if (CrtReal.AreEquals(Math.Abs(a), 0) && !CrtReal.AreEquals(Math.Abs(b), 0))
            {
                var t = -c / (2 * b);
                xs.Add(CrtFactory.EngineFactory.Intersection(t, this));
            }

            if (!CrtReal.AreEquals(Math.Abs(a), 0))
            {
                var disc = b * b - 4 * a * c;
                if (CrtReal.CompareTo(disc, 0) >= 0)
                {
                    var t0 = (-b - Math.Sqrt(disc)) / (2 * a);
                    var t1 = (-b + Math.Sqrt(disc)) / (2 * a);

                    if (t0 > t1)
                    {
                        var tmp = t0;
                        t0 = t1;
                        t1 = tmp;
                    }

                    var y0 = r.Origin.Y + t0 * r.Direction.Y;
                    if ((CrtReal.CompareTo(Minimum, y0) < 0) && (CrtReal.CompareTo(y0, Maximum) < 0))
                    {
                        xs.Add(CrtFactory.EngineFactory.Intersection(t0, this));
                    }

                    var y1 = r.Origin.Y + t1 * r.Direction.Y;
                    if ((CrtReal.CompareTo(Minimum, y1) < 0) && (CrtReal.CompareTo(y1, Maximum) < 0))
                    {
                        xs.Add(CrtFactory.EngineFactory.Intersection(t1, this));
                    }
                }
            }

            IntersectCaps(r, xs);

            return xs;
        }

        public override CrtVector LocalNormalAt(CrtPoint point)
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

            double y = Math.Sqrt(dist);
            
            if (point.Y > 0)
            {
                y *= -1;
            }

            return CrtFactory.CoreFactory.Vector(point.X, y, point.Z);
        }

        #region caps

        public bool CheckCap(CrtRay r, double t, double y)
        {
            var x = r.Origin.X + t * r.Direction.X;
            var z = r.Origin.Z + t * r.Direction.Z;
            return CrtReal.CompareTo(x * x + z * z, y * y) <= 0;
        }

        public void IntersectCaps(CrtRay r, IList<CrtIntersection> xs)
        {
            // caps only matter if the cone is closed, and might possibly be
            // intersected by the ray.
            if (MinimumClosed)
            {
                // check for an intersection with the lower end cap by intersecting
                // the ray with the plane at y=cone.minimum
                var t = (Minimum - r.Origin.Y) / r.Direction.Y;
                if (CheckCap(r, t, Minimum))
                {
                    xs.Add(CrtFactory.EngineFactory.Intersection(t, this));
                }
            }

            if (MaximumClosed)
            {
                // check for an intersection with the upper end cap by intersecting
                // the ray with the plane at y=cone.maximum
                var t = (Maximum - r.Origin.Y) / r.Direction.Y;
                if (CheckCap(r, t, Maximum))
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
                Minimum = CrtFactory.CoreFactory.Point(Minimum, Minimum, Minimum),
                Maximum = CrtFactory.CoreFactory.Point(Maximum, Maximum, Maximum)
            };
        }

        #region fluent mode

        public CrtCone WithMinimum(double m)
        {
            Minimum = m;
            return this;
        }

        public CrtCone WithMaximum(double m)
        {
            Maximum = m;
            return this;
        }

        public CrtCone WithMinimumClosed()
        {
            MinimumClosed = true;
            return this;
        }

        public CrtCone WithMaximumClosed()
        {
            MaximumClosed = true;
            return this;
        }

        #endregion
    }
}
