using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ccml.raytracer.engine.core.Materials.Patterns
{
    public class CrtPerturbedColorPattern : CrtPattern
    {
        public Func<CrtPoint, CrtColor, CrtColor> PerturbationFunction { get; private set; }
        public CrtPattern Pattern { get; private set; }

        public CrtPerturbedColorPattern(CrtPattern pattern, Func<CrtPoint, CrtColor, CrtColor> perturbationFunction)
        {
            Pattern = pattern;
            PerturbationFunction = perturbationFunction;
        }

        public override CrtColor PatternAt(CrtPoint point)
        {
            return PerturbationFunction(point, Pattern.PatternAt(point));
        }
    }
}
