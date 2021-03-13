using ccml.raytracer.engine.core;
using ccml.raytracer.engine.core.Materials;
using ccml.raytracer.ui.monogame.screen;
using System;
using System.Threading.Tasks;

namespace chapter09.exercise.monogame
{
    class Program
    {
        private static bool _isDirty = false;
        private static CrtCanvas _canvas;
        private static MonoGameRaytracerWindow _window;

        private static async Task Render(int hSize, int vSize)
        {
            var world = CrtFactory.World();
            //
            // Add floor
            var floor = CrtFactory.Plane();
            floor.Material = CrtFactory.UniformColorMaterial();
            ((CrtUniformColorMaterial)floor.Material).Color = CrtFactory.Color(1, 0.9, 0.9);
            floor.Material.Specular = 0;
            world.Objects.Add(floor);
            //
            // Add walls
            for (int i = 0; i < 6; i++)
            {
                var wall = CrtFactory.Plane();
                wall.Material = floor.Material;
                wall.TransformMatrix =
                    CrtFactory.YRotationMatrix(Math.PI * 2 / 360.0 * i * 60)
                    *
                    CrtFactory.TranslationMatrix(0, 0, 10)
                    *
                    CrtFactory.XRotationMatrix(Math.PI / 2);
                world.Objects.Add(wall);
            }
            //
            // Add large sphere in the middle
            var middle = CrtFactory.Sphere();
            middle.TransformMatrix = CrtFactory.TranslationMatrix(-0.5, 0.75, 0.5);
            ((CrtUniformColorMaterial)middle.Material).Color = CrtFactory.Color(0.1, 1, 0.5);
            middle.Material.Diffuse = 0.7;
            middle.Material.Specular = 0.3;
            world.Objects.Add(middle);
            //
            // Add smaller sphere on the right
            var right = CrtFactory.Sphere();
            right.TransformMatrix =
                CrtFactory.TranslationMatrix(1.5, 0, -0.5)
                *
                CrtFactory.ScalingMatrix(0.5, 0.5, 0.5);
            ((CrtUniformColorMaterial)right.Material).Color = CrtFactory.Color(0.5, 1, 0.1);
            right.Material.Diffuse = 0.7;
            right.Material.Specular = 0.3;
            world.Objects.Add(right);
            //
            // Add smaller sphere on the left
            var left = CrtFactory.Sphere();
            left.TransformMatrix =
                CrtFactory.TranslationMatrix(-1.5, 0.33, -0.75)
                *
                CrtFactory.ScalingMatrix(0.33, 0.33, 0.33);
            ((CrtUniformColorMaterial)left.Material).Color = CrtFactory.Color(1, 0.8, 0.1);
            left.Material.Diffuse = 0.7;
            left.Material.Specular = 0.3;
            world.Objects.Add(left);
            //
            world.Lights.Add(
                CrtFactory.PointLight(CrtFactory.Point(-5, 6, -5), CrtFactory.Color(1, 1, 1))
            );
            //
            int nbr = 36;
            for (int i = 0; i <= nbr; i++)
            {
                var camera = CrtFactory.Camera(hSize, vSize, Math.PI / 3.0);
                camera.ViewTransformMatrix =
                    CrtFactory.ViewTransformation(
                        CrtFactory.YRotationMatrix(Math.PI/360 * i * 20) * CrtFactory.Point(0, 2, -6),
                        CrtFactory.Point(0.0, 1.0, 0.0),
                        CrtFactory.Vector(0.0, 1.0, 0.0)
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
