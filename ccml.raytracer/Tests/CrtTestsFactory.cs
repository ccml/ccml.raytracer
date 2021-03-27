using ccml.raytracer.Core;
using ccml.raytracer.Engine;
using ccml.raytracer.Materials.Patterns;
using ccml.raytracer.Shapes;

namespace ccml.raytracer.Tests
{
    public class CrtTestsFactory
    {
        /// <summary>
        /// Create a world for test purpose
        /// </summary>
        /// <returns>The test world</returns>
        public CrtWorld DefaultWorld()
        {
            var w = CrtFactory.EngineFactory.World();
            w.Add(CrtFactory.LightFactory.PointLight(CrtFactory.CoreFactory.Point(-10, 10, -10), CrtColor.COLOR_WHITE));
            var s1 = CrtFactory.ShapeFactory.Sphere();
            s1.Material = CrtFactory.MaterialFactory.SpecificMaterial(CrtFactory.CoreFactory.Color(0.8, 1.0, 0.6), diffuse: 0.7, specular: 0.2);
            var s2 = CrtFactory.ShapeFactory.Sphere();
            s2.TransformMatrix = CrtFactory.TransformationFactory.ScalingMatrix(0.5, 0.5, 0.5);
            w.Add(s1, s2);
            return w;
        }

        public CrtShape TestShape() => new CrtTestShape();

        public CrtPattern TestPattern() => new CrtTestPattern();
    }
}
