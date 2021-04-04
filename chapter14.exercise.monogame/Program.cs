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

namespace chapter14.exercise.monogame
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

        private CrtShape HexagonCorner()
        {
            return 
                CrtFactory.ShapeFactory.Sphere()
                    .WithTransformationMatrix(
                        CrtFactory.TransformationFactory.TranslationMatrix(0, 0, -1)
                        *
                        CrtFactory.TransformationFactory.ScalingMatrix(0.25, 0.25, 0.25)
                    );
        }

        private CrtShape HexagonEdge()
        {
            return
                CrtFactory.ShapeFactory.Cylinder()
                    .WithMinimum(0)
                    .WithMaximum(1)
                    .WithTransformationMatrix(
                        CrtFactory.TransformationFactory.TranslationMatrix(0, 0, -1)
                        *
                        CrtFactory.TransformationFactory.YRotationMatrix(-Math.PI / 6)
                        *
                        CrtFactory.TransformationFactory.ZRotationMatrix(-Math.PI / 2)
                        *
                        CrtFactory.TransformationFactory.ScalingMatrix(0.25, 1, 0.25)
                    );
        }

        private CrtShape HexagonSide()
        {
            return CrtFactory.ShapeFactory.Group().Add(HexagonCorner(), HexagonEdge());
        }

        private CrtShape Hexagon()
        {
            var hex = CrtFactory.ShapeFactory.Group();
            for (int n = 0; n < 6; n++)
            {
                hex.Add(HexagonSide().WithTransformationMatrix(
                    CrtFactory.TransformationFactory.YRotationMatrix(n * Math.PI/3)
                ));
            }
            return hex;
        }

        private void PrepareWorld(int hSize, int vSize)
        {
            _world = CrtFactory.EngineFactory.World();
            //
            _world.Add(Hexagon().WithTransformationMatrix(
                CrtFactory.TransformationFactory.TranslationMatrix(0, 2, 0)
            ));
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
            int hSize = 480;
            int vSize = 320;
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
