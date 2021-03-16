using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ccml.raytracer.engine.core.Lights;
using ccml.raytracer.engine.core.Shapes;

namespace ccml.raytracer.engine.core
{
    public class CrtWorld
    {
        public List<CrtShape> Objects { get; private set; }
        public List<CrtPointLight> Lights { get; private set; }

        internal CrtWorld()
        {
            Objects = new List<CrtShape>();
            Lights = new List<CrtPointLight>();
        }

        public void Add(params CrtShape[] objects)
        {
            Objects.AddRange(objects);
        }

        public void Add(params CrtPointLight[] lights)
        {
            Lights.AddRange(lights);
        }

        private List<CrtIntersection> Intersect(CrtRay r)
        {
            var intersections = new List<CrtIntersection>();
            foreach (var anObject in Objects)
            {
                intersections.AddRange(anObject.Intersect(r));
                intersections = CrtFactory.Intersections(intersections.ToArray());
            }

            return intersections;
        }

        private CrtIntersection Hit(List<CrtIntersection> intersections)
        {
            return intersections.FirstOrDefault(i => CrtReal.CompareTo(i.T, 0.0) > 0);
        }

        public CrtColor ColorAt(CrtRay r, int remaining = 4)
        {
            var intersections = Intersect(r);
            var hit = Hit(intersections);
            if (hit == null)
            {
                return CrtColor.COLOR_BLACK;
            }
            else
            {
                var comps = CrtFactory.Engine().PrepareComputations(hit, r);
                var c = CrtFactory.Engine().ShadeHit(this, comps, remaining);
                return c;
            }
        }

        public bool IsShadowed(CrtPoint point)
        {
            var v = Lights[0].Position - point;
            var distance = !v;
            var direction = ~v;
            var r = CrtFactory.Ray(point, direction);
            var intersections = Intersect(r);
            var h = Hit(intersections);
            if ((h != null) && (h.T < distance))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Compute the reflected color at an intersection point
        /// </summary>
        /// <param name="comps">the intersection parameter</param>
        /// <returns>the reflected color</returns>
        public CrtColor ReflectedColor(tests.math.core.CrtIntersectionComputation comps, int remaining = 4)
        {
            if ((remaining <= 0) || comps.TheObject.Material.IsReflective)
            {
                return CrtColor.COLOR_BLACK;
            }
            var reflectRay = CrtFactory.Ray(comps.OverPoint, comps.ReflectVector);
            var color = ColorAt(reflectRay, remaining -1);
            return color * comps.TheObject.Material.Reflective;
        }
    }
}
