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

        public CrtColor ColorAt(CrtRay r)
        {
            var intersections = new List<CrtIntersection>();
            foreach (var anObject in Objects)
            {
                intersections.AddRange(anObject.Intersect(r));
                intersections = CrtFactory.Intersections(intersections.ToArray());
            }
            var hit = intersections.FirstOrDefault(i => CrtReal.CompareTo(i.T, 0.0) > 0);
            if (hit == null)
            {
                return CrtFactory.Color(0, 0, 0);
            }
            else
            {
                var comps = CrtFactory.Engine().PrepareComputations(hit, r);
                var c = CrtFactory.Engine().ShadeHit(this, comps);
                return c;
            }
        }
    }
}
