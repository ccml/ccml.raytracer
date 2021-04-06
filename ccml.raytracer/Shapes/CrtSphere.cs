using System;
using System.Collections.Generic;
using ccml.raytracer.Core;
using ccml.raytracer.Engine;

namespace ccml.raytracer.Shapes
{
    public class CrtSphere : CrtShape
    {
        // (*) Just for convenience but it's always equals to (0,0,0) and don't change
        public CrtPoint Center { get; private set; }
        
        internal CrtSphere() : base()
        {
            Center = CrtFactory.CoreFactory.Point(0, 0, 0);
        }

        public override IList<CrtIntersection> LocalIntersect(CrtRay r)
        {
            // # the vector from the sphere's center, to the ray origin
            // # remember: the sphere is centered at the world origin
            // sphere_to_ray ← ray.origin - point(0, 0, 0)
            // a ← dot(ray.direction, ray.direction)
            // b ← 2 * dot(ray.direction, sphere_to_ray)
            // c ← dot(sphere_to_ray, sphere_to_ray) - 1
            // discriminant ← b² -4 * a * c
            // if discriminant < 0 ==> no intersections
            // else
            //   t1 ← (-b - √(discriminant)) / (2 * a)
            //   t2 ← (-b + √(discriminant)) / (2 * a)
            // make sure the intersections are returned in increasing order
            //
            var sphereToRay = r.Origin - Center;
            var a = r.Direction * r.Direction;
            var b = 2 * (r.Direction * sphereToRay);
            var c = (sphereToRay * sphereToRay) - 1;
            var discriminant = b * b - 4 * a * c;
            var compare = CrtReal.CompareTo(discriminant, 0.0);
            if (compare < 0)
            {
                return new List<CrtIntersection>();
            }
            else
            {
                var t1 = (-b - Math.Sqrt(discriminant)) / (2 * a);
                var t2 = (-b + Math.Sqrt(discriminant)) / (2 * a);
                return CrtFactory.EngineFactory.Intersections(
                    CrtFactory.EngineFactory.Intersection(t1, this),
                    CrtFactory.EngineFactory.Intersection(t2, this)
                );
            }
        }

        public override CrtVector LocalNormalAt(CrtPoint point, CrtIntersection intersection = null)
        {
            return point - Center;
        }

        private CrtBoundingBox _objectBounds = new CrtBoundingBox()
        {
            Minimum = CrtFactory.CoreFactory.Point(-1, -1, -1),
            Maximum = CrtFactory.CoreFactory.Point(1, 1, 1)
        };

        public override CrtBoundingBox ObjectBounds()
        {
            return _objectBounds;
        }

        public static bool operator ==(CrtSphere s1, CrtSphere s2)
        {
            return (((CrtShape) s1) == ((CrtShape) s2));
                   // because (*
                   //&&
                   //s1.Center == s2.Center;
        }

        public static bool operator !=(CrtSphere s1, CrtSphere s2)
        {
            return (((CrtShape)s1) != ((CrtShape)s2));
                   // because (*
                   //||
                   //s1.Center != s2.Center;
        }

        protected bool Equals(CrtSphere other)
        {
            return base.Equals(other) && Equals(Center, other.Center);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((CrtSphere) obj);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(base.GetHashCode(), Center);
        }
    }
}
