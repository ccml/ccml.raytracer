using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ccml.raytracer.engine.core.Shapes;

namespace ccml.raytracer.engine.core
{
    public class CrtIntersection
    {
        public double T { get; private set; }
        public CrtShape TheObject { get; private set; }

        internal CrtIntersection(double t, CrtShape theObject)
        {
            T = t;
            TheObject = theObject;
        }
    }
}
