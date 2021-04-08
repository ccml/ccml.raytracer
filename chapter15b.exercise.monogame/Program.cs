using System;
using System.Threading.Tasks;
using ccml.raytracer;
using ccml.raytracer.Core;
using ccml.raytracer.Engine;
using ccml.raytracer.ui.monogame.screen;
using Microsoft.Xna.Framework.Input;

namespace chapter15b.exercise.monogame
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
        private int _nbrSteps = 5;
        private CrtCamera _camera;

        private void PrepareWorld(int hSize, int vSize)
        {
            //
            _world = CrtFactory.EngineFactory.World();
            // the room
            //var room = CrtFactory.ShapeFactory.Cube()
            //    .WithTransformationMatrix(
            //        CrtFactory.TransformationFactory.ScalingMatrix(20, 20, 20)
            //    )
            //    .WithMaterial(
            //        CrtFactory.MaterialFactory.DefaultMaterial
            //            .WithPattern(
            //                CrtFactory.PatternFactory.Checker3DPattern(
            //                        CrtColor.COLOR_GREEN,
            //                        CrtColor.COLOR_WHITE
            //                    )
            //                    .WithTransformMatrix(
            //                        CrtFactory.TransformationFactory.ScalingMatrix(0.1, 0.1, 0.1)
            //                    )
            //            )
            //    );
            //_world.Add(room);
            //
            // Add an object from a file
            var parser = CrtFactory.FileFormatFactory.ObjParser;
            parser.LoadFile("teapot_low.obj");
            var teapot = parser.ObjToGroup();
            teapot.WithTransformationMatrix(
                CrtFactory.TransformationFactory.XRotationMatrix(-Math.PI/2)
            );
            _world.Add(teapot);
            //
            // add a light
            _world.Add(
                CrtFactory.LightFactory.PointLight(
                    CrtFactory.CoreFactory.Point(50, 50, -50),
                    CrtFactory.CoreFactory.Color(1, 1, 1)
                )
            );
            //
            _eyePosition = CrtFactory.CoreFactory.Point(0, 50, -50);
            _lookAtPosition = CrtFactory.CoreFactory.Point(0.0, 0.0, 0.0);
            var distance = !(_lookAtPosition - _eyePosition);
            _distanceStep = distance / _nbrSteps;
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
                if (dist > (_distanceStep * 1.5))
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
                if (dist < (_distanceStep * (_nbrSteps - 1)))
                {
                    _eyePosition = _eyePosition - ~lookVector * _distanceStep;
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
