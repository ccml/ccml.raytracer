using System;
using System.Collections.Generic;
using System.Linq;
using ccml.raytracer.Core;
using ccml.raytracer.Lights;
using ccml.raytracer.Shapes;

namespace ccml.raytracer.Engine
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
                intersections = CrtFactory.EngineFactory.Intersections(intersections.ToArray());
            }

            return intersections;
        }

        private CrtIntersection Hit(List<CrtIntersection> intersections, bool forShadow = false)
        {
            return intersections.FirstOrDefault(i => (!forShadow || !i.TheObject.Material.IsTransparent) && CrtReal.CompareTo(i.T, 0.0) > 0);
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
                var comps = CrtFactory.EngineFactory.Engine().PrepareComputations(hit, r);
                var c = CrtFactory.EngineFactory.Engine().ShadeHit(this, comps, remaining);
                return c;
            }
        }

        public bool IsShadowed(CrtPoint point)
        {
            var v = Lights[0].Position - point;
            var distance = !v;
            var direction = ~v;
            var r = CrtFactory.EngineFactory.Ray(point, direction);
            var intersections = Intersect(r);
            var h = Hit(intersections, forShadow: true);
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
        public CrtColor ReflectedColor(CrtIntersectionComputation comps, int remaining = 5)
        {
            if ((remaining <= 0) || (!comps.TheObject.Material.IsReflective))
            {
                return CrtColor.COLOR_BLACK;
            }
            var reflectRay = CrtFactory.EngineFactory.Ray(comps.OverPoint, comps.ReflectVector);
            var color = ColorAt(reflectRay, remaining -1);
            return color * comps.TheObject.Material.Reflective;
        }

        public CrtColor RefractedColor(CrtIntersectionComputation comps, int remaining = 5)
        {
            // not transparent or max depth reached
            if ((remaining <= 0) || (!comps.TheObject.Material.IsTransparent))
            {
                return CrtColor.COLOR_BLACK;
            }
            //
            // total internal reflection
            var nRatio = comps.N1 / comps.N2;
            var cosI = comps.EyeVector * comps.NormalVector;
            var sin2T = nRatio * nRatio * (1 - cosI * cosI);
            if (CrtReal.CompareTo(sin2T, 1.0) > 0)
            {
                return CrtColor.COLOR_BLACK;
            }
            //
            var cosT = Math.Sqrt(1.0 - sin2T);
            var direction = comps.NormalVector * (nRatio * cosI - cosT) - comps.EyeVector * nRatio;
            var refractRay = CrtFactory.EngineFactory.Ray(comps.UnderPoint, direction);
            var color = ColorAt(refractRay, remaining - 1);
            return color * comps.TheObject.Material.Transparency;
        }
    }
}
