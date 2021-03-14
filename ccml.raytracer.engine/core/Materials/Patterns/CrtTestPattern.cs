using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ccml.raytracer.engine.core.Materials.Patterns
{
    public class CrtTestPattern : CrtPattern
    {
        public override CrtColor PatternAt(CrtPoint point)
        {
            return CrtFactory.Color(point.X, point.Y, point.Z);
        }
    }
}
