using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ccml.raytracer.engine.core.Shapes
{
    public class CrtSphere : CrtShape
    {
        public CrtPoint Center { get; private set; }

        internal CrtSphere() : base()
        {
            Center = new CrtPoint(0, 0, 0);
        }

        public override IList<CrtIntersection> Intersect(CrtRay r)
        {
            // !!! The intersection must be done with the ray 
            //     transformed in the shape world !!!
            //
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
            // The ray in the shape world
            var tr = r.Transform(InverseTransformMatrix);
            //
            var sphereToRay = tr.Origin - Center;
            var a = tr.Direction * tr.Direction;
            var b = 2 * (tr.Direction * sphereToRay);
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
                return CrtFactory.Intersections(
                    CrtFactory.Intersection(t1, this),
                    CrtFactory.Intersection(t2, this)
                );
            }
        }
    }
}
