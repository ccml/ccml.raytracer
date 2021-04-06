using System;
using System.Collections.Generic;
using ccml.raytracer.Core;
using ccml.raytracer.Engine;
using ccml.raytracer.Materials;

namespace ccml.raytracer.Shapes
{
    public abstract class CrtShape
    {
        public CrtShape Parent { get; set; }

        public CrtBoundingBox ObjectBoundingBox { get; private set; }
        public CrtBoundingBox WorldBoundingBox { get; private set; }

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
                ResetBounds();
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
        public CrtVector NormalAt(CrtPoint point, CrtIntersection intersection = null)
        {
            var shapePoint = WorldToObject(point);
            var shapeNormal = LocalNormalAt(shapePoint, intersection);
            return NormalToWorld(shapeNormal);
        }

        /// <summary>
        /// Return the normal on the shape at a point
        /// </summary>
        /// <param name="point">A point on the shape</param>
        /// <returns>the normal</returns>
        public abstract CrtVector LocalNormalAt(CrtPoint point, CrtIntersection intersection = null);

        public CrtPoint WorldToObject(CrtPoint point)
        {
            if (!(Parent is null))
            {
                point = Parent.WorldToObject(point);
            }
            return InverseTransformMatrix * point;
        }

        public CrtVector NormalToWorld(CrtVector normal)
        {
            normal = TransposedInverseTransformMatrix * normal;
            normal.W = 0;
            normal = ~normal;
            if (!(Parent is null))
            {
                normal = Parent.NormalToWorld(normal);
            }

            return normal;
        }

        public virtual CrtBoundingBox ObjectBounds()
        {
            throw new NotImplementedException();
        }

        public void ResetBounds()
        {
            WorldBoundingBox = Bounds();
            if (!(Parent is null))
            {
                Parent.ResetBounds();
            }
        }

        public CrtBoundingBox Bounds()
        {
            var objectBounds = ObjectBounds();
            ObjectBoundingBox = objectBounds;
            if (objectBounds == null)
            {
                return null;
            }
            //
            CrtPoint[] edges = new CrtPoint[8];
            
            edges[0] = CrtFactory.CoreFactory.Point(objectBounds.Minimum.X, objectBounds.Minimum.Y, objectBounds.Minimum.Z);
            edges[1] = CrtFactory.CoreFactory.Point(objectBounds.Minimum.X, objectBounds.Minimum.Y, objectBounds.Maximum.Z);
            
            edges[2] = CrtFactory.CoreFactory.Point(objectBounds.Minimum.X, objectBounds.Maximum.Y, objectBounds.Minimum.Z);
            edges[3] = CrtFactory.CoreFactory.Point(objectBounds.Minimum.X, objectBounds.Maximum.Y, objectBounds.Maximum.Z);
            
            edges[4] = CrtFactory.CoreFactory.Point(objectBounds.Maximum.X, objectBounds.Minimum.Y, objectBounds.Minimum.Z);
            edges[5] = CrtFactory.CoreFactory.Point(objectBounds.Maximum.X, objectBounds.Minimum.Y, objectBounds.Maximum.Z);
            
            edges[6] = CrtFactory.CoreFactory.Point(objectBounds.Maximum.X, objectBounds.Maximum.Y, objectBounds.Minimum.Z);
            edges[7] = CrtFactory.CoreFactory.Point(objectBounds.Maximum.X, objectBounds.Maximum.Y, objectBounds.Maximum.Z);
            //
            for (int i = 0; i < 8; i++)
            {
                edges[i] = TransformMatrix * edges[i];
            }
            //
            var bbox = new CrtBoundingBox()
            {
                Minimum = CrtFactory.CoreFactory.Point(edges[0].X, edges[0].Y, edges[0].Z),
                Maximum = CrtFactory.CoreFactory.Point(edges[0].X, edges[0].Y, edges[0].Z)
            };
            //
            for (int i = 1; i < 8; i++)
            {
                bbox.Minimum.X = Math.Min(bbox.Minimum.X, edges[i].X);
                bbox.Minimum.Y = Math.Min(bbox.Minimum.Y, edges[i].Y);
                bbox.Minimum.Z = Math.Min(bbox.Minimum.Z, edges[i].Z);
                
                bbox.Maximum.X = Math.Max(bbox.Maximum.X, edges[i].X);
                bbox.Maximum.Y = Math.Max(bbox.Maximum.Y, edges[i].Y);
                bbox.Maximum.Z = Math.Max(bbox.Maximum.Z, edges[i].Z);
            }
            //
            return bbox;
        }

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
