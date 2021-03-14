using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using ccml.raytracer.engine.core;
using ccml.raytracer.engine.core.Materials.Patterns;
using ccml.raytracer.engine.core.Materials.Patterns.Noises;
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
                () => CrtFactory.Checker3DPattern(
                    CrtFactory.Color(1, 0, 1),
                    CrtFactory.Color(0, 1, 0)
                ),
                () => CrtFactory.PerturbedColorPattern(
                    CrtFactory.SolidColor(CrtFactory.Color(0,0,0.8)),
                    (p,c) =>
                    {
                        return CrtFactory.Color(
                            c.Red * (1 + PerlinNoise.Noise(p.X, p.Y, p.Z)),
                            c.Green * (1 + PerlinNoise.Noise(p.X, p.Y, p.Z)),
                            c.Blue * (1 + PerlinNoise.Noise(p.X, p.Y, p.Z))
                        );
                    }),
                () => CrtFactory.PerturbedPattern(
                    CrtFactory.StripePattern(
                        CrtFactory.Color(1, 1, 1),
                        CrtFactory.Color(0, 1, 0)
                    ),
                    p => CrtFactory.Point(
                        p.X * PerlinNoise.Noise(p.X, p.Y, p.Z),
                        p.Y * PerlinNoise.Noise(p.X, p.Y, p.Z),
                        p.Z * PerlinNoise.Noise(p.X, p.Y, p.Z)
                    )
                ),
                () => CrtFactory.PerturbedPattern(
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
                    ),
                    p => CrtFactory.Point(
                        p.X * PerlinNoise.Noise(p.X, p.Y, p.Z),
                        p.Y * PerlinNoise.Noise(p.X, p.Y, p.Z),
                        p.Z * PerlinNoise.Noise(p.X, p.Y, p.Z)
                    )
                ),
                () => CrtFactory.BlendedPattern(
                    CrtFactory.StripePattern(
                        CrtFactory.Color(1, 1, 1),
                        CrtFactory.Color(0, 1, 0)
                    ),
                    CrtFactory.StripePattern(
                        CrtFactory.Color(1, 1, 1),
                        CrtFactory.Color(0, 1, 0)
                    ),
                    CrtBlendedPattern.BLENDING_METHOD_INVERT_XY_AVERAGE
                ),
                () => CrtFactory.BlendedPattern(
                    CrtFactory.StripePattern(
                        CrtFactory.Color(1, 1, 1),
                        CrtFactory.Color(0, 1, 0)
                    ),
                    CrtFactory.StripePattern(
                        CrtFactory.Color(1, 1, 1),
                        CrtFactory.Color(0, 1, 0)
                    ),
                    CrtBlendedPattern.BLENDING_METHOD_INVERT_XZ_AVERAGE
                ),
                () => CrtFactory.BlendedPattern(
                    CrtFactory.StripePattern(
                        CrtFactory.Color(1, 1, 1),
                        CrtFactory.Color(0, 1, 0)
                    ),
                    CrtFactory.StripePattern(
                        CrtFactory.Color(1, 1, 1),
                        CrtFactory.Color(0, 1, 0)
                    ),
                    CrtBlendedPattern.BLENDING_METHOD_INVERT_YZ_AVERAGE
                ),
                () => CrtFactory.StripePattern(
                    CrtFactory.Color(0, 0, 1),
                    CrtFactory.Color(1, 0, 0)
                ),
                () => CrtFactory.RingPattern(
                    CrtFactory.Color(0, 0, 1),
                    CrtFactory.Color(1, 0, 0)
                ),
                () => CrtFactory.GradientPattern(
                    CrtFactory.Color(0, 0, 1),
                    CrtFactory.Color(1, 0, 0)
                ),
                () => CrtFactory.RadialGradientPattern(
                    CrtFactory.Color(0, 0, 1),
                    CrtFactory.Color(1, 0, 0)
                )
            };
            //
            var world = CrtFactory.World();
            //
            // Add floor
            var floor = CrtFactory.Plane();
            floor.Material = CrtFactory.Material();
            floor.Material.Specular = 0;
            world.Objects.Add(floor);
            //
            // Add large sphere in the middle
            var middle = CrtFactory.Sphere();
            middle.TransformMatrix = CrtFactory.TranslationMatrix(0, 1, 0);
            middle.Material.Diffuse = 0.7;
            middle.Material.Specular = 0.3;
            world.Objects.Add(middle);
            //
            world.Lights.Add(
                CrtFactory.PointLight(CrtFactory.Point(-5, 6, -5), CrtFactory.Color(1, 1, 1))
            );
            //
            for (int j = 0; j < patterns.Length; j++)
            {
                world.Objects[0].Material.Pattern = patterns[j]();
                var middlePattern = 
                middle.Material.Pattern = patterns[(j+1)%patterns.Length]();
                middle.Material.Pattern.TransformMatrix = CrtFactory.ScalingMatrix(0.25, 0.25, 1);
                int nbr = 36;
                for (int i = 0; i <= nbr; i++)
                {
                    var camera = CrtFactory.Camera(hSize, vSize, Math.PI / 3.0);
                    camera.ViewTransformMatrix =
                        CrtFactory.ViewTransformation(
                            CrtFactory.YRotationMatrix(Math.PI / 360 * i * 20) * CrtFactory.Point(0, 2, -6),
                            CrtFactory.Point(0.0, 1.0, 0.0),
                            CrtFactory.Vector(0.0, 1.0, 0.0)
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
