using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualBasic.CompilerServices;

namespace ccml.raytracer.engine.core.Materials
{
    public abstract class CrtMaterial
    {
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

        /// <summary>
        /// Create a material
        /// </summary>
        /// <param name="ambient">the % part of the reflected ambient light</param>
        /// <param name="diffuse">the % part of the reflected diffuse light</param>
        /// <param name="specular">the % part of the reflected specular light</param>
        /// <param name="shininess">+/- 10 very large highlight ==> +/- 200 very small highlight</param>
        internal CrtMaterial(double ambient, double diffuse, double specular, double shininess)
        {
            Ambient = ambient;
            Diffuse = diffuse;
            Specular = specular;
            Shininess = shininess;
        }

        public static bool operator ==(CrtMaterial m1, CrtMaterial m2)
        {
            if (m1 is null) throw new ArgumentException();
            if (m2 is null) throw new ArgumentException();
            return m1.SpecificEquals(m2)
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
            return !m1.SpecificEquals(m2)
                   ||
                   !CrtReal.AreEquals(m1.Ambient, m2.Ambient)
                   ||
                   !CrtReal.AreEquals(m1.Diffuse, m2.Diffuse)
                   ||
                   !CrtReal.AreEquals(m1.Specular, m2.Specular)
                   ||
                   !CrtReal.AreEquals(m1.Shininess, m2.Shininess);
        }

        protected abstract bool SpecificEquals(CrtMaterial m);

        protected bool Equals(CrtMaterial other)
        {
            return _ambient.Equals(other._ambient) && _diffuse.Equals(other._diffuse) && _specular.Equals(other._specular) && _shininess.Equals(other._shininess);
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
            return HashCode.Combine(_ambient, _diffuse, _specular, _shininess);
        }
    }
}
