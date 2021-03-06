using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ccml.raytracer.engine.core
{
    /// <summary>
    /// Represent a ray
    /// </summary>
    public class CrtRay
    {
        /// <summary>
        /// Origin of the ray
        /// </summary>
        public CrtPoint Origin { get; private set; }
        /// <summary>
        /// Direction of the ray
        /// </summary>
        public CrtVector Direction { get; private set; }
        
        internal CrtRay(CrtPoint origin, CrtVector direction)
        {
            Origin = origin;
            Direction = direction;
        }

        /// <summary>
        /// Return the position of the ray after 't' unit of time
        /// </summary>
        /// <param name="t">the number of unit of time</param>
        /// <returns>The position of the ray</returns>
        public CrtPoint PositionAtTime(double t)
        {
            return Origin + (Direction * t);
        }
    }
}
