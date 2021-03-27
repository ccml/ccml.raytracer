using System.Collections.Generic;
using ccml.raytracer.Core;
using ccml.raytracer.Engine;

namespace ccml.raytracer.Shapes
{
    public class CrtPlane : CrtShape
    {
        private CrtVector _planeNormal = CrtFactory.CoreFactory.Vector(0, 1, 0);

        public override IList<CrtIntersection> LocalIntersect(CrtRay r)
        {
            if (CrtReal.AreEquals(r.Direction.Y, 0.0))
            {
                return new List<CrtIntersection>();
            }

            return new List<CrtIntersection>
            {
                new CrtIntersection(
                    -r.Origin.Y / r.Direction.Y,
                    this
                )
            };
        }

        public override CrtVector LocalNormalAt(CrtPoint point)
        {
            return _planeNormal;
        }
    }
}
