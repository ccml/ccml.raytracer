using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ccml.raytracer;
using ccml.raytracer.Core;
using ccml.raytracer.Engine;
using ccml.raytracer.Materials.Patterns.Noises;
using ccml.raytracer.Shapes;
using ccml.raytracer.ui.monogame.screen;

namespace chapter13a.exercise.monogame
{
    class Program
    {
        private static bool _isDirty = false;
        private static CrtCanvas _canvas;
        private static MonoGameRaytracerWindow _window;

        private static CrtShape GetTableLeg(double legHeight, double legThickness)
        {
            return CrtFactory.ShapeFactory.Cube()
                .WithTransformationMatrix(
                    CrtFactory.TransformationFactory.TranslationMatrix(0, legHeight / 2, 0)
                    *
                    CrtFactory.TransformationFactory.ScalingMatrix(legThickness / 2, legHeight / 2, legThickness / 2)
                )
                .WithMaterial(
                    CrtFactory.MaterialFactory.DefaultMaterial
                        .WithPattern(
                            CrtFactory.PatternFactory.SolidColor(CrtColor.COLOR_GREEN)
                        )
                );
        }

        private static async Task Render(int hSize, int vSize)
        {
            //
            var world = CrtFactory.EngineFactory.World();
            //
            // add floor
            world.Add(
                CrtFactory.ShapeFactory.Plane()
                    .WithTransformationMatrix(
                        CrtFactory.TransformationFactory.TranslationMatrix(0,-2, 0)
                    )
            );
            // 
            // Some cylinders
            var offsets = new List<CrtMatrix>();
            var shapes = new List<CrtShape>();
            {
                var cyl =
                    CrtFactory.ShapeFactory.Cylinder()
                        .WithMinimum(-0.5)
                        .WithMaximum(0.5);
                cyl.WithMaterial(CrtFactory.MaterialFactory.PerfectMirror.WithDiffuse(0.5).WithReflective(0.5));
                offsets.Add(CrtFactory.TransformationFactory.TranslationMatrix(-2.5, 0, 0));
                world.Add(cyl);
                shapes.Add(cyl);
            }
            {
                var cyl =
                    CrtFactory.ShapeFactory.Cylinder()
                        .WithMinimum(-0.5)
                        .WithMaximum(0.5)
                        .WithMinimumClosed();
                cyl.WithMaterial(CrtFactory.MaterialFactory.DefaultMaterial);
                offsets.Add(CrtFactory.TransformationFactory.TranslationMatrix(0, 0, -2.5));
                world.Add(cyl);
                shapes.Add(cyl);
            }
            {
                var cyl =
                    CrtFactory.ShapeFactory.Cylinder()
                        .WithMinimum(-0.5)
                        .WithMaximum(0.5)
                        .WithMaximumClosed();
                cyl.WithMaterial(CrtFactory.MaterialFactory.DefaultMaterial);
                offsets.Add(CrtFactory.TransformationFactory.TranslationMatrix(0, 0, 2.5));
                world.Add(cyl);
                shapes.Add(cyl);
            }
            {
                var cyl =
                    CrtFactory.ShapeFactory.Cylinder()
                        .WithMinimum(-0.5)
                        .WithMaximum(0.5)
                        .WithMinimumClosed()
                        .WithMaximumClosed();
                cyl.WithMaterial(CrtFactory.MaterialFactory.PerfectMirror.WithDiffuse(0.5).WithReflective(0.5));
                offsets.Add(CrtFactory.TransformationFactory.TranslationMatrix(2.5, 0, 0));
                world.Add(cyl);
                shapes.Add(cyl);
            }
            {
                var cyl =
                    CrtFactory.ShapeFactory.Cylinder();
                cyl.WithMaterial(CrtFactory.MaterialFactory.DefaultMaterial);
                offsets.Add(CrtFactory.TransformationFactory.TranslationMatrix(-5, 0, 5));
                world.Add(cyl);
                shapes.Add(cyl);
            }
            {
                var cyl =
                    CrtFactory.ShapeFactory.Cylinder().WithMinimum(0).WithMinimumClosed();
                cyl.WithMaterial(CrtFactory.MaterialFactory.DefaultMaterial);
                offsets.Add(CrtFactory.TransformationFactory.TranslationMatrix(5, 0, 5));
                world.Add(cyl);
                shapes.Add(cyl);
            }
            //
            // add a light
            world.Add(
                CrtFactory.LightFactory.PointLight(
                    CrtFactory.CoreFactory.Point(5, 5, -5),
                    CrtFactory.CoreFactory.Color(1, 1, 1)
                )
            );
            //
            var camera = CrtFactory.EngineFactory.Camera(hSize, vSize, Math.PI / 3.0);
            camera.ViewTransformMatrix =
                CrtFactory.EngineFactory.ViewTransformation(
                    CrtFactory.CoreFactory.Point(0, 6, -10),
                    CrtFactory.CoreFactory.Point(0.0, 0.5, 0.0),
                    CrtFactory.CoreFactory.Vector(0.0, 1.0, 0.0)
                );
            int N = 20;
            for (int i = 0; i < N+1; i++)
            {
                for (int j = 0; j < shapes.Count; j++)
                {
                    shapes[j].TransformMatrix =
                        offsets[j]
                        *
                        CrtFactory.TransformationFactory.XRotationMatrix(i * 2 * Math.PI / N);

                }
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
