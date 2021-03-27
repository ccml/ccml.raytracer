using System;
using System.Collections.Generic;
using ccml.raytracer.Core;
using ccml.raytracer.Engine;
using ccml.raytracer.Materials;

namespace ccml.raytracer.Shapes
{
    public abstract class CrtShape
    {
        public CrtMaterial Material { get; set; }

        private CrtMatrix _transformMatrix;
        // The transformation matrix of the object to the overall world
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
            Material = CrtFactory.MaterialFactory.DefaultMaterial;
            TransformMatrix = CrtFactory.TransformationFactory.IdentityMatrix(4, 4);
        }

        /// <summary>
        /// Returns the list of times 't' where the ray hit the shape
        /// </summary>
        /// <param name="r">the ray</param>
        /// <returns>list of times 't' where the ray hit the shape</returns>
        public IList<CrtIntersection> Intersect(CrtRay r)
        {
            // !!! The intersection must be done with the ray 
            //     transformed in the shape world !!!
            //
            // The ray in the shape world
            var localRay = r.Transform(InverseTransformMatrix);
            //
            return LocalIntersect(localRay);
        }

        /// <summary>
        /// Returns the list of times 't' where the ray hit the shape
        /// </summary>
        /// <param name="r">the ray in shape world coordinates</param>
        /// <returns>list of times 't' where the ray hit the shape</returns>
        public abstract IList<CrtIntersection> LocalIntersect(CrtRay r);

        /// <summary>
        /// Return the normal on the shape at a point
        /// </summary>
        /// <param name="point">A point on the shape in shape world coordinates</param>
        /// <returns>the normal</returns>
        public CrtVector NormalAt(CrtPoint point)
        {
            var shapePoint = InverseTransformMatrix * point;
            var shapeNormal = LocalNormalAt(shapePoint);
            var worldNormal = TransposedInverseTransformMatrix * ((CrtTuple)shapeNormal);
            worldNormal.W = 0.0;
            return ~CrtFactory.CoreFactory.Vector(worldNormal.X, worldNormal.Y, worldNormal.Z);
        }

        /// <summary>
        /// Return the normal on the shape at a point
        /// </summary>
        /// <param name="point">A point on the shape</param>
        /// <returns>the normal</returns>
        public abstract CrtVector LocalNormalAt(CrtPoint point);

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

        // Fluent mode

        public CrtShape WithMaterial(CrtMaterial material)
        {
            this.Material = material;
            return this;
        }

        public CrtShape WithTransformationMatrix(CrtMatrix transformMatrix)
        {
            this.TransformMatrix = transformMatrix;
            return this;
        }
    }
}
