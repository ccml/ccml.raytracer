using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ccml.raytracer.math.core
{
    /// <summary>
    /// A point in a 3D space.
    /// 
    /// NB) A point is a tuple with W = 1.0
    /// </summary>
    public class CrtPoint : CrtTuple
    {
        public CrtPoint(double x, double y, double z) : base(x, y, z, 1.0)
        {
        }
    }
}
