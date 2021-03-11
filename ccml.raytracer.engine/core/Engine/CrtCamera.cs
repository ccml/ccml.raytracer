using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ccml.raytracer.engine.core.Engine
{
    public class CrtCamera
    {
        public int HSize { get; private set; }
        public int VSize { get; private set; }
        public double FieldOfView { get; private set; }

        public double HalfView { get; private set; }
        public double HalfWidth { get; private set; }
        public double HalfHeight { get; private set; }
        public double Aspect { get; private set; }
        public double PixelSize { get; private set; }

        private CrtMatrix _viewTransformMatrix;
        public CrtMatrix ViewTransformMatrix
        {
            get => _viewTransformMatrix;
            set
            {
                _viewTransformMatrix = value;
                InverseViewTransformMatrix = _viewTransformMatrix.Inverse();
            }
        }
        public CrtMatrix InverseViewTransformMatrix { get; private set; }

        internal CrtCamera(int hSize, int vSize, double fieldOfView)
        {
            HSize = hSize;
            VSize = vSize;
            FieldOfView = fieldOfView;
            //
            HalfView = Math.Tan(fieldOfView / 2.0);
            Aspect = (double)hSize / (double)vSize;
            if (CrtReal.CompareTo(Aspect, 1) >= 0)
            {
                HalfWidth = HalfView;
                HalfHeight = HalfView / Aspect;
            }
            else
            {
                HalfWidth = HalfView * Aspect;
                HalfHeight = HalfView;
            }
            PixelSize = (HalfWidth * 2) / hSize;
            //
            ViewTransformMatrix = CrtFactory.IdentityMatrix(4, 4);
        }

        public CrtRay RayForPixel(int px, int py)
        {
            // the offset from the edge of the canvas to the pixel's center
            var xOffset = (px + 0.5) * PixelSize;
            var yOffset = (py + 0.5) * PixelSize;
            // the untransformed coordinates of the pixel in world space.
            // (remember that the camera looks toward -z, so +x is to the *left*.)
            var worldX = HalfWidth - xOffset;
            var worldY = HalfHeight - yOffset;
            // using the camera matrix, transform the canvas point and the origin,
            // and then compute the ray's direction vector.
            // (remember that the canvas is at z=-1)
            var pixel = InverseViewTransformMatrix * CrtFactory.Point(worldX, worldY, -1.0);
            var origin = InverseViewTransformMatrix * CrtFactory.Point(0, 0, 0);
            var direction = ~(pixel - origin);
            return CrtFactory.Ray(origin, direction);
        }

        /// <summary>
        /// Render a world and returns the result image
        /// </summary>
        /// <param name="w">The image of the rendered world</param>
        /// <returns></returns>
        public CrtCanvas Render(CrtWorld world)
        {
            var image = CrtFactory.Canvas(HSize, VSize);
            Parallel.For(0, VSize, new ParallelOptions {MaxDegreeOfParallelism = Environment.ProcessorCount - 2},
                y =>
                {
                    for (int x = 0; x < HSize; x++)
                    {
                        var ray = RayForPixel(x, y);
                        var color = world.ColorAt(ray);
                        image[x, y] = color;
                    }
                }
            );
            return image;
        }
    }
}
