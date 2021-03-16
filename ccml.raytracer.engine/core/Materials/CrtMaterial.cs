using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ccml.raytracer.engine.core.Materials.Patterns;
using Microsoft.VisualBasic.CompilerServices;

namespace ccml.raytracer.engine.core.Materials
{
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
                _isReflective = CrtReal.AreEquals(_reflective, 0.0);
            }
        }

        private bool _isReflective = false;
        public bool IsReflective => _isReflective;

        /// <summary>
        /// Create a color material
        /// </summary>
        /// <param name="color">Color of the material</param>
        /// <param name="ambient">the % part of the reflected ambient light</param>
        /// <param name="diffuse">the % part of the reflected diffuse light</param>
        /// <param name="specular">the % part of the reflected specular light</param>
        /// <param name="shininess">+/- 10 very large highlight ==> +/- 200 very small highlight</param>
        internal CrtMaterial(CrtColor color, double ambient, double diffuse, double specular, double shininess, double reflective)
        {
            Color = color;
            Ambient = ambient;
            Diffuse = diffuse;
            Specular = specular;
            Shininess = shininess;
        }

        /// <summary>
        /// Create a pattern material
        /// </summary>
        /// <param name="pattern">Pattern of the material</param>
        /// <param name="ambient">the % part of the reflected ambient light</param>
        /// <param name="diffuse">the % part of the reflected diffuse light</param>
        /// <param name="specular">the % part of the reflected specular light</param>
        /// <param name="shininess">+/- 10 very large highlight ==> +/- 200 very small highlight</param>
        internal CrtMaterial(CrtPattern pattern, double ambient, double diffuse, double specular, double shininess)
        {
            Pattern = pattern;
            Ambient = ambient;
            Diffuse = diffuse;
            Specular = specular;
            Shininess = shininess;
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
    }
}
