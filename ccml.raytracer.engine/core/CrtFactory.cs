using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using System.Xml.Linq;
using ccml.raytracer.engine.core.Engine;
using ccml.raytracer.engine.core.Lights;
using ccml.raytracer.engine.core.Materials;
using ccml.raytracer.engine.core.Materials.Patterns;
using ccml.raytracer.engine.core.Materials.Patterns.Noises;
using ccml.raytracer.engine.core.Shapes;

namespace ccml.raytracer.engine.core
{
    public static class CrtFactory
    {
        /// <summary>
        /// Create a tuple
        /// </summary>
        /// <param name="x">coordinate on x axis</param>
        /// <param name="y">coordinate on y axis</param>
        /// <param name="z">coordinate on z axis</param>
        /// <param name="w">1.0 for points and 0.0 for vectors</param>
        /// <returns></returns>
        public static CrtTuple Tuple(double x, double y, double z, double w) 
            =>
                CrtReal.AreEquals(w, 1.0) ? 
                    Point(x, y, z) 
                    : CrtReal.AreEquals(w, 0.0) ?
                        Vector(x, y, z)
                        : 
                        new CrtTuple(x, y, z, w);

        /// <summary>
        /// Create a 3D point
        /// </summary>
        /// <param name="x">coordinate on x axis</param>
        /// <param name="y">coordinate on y axis</param>
        /// <param name="z">coordinate on z axis</param>
        /// <returns></returns>
        public static CrtPoint Point(double x, double y, double z) => new CrtPoint(x, y, z);

        /// <summary>
        /// Create a 3D vector
        /// </summary>
        /// <param name="x">coordinate on x axis</param>
        /// <param name="y">coordinate on y axis</param>
        /// <param name="z">coordinate on z axis</param>
        /// <returns></returns>
        public static CrtVector Vector(double x, double y, double z) => new CrtVector(x, y, z);

        /// <summary>
        /// Create a color
        /// </summary>
        /// <param name="red">red component</param>
        /// <param name="green">green component</param>
        /// <param name="blue">blue component</param>
        /// <returns></returns>
        public static CrtColor Color(double red, double green, double blue) => new CrtColor { Red = red, Blue = blue, Green = green };

        /// <summary>
        /// Create a Canvas
        /// </summary>
        /// <param name="width">Width in pixels</param>
        /// <param name="height">Height in pixels</param>
        /// <returns>The canvas</returns>
        public static CrtCanvas Canvas(int width, int height) => new CrtCanvas(width, height);

        /// <summary>
        /// Create a matrix
        /// </summary>
        /// <param name="values">values of matrix elements = array(row) of array(columns) of double</param>
        /// <returns>The matrix</returns>
        public static CrtMatrix Matrix(params double[][] values)
        {
            int nbrCols = values[0].Length;
            int nbrRows = values.Length;
            var matrix = new CrtMatrix(nbrRows, nbrCols);
            for (int r = 0; r < matrix.NbrRows; r++)
            {
                for (int c = 0; c < matrix.NbrCols; c++)
                {
                    matrix[r,c] = values[r][c];
                }
            }
            return matrix;
        }

        /// <summary>
        /// Create an identity matrix
        /// </summary>
        /// <param name="nbrRows">Number of rows</param>
        /// <param name="nbrCols">Number of columns</param>
        /// <returns>The matrix</returns>
        public static CrtMatrix IdentityMatrix(int nbrRows, int nbrCols)
        {
            var matrix = new CrtMatrix(nbrRows, nbrCols);
            return matrix;
        }

        /// <summary>
        /// Create a translation matrix
        /// </summary>
        /// <param name="dx">The move on the x axis</param>
        /// <param name="dy">The move on the y axis</param>
        /// <param name="dz">The move on the z axis</param>
        /// <returns>The matrix</returns>
        public static CrtMatrix TranslationMatrix(double dx, double dy, double dz)
        {
            var result = new CrtMatrix(4, 4);
            result[0, 3] = dx;
            result[1, 3] = dy;
            result[2, 3] = dz;
            return result;
        }

        /// <summary>
        /// Create a scaling matrix
        /// </summary>
        /// <param name="sx">The scale factor on the x axis</param>
        /// <param name="sy">The scale factor on the y axis</param>
        /// <param name="sz">The scale factor on the z axis</param>
        /// <returns>The matrix</returns>
        public static CrtMatrix ScalingMatrix(double sx, double sy, double sz)
        {
            var result = new CrtMatrix(4, 4);
            result[0, 0] = sx;
            result[1, 1] = sy;
            result[2, 2] = sz;
            return result;
        }

        /// <summary>
        /// Create a rotation matrix around the X axis
        /// </summary>
        /// <param name="angle">the rotation angle</param>
        /// <returns>The matrix</returns>
        public static CrtMatrix XRotationMatrix(double angle)
        {
            var result = new CrtMatrix(4, 4);
            var cosa = Math.Cos(angle);
            var sina = Math.Sin(angle);
            result[1, 1] = cosa;
            result[1, 2] = -sina;
            result[2, 1] = sina;
            result[2, 2] = cosa;
            return result;
        }

        /// <summary>
        /// Create a rotation matrix around the Y axis
        /// </summary>
        /// <param name="angle">the rotation angle</param>
        /// <returns>The matrix</returns>
        public static CrtMatrix YRotationMatrix(double angle)
        {
            var result = new CrtMatrix(4, 4);
            var cosa = Math.Cos(angle);
            var sina = Math.Sin(angle);
            result[0, 0] = cosa;
            result[0, 2] = sina;
            result[2, 0] = -sina;
            result[2, 2] = cosa;
            return result;
        }

        /// <summary>
        /// Create a rotation matrix around the Z axis
        /// </summary>
        /// <param name="angle">the rotation angle</param>
        /// <returns>The matrix</returns>
        public static CrtMatrix ZRotationMatrix(double angle)
        {
            var result = new CrtMatrix(4, 4);
            var cosa = Math.Cos(angle);
            var sina = Math.Sin(angle);
            result[0, 0] = cosa;
            result[0, 1] = -sina;
            result[1, 0] = sina;
            result[1, 1] = cosa;
            return result;
        }

        /// <summary>
        /// Create a shearing matrix
        /// </summary>
        /// <param name="sxy">scale factor on X axis in proportion to Y axis</param>
        /// <param name="sxz">scale factor on X axis in proportion to Z axis</param>
        /// <param name="syx">scale factor on Y axis in proportion to X axis</param>
        /// <param name="syz">scale factor on Y axis in proportion to Z axis</param>
        /// <param name="szx">scale factor on Z axis in proportion to X axis</param>
        /// <param name="szy">scale factor on Z axis in proportion to Y axis</param>
        /// <returns>The matrix</returns>
        public static CrtMatrix ShearingMatrix(double sxy, double sxz, double syx, double syz, double szx, double szy)
        {
            var result = new CrtMatrix(4, 4);
            result[0, 1] = sxy;
            result[0, 2] = sxz;
            result[1, 0] = syx;
            result[1, 2] = syz;
            result[2, 0] = szx;
            result[2, 1] = szy;
            return result;
        }

        /// <summary>
        /// Create a ray
        /// </summary>
        /// <param name="origin">Origin of the ray</param>
        /// <param name="direction">Direction of the ray</param>
        /// <returns>The ray</returns>
        public static CrtRay Ray(CrtPoint origin, CrtVector direction) => new CrtRay(origin, direction);

        /// <summary>
        /// Create an intersection describing where an object has been hit
        /// </summary>
        /// <param name="t">the number of unit of time needed by the ray to hit the object</param>
        /// <param name="theObject">the object that has been hit</param>
        /// <returns>the intersection</returns>
        public static CrtIntersection Intersection(double t, CrtShape theObject) => new CrtIntersection(t, theObject);

        /// <summary>
        /// Create a list of hit
        /// </summary>
        /// <param name="xs">the hits</param>
        /// <returns>the list</returns>
        public static List<CrtIntersection> Intersections(params CrtIntersection[] xs) => xs.OrderBy(i => i.T).ToList();

        /// <summary>
        /// Create the ray tracer engine
        /// </summary>
        /// <returns>the ray tracer engine</returns>
        public static CrtEngine Engine() => new CrtEngine();

        /// <summary>
        /// Create a unit sphere
        /// </summary>
        /// <returns>The sphere</returns>
        public static CrtSphere Sphere() => new CrtSphere();

        /// <summary>
        /// Create a plane
        /// </summary>
        /// <returns>The plane</returns>
        public static CrtPlane Plane() => new CrtPlane();

        /// <summary>
        /// Create a point light
        ///   A light source with no size, existing at a single point in space.It is also defined by its intensity,
        ///   or how bright it is. This intensity also describes the color of the light source.
        /// </summary>
        /// <param name="position">the position of a light</param>
        /// <param name="intensity">the intensity/color of the light</param>
        /// <returns>the point light</returns>
        public static CrtPointLight PointLight(CrtPoint position, CrtColor intensity) =>
            new CrtPointLight(position, intensity);

        public static readonly CrtMaterialFactory MaterialFactory = new CrtMaterialFactory();

        /// <summary>
        /// Create an empty world
        /// </summary>
        /// <returns>The world</returns>
        public static CrtWorld World()
        {
            return new CrtWorld();
        }

        /// <summary>
        /// Create a world for test purpose
        /// </summary>
        /// <returns>The test world</returns>
        public static CrtWorld DefaultWorld()
        {
            var w = World();
            w.Add(PointLight(Point(-10, 10, -10), CrtColor.COLOR_WHITE));
            var s1 = CrtFactory.Sphere();
            s1.Material = CrtFactory.MaterialFactory.SpecificMaterial(CrtFactory.Color(0.8, 1.0, 0.6), diffuse:0.7, specular:0.2);
            var s2 = CrtFactory.Sphere();
            s2.TransformMatrix = CrtFactory.ScalingMatrix(0.5, 0.5, 0.5);
            w.Add(s1, s2);
            return w;
        }

        /// <summary>
        /// Create a view transformation matrix
        /// </summary>
        /// <param name="from">Eye position</param>
        /// <param name="to">A point where the eye look at</param>
        /// <param name="up">Indicate which direction is up</param>
        /// <returns>The transformation matrix</returns>
        public static CrtMatrix ViewTransformation(CrtPoint from, CrtPoint to, CrtVector up)
        {
            var forward = ~(to - from);
            var upn = ~up;
            var left = forward ^ upn;
            var trueUp = left ^ forward;
            var orientation = CrtFactory.Matrix(
                new double[] { left.X, left.Y, left.Z, 0.0 },
                new double[] { trueUp.X, trueUp.Y, trueUp.Z, 0.0 },
                new double[] { -forward.X, -forward.Y, -forward.Z, 0.0 },
                new double[] { 0.0, 0.0, 0.0, 1.0 }
            );
            return orientation * CrtFactory.TranslationMatrix(-from.X, -from.Y, -from.Z);
        }

        /// <summary>
        /// Create a camera
        /// </summary>
        /// <param name="hSize">Horizontal image resolution</param>
        /// <param name="vSize">Vertical image resolution</param>
        /// <param name="fieldOfView">View angle of the camera</param>
        /// <returns>The camera</returns>
        public static CrtCamera Camera(int hSize, int vSize, double fieldOfView) =>
            new CrtCamera(hSize, vSize, fieldOfView);

        public static CrtShape TestShape() => new CrtTestShape();

        public static CrtPattern TestPattern() => new CrtTestPattern();
        
        public static CrtPatternFactory PatternFactory = new CrtPatternFactory();

        public static CrtPerturbationFactory PerturbationFactory = new CrtPerturbationFactory();

    }
}
