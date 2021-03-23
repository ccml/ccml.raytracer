using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ccml.raytracer.engine.core.Shapes;

namespace ccml.raytracer.engine.core.Materials.Patterns
{
    public abstract class CrtPattern
    {
        private CrtMatrix _transformMatrix;
        // The transformation matrix of the pattern to to the object coordinates
        public CrtMatrix TransformMatrix
        {
            get => _transformMatrix;
            set
            {
                _transformMatrix = value;
                InverseTransformMatrix = _transformMatrix.Inverse();
            }
        }

        /// <summary>
        /// The transformation matrix to the pattern coordinates
        /// </summary>
        public CrtMatrix InverseTransformMatrix { get; private set; }

        internal CrtPattern()
        {
            TransformMatrix = CrtFactory.IdentityMatrix(4, 4);
        }

        public CrtColor PatternAt(CrtShape theObject, CrtPoint point)
        {
            var objectPoint = theObject.InverseTransformMatrix * point;
            var patternPoint = InverseTransformMatrix * objectPoint;
            return PatternAt(patternPoint);
        }

        public abstract CrtColor PatternAt(CrtPoint point);

        public CrtPattern WithTransformMatrix(CrtMatrix transformMatrix)
        {
            TransformMatrix = transformMatrix;
            return this;
        }
    }
}
