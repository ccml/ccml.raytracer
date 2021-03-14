using System;
using System.Threading.Tasks;
using ccml.raytracer.engine.core;
using ccml.raytracer.engine.core.Engine;
using ccml.raytracer.engine.core.Materials;
using ccml.raytracer.ui.monogame.screen;

namespace chapter07.exercise.monogame
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
            var floor = CrtFactory.Sphere();
            floor.TransformMatrix = CrtFactory.ScalingMatrix(10, 0.01, 10);
            floor.Material = CrtFactory.Material();
            floor.Material.Color = CrtFactory.Color(1, 0.9, 0.9);
            floor.Material.Specular = 0;
            world.Objects.Add(floor);
            //
            // Add left wall
            var leftWall = CrtFactory.Sphere();
            leftWall.TransformMatrix =
                CrtFactory.TranslationMatrix(0, 0, 5)
                *
                CrtFactory.YRotationMatrix(-Math.PI / 4.0)
                *
                CrtFactory.XRotationMatrix(Math.PI / 2.0)
                *
                CrtFactory.ScalingMatrix(10, 0.01, 10);
            leftWall.Material = floor.Material;
            world.Objects.Add(leftWall);
            //
            // Add right wall
            var rightWall = CrtFactory.Sphere();
            rightWall.TransformMatrix =
                CrtFactory.TranslationMatrix(0, 0, 5)
                *
                CrtFactory.YRotationMatrix(Math.PI / 4.0)
                *
                CrtFactory.XRotationMatrix(Math.PI / 2.0)
                *
                CrtFactory.ScalingMatrix(10, 0.01, 10);
            rightWall.Material = floor.Material;
            world.Objects.Add(rightWall);
            //
            // Add large sphere in the middle
            var middle = CrtFactory.Sphere();
            middle.TransformMatrix = CrtFactory.TranslationMatrix(-0.5, 1, 0.5);
            middle.Material.Color = CrtFactory.Color(0.1, 1, 0.5);
            middle.Material.Diffuse = 0.7;
            middle.Material.Specular = 0.3;
            world.Objects.Add(middle);
            //
            // Add smaller sphere on the right
            var right = CrtFactory.Sphere();
            right.TransformMatrix =
                CrtFactory.TranslationMatrix(1.5, 0.5, -0.5)
                *
                CrtFactory.ScalingMatrix(0.5, 0.5, 0.5);
            right.Material.Color = CrtFactory.Color(0.5, 1, 0.1);
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
            left.Material.Color = CrtFactory.Color(1, 0.8, 0.1);
            left.Material.Diffuse = 0.7;
            left.Material.Specular = 0.3;
            world.Objects.Add(left);
            //
            world.Lights.Add(
                CrtFactory.PointLight(CrtFactory.Point(-10, 10, -10), CrtFactory.Color(1, 1, 1))
            );
            //
            int nbr = 36;
            for (int i = 0; i <= nbr; i++)
            {
                var camera = CrtFactory.Camera(hSize, vSize, Math.PI / 3.0);
                camera.ViewTransformMatrix =
                    CrtFactory.ViewTransformation(
                        CrtFactory.Point(0, 2, -6),
                        CrtFactory.Point(0.0, 1.0, 0.0),
                        CrtFactory.ZRotationMatrix(Math.PI / nbr * i * 2) * CrtFactory.Vector(0.0, 1.0, 0.0)
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
