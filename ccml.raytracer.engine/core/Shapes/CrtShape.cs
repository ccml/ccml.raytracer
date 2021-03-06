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
            }
        }

        /// <summary>
        /// The transforamtion matrix to the shape world
        /// </summary>
        public CrtMatrix InverseTransformMatrix { get; private set; }

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
    }
}
