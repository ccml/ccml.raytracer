using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ccml.raytracer.engine.core.Materials.Patterns
{
    public class CrtGradientPattern : CrtPattern
    {
        public CrtColor GradientA { get; private set; }
        public CrtColor GradientB { get; private set; }

        internal CrtGradientPattern(CrtColor gradientA, CrtColor gradientB)
        {
            GradientA = gradientA;
            GradientB = gradientB;
        }

        public override CrtColor PatternAt(CrtPoint point)
        {
            var distance = GradientB - GradientA;
            var fraction = point.X - Math.Floor(point.X);
            return GradientA + distance * fraction;
        }
    }
}
