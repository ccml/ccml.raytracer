using System.Collections.Generic;
using ccml.raytracer.Core;

namespace ccml.raytracer.Engine
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

        /// <summary>
        /// Return the rays resulting of the applying of a transformation matrix on the current ray
        /// </summary>
        /// <param name="transformationMatrix">the transformation matrix</param>
        /// <returns>the resulting ray</returns>
        public CrtRay Transform(CrtMatrix transformationMatrix)
        {
            return CrtFactory.EngineFactory.Ray(transformationMatrix * Origin, transformationMatrix * Direction);
        }

        public IList<CrtIntersection> Intersect(CrtWorld w)
        {
            var intersections = new List<CrtIntersection>();
            foreach (var wObject in w.Objects)
            {
                var wIntersections = wObject.Intersect(this);
                intersections.AddRange(wIntersections);
            }
            return CrtFactory.EngineFactory.Intersections(intersections.ToArray());
        }
    }
}
