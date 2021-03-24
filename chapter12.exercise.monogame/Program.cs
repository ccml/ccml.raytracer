using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ccml.raytracer.engine.core;
using ccml.raytracer.engine.core.Materials.Patterns;
using ccml.raytracer.engine.core.Materials.Patterns.Noises;
using ccml.raytracer.ui.monogame.screen;

namespace chapter12.exercise.monogame
{
    class Program
    {
        private static bool _isDirty = false;
        private static CrtCanvas _canvas;
        private static MonoGameRaytracerWindow _window;

        private static async Task Render(int hSize, int vSize)
        {
            //
            var world = CrtFactory.World();
            //
            // Add floor
            var depth = 1.5;
            var floor = CrtFactory.Plane();
            floor.WithTransformationMatrix(CrtFactory.TranslationMatrix(0, -depth, 0));
            floor.WithMaterial(
                CrtFactory.MaterialFactory.DefaultMaterial.WithPattern(
                    CrtFactory.PatternFactory.PerlinNoisePattern(
                        new Dictionary<double, CrtColor>()
                        {
                            { 0, CrtFactory.Color(0.7, 0.8, 0.75) },
                            { 0.35, CrtFactory.Color(0.6, 0.55, 0.5) },
                            { 0.55, CrtFactory.Color(0.3, 0.9, 0.3) },
                            { 1, CrtFactory.Color(0.15, 1, 0.15) },
                        }
                    ).WithTransformMatrix(CrtFactory.ScalingMatrix(1, 1.25, 1.15))
                )
            );
            floor.Material.Specular = 0;
            world.Objects.Add(floor);
            //
            // Add water
            var water = CrtFactory.Plane();
            water.WithTransformationMatrix(CrtFactory.TranslationMatrix(0, -depth - 0.1, 0));
            water.WithMaterial(
                CrtFactory.MaterialFactory.Water
                    .WithPattern(
                        CrtFactory.PatternFactory.PerlinNoisePattern(
                                new Dictionary<double, CrtColor>()
                                {
                                    { 0.0, CrtFactory.Color(0.15, 0.15, 0.35) },
                                    { 1.0, CrtFactory.Color(0.55, 0.55, 0.75) }
                                }
                            )
                            .WithTransformMatrix(CrtFactory.ScalingMatrix(0.1, 0.1, 0.1))
                    )
            );
            world.Objects.Add(water);
            //
            // Add some rocks
            {
                var rock = CrtFactory.Sphere();
                rock
                    .WithTransformationMatrix(
                        CrtFactory.TranslationMatrix(0, 0, 6)
                        *
                        CrtFactory.ScalingMatrix(2, 2, 2)
                    );
                rock.WithMaterial(
                    CrtFactory.MaterialFactory.DefaultMaterial
                        .WithPattern(
                            CrtFactory.PatternFactory.MarblePattern(
                                new Dictionary<double, CrtColor>()
                                {
                                    { 0.0, CrtFactory.Color(0.8, 0.8, 0.8) },
                                    { 0.1, CrtFactory.Color(0.25, 0.25, 0.25) },
                                    { 0.3, CrtFactory.Color(0.7, 0.7, 0.7) },
                                    { 0.7, CrtFactory.Color(0.25, 0.25, 0.25) },
                                    { 0.8, CrtFactory.Color(0.7, 0.7, 0.7) },
                                    { 1.0, CrtFactory.Color(0.9, 0.95, 0.85) }
                                }
                            )
                                .WithTransformMatrix(
                                    CrtFactory.ZRotationMatrix(Math.PI/3)
                                    *
                                    CrtFactory.ScalingMatrix(0.35,0.35,0.35)
                                )
                        )
                        .WithDiffuse(0.2)
                        .WithSpecular(0.2)
                        .WithShininess(300)
                        .WithReflective(1)
                        .WithTransparency(0)
                );
                world.Objects.Add(rock);
            }
            {
                var rock = CrtFactory.Sphere();
                rock
                    .WithTransformationMatrix(
                    CrtFactory.TranslationMatrix(0, 0.5 - depth, 0)
                    *
                    CrtFactory.ScalingMatrix(0.5,0.5,0.5)
                );
                rock.WithMaterial(CrtFactory.MaterialFactory.Glass);
                rock.Material
                    .WithColor(CrtFactory.Color(0.40, 0.10, 0.10))
                    .WithDiffuse(0.2)
                    .WithSpecular(0.3)
                    .WithShininess(300)
                    .WithReflective(0.9)
                    .WithTransparency(0.9);
                world.Objects.Add(rock);
            }
            {
                var rock = CrtFactory.Sphere();
                rock.WithTransformationMatrix(
                    CrtFactory.TranslationMatrix(1.25, 0.25 - depth, 0)
                    *
                    CrtFactory.ScalingMatrix(1, 0.25, 0.75)
                );
                rock.Material.WithPattern(CrtFactory.PatternFactory.ColorPerturbedPattern(
                    CrtFactory.PatternFactory.SolidColor(CrtFactory.Color(0.75, 0.65, 0.8)),
                    (p, c) =>
                    {
                        var n = PerlinNoise.Noise(p.X, p.Y, p.Z);
                        return CrtFactory.Color(
                            c.Red * (1 + n),
                            c.Green * (1 + n),
                            c.Blue * (1 + n)
                        );
                    }));
                rock.Material.Diffuse = 0.7;
                rock.Material.Specular = 0.3;
                world.Objects.Add(rock);
            }
            {
                var rock = CrtFactory.Sphere();
                rock.WithTransformationMatrix(
                    CrtFactory.TranslationMatrix(-0.5, 0.15 - depth, -0.75)
                    *
                    CrtFactory.ScalingMatrix(0.75, 0.15, 0.75)
                );
                rock.Material.WithPattern(CrtFactory.PatternFactory.ColorPerturbedPattern(
                    CrtFactory.PatternFactory.SolidColor(CrtFactory.Color(0.8, 0.75, 0.7)),
                    (p, c) =>
                    {
                        var n = PerlinNoise.Noise(p.X, p.Y, p.Z);
                        return CrtFactory.Color(
                            c.Red * (1 + n),
                            c.Green * (1 + n),
                            c.Blue * (1 + n)
                        );
                    }));
                rock.Material.Diffuse = 0.7;
                rock.Material.Specular = 0.3;
                world.Objects.Add(rock);
            }
            {
                var rock = CrtFactory.Sphere();
                rock.WithTransformationMatrix(
                    CrtFactory.TranslationMatrix(-1, 0.35 - depth, 0.75)
                    *
                    CrtFactory.ScalingMatrix(0.5, 0.35, 1.25)
                );
                rock.Material.WithPattern(CrtFactory.PatternFactory.ColorPerturbedPattern(
                    CrtFactory.PatternFactory.SolidColor(CrtFactory.Color(0.75, 0.75, 0.85)),
                    (p, c) =>
                    {
                        var n = PerlinNoise.Noise(p.X, p.Y, p.Z);
                        return CrtFactory.Color(
                            c.Red * (1 + n),
                            c.Green * (1 + n),
                            c.Blue * (1 + n)
                        );
                    }));
                rock.Material.Diffuse = 0.7;
                rock.Material.Specular = 0.3;
                world.Objects.Add(rock);
            }
            {
                var rock = CrtFactory.Sphere();
                rock.WithTransformationMatrix(
                    CrtFactory.TranslationMatrix(1.25, 0.25 - depth, 1.75)
                    *
                    CrtFactory.YRotationMatrix(-Math.PI / 4)
                    *
                    CrtFactory.ScalingMatrix(1, 0.25, 0.75)
                );
                rock.Material.WithPattern(CrtFactory.PatternFactory.ColorPerturbedPattern(
                    CrtFactory.PatternFactory.SolidColor(CrtFactory.Color(0.85, 0.85, 0.75)),
                    (p, c) =>
                    {
                        return CrtFactory.Color(
                            c.Red * (1 + PerlinNoise.Noise(p.X, p.Y, p.Z)),
                            c.Green * (1 + PerlinNoise.Noise(p.X, p.Y, p.Z)),
                            c.Blue * (1 + PerlinNoise.Noise(p.X, p.Y, p.Z))
                        );
                    }));
                rock.Material.Diffuse = 0.7;
                rock.Material.Specular = 0.3;
                world.Objects.Add(rock);
            }
            //
            world.Lights.Add(
                CrtFactory.PointLight(CrtFactory.Point(-5, 6, -5), CrtFactory.Color(1, 1, 1))
            );
            //
            var camera = CrtFactory.Camera(hSize, vSize, Math.PI / 3.0);
            camera.ViewTransformMatrix =
                CrtFactory.ViewTransformation(
                    CrtFactory.Point(-1, 2, -6),
                    CrtFactory.Point(0.0, 1.0, 0.0),
                    CrtFactory.Vector(0.0, 1.0, 0.0)
                );
            //
            _canvas = camera.Render(world);
            _isDirty = true;
            //
            //{
            //    var step = depth / 20;
            //    var move = 0.0;
            //    for (int i = 0; i < 40; i++)
            //    {
            //        move += step;
            //        if (move >= depth)
            //        {
            //            step = -step;
            //        }
            //        if ((move + step) <= 0)
            //        {
            //            step = -step;
            //        }
            //        for (int iRock = 2; iRock < world.Objects.Count; iRock++)
            //        {
            //            var rock = world.Objects[iRock];
            //            rock.WithTransformationMatrix(CrtFactory.TranslationMatrix(0, step, 0) * rock.TransformMatrix);
            //        }
            //        _canvas = camera.Render(world);
            //        _isDirty = true;
            //    }
            //}
            {
                var step = depth / 10;
                for (int i = 0; i < 10; i++)
                {
                    water.WithTransformationMatrix(CrtFactory.TranslationMatrix(0, step, 0) * water.TransformMatrix);
                    _canvas = camera.Render(world);
                    _isDirty = true;
                }
            }
            {
                var step = depth / 20;
                var move = 0.0;
                for (int i = 0; i < 40; i++)
                {
                    move += step;
                    if (move >= depth)
                    {
                        step = -step;
                    }
                    if ((move + step) <= 0)
                    {
                        step = -step;
                    }
                    for (int iRock = 2; iRock < world.Objects.Count; iRock++)
                    {
                        var rock = world.Objects[iRock];
                        rock.WithTransformationMatrix(CrtFactory.TranslationMatrix(0, step, 0) * rock.TransformMatrix);
                    }
                    _canvas = camera.Render(world);
                    _isDirty = true;
                }
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
