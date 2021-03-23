using System;
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
            // Add water
            var water = CrtFactory.Plane();
            water.WithMaterial(CrtFactory.MaterialFactory.Water.WithColor(CrtFactory.Color(0.15, 0.15, 0.55)));
            world.Objects.Add(water);
            //
            // Add floor
            var depth = 1.5;
            var floor = CrtFactory.Plane();
            floor.WithTransformationMatrix(CrtFactory.TranslationMatrix(0, -depth, 0));
            floor.Material = CrtFactory.MaterialFactory.DefaultMaterial;
            floor.Material.WithPattern(
                CrtFactory.BlendedPattern(
                    CrtFactory.StripePattern(
                        CrtFactory.Color(1, 1, 1),
                        CrtFactory.Color(0, 1, 0)
                    ),
                    CrtFactory.StripePattern(
                        CrtFactory.Color(1, 1, 1),
                        CrtFactory.Color(0, 1, 0)
                    ),
                    CrtBlendedPattern.BLENDING_METHOD_INVERT_XZ_AVERAGE
                ).WithTransformMatrix(CrtFactory.ScalingMatrix(0.2, 1.0, 0.2))
            );
            floor.Material.Specular = 0;
            world.Objects.Add(floor);
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
                rock.WithMaterial(CrtFactory.MaterialFactory.Glass);
                rock.Material
                    .WithColor(CrtFactory.Color(0.40, 0.40, 0.40))
                    .WithDiffuse(0.2)
                    .WithSpecular(0.3)
                    .WithShininess(300)
                    .WithReflective(1)
                    .WithTransparency(0.5);
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
                rock.Material.WithPattern(CrtFactory.PerturbedColorPattern(
                    CrtFactory.SolidColor(CrtFactory.Color(0.75, 0.65, 0.8)),
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
            {
                var rock = CrtFactory.Sphere();
                rock.WithTransformationMatrix(
                    CrtFactory.TranslationMatrix(-0.5, 0.15 - depth, -0.75)
                    *
                    CrtFactory.ScalingMatrix(0.75, 0.15, 0.75)
                );
                rock.Material.WithPattern(CrtFactory.PerturbedColorPattern(
                    CrtFactory.SolidColor(CrtFactory.Color(0.8, 0.75, 0.7)),
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
            {
                var rock = CrtFactory.Sphere();
                rock.WithTransformationMatrix(
                    CrtFactory.TranslationMatrix(-1, 0.35 - depth, 0.75)
                    *
                    CrtFactory.ScalingMatrix(0.5, 0.35, 1.25)
                );
                rock.Material.WithPattern(CrtFactory.PerturbedColorPattern(
                    CrtFactory.SolidColor(CrtFactory.Color(0.75, 0.75, 0.85)),
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
            {
                var rock = CrtFactory.Sphere();
                rock.WithTransformationMatrix(
                    CrtFactory.TranslationMatrix(1.25, 0.25 - depth, 1.75)
                    *
                    CrtFactory.YRotationMatrix(-Math.PI / 4)
                    *
                    CrtFactory.ScalingMatrix(1, 0.25, 0.75)
                );
                rock.Material.WithPattern(CrtFactory.PerturbedColorPattern(
                    CrtFactory.SolidColor(CrtFactory.Color(0.85, 0.85, 0.75)),
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
