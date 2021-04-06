using ccml.raytracer.Core;
using System;

namespace ccml.raytracer.Shapes
{
    public class CrtShapeFactory
    {
        /// <summary>
        /// Create a group
        /// </summary>
        /// <returns>The group</returns>
        public CrtGroup Group() => new CrtGroup();

        /// <summary>
        /// Create a unit sphere
        /// </summary>
        /// <returns>The sphere</returns>
        public CrtSphere Sphere() => new CrtSphere();

        /// <summary>
        /// Create a plane
        /// </summary>
        /// <returns>The plane</returns>
        public CrtPlane Plane() => new CrtPlane();

        /// <summary>
        /// Create a cube
        /// </summary>
        /// <returns></returns>
        public CrtCube Cube() => new CrtCube();

        /// <summary>
        /// Create a cylinder
        /// </summary>
        /// <returns></returns>
        public CrtCylinder Cylinder() => new CrtCylinder();


        /// <summary>
        /// Create a cone
        /// </summary>
        /// <returns></returns>
        public CrtCone Cone() => new CrtCone();

        /// <summary>
        /// Create a triangle from 3 points
        /// </summary>
        /// <param name="p1">point 1</param>
        /// <param name="p2">point 2</param>
        /// <param name="p3">point 3</param>
        /// <returns>The triangle</returns>
        public CrtTriangle Triangle(CrtPoint p1, CrtPoint p2, CrtPoint p3) => new CrtTriangle(p1, p2, p3);

        /// <summary>
        /// Create a smooth triangle from 3 points
        /// </summary>
        /// <param name="p1">point 1</param>
        /// <param name="p2">point 2</param>
        /// <param name="p3">point 3</param>
        /// <param name="n1">normal at point 1</param>
        /// <param name="n2">normal at point 2</param>
        /// <param name="n3">normal at point 3</param>
        /// <returns>The triangle</returns>
        public CrtSmoothTriangle SmoothTriangle(
            CrtPoint p1, CrtPoint p2, CrtPoint p3, 
            CrtVector n1, CrtVector n2, CrtVector n3
        ) => new CrtSmoothTriangle(p1, p2, p3, n1, n2, n3);
    }
}
