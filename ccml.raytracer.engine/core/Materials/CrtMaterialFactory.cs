using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ccml.raytracer.engine.core.Materials
{
    public class CrtMaterialFactory
    {
        internal CrtMaterialFactory()
        {
            
        }

        /// <summary>
        /// Create an uniform color material
        /// </summary>
        /// <param name="color">the color of the surface</param>
        /// <param name="ambient">the % part of the reflected ambient light</param>
        /// <param name="diffuse">the % part of the reflected diffuse light</param>
        /// <param name="specular">the % part of the reflected specular light</param>
        /// <param name="shininess">+/- 10 very large highlight ==> +/- 200 very small highlight</param>
        /// <param name="reflective">say how many a material is reflective (0.0 = not reflective at all, 1.0 = perfect mirror)</param>
        /// <param name="transparency">say how many a material is transparent (0.0 = opaque, 1.0 = 100% transparent)</param>
        /// <param name="refractiveIndex">determines the degree to which light will bend when entering or exiting the material, compared to other materials</param>
        /// <returns>the material</returns>
        public CrtMaterial SpecificMaterial(CrtColor color, double ambient = 0.1, double diffuse = 0.9, double specular = 0.9,
            double shininess = 200, double reflective = 0.0, double transparency = 0.0, double refractiveIndex = 1.0)
            => new CrtMaterial(color, ambient, diffuse, specular, shininess, reflective, transparency, refractiveIndex);

        /// <summary>
        /// Create a default material
        ///     : color = white (no patterns)
        ///     : ambient = 0.1
        ///     : diffuse = 0.9
        ///     : specular = 0.9
        ///     : shininess = 200
        ///     : reflective = 0.0
        ///     : transparency = 0.0
        ///     : refractiveIndex = 1.0
        /// </summary>
        /// <returns>the material</returns>
        public CrtMaterial DefaultMaterial => SpecificMaterial(CrtColor.COLOR_WHITE);

        /// <summary>
        /// Create a default air
        ///     : color = white (no patterns)
        ///     : ambient = 0.1
        ///     : diffuse = 0.9
        ///     : specular = 0.9
        ///     : shininess = 200
        ///     : reflective = 0.0
        ///     : transparency = 1.0
        ///     : refractiveIndex = 1.00029
        /// </summary>
        /// <returns>the material</returns>
        public CrtMaterial Air => SpecificMaterial(CrtColor.COLOR_WHITE, transparency: 1.0, refractiveIndex: 1.00029);

        /// <summary>
        /// Create a default water
        ///     : color = white (no patterns)
        ///     : ambient = 0.1
        ///     : diffuse = 0.9
        ///     : specular = 0.9
        ///     : shininess = 200
        ///     : reflective = 0.0
        ///     : transparency = 1.0
        ///     : refractiveIndex = 1.333
        /// </summary>
        /// <returns>the material</returns>
        public CrtMaterial Water => SpecificMaterial(CrtColor.COLOR_WHITE, transparency: 1.0, refractiveIndex: 1.333);

        /// <summary>
        /// Create a default glass
        ///     : color = white (no patterns)
        ///     : ambient = 0.1
        ///     : diffuse = 0.9
        ///     : specular = 0.9
        ///     : shininess = 200
        ///     : reflective = 0.0
        ///     : transparency = 1.0
        ///     : refractiveIndex = 1.5
        /// </summary>
        /// <returns>the material</returns>
        public CrtMaterial Glass => SpecificMaterial(CrtColor.COLOR_WHITE, transparency: 1.0, refractiveIndex: 1.5);
    }
}
