using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ccml.raytracer.Core
{
    public class CrtCoreFactory
    {
        /// <summary>
        /// Create a tuple
        /// </summary>
        /// <param name="x">coordinate on x axis</param>
        /// <param name="y">coordinate on y axis</param>
        /// <param name="z">coordinate on z axis</param>
        /// <param name="w">1.0 for points and 0.0 for vectors</param>
        /// <returns></returns>
        public CrtTuple Tuple(double x, double y, double z, double w)
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
        public CrtPoint Point(double x, double y, double z) => new CrtPoint(x, y, z);

        /// <summary>
        /// Create a 3D vector
        /// </summary>
        /// <param name="x">coordinate on x axis</param>
        /// <param name="y">coordinate on y axis</param>
        /// <param name="z">coordinate on z axis</param>
        /// <returns></returns>
        public CrtVector Vector(double x, double y, double z) => new CrtVector(x, y, z);

        /// <summary>
        /// Create a color
        /// </summary>
        /// <param name="red">red component</param>
        /// <param name="green">green component</param>
        /// <param name="blue">blue component</param>
        /// <returns></returns>
        public CrtColor Color(double red, double green, double blue) => new CrtColor { Red = red, Blue = blue, Green = green };

        /// <summary>
        /// Create a matrix
        /// </summary>
        /// <param name="values">values of matrix elements = array(row) of array(columns) of double</param>
        /// <returns>The matrix</returns>
        public CrtMatrix Matrix(params double[][] values)
        {
            int nbrCols = values[0].Length;
            int nbrRows = values.Length;
            var matrix = new CrtMatrix(nbrRows, nbrCols);
            for (int r = 0; r < matrix.NbrRows; r++)
            {
                for (int c = 0; c < matrix.NbrCols; c++)
                {
                    matrix[r, c] = values[r][c];
                }
            }
            return matrix;
        }
    }
}
