using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ccml.raytracer.engine.core.Materials.Patterns.Noises;

namespace ccml.raytracer.engine.core.Materials.Patterns
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
