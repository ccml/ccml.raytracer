using System.Collections.Generic;
using ccml.raytracer.Core;
using ccml.raytracer.Engine;
using ccml.raytracer.Shapes;

namespace ccml.raytracer.Tests
{
    public class CrtTestShape : CrtShape
    {
        public CrtRay SavedRay { get; private set; }

        public override IList<CrtIntersection> LocalIntersect(CrtRay r)
        {
            SavedRay = r;
            return new List<CrtIntersection> ()
            { 
                new CrtIntersection(1.0, this)
            };
        }

        public override CrtVector LocalNormalAt(CrtPoint point)
        {
            return point - CrtFactory.CoreFactory.Point(0, 0, 0);
        }
    }
}
