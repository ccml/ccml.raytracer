using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ccml.raytracer.engine.core.Engine
{
    public class CrtEngine
    {
        internal CrtEngine()
        {
            
        }

        public CrtIntersection Hit(IList<CrtIntersection> intersections)
        {
            return intersections.FirstOrDefault(i => CrtReal.CompareTo(i.T, 0.0) >= 0);
        }
    }
}
