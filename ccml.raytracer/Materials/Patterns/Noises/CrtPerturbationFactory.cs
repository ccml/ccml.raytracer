using System;
using System.Collections.Generic;
using ccml.raytracer.Core;

namespace ccml.raytracer.Materials.Patterns.Noises
{
    public class CrtPerturbationFactory
    {
        public Func<CrtPoint, Dictionary<double, CrtColor>, CrtColor> Perlin =
            (p, colors) =>
            {
                double r, g, b = 0;
                //
                var n = PerlinNoise.Noise(p.X, p.Y, p.Z);
                //
                double n1 = 0.0;
                double n2 = 0.0;
                CrtColor c1 = null;
                CrtColor c2 = null;
                //
                var colorsEnumerator = colors.GetEnumerator();
                var found = false;
                while (!found && colorsEnumerator.MoveNext())
                {
                    var akpColor = colorsEnumerator.Current;
                    if (n < akpColor.Key)
                    {
                        if (c1 == null)
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
                if ((c2 == null) || (CrtReal.AreEquals(n1, n2)))
                {
                    return c1;
                }
                //
                r = c1.Red * (n - n1) / (n2 - n1) + c2.Red * (n2 - n) / (n2 - n1);
                g = c1.Green * (n - n1) / (n2 - n1) + c2.Green * (n2 - n) / (n2 - n1);
                b = c1.Blue * (n - n1) / (n2 - n1) + c2.Blue * (n2 - n) / (n2 - n1);
                //
                return CrtFactory.CoreFactory.Color(r, g, b);
            };
    }
}
