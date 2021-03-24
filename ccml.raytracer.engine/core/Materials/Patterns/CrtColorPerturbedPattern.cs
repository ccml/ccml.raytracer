using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ccml.raytracer.engine.core.Materials.Patterns
{
    public class CrtColorPerturbedPattern : CrtPattern
    {
        public Func<CrtPoint, CrtColor, CrtColor> PerturbationFunction { get; private set; }
        public CrtPattern Pattern { get; private set; }

        internal CrtColorPerturbedPattern(CrtPattern pattern, Func<CrtPoint, CrtColor, CrtColor> perturbationFunction)
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
