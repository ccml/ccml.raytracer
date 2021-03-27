using ccml.raytracer.ui.monogame.screen;
using System;
using System.Threading.Tasks;
using ccml.raytracer;
using ccml.raytracer.Engine;

namespace chapter09.exercise.monogame
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
            // Add floor
            var floor = CrtFactory.ShapeFactory.Plane();
            floor.Material = CrtFactory.MaterialFactory.DefaultMaterial;
            floor.Material.Color = CrtFactory.CoreFactory.Color(1, 0.9, 0.9);
            floor.Material.Specular = 0;
            world.Objects.Add(floor);
            //
            // Add walls
            for (int i = 0; i < 6; i++)
            {
                var wall = CrtFactory.ShapeFactory.Plane();
                wall.Material = floor.Material;
                wall.TransformMatrix =
                    CrtFactory.TransformationFactory.YRotationMatrix(Math.PI * 2 / 360.0 * i * 60)
                    *
                    CrtFactory.TransformationFactory.TranslationMatrix(0, 0, 10)
                    *
                    CrtFactory.TransformationFactory.XRotationMatrix(Math.PI / 2);
                world.Objects.Add(wall);
            }
            //
            // Add large sphere in the middle
            var middle = CrtFactory.ShapeFactory.Sphere();
            middle.TransformMatrix = CrtFactory.TransformationFactory.TranslationMatrix(-0.5, 0.75, 0.5);
            middle.Material.Color = CrtFactory.CoreFactory.Color(0.1, 1, 0.5);
            middle.Material.Diffuse = 0.7;
            middle.Material.Specular = 0.3;
            world.Objects.Add(middle);
            //
            // Add smaller sphere on the right
            var right = CrtFactory.ShapeFactory.Sphere();
            right.TransformMatrix =
                CrtFactory.TransformationFactory.TranslationMatrix(1.5, 0, -0.5)
                *
                CrtFactory.TransformationFactory.ScalingMatrix(0.5, 0.5, 0.5);
            right.Material.Color = CrtFactory.CoreFactory.Color(0.5, 1, 0.1);
            right.Material.Diffuse = 0.7;
            right.Material.Specular = 0.3;
            world.Objects.Add(right);
            //
            // Add smaller sphere on the left
            var left = CrtFactory.ShapeFactory.Sphere();
            left.TransformMatrix =
                CrtFactory.TransformationFactory.TranslationMatrix(-1.5, 0.33, -0.75)
                *
                CrtFactory.TransformationFactory.ScalingMatrix(0.33, 0.33, 0.33);
            left.Material.Color = CrtFactory.CoreFactory.Color(1, 0.8, 0.1);
            left.Material.Diffuse = 0.7;
            left.Material.Specular = 0.3;
            world.Objects.Add(left);
            //
            world.Lights.Add(
                CrtFactory.LightFactory.PointLight(CrtFactory.CoreFactory.Point(-5, 6, -5), CrtFactory.CoreFactory.Color(1, 1, 1))
            );
            //
            int nbr = 36;
            for (int i = 0; i <= nbr; i++)
            {
                var camera = CrtFactory.EngineFactory.Camera(hSize, vSize, Math.PI / 3.0);
                camera.ViewTransformMatrix =
                    CrtFactory.EngineFactory.ViewTransformation(
                        CrtFactory.TransformationFactory.YRotationMatrix(Math.PI/360 * i * 20) * CrtFactory.CoreFactory.Point(0, 2, -6),
                        CrtFactory.CoreFactory.Point(0.0, 1.0, 0.0),
                        CrtFactory.CoreFactory.Vector(0.0, 1.0, 0.0)
                    );
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
            int hSize = 640;
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
