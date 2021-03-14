using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ccml.raytracer.engine.core.Shapes;

namespace ccml.raytracer.engine.core.Materials.Patterns
{
    public class CrtStripedPattern : CrtPattern
    {
        public CrtColor ColorA { get; private set; }
        public CrtColor ColorB { get; private set; }

        internal CrtStripedPattern(CrtColor colorA, CrtColor colorB)
        {
            ColorA = colorA;
            ColorB = colorB;
        }

        public override CrtColor PatternAt(CrtPoint point)
        {
            return CrtReal.AreEquals(Math.Floor(point.X) % 2.0, 0.0) ? ColorA : ColorB;
        }
    }
}
