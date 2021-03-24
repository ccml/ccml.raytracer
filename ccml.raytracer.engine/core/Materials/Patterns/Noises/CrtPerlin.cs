using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ccml.raytracer.engine.core.Materials.Patterns.Noises
{
    public class CrtPerlin
    {
        private Dictionary<double, CrtColor> _colors;

        public CrtPerlin(Dictionary<double, CrtColor> colors)
        {
            _colors = colors;
        }

        protected virtual double Noise(CrtPoint p)
        {
            return PerlinNoise.Noise(p.X, p.Y, p.Z);
        }

        public CrtColor Perturbation(CrtPoint p)
        {
            double r, g, b = 0;
            //
            var n = Noise(p);
            var index = 0;
            //
            double n1 = 0.0;
            double n2 = 0.0;
            CrtColor c1 = null;
            CrtColor c2 = null;
            //
            var colorsEnumerator = _colors.GetEnumerator();
            var found = false;
            while (!found && colorsEnumerator.MoveNext())
            {
                var akpColor = colorsEnumerator.Current;
                if (n < akpColor.Key)
                {
                    if (c1 is null)
                    {
                        n1 = akpColor.Key;
                        c1 = akpColor.Value;

                    }
                    n2 = akpColor.Key;
                    c2 = akpColor.Value;
                    found = true;
                }
                else
                {
                    n1 = akpColor.Key;
                    c1 = akpColor.Value;
                }
            }
            if ((c2 is null) || (CrtReal.AreEquals(n1, n2)))
            {
                return c1;
            }
            //
            r = c1.Red * (n - n1) / (n2 - n1) + c2.Red * (n2 - n) / (n2 - n1);
            g = c1.Green * (n - n1) / (n2 - n1) + c2.Green * (n2 - n) / (n2 - n1);
            b = c1.Blue * (n - n1) / (n2 - n1) + c2.Blue * (n2 - n) / (n2 - n1);
            //
            return CrtFactory.Color(r, g, b);
        }
    }
}
