using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ccml.raytracer.engine.core.Materials
{
    /// <summary>
    /// Material having an uniform color
    /// </summary>
    public class CrtUniformColorMaterial : CrtMaterial
    {
        public CrtColor Color { get; set; }

        internal CrtUniformColorMaterial(
            CrtColor color,
            double ambient, 
            double diffuse, 
            double specular, 
            double shininess
        ) : base(ambient, diffuse, specular, shininess)
        {
            Color = color;
        }

        protected override bool SpecificEquals(CrtMaterial m)
        {
            if (!(m is CrtUniformColorMaterial)) return false;
            return ((CrtUniformColorMaterial) m).Color == Color;
        }
    }
}
