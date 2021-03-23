using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ccml.raytracer.engine.core.Materials.Patterns;
using Microsoft.VisualBasic.CompilerServices;

namespace ccml.raytracer.engine.core.Materials
{
    /// <summary>
    /// Tips :
    /// 
    ///     - When rendering glass or any similar material, set both transparency and reflectivity to high values, 0.9 or even 1.
    ///       This allows the Fresnel effect to kick in, and gives your material an added touch of realism!
    ///
    ///     - The more transparent or reflective the surface, the smaller the diffuse property should be.
    ///
    ///     - If you’d like a subtly colored mirror, or slightly tinted glass, use a very dark color, instead of a very light one.
    ///
    ///     - Reflective and transparent surfaces pair nicely with tight specular highlights. Set specular to 1 and bump shininess
    ///       to 300 or more to get a highlight that really shines.
    /// 
    /// </summary>
    public class CrtMaterial
    {
        public CrtColor Color { get; set; }

        public CrtPattern Pattern { get; set; }

        private double _ambient;
        public double Ambient
        {
            get => _ambient;
            set
            {
                if (CrtReal.CompareTo(value, 0.0) < 0) throw new ArgumentException();
                if (CrtReal.CompareTo(value, 1.0) > 0) throw new ArgumentException();
                _ambient = value;
            }
        }

        private double _diffuse;
        public double Diffuse
        {
            get => _diffuse;
            set
            {
                if (CrtReal.CompareTo(value, 0.0) < 0) throw new ArgumentException();
                if (CrtReal.CompareTo(value, 1.0) > 0) throw new ArgumentException();
                _diffuse = value;
            }
        }

        private double _specular;
        public double Specular
        {
            get => _specular;
            set
            {
                if (CrtReal.CompareTo(value, 0.0) < 0) throw new ArgumentException();
                if (CrtReal.CompareTo(value, 1.0) > 0) throw new ArgumentException();
                _specular = value;
            }
        }

        private double _shininess;

        public double Shininess
        {
            get => _shininess;
            set
            {
                if (CrtReal.CompareTo(value, 0.0) < 0) throw new ArgumentException();
                _shininess = value;
            }
        }

        private double _reflective;
        public double Reflective
        {
            get => _reflective;
            set
            {
                if (CrtReal.CompareTo(value, 0.0) < 0) throw new ArgumentException();
                _reflective = value;
                _isReflective = !CrtReal.AreEquals(_reflective, 0.0);
            }
        }

        private double _transparency;
        public double Transparency
        {
            get => _transparency;
            set
            {
                if (CrtReal.CompareTo(value, 0.0) < 0) throw new ArgumentException();
                _transparency = value;
                _isTransparent = !CrtReal.AreEquals(_transparency, 0.0);
            }
        }

        private double _refractiveIndex;
        public double RefractiveIndex
        {
            get => _refractiveIndex;
            set
            {
                if (CrtReal.CompareTo(value, 0.0) < 0) throw new ArgumentException();
                _refractiveIndex = value;
            }
        }

        private bool _isReflective = false;
        public bool IsReflective => _isReflective;

        private bool _isTransparent = false;
        public bool IsTransparent => _isTransparent;

        /// <summary>
        /// Create a color material
        /// </summary>
        /// <param name="color">Color of the material</param>
        /// <param name="ambient">the % part of the reflected ambient light</param>
        /// <param name="diffuse">the % part of the reflected diffuse light</param>
        /// <param name="specular">the % part of the reflected specular light</param>
        /// <param name="shininess">+/- 10 very large highlight ==> +/- 200 very small highlight</param>
        internal CrtMaterial(CrtColor color, double ambient, double diffuse, double specular, double shininess, double reflective, double transparency, double refractiveIndex)
        {
            Color = color;
            Ambient = ambient;
            Diffuse = diffuse;
            Specular = specular;
            Shininess = shininess;
            Reflective = reflective;
            Transparency = transparency;
            RefractiveIndex = refractiveIndex;
        }

        public bool HasPattern => Pattern != null;

        public static bool operator ==(CrtMaterial m1, CrtMaterial m2)
        {
            if (m1 is null) throw new ArgumentException();
            if (m2 is null) throw new ArgumentException();
            return m1.HasPattern == m2.HasPattern
                   &&
                   (
                       (!m1.HasPattern && (m1.Color == m2.Color))
                       ||
                       (m1.HasPattern && (m1.Pattern == m2.Pattern))
                   )
                   &&
                   CrtReal.AreEquals(m1.Ambient, m2.Ambient)
                   &&
                   CrtReal.AreEquals(m1.Diffuse, m2.Diffuse)
                   &&
                   CrtReal.AreEquals(m1.Specular, m2.Specular)
                   &&
                   CrtReal.AreEquals(m1.Shininess, m2.Shininess);
        }

        public static bool operator !=(CrtMaterial m1, CrtMaterial m2)
        {
            if (m1 is null) throw new ArgumentException();
            if (m2 is null) throw new ArgumentException();
            return m1.HasPattern != m2.HasPattern
                   ||
                   (!m1.HasPattern && (m1.Color != m2.Color))
                   ||
                   (m1.HasPattern && (m1.Pattern != m2.Pattern))
                   ||
                   !CrtReal.AreEquals(m1.Ambient, m2.Ambient)
                   ||
                   !CrtReal.AreEquals(m1.Diffuse, m2.Diffuse)
                   ||
                   !CrtReal.AreEquals(m1.Specular, m2.Specular)
                   ||
                   !CrtReal.AreEquals(m1.Shininess, m2.Shininess);
        }

        protected bool Equals(CrtMaterial other)
        {
            return _ambient.Equals(other._ambient) && _diffuse.Equals(other._diffuse) && _specular.Equals(other._specular) && _shininess.Equals(other._shininess) && Equals(Color, other.Color) && Equals(Pattern, other.Pattern);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((CrtMaterial) obj);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(_ambient, _diffuse, _specular, _shininess, Color, Pattern);
        }

        // Fluent Mode
        
        public CrtMaterial WithColor(CrtColor color)
        {
            Color = color;
            return this;
        }

        public CrtMaterial WithPattern(CrtPattern pattern)
        {
            Pattern = pattern;
            return this;
        }

        public CrtMaterial WithAmbient(double ambient)
        {
            Ambient = ambient;
            return this;
        }

        public CrtMaterial WithDiffuse(double diffuse)
        {
            Diffuse = diffuse;
            return this;
        }

        public CrtMaterial WithSpecular(double specular)
        {
            Specular = specular;
            return this;
        }

        public CrtMaterial WithShininess(double shininess)
        {
            Shininess = shininess;
            return this;
        }

        public CrtMaterial WithReflective(double reflective)
        {
            Reflective = reflective;
            return this;
        }

        public CrtMaterial WithTransparency(double transparency)
        {
            Transparency = transparency;
            return this;
        }

        public CrtMaterial WithRefractiveIndex(double refractiveIndex)
        {
            RefractiveIndex = refractiveIndex;
            return this;
        }
    }
}
