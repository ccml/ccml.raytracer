using System;

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
    }
}
