using System;
using ccml.raytracer.Core;

namespace ccml.raytracer.Materials.Patterns
{
    public class CrtPointPerturbedPattern : CrtPattern
    {
        public Func<CrtPoint, CrtPoint> PerturbationFunction { get; private set; }
        public CrtPattern Pattern { get; private set; }

        internal CrtPointPerturbedPattern(CrtPattern pattern, Func<CrtPoint, CrtPoint> perturbationFunction)
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
