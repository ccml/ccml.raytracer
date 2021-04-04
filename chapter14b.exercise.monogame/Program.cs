using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ccml.raytracer;
using ccml.raytracer.Core;
using ccml.raytracer.Engine;
using ccml.raytracer.Materials.Patterns.Noises;
using ccml.raytracer.Shapes;
using ccml.raytracer.ui.monogame.screen;
using Microsoft.Xna.Framework.Input;

namespace chapter14b.exercise.monogame
{
    class Program
    {
        private bool _isRendering = false;
        private bool _isDirty = false;
        private CrtCanvas _canvas;
        private MonoGameRaytracerWindow _window;

        private CrtWorld _world;
        private CrtPoint _eyePosition;
        private CrtPoint _lookAtPosition;
        private double _distanceStep = 0.0;
        private CrtCamera _camera;

        private CrtShape GetTableLeg(double legHeight, double legThickness)
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

        private void PrepareWorld(int hSize, int vSize)
        {
            //
            _world = CrtFactory.EngineFactory.World();
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
            _world.Add(room);
            //
            // Add a table
            var tableHeight = 1.0;
            var tableThickness = 0.1;
            var tableWidth = 1.0;
            var tableLength = 2.0;
            {
                var table = CrtFactory.ShapeFactory.Group();
                var tableSurface = CrtFactory.ShapeFactory.Cube()
                    .WithTransformationMatrix(
                        CrtFactory.TransformationFactory.TranslationMatrix(0, 1 + tableThickness / 2, 0)
                        *
                        CrtFactory.TransformationFactory.ScalingMatrix(tableLength / 2, tableThickness / 2, tableWidth / 2)
                    )
                    .WithMaterial(
                        CrtFactory.MaterialFactory.DefaultMaterial
                            .WithPattern(
                                CrtFactory.PatternFactory.SolidColor(CrtColor.COLOR_RED)
                            )
                    );
                table.Add(tableSurface);
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
                    table.Add(tableLeg);
                }
                {
                    var tableLeg = GetTableLeg(tableHeight, tableThickness);
                    tableLeg
                        .WithTransformationMatrix(
                            CrtFactory.TransformationFactory.TranslationMatrix(-xoffset, 0, yoffset)
                            *
                            tableLeg.TransformMatrix
                        );
                    table.Add(tableLeg);
                }
                {
                    var tableLeg = GetTableLeg(tableHeight, tableThickness);
                    tableLeg
                        .WithTransformationMatrix(
                            CrtFactory.TransformationFactory.TranslationMatrix(-xoffset, 0, -yoffset)
                            *
                            tableLeg.TransformMatrix
                        );
                    table.Add(tableLeg);
                }
                {
                    var tableLeg = GetTableLeg(tableHeight, tableThickness);
                    tableLeg
                        .WithTransformationMatrix(
                            CrtFactory.TransformationFactory.TranslationMatrix(xoffset, 0, -yoffset)
                            *
                            tableLeg.TransformMatrix
                        );
                    table.Add(tableLeg);
                }
                _world.Add(table);
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
                _world.Add(mirror);
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
                _world.Add(mirror);
            }
            //
            // Add some spheres
            {
                var rock = CrtFactory.ShapeFactory.Sphere();
                rock.WithTransformationMatrix(
                    CrtFactory.TransformationFactory.TranslationMatrix(0, tableWidth + tableThickness + 0.06, 0)
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
                _world.Objects.Add(rock);
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
                _world.Objects.Add(rock);
            }
            // 
            // A cup
            var aCup = new List<CrtShape>();
            {
                var cup = CrtFactory.ShapeFactory.Group();
                aCup.Add(
                    CrtFactory.ShapeFactory.Cone()
                        .WithMinimum(0.2).WithMaximum(1)
                        .WithMinimumClosed()
                        .WithMaterial(CrtFactory.MaterialFactory.DefaultMaterial)
                        .WithTransformationMatrix(
                            CrtFactory.TransformationFactory.TranslationMatrix(0, 0.050, 0)
                            *
                            CrtFactory.TransformationFactory.ScalingMatrix(0.2, 0.3, 0.2)
                        )
                );
                aCup.Add(
                    CrtFactory.ShapeFactory.Cylinder()
                        .WithMinimum(0)
                        .WithMaximum(0.6)
                        .WithMaximumClosed()
                        .WithMinimumClosed()
                        .WithMaterial(CrtFactory.MaterialFactory.DefaultMaterial)
                        .WithTransformationMatrix(
                            CrtFactory.TransformationFactory.TranslationMatrix(0, 0.05, 0)
                            *
                            CrtFactory.TransformationFactory.ScalingMatrix(0.04, 0.1, 0.04)
                        )
                );
                aCup.Add(
                    CrtFactory.ShapeFactory.Cone()
                        .WithMinimum(-0.5).WithMaximum(-0.2)
                        .WithMinimumClosed()
                        .WithMaterial(CrtFactory.MaterialFactory.DefaultMaterial)
                        .WithTransformationMatrix(
                            CrtFactory.TransformationFactory.TranslationMatrix(0, 0.07, 0)
                            *
                            CrtFactory.TransformationFactory.ScalingMatrix(0.2, 0.1, 0.2)
                        )
                );
                // Add water in glass
                aCup.Add(
                    CrtFactory.ShapeFactory.Cone()
                        .WithMinimum(0.2).WithMaximum(0.9)
                        .WithMaximumClosed()
                        .WithMaterial(
                            CrtFactory.MaterialFactory.Water
                                .WithColor(CrtColor.COLOR_BLUE)
                                .WithTransparency(0.5)
                                .WithReflective(0.75)
                        )
                        .WithTransformationMatrix(
                            CrtFactory.TransformationFactory.TranslationMatrix(0, 0.05, 0)
                            *
                            CrtFactory.TransformationFactory.ScalingMatrix(0.19, 0.3, 0.19)
                        )
                );
                cup.WithTransformationMatrix(
                    // put the cup on the table
                    CrtFactory.TransformationFactory.TranslationMatrix(tableLength / 3, tableHeight + tableThickness, tableWidth / 3)
                );
                cup.Add(aCup.ToArray());
                _world.Add(cup);
            }
            //
            // add a light
            _world.Add(
                CrtFactory.LightFactory.PointLight(
                    CrtFactory.CoreFactory.Point(5, 5, -5),
                    CrtFactory.CoreFactory.Color(1, 1, 1)
                )
            );
            //
            _eyePosition = CrtFactory.CoreFactory.Point(0, 4.5, -4);
            _lookAtPosition = CrtFactory.CoreFactory.Point(0.0, 1.5, 0.0);
            _distanceStep = !(_lookAtPosition - _eyePosition) / 5;
            SetupCamera(hSize, vSize);
        }

        private void SetupCamera(int hSize, int vSize)
        {
            var camera = CrtFactory.EngineFactory.Camera(hSize, vSize, Math.PI / 3.0);
            camera.RenderingDepth = 8;
            camera.ViewTransformMatrix =
                CrtFactory.EngineFactory.ViewTransformation(
                    _eyePosition,
                    _lookAtPosition,
                    CrtFactory.CoreFactory.Vector(0.0, 1.0, 0.0)
                );
            _camera = camera;
        }

        private async Task Render(int hSize, int vSize)
        {
            _isRendering = true;
            if (_world == null)
            {
                PrepareWorld(hSize, vSize);
            }
            _canvas = _camera.Render(_world);
            _isDirty = true;
            _isRendering = false;
        }

        private async Task UpdateImage()
        {
            if (_isRendering) return;

            if (_canvas != null && _isDirty)
            {
                _isDirty = false;
                _window.Image.RefreshPointsColors(_canvas);
            }

            // Poll for current keyboard state
            KeyboardState state = Keyboard.GetState();

            // If they hit esc, exit
            if (state.IsKeyDown(Keys.Escape)) _window.Exit();

            var mustRender = false;

            // Move the camera around the table
            if (state.IsKeyDown(Keys.Right))
            {
                _eyePosition =
                    CrtFactory.TransformationFactory.YRotationMatrix(-Math.PI / 6)
                    *
                    _eyePosition;
                SetupCamera(_window.Image.Width, _window.Image.Heigth);
                mustRender = true;
            }

            if (state.IsKeyDown(Keys.Left))
            {
                _eyePosition =
                    CrtFactory.TransformationFactory.YRotationMatrix(Math.PI / 6)
                    *
                    _eyePosition;
                SetupCamera(_window.Image.Width, _window.Image.Heigth);
                mustRender = true;
            }

            if (state.IsKeyDown(Keys.Up))
            {
                var lookVector = _lookAtPosition - _eyePosition;
                var dist = !lookVector;
                if (dist > 1)
                {
                    _eyePosition = _eyePosition + ~lookVector * _distanceStep;
                    SetupCamera(_window.Image.Width, _window.Image.Heigth);
                    mustRender = true;
                }
            }

            if (state.IsKeyDown(Keys.Down))
            {
                var lookVector = _lookAtPosition - _eyePosition;
                var dist = !lookVector;
                if (dist < 4)
                {
                    _eyePosition = _eyePosition - lookVector * _distanceStep;
                    SetupCamera(_window.Image.Width, _window.Image.Heigth);
                    mustRender = true;
                }
            }

            if (mustRender)
            {
                Task.Run(async () => Render(_window.Image.Width, _window.Image.Heigth));
            }
        }

        private void Run()
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

        static void Main(string[] args)
        {
            new Program().Run();
        }
    }
}
