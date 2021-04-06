using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ccml.raytracer.Core;
using ccml.raytracer.Shapes;

namespace ccml.raytracer.Engine
{
    public class CrtEngineFactory
    {
        /// <summary>
        /// Create a Canvas
        /// </summary>
        /// <param name="width">Width in pixels</param>
        /// <param name="height">Height in pixels</param>
        /// <returns>The canvas</returns>
        public CrtCanvas Canvas(int width, int height) => new CrtCanvas(width, height);

        /// <summary>
        /// Create a ray
        /// </summary>
        /// <param name="origin">Origin of the ray</param>
        /// <param name="direction">Direction of the ray</param>
        /// <returns>The ray</returns>
        public CrtRay Ray(CrtPoint origin, CrtVector direction) => new CrtRay(origin, direction);

        /// <summary>
        /// Create an intersection describing where an object has been hit
        /// </summary>
        /// <param name="t">the number of unit of time needed by the ray to hit the object</param>
        /// <param name="theObject">the object that has been hit</param>
        /// <param name="u">triangle intersection parameter</param>
        /// <param name="v">triangle intersection parameter</param>
        /// <returns>the intersection</returns>
        public CrtIntersection Intersection(double t, CrtShape theObject, double u = 0.0, double v = 0.0) => new CrtIntersection(t, theObject, u, v);

        /// <summary>
        /// Create a list of hit
        /// </summary>
        /// <param name="xs">the hits</param>
        /// <returns>the list</returns>
        public List<CrtIntersection> Intersections(params CrtIntersection[] xs) => xs.OrderBy(i => i.T).ToList();

        /// <summary>
        /// Create the ray tracer engine
        /// </summary>
        /// <returns>the ray tracer engine</returns>
        public CrtEngine Engine() => new CrtEngine();

        /// <summary>
        /// Create an empty world
        /// </summary>
        /// <returns>The world</returns>
        public CrtWorld World()
        {
            return new CrtWorld();
        }

        /// <summary>
        /// Create a view transformation matrix
        /// </summary>
        /// <param name="from">Eye position</param>
        /// <param name="to">A point where the eye look at</param>
        /// <param name="up">Indicate which direction is up</param>
        /// <returns>The transformation matrix</returns>
        public CrtMatrix ViewTransformation(CrtPoint from, CrtPoint to, CrtVector up)
        {
            var forward = ~(to - from);
            var upn = ~up;
            var left = forward ^ upn;
            var trueUp = left ^ forward;
            var orientation = CrtFactory.CoreFactory.Matrix(
                new double[] { left.X, left.Y, left.Z, 0.0 },
                new double[] { trueUp.X, trueUp.Y, trueUp.Z, 0.0 },
                new double[] { -forward.X, -forward.Y, -forward.Z, 0.0 },
                new double[] { 0.0, 0.0, 0.0, 1.0 }
            );
            return orientation * CrtFactory.TransformationFactory.TranslationMatrix(-from.X, -from.Y, -from.Z);
        }

        /// <summary>
        /// Create a camera
        /// </summary>
        /// <param name="hSize">Horizontal image resolution</param>
        /// <param name="vSize">Vertical image resolution</param>
        /// <param name="fieldOfView">View angle of the camera</param>
        /// <returns>The camera</returns>
        public CrtCamera Camera(int hSize, int vSize, double fieldOfView) =>
            new CrtCamera(hSize, vSize, fieldOfView);

    }
}
