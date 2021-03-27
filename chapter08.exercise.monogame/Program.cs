using System;
using System.Threading.Tasks;
using ccml.raytracer;
using ccml.raytracer.Engine;
using ccml.raytracer.Shapes;
using ccml.raytracer.ui.monogame.screen;

namespace chapter08.exercise.monogame
{
    class Program
    {
        private static bool _isDirty = false;
        private static CrtCanvas _canvas;
        private static MonoGameRaytracerWindow _window;

        private static async Task Render(int hSize, int vSize)
        {
            var world = CrtFactory.EngineFactory.World();
            //
            // Add right wall
            var rightWall = CrtFactory.ShapeFactory.Sphere();
            rightWall.TransformMatrix =
                CrtFactory.TransformationFactory.TranslationMatrix(0, 0, 8)
                *
                CrtFactory.TransformationFactory.YRotationMatrix(Math.PI / 4.0)
                *
                CrtFactory.TransformationFactory.XRotationMatrix(Math.PI / 2.0)
                *
                CrtFactory.TransformationFactory.ScalingMatrix(10, 0.01, 10);
            rightWall.Material = CrtFactory.MaterialFactory.DefaultMaterial;
            rightWall.Material.Color = CrtFactory.CoreFactory.Color(1, 0.9, 0.9);
            rightWall.Material.Specular = 0;
            world.Objects.Add(rightWall);
            //
            {
                var sphere = CrtFactory.ShapeFactory.Sphere();
                sphere.TransformMatrix = CrtFactory.TransformationFactory.TranslationMatrix(-1.25, 0, -0.75);
                sphere.Material.Color = CrtFactory.CoreFactory.Color(0.1, 1, 0.5);
                sphere.Material.Diffuse = 0.7;
                sphere.Material.Specular = 0.3;
                world.Objects.Add(sphere);
            }
            {
                var sphere = CrtFactory.ShapeFactory.Sphere();
                sphere.TransformMatrix = CrtFactory.TransformationFactory.TranslationMatrix(-0.75, 0.75, -0.75);
                sphere.Material.Color = CrtFactory.CoreFactory.Color(0.1, 0.1, 0.8);
                sphere.Material.Diffuse = 0.7;
                sphere.Material.Specular = 0.3;
                world.Objects.Add(sphere);
            }
            {
                var sphere = CrtFactory.ShapeFactory.Sphere();
                sphere.TransformMatrix =
                    CrtFactory.TransformationFactory.TranslationMatrix(-0.7, 1.25, -1.3)
                    *
                    CrtFactory.TransformationFactory.ZRotationMatrix(-Math.PI / 6)
                    *
                    CrtFactory.TransformationFactory.ScalingMatrix(0.2, 1, 0.2);
                sphere.Material.Color = CrtFactory.CoreFactory.Color(0.1, 0.1, 0.8);
                sphere.Material.Diffuse = 0.7;
                sphere.Material.Specular = 0.3;
                world.Objects.Add(sphere);
            }
            {
                var sphere = CrtFactory.ShapeFactory.Sphere();
                sphere.TransformMatrix =
                    CrtFactory.TransformationFactory.TranslationMatrix(-0.25, 1.25, -1.25)
                    *
                    CrtFactory.TransformationFactory.YRotationMatrix(Math.PI/4)
                    *
                    CrtFactory.TransformationFactory.ScalingMatrix(1.25, 0.2, 0.2);
                sphere.Material.Color = CrtFactory.CoreFactory.Color(0.1, 0.1, 0.8);
                sphere.Material.Diffuse = 0.7;
                sphere.Material.Specular = 0.3;
                world.Objects.Add(sphere);
            }
            {
                var sphere = CrtFactory.ShapeFactory.Sphere();
                sphere.TransformMatrix =
                    CrtFactory.TransformationFactory.TranslationMatrix(-0.1, 1.0, -1.25)
                    *
                    CrtFactory.TransformationFactory.YRotationMatrix(Math.PI / 4)
                    *
                    CrtFactory.TransformationFactory.ScalingMatrix(1.25, 0.2, 0.2);
                sphere.Material.Color = CrtFactory.CoreFactory.Color(0.8, 0.8, 0.0);
                sphere.Material.Diffuse = 0.7;
                sphere.Material.Specular = 0.3;
                world.Objects.Add(sphere);
            }
            {
                var sphere = CrtFactory.ShapeFactory.Sphere();
                sphere.TransformMatrix =
                    CrtFactory.TransformationFactory.TranslationMatrix(-0.15, 0.75, -1.25)
                    *
                    CrtFactory.TransformationFactory.YRotationMatrix(Math.PI / 4)
                    *
                    CrtFactory.TransformationFactory.ScalingMatrix(1.25, 0.2, 0.2);
                sphere.Material.Color = CrtFactory.CoreFactory.Color(0.8, 0.0, 0.8);
                sphere.Material.Diffuse = 0.7;
                sphere.Material.Specular = 0.3;
                world.Objects.Add(sphere);
            }
            CrtSphere littleFinger = null;
            {
                littleFinger = CrtFactory.ShapeFactory.Sphere();
                littleFinger.Material.Color = CrtFactory.CoreFactory.Color(0.8, 0.0, 0.8);
                littleFinger.Material.Diffuse = 0.7;
                littleFinger.Material.Specular = 0.3;
                world.Objects.Add(littleFinger);
            }
            //
            world.Lights.Add(
                CrtFactory.LightFactory.PointLight(CrtFactory.CoreFactory.Point(-15, 0, -10), CrtFactory.CoreFactory.Color(1, 1, 1))
            );
            //
            var camera = CrtFactory.EngineFactory.Camera(hSize, vSize, Math.PI / 3.0);
            camera.ViewTransformMatrix =
                CrtFactory.EngineFactory.ViewTransformation(
                    CrtFactory.CoreFactory.Point(-1.5, 0, -8),
                    CrtFactory.CoreFactory.Point(0.5, 0, 0.0),
                    CrtFactory.CoreFactory.Vector(0.0, 1.0, 0.0)
                );
            for (int i = 0; i < 10; i++)
            {
                littleFinger.TransformMatrix =
                    CrtFactory.TransformationFactory.TranslationMatrix(-0.2, 0.5, -1.25)
                    *
                    CrtFactory.TransformationFactory.ZRotationMatrix(-Math.PI / 6 * i / 10.0)
                    *
                    CrtFactory.TransformationFactory.ScalingMatrix(1.25, 0.2, 0.2);
                _canvas = camera.Render(world);
                _isDirty = true;
            }
            for (int i = 0; i < 10; i++)
            {
                littleFinger.TransformMatrix =
                    CrtFactory.TransformationFactory.TranslationMatrix(-0.2, 0.5, -1.25)
                    *
                    CrtFactory.TransformationFactory.ZRotationMatrix(-Math.PI / 6 * (9-i) / 10.0)
                    *
                    CrtFactory.TransformationFactory.ScalingMatrix(1.25, 0.2, 0.2);
                _canvas = camera.Render(world);
                _isDirty = true;
            }
            Console.WriteLine("Done !");
        }

        private static async Task UpdateImage()
        {
            if (_canvas != null && _isDirty)
            {
                _isDirty = false;
                _window.Image.RefreshPointsColors(_canvas);
            }
        }

        static void Main(string[] args)
        {
            int hSize = 600;
            int vSize = 480;
            //
            _window = new MonoGameRaytracerWindow(
                hSize,
                vSize,
                async () => await Render(hSize, vSize),
                async () => await UpdateImage()
            );
            _window.Run();
        }
    }
}
