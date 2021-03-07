using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ccml.raytracer.engine.core.Lights
{
    public class CrtPointLight
    {
        public CrtPoint Position { get; private set; }
        public CrtColor Intensity { get; private set; }

        internal CrtPointLight(CrtPoint position, CrtColor intensity)
        {
            Position = position;
            Intensity = intensity;
        }
    }
}
