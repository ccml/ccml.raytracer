using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ccml.raytracer.engine.core.Materials.Patterns.Noises
{
    public class CrtMarble : CrtPerlin
    {
        public CrtMarble(Dictionary<double, CrtColor> colors) : base(colors) { }

        protected override double Noise(CrtPoint p)
        {
            return Math.Cos(p.X + PerlinNoise.Noise(p.X, p.Y, p.Z));
        }
    }
}
