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

        public static CrtCanvas Canvas(int width, int height) => new CrtCanvas(width, height);
    }
}
