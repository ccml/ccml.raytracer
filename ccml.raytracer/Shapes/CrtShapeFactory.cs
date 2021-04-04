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
    }
}
