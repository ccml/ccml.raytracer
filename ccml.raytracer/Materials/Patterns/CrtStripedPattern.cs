using System;
using ccml.raytracer.Core;

namespace ccml.raytracer.Materials.Patterns
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
