using System;
using ccml.raytracer.Core;

namespace ccml.raytracer.Materials.Patterns
{
    public class CrtRadialGradientPattern : CrtPattern
    {
        public CrtColor GradientA { get; private set; }
        public CrtColor GradientB { get; private set; }

        internal CrtRadialGradientPattern(CrtColor gradientA, CrtColor gradientB)
        {
            GradientA = gradientA;
            GradientB = gradientB;
        }

        public override CrtColor PatternAt(CrtPoint point)
        {
            var distance = GradientB - GradientA;
            var xzLength = Math.Sqrt(point.X * point.X + point.Z * point.Z);
            var fraction = xzLength - Math.Floor(xzLength);
            return GradientA + distance * fraction;
        }
    }
}
