using ccml.raytracer.Core;
using ccml.raytracer.Materials.Patterns.Noises;

namespace ccml.raytracer.Materials.Patterns
{
    public class CrtPerlinNoisePattern : CrtPattern
    {
        private CrtPerlin _perlinBasePertubation;

        public CrtPerlinNoisePattern(CrtPerlin perlinBasePertubation)
        {
            _perlinBasePertubation = perlinBasePertubation;
        }


        public override CrtColor PatternAt(CrtPoint point)
        {
            return _perlinBasePertubation.Perturbation(point);
        }
    }
}
