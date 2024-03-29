﻿using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using ccml.raytracer;
using ccml.raytracer.Core;
using ccml.raytracer.Engine;
using ccml.raytracer.Materials.Patterns;
using ccml.raytracer.Materials.Patterns.Noises;
using ccml.raytracer.ui.monogame.screen;

namespace chapter10.exercise.monogame
{
    class Program
    {
        private static bool _isDirty = false;
        private static CrtCanvas _canvas;
        private static MonoGameRaytracerWindow _window;

        private static async Task Render(int hSize, int vSize)
        {
            var random = new Random();
            var patterns = new Func<CrtPattern>[]
            {
                () => CrtFactory.PatternFactory.Checker3DPattern(
                    CrtFactory.CoreFactory.Color(1, 0, 1),
                    CrtFactory.CoreFactory.Color(0, 1, 0)
                ),
                () => CrtFactory.PatternFactory.PerlinNoisePattern(
                    new Dictionary<double, CrtColor>()
                    {
                        { 0, CrtColor.COLOR_BLUE },
                        { 1.0, CrtColor.COLOR_WHITE }
                    }
                    ),
                () => CrtFactory.PatternFactory.PointPerturbedPattern(
                    CrtFactory.PatternFactory.StripePattern(
                        CrtFactory.CoreFactory.Color(1, 1, 1),
                        CrtFactory.CoreFactory.Color(0, 1, 0)
                    ),
                    p =>
                    {
                        var n = PerlinNoise.Noise(p.X, p.Y, p.Z);
                        return CrtFactory.CoreFactory.Point(
                            p.X * n,
                            p.Y * n,
                            p.Z * n
                        );
                    }),
                () => CrtFactory.PatternFactory.PointPerturbedPattern(
                    CrtFactory.PatternFactory.BlendedPattern(
                        CrtFactory.PatternFactory.StripePattern(
                            CrtFactory.CoreFactory.Color(1, 1, 1),
                            CrtFactory.CoreFactory.Color(0, 1, 0)
                        ),
                        CrtFactory.PatternFactory.StripePattern(
                            CrtFactory.CoreFactory.Color(1, 1, 1),
                            CrtFactory.CoreFactory.Color(0, 1, 0)
                        ),
                        CrtBlendedPattern.BLENDING_METHOD_INVERT_XZ_AVERAGE
                    ),
                    p =>
                    {
                        var n = PerlinNoise.Noise(p.X, p.Y, p.Z);
                        return CrtFactory.CoreFactory.Point(
                            p.X * n,
                            p.Y * n,
                            p.Z * n
                        );
                    }),
                () => CrtFactory.PatternFactory.BlendedPattern(
                    CrtFactory.PatternFactory.StripePattern(
                        CrtFactory.CoreFactory.Color(1, 1, 1),
                        CrtFactory.CoreFactory.Color(0, 1, 0)
                    ),
                    CrtFactory.PatternFactory.StripePattern(
                        CrtFactory.CoreFactory.Color(1, 1, 1),
                        CrtFactory.CoreFactory.Color(0, 1, 0)
                    ),
                    CrtBlendedPattern.BLENDING_METHOD_INVERT_XY_AVERAGE
                ),
                () => CrtFactory.PatternFactory.BlendedPattern(
                    CrtFactory.PatternFactory.StripePattern(
                        CrtFactory.CoreFactory.Color(1, 1, 1),
                        CrtFactory.CoreFactory.Color(0, 1, 0)
                    ),
                    CrtFactory.PatternFactory.StripePattern(
                        CrtFactory.CoreFactory.Color(1, 1, 1),
                        CrtFactory.CoreFactory.Color(0, 1, 0)
                    ),
                    CrtBlendedPattern.BLENDING_METHOD_INVERT_XZ_AVERAGE
                ),
                () => CrtFactory.PatternFactory.BlendedPattern(
                    CrtFactory.PatternFactory.StripePattern(
                        CrtFactory.CoreFactory.Color(1, 1, 1),
                        CrtFactory.CoreFactory.Color(0, 1, 0)
                    ),
                    CrtFactory.PatternFactory.StripePattern(
                        CrtFactory.CoreFactory.Color(1, 1, 1),
                        CrtFactory.CoreFactory.Color(0, 1, 0)
                    ),
                    CrtBlendedPattern.BLENDING_METHOD_INVERT_YZ_AVERAGE
                ),
                () => CrtFactory.PatternFactory.StripePattern(
                    CrtFactory.CoreFactory.Color(0, 0, 1),
                    CrtFactory.CoreFactory.Color(1, 0, 0)
                ),
                () => CrtFactory.PatternFactory.RingPattern(
                    CrtFactory.CoreFactory.Color(0, 0, 1),
                    CrtFactory.CoreFactory.Color(1, 0, 0)
                ),
                () => CrtFactory.PatternFactory.GradientPattern(
                    CrtFactory.CoreFactory.Color(0, 0, 1),
                    CrtFactory.CoreFactory.Color(1, 0, 0)
                ),
                () => CrtFactory.PatternFactory.RadialGradientPattern(
                    CrtFactory.CoreFactory.Color(0, 0, 1),
                    CrtFactory.CoreFactory.Color(1, 0, 0)
                )
            };
            //
            var world = CrtFactory.EngineFactory.World();
            //
            // Add floor
            var floor = CrtFactory.ShapeFactory.Plane();
            floor.Material = CrtFactory.MaterialFactory.DefaultMaterial;
            floor.Material.Specular = 0;
            world.Objects.Add(floor);
            //
            // Add large sphere in the middle
            var middle = CrtFactory.ShapeFactory.Sphere();
            middle.TransformMatrix = CrtFactory.TransformationFactory.TranslationMatrix(0, 1, 0);
            middle.Material.Diffuse = 0.7;
            middle.Material.Specular = 0.3;
            world.Objects.Add(middle);
            //
            world.Lights.Add(
                CrtFactory.LightFactory.PointLight(CrtFactory.CoreFactory.Point(-5, 6, -5), CrtFactory.CoreFactory.Color(1, 1, 1))
            );
            //
            for (int j = 0; j < patterns.Length; j++)
            {
                world.Objects[0].Material.Pattern = patterns[j]();
                var middlePattern = 
                middle.Material.Pattern = patterns[(j+1)%patterns.Length]();
                middle.Material.Pattern.TransformMatrix = CrtFactory.TransformationFactory.ScalingMatrix(0.25, 0.25, 1);
                int nbr = 36;
                for (int i = 0; i <= nbr; i++)
                {
                    var camera = CrtFactory.EngineFactory.Camera(hSize, vSize, Math.PI / 3.0);
                    camera.ViewTransformMatrix =
                        CrtFactory.EngineFactory.ViewTransformation(
                            CrtFactory.TransformationFactory.YRotationMatrix(Math.PI / 360 * i * 20) * CrtFactory.CoreFactory.Point(0, 2, -6),
                            CrtFactory.CoreFactory.Point(0.0, 1.0, 0.0),
                            CrtFactory.CoreFactory.Vector(0.0, 1.0, 0.0)
                        );
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
