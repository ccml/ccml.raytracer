using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ccml.raytracer.engine.core.Shapes
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
            return point - CrtFactory.Point(0, 0, 0);
        }
    }
}
