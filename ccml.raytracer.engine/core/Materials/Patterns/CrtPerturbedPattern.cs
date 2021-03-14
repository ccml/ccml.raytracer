using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ccml.raytracer.engine.core.Materials.Patterns
{
    public class CrtPerturbedPattern : CrtPattern
    {
        public Func<CrtPoint, CrtPoint> PerturbationFunction { get; private set; }
        public CrtPattern Pattern { get; private set; }

        public CrtPerturbedPattern(CrtPattern pattern, Func<CrtPoint, CrtPoint> perturbationFunction)
        {
            Pattern = pattern;
            PerturbationFunction = perturbationFunction;
        }
        
        public override CrtColor PatternAt(CrtPoint point)
        {
            return Pattern.PatternAt(PerturbationFunction(point));
        }
    }
}
