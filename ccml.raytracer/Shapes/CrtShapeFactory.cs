namespace ccml.raytracer.Shapes
{
    public class CrtShapeFactory
    {
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
    }
}
