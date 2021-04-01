using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ccml.raytracer;
using ccml.raytracer.Core;
using ccml.raytracer.Engine;
using ccml.raytracer.Materials.Patterns;
using ccml.raytracer.Materials.Patterns.Noises;
using ccml.raytracer.Shapes;
using ccml.raytracer.ui.monogame.screen;

namespace chapter12.exercise.monogame
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
                    CrtFactory.TransformationFactory.TranslationMatrix(0, legHeight/2, 0)
                    *
                    CrtFactory.TransformationFactory.ScalingMatrix(legThickness/2, legHeight/2, legThickness/2)
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
            // the room
            var room = CrtFactory.ShapeFactory.Cube()
                .WithTransformationMatrix(
                    CrtFactory.TransformationFactory.TranslationMatrix(0, 3, 0)
                    *
                    CrtFactory.TransformationFactory.ScalingMatrix(6, 3, 6)
                )
                .WithMaterial(
                    CrtFactory.MaterialFactory.DefaultMaterial
                        .WithPattern(
                            CrtFactory.PatternFactory.Checker3DPattern(
                                    CrtColor.COLOR_GREEN, 
                                    CrtColor.COLOR_WHITE
                            )
                            .WithTransformMatrix(
                                CrtFactory.TransformationFactory.ScalingMatrix(0.1, 0.1, 0.1)
                            )
                        )
                );
            world.Add(room);
            //
            // Add a table
            var tableHeight = 1.0;
            var tableThickness = 0.1;
            var tableWidth = 1.0;
            var tableLength = 2.0;
            {
                var tableSurface = CrtFactory.ShapeFactory.Cube()
                    .WithTransformationMatrix(
                        CrtFactory.TransformationFactory.TranslationMatrix(0, 1 + tableThickness/2, 0)
                        *
                        CrtFactory.TransformationFactory.ScalingMatrix(tableLength/2, tableThickness/2, tableWidth/2)
                    )
                    .WithMaterial(
                        CrtFactory.MaterialFactory.DefaultMaterial
                            .WithPattern(
                                CrtFactory.PatternFactory.PerlinNoisePattern(
                                    new Dictionary<double, CrtColor>()
                                    {
                                        { 0.0, CrtFactory.CoreFactory.Color(0.7,0.5, 0.7) },
                                        { 1.0, CrtFactory.CoreFactory.Color(0.35,0, 0.2) }
                                    }
                                )
                                .WithTransformMatrix(
                                    CrtFactory.TransformationFactory.ScalingMatrix(0.25, 0.25, 0.25)
                                )
                            )
                    );
                world.Add(tableSurface);
                //
                var xoffset = tableLength / 2 - tableThickness / 2;
                var yoffset = tableWidth / 2 - tableThickness / 2;
                {
                    var tableLeg = GetTableLeg(tableHeight, tableThickness);
                    tableLeg
                        .WithTransformationMatrix(
                            CrtFactory.TransformationFactory.TranslationMatrix(xoffset, 0, yoffset)
                            *
                            tableLeg.TransformMatrix
                        );
                    world.Add(tableLeg);
                }
                {
                    var tableLeg = GetTableLeg(tableHeight, tableThickness);
                    tableLeg
                        .WithTransformationMatrix(
                            CrtFactory.TransformationFactory.TranslationMatrix(-xoffset, 0, yoffset)
                            *
                            tableLeg.TransformMatrix
                        );
                    world.Add(tableLeg);
                }
                {
                    var tableLeg = GetTableLeg(tableHeight, tableThickness);
                    tableLeg
                        .WithTransformationMatrix(
                            CrtFactory.TransformationFactory.TranslationMatrix(-xoffset, 0, -yoffset)
                            *
                            tableLeg.TransformMatrix
                        );
                    world.Add(tableLeg);
                }
                {
                    var tableLeg = GetTableLeg(tableHeight, tableThickness);
                    tableLeg
                        .WithTransformationMatrix(
                            CrtFactory.TransformationFactory.TranslationMatrix(xoffset, 0, -yoffset)
                            *
                            tableLeg.TransformMatrix
                        );
                    world.Add(tableLeg);
                }
            }
            //
            // Add mirrors
            {
                var mirror = CrtFactory.ShapeFactory.Cube()
                    .WithMaterial(
                        CrtFactory.MaterialFactory.PerfectMirror
                    )
                    .WithTransformationMatrix(
                        CrtFactory.TransformationFactory.TranslationMatrix(0, 1 + tableHeight, 6)
                        *
                        CrtFactory.TransformationFactory.ScalingMatrix(1, 1, 0.05)
                    );
                world.Add(mirror);
            }
            {
                var mirror = CrtFactory.ShapeFactory.Cube()
                    .WithMaterial(
                        CrtFactory.MaterialFactory.PerfectMirror
                    )
                    .WithTransformationMatrix(
                        CrtFactory.TransformationFactory.TranslationMatrix(0, 1 + tableHeight, -6)
                        *
                        CrtFactory.TransformationFactory.ScalingMatrix(1, 1, 0.05)
                    );
                world.Add(mirror);
            }
            //
            // Add some spheres
            {
                var rock = CrtFactory.ShapeFactory.Sphere();
                rock.WithTransformationMatrix(
                    CrtFactory.TransformationFactory.TranslationMatrix(0, tableWidth + tableThickness + 0.12, 0)
                    *
                    CrtFactory.TransformationFactory.YRotationMatrix(-Math.PI / 4)
                    *
                    CrtFactory.TransformationFactory.ScalingMatrix(0.25, 0.06, 0.175)
                );
                rock.Material.WithPattern(CrtFactory.PatternFactory.ColorPerturbedPattern(
                    CrtFactory.PatternFactory.SolidColor(CrtFactory.CoreFactory.Color(0.85, 0.85, 0.75)),
                    (p, c) =>
                    {
                        return CrtFactory.CoreFactory.Color(
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
                var rock = CrtFactory.ShapeFactory.Sphere();
                rock.WithTransformationMatrix(
                    CrtFactory.TransformationFactory.TranslationMatrix(tableLength, 0.5, tableWidth)
                    *
                    CrtFactory.TransformationFactory.ScalingMatrix(0.5, 0.5, 0.5)
                );
                rock.Material.WithPattern(CrtFactory.PatternFactory.ColorPerturbedPattern(
                    CrtFactory.PatternFactory.SolidColor(CrtFactory.CoreFactory.Color(0.85, 0.85, 0.75)),
                    (p, c) =>
                    {
                        return CrtFactory.CoreFactory.Color(
                            c.Red * (1 + PerlinNoise.Noise(p.X, p.Y, p.Z)),
                            c.Green * (1 + PerlinNoise.Noise(p.X, p.Y, p.Z)),
                            c.Blue * (1 + PerlinNoise.Noise(p.X, p.Y, p.Z))
                        );
                    }));
                rock.Material.Diffuse = 0.3;
                rock.Material.Specular = 0.8;
                rock.Material.WithReflective(0.5);
                world.Objects.Add(rock);
            }
            //
            // add a light
            world.Add(
                CrtFactory.LightFactory.PointLight(
                    CrtFactory.CoreFactory.Point(-3, 5.5, -3),
                    CrtFactory.CoreFactory.Color(1, 1, 1)
                )
            );
            //
            var camera = CrtFactory.EngineFactory.Camera(hSize, vSize, Math.PI / 3.0);
            camera.ViewTransformMatrix =
                CrtFactory.EngineFactory.ViewTransformation(
                    CrtFactory.CoreFactory.Point(0, 2.5, -5),
                    CrtFactory.CoreFactory.Point(0.0, 1.5, 0.0),
                    CrtFactory.CoreFactory.Vector(0.0, 1.0, 0.0)
                );
            for (int i = 0; i < 9; i++)
            {
                camera.ViewTransformMatrix =
                    CrtFactory.EngineFactory.ViewTransformation(
                        CrtFactory.TransformationFactory.YRotationMatrix(i * Math.PI / 4) * CrtFactory.CoreFactory.Point(0, 2.5, -5),
                        CrtFactory.CoreFactory.Point(0.0, 1.5, 0.0),
                        CrtFactory.CoreFactory.Vector(0.0, 1.0, 0.0)
                    );
                //
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
