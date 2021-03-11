using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using ccml.raytracer.engine.core.Materials;

namespace ccml.raytracer.engine.core.Shapes
{
    public abstract class CrtShape
    {
        public CrtMaterial Material { get; set; }

        private CrtMatrix _transformMatrix;
        // The transformation matrix to the overall world
        public CrtMatrix TransformMatrix
        {
            get => _transformMatrix;
            set
            {
                _transformMatrix = value;
                InverseTransformMatrix = _transformMatrix.Inverse();
                TransposedInverseTransformMatrix = InverseTransformMatrix.Transpose();
            }
        }

        /// <summary>
        /// The transformation matrix to the shape world
        /// </summary>
        public CrtMatrix InverseTransformMatrix { get; private set; }

        /// <summary>
        /// The transformation matrix to the overall world
        /// </summary>
        public CrtMatrix TransposedInverseTransformMatrix { get; private set; }

        internal CrtShape()
        {
            Material = CrtFactory.UniformColorMaterial();
            TransformMatrix = CrtFactory.IdentityMatrix(4, 4);
        }

        /// <summary>
        /// Returns the list of times 't' where the ray hit the shape
        /// </summary>
        /// <param name="r">the ray</param>
        /// <returns>list of times 't' where the ray hit the shape</returns>
        public abstract IList<CrtIntersection> Intersect(CrtRay r);

        /// <summary>
        /// Return the normal on the shape at a point
        /// </summary>
        /// <param name="point">A point on the shape</param>
        /// <returns>the normal</returns>
        public abstract CrtVector NormalAt(CrtPoint point);

        public static bool operator ==(CrtShape s1, CrtShape s2)
        {
            if (s1 is null) throw new ArgumentException();
            if (s2 is null) throw new ArgumentException();
            return s1.TransformMatrix == s2.TransformMatrix
                   && 
                   s1.Material == s2.Material;
        }

        public static bool operator !=(CrtShape s1, CrtShape s2)
        {
            if (s1 is null) throw new ArgumentException();
            if (s2 is null) throw new ArgumentException();
            return s1.TransformMatrix != s2.TransformMatrix
                   ||
                   s1.Material != s2.Material;
        }

        protected bool Equals(CrtShape other)
        {
            return Equals(_transformMatrix, other._transformMatrix) && Equals(Material, other.Material);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((CrtShape) obj);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(_transformMatrix, Material);
        }
    }
}
