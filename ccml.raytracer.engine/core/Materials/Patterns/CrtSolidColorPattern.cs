using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ccml.raytracer.engine.core.Materials.Patterns
{
    public class CrtSolidColorPattern : CrtPattern
    {
        public CrtColor Color { get; private set; }

        internal CrtSolidColorPattern(CrtColor color)
        {
            Color = color;
        }

        public override CrtColor PatternAt(CrtPoint point)
        {
            return Color;
        }
    }
}
