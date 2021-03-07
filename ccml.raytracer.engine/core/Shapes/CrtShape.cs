using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace ccml.raytracer.engine.core.Shapes
{
    public abstract class CrtShape
    {
        private CrtMatrix _transformMatrix;
        // The transformation matrix to the overall world
        public CrtMatrix TransformMatrix
        {
            get => _transformMatrix;
            private set
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
            TransformMatrix = CrtFactory.IdentityMatrix(4, 4);
        }

        /// <summary>
        /// Returns the list of times 't' where the ray hit the shape
        /// </summary>
        /// <param name="r">the ray</param>
        /// <returns>list of times 't' where the ray hit the shape</returns>
        public abstract IList<CrtIntersection> Intersect(CrtRay r);

        /// <summary>
        /// Set the transformation matrix for the shape
        /// </summary>
        /// <param name="transformMatrix"></param>
        public void SetTransformMatrix(CrtMatrix transformMatrix)
        {
            TransformMatrix = transformMatrix;
        }

        /// <summary>
        /// Return the normal on the shape at a point
        /// </summary>
        /// <param name="point">A point on the shape</param>
        /// <returns>the normal</returns>
        public abstract CrtVector NormalAt(CrtPoint point);
    }
}
