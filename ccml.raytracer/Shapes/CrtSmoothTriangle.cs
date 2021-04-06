using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ccml.raytracer.Core;
using ccml.raytracer.Engine;

namespace ccml.raytracer.Shapes
{
    public class CrtSmoothTriangle : CrtTriangle
    {
        internal CrtSmoothTriangle(
            CrtPoint p1, CrtPoint p2, CrtPoint p3,
            CrtVector n1, CrtVector n2, CrtVector n3
        ) : base(p1, p2, p3)
        {
            N1 = n1;
            N2 = n2;
            N3 = n3;
        }

        public CrtVector N1 { get; }
        public CrtVector N2 { get; }
        public CrtVector N3 { get; }

        public override CrtVector LocalNormalAt(CrtPoint point, CrtIntersection intersection = null)
        {
            return N2 * intersection.U + N3 * intersection.V + N1 * (1 - intersection.U - intersection.V);
        }
    }
}
