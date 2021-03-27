using ccml.raytracer.Core;
using ccml.raytracer.Engine;
using ccml.raytracer.Lights;
using ccml.raytracer.Materials;
using ccml.raytracer.Materials.Patterns;
using ccml.raytracer.Materials.Patterns.Noises;
using ccml.raytracer.Shapes;
using ccml.raytracer.Tests;
using ccml.raytracer.Transformation;

namespace ccml.raytracer
{
    public static class CrtFactory
    {

        public static CrtCoreFactory CoreFactory = new CrtCoreFactory();

        public static CrtTransformationFactory TransformationFactory = new CrtTransformationFactory();

        public static CrtEngineFactory EngineFactory = new CrtEngineFactory();

        public static CrtLightFactory LightFactory = new CrtLightFactory();

        public static CrtShapeFactory ShapeFactory = new CrtShapeFactory();

        public static readonly CrtMaterialFactory MaterialFactory = new CrtMaterialFactory();

        public static CrtPatternFactory PatternFactory = new CrtPatternFactory();

        public static CrtPerturbationFactory PerturbationFactory = new CrtPerturbationFactory();

        public static CrtTestsFactory TestsFactory = new CrtTestsFactory();

    }
}
