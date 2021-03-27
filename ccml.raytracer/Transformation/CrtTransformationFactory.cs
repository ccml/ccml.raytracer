using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ccml.raytracer.Core;

namespace ccml.raytracer.Transformation
{
    public class CrtTransformationFactory
    {

        /// <summary>
        /// Create an identity matrix
        /// </summary>
        /// <param name="nbrRows">Number of rows</param>
        /// <param name="nbrCols">Number of columns</param>
        /// <returns>The matrix</returns>
        public CrtMatrix IdentityMatrix(int nbrRows, int nbrCols)
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
        public CrtMatrix TranslationMatrix(double dx, double dy, double dz)
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
        public CrtMatrix ScalingMatrix(double sx, double sy, double sz)
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
        public CrtMatrix XRotationMatrix(double angle)
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
        public CrtMatrix YRotationMatrix(double angle)
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
        public CrtMatrix ZRotationMatrix(double angle)
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
        public CrtMatrix ShearingMatrix(double sxy, double sxz, double syx, double syz, double szx, double szy)
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
