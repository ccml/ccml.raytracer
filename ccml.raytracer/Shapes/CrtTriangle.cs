using ccml.raytracer.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ccml.raytracer.Engine;

namespace ccml.raytracer.Shapes
{
    public class CrtTriangle : CrtShape
    {
        internal CrtTriangle(CrtPoint p1, CrtPoint p2, CrtPoint p3)
        {
            P1 = p1;
            P2 = p2;
            P3 = p3;
            Setup();
            ResetBounds();
        }

        private void Setup()
        {
            E1 = P2 - P1;
            E2 = P3 - P1;
            Normal = ~(E2^E1);
        }

        public CrtPoint P1 { get; }
        public CrtPoint P2 { get; }
        public CrtPoint P3 { get; }

        /// <summary>
        /// Edge vector from P1 to P2
        /// </summary>
        public CrtVector E1 { get; set; }
        /// <summary>
        /// Edge vector from P1 to P3
        /// </summary>
        public CrtVector E2 { get; set; }
        /// <summary>
        /// Normal to the triangle
        /// </summary>
        public CrtVector Normal { get; set; }

        public override IList<CrtIntersection> LocalIntersect(CrtRay r)
        {
            var dirCrossE2 = r.Direction ^ E2;
            var det = E1 * dirCrossE2;
            //
            // ray is parallel triangle
            if (CrtReal.AreEquals(det, 0)) return new List<CrtIntersection>();
            //
            var f = 1.0 / det;
            //
            var p1ToOrigin = r.Origin - P1;
            var u = f * (p1ToOrigin * dirCrossE2);
            //
            // ray miss (P1-P3) edge
            if ((CrtReal.CompareTo(u, 0) < 0) || (CrtReal.CompareTo(u, 1) > 0)) return new List<CrtIntersection>();
            //
            var originCrossE1 = p1ToOrigin ^ E1;
            var v = f * (r.Direction * originCrossE1);
            //
            // ray miss (P1-P2) and (P2-P3) edges
            if ((CrtReal.CompareTo(v, 0) < 0) || (CrtReal.CompareTo(u+v, 1) > 0)) return new List<CrtIntersection>();
            //
            var t = f * (E2 * originCrossE1);
            return CrtFactory.EngineFactory.Intersections(CrtFactory.EngineFactory.Intersection(t, this));
        }

        public override CrtVector LocalNormalAt(CrtPoint point)
        {
            return Normal;
        }

        public override CrtBoundingBox ObjectBounds()
        {
            // first time called by the constructor of parent and P1 is not yet set
            if (P1 is null) return null; 
            //
            return new CrtBoundingBox()
            {
                Minimum = CrtFactory.CoreFactory.Point(
                    Math.Min(Math.Min(P1.X, P2.X), P3.X) - 0.001,
                    Math.Min(Math.Min(P1.Y, P2.Y), P3.Y) - 0.001,
                    Math.Min(Math.Min(P1.Z, P2.Z), P3.Z) - 0.001
                ),
                Maximum = CrtFactory.CoreFactory.Point(
                    Math.Max(Math.Max(P1.X, P2.X), P3.X) + 0.001,
                    Math.Max(Math.Max(P1.Y, P2.Y), P3.Y) + 0.001,
                    Math.Max(Math.Max(P1.Z, P2.Z), P3.Z) + 0.001
                )
            };
        }
    }
}
