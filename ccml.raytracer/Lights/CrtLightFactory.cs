using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ccml.raytracer.Core;

namespace ccml.raytracer.Lights
{
    public class CrtLightFactory
    {
        /// <summary>
        /// Create a point light
        ///   A light source with no size, existing at a single point in space.It is also defined by its intensity,
        ///   or how bright it is. This intensity also describes the color of the light source.
        /// </summary>
        /// <param name="position">the position of a light</param>
        /// <param name="intensity">the intensity/color of the light</param>
        /// <returns>the point light</returns>
        public CrtPointLight PointLight(CrtPoint position, CrtColor intensity) =>
            new CrtPointLight(position, intensity);
    }
}
