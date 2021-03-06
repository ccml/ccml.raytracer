using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ccml.raytracer.engine.core.Shapes
{
    public abstract class CrtShape
    {
        internal CrtShape()
        {
            
        }

        /// <summary>
        /// Returns the list of times 't' where the ray hit the shape
        /// </summary>
        /// <param name="r">the ray</param>
        /// <returns>list of times 't' where the ray hit the shape</returns>
        public abstract IList<CrtIntersection> Intersect(CrtRay r);
    }
}
