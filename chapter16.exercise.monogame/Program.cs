using System;
using System.Runtime.InteropServices.ComTypes;
using System.Threading.Tasks;
using ccml.raytracer;
using ccml.raytracer.Core;
using ccml.raytracer.Engine;
using ccml.raytracer.Shapes;
using ccml.raytracer.Transformation;
using ccml.raytracer.ui.monogame.screen;
using Microsoft.Xna.Framework.Input;

namespace chapter16.exercise.monogame
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

        private CrtShape DieVertice(CrtColor color, double x, double y, double z)
        {
            return CrtFactory.ShapeFactory.Csg(
                CrtCSG.DIFFERENCE,
                CrtFactory.ShapeFactory.Cube()
                .WithMaterial(
                    CrtFactory.MaterialFactory.DefaultMaterial.WithColor(color)
                ),
                CrtFactory.ShapeFactory.Sphere().WithTransformationMatrix(
                    CrtFactory.TransformationFactory.TranslationMatrix(-x, -y, -z)
                )
                .WithMaterial(
                    CrtFactory.MaterialFactory.DefaultMaterial.WithColor(color)
                )
            )
            .WithTransformationMatrix(
                CrtFactory.TransformationFactory.TranslationMatrix(x, y, z)
                *
                CrtFactory.TransformationFactory.ScalingMatrix(0.1, 0.1, 0.1)
            );
        }

        private CrtShape DieEdge(CrtColor color, char rotationAxe, double x, double y, double z)
        {
            CrtMatrix cylTransform = null;
            switch (rotationAxe)
            {
                case 'x':
                    cylTransform = CrtFactory.TransformationFactory.XRotationMatrix(Math.PI / 2);
                    break;
                case 'y':
                    cylTransform = CrtFactory.TransformationFactory.IdentityMatrix(4,4);
                    break;
                case 'z':
                    cylTransform = CrtFactory.TransformationFactory.ZRotationMatrix(Math.PI / 2);
                    break;
            }
            return CrtFactory.ShapeFactory.Csg(
                CrtCSG.DIFFERENCE,
                CrtFactory.ShapeFactory.Cube()
                .WithMaterial(
                    CrtFactory.MaterialFactory.DefaultMaterial.WithColor(color)
                ),
                CrtFactory.ShapeFactory.Cylinder()
                .WithMaximum(1).WithMinimum(-1)
                .WithMaximumClosed().WithMinimumClosed()
                .WithTransformationMatrix(
                    CrtFactory.TransformationFactory.TranslationMatrix(-x, -y, -z)
                    *
                    cylTransform
                )
                .WithMaterial(
                    CrtFactory.MaterialFactory.DefaultMaterial.WithColor(color)
                )
            )
            .WithTransformationMatrix(
                CrtFactory.TransformationFactory.TranslationMatrix(x, y, z)
                *
                CrtFactory.TransformationFactory.ScalingMatrix(
                    CrtReal.AreEquals(x, 0) ? 0.91 : 0.1,
                    CrtReal.AreEquals(y, 0) ? 0.91 : 0.1,
                    CrtReal.AreEquals(z, 0) ? 0.91 : 0.1
                )
            );
        }

        private CrtShape DiePoint(CrtColor color, double tx, double ty, double tz, double size)
        {
            return CrtFactory.ShapeFactory.Sphere()
                .WithTransformationMatrix(
                    CrtFactory.TransformationFactory.TranslationMatrix(tx, ty, tz)
                    *
                    CrtFactory.TransformationFactory.ScalingMatrix(size, size, size)
                )
                .WithMaterial(
                    CrtFactory.MaterialFactory.DefaultMaterial.WithColor(color)
                );
        }

        private CrtShape SixSidedDie(CrtColor cubeColor, CrtColor pointColor)
        {
            // the cube
            CrtShape cube = CrtFactory.ShapeFactory.Cube()
                .WithMaterial(
                    CrtFactory.MaterialFactory.DefaultMaterial.WithColor(cubeColor)
                );

            var pointsEdges = CrtFactory.ShapeFactory.Group();
            double pointSize = 0.1;
            // 1 points on 1, 0, 0
            {
                pointsEdges.Add(DiePoint(pointColor, 1, 0, 0, pointSize));
            }
            // 6 point on -1, 0, 0
            {
                pointsEdges.Add(DiePoint(pointColor, -1, -0.4, -0.4, pointSize));
                pointsEdges.Add(DiePoint(pointColor, -1, -0.4, 0, pointSize));
                pointsEdges.Add(DiePoint(pointColor, -1, -0.4, 0.4, pointSize));
                pointsEdges.Add(DiePoint(pointColor, -1, 0.4, -0.4, pointSize));
                pointsEdges.Add(DiePoint(pointColor, -1, 0.4, 0, pointSize));
                pointsEdges.Add(DiePoint(pointColor, -1, 0.4, 0.4, pointSize));
            }
            // 5 points on 0, 1, 0
            {
                pointsEdges.Add(DiePoint(pointColor, 0, 1, 0, pointSize));
                pointsEdges.Add(DiePoint(pointColor, -0.4, 1, -0.4, pointSize));
                pointsEdges.Add(DiePoint(pointColor, 0.4, 1, -0.4, pointSize));
                pointsEdges.Add(DiePoint(pointColor, -0.4, 1, 0.4, pointSize));
                pointsEdges.Add(DiePoint(pointColor, 0.4, 1, 0.4, pointSize));
            }
            // 2 points on 0, -1, 0
            {
                pointsEdges.Add(DiePoint(pointColor, -0.4, -1, -0.4, pointSize));
                pointsEdges.Add(DiePoint(pointColor, 0.4, -1, 0.4, pointSize));
            }
            // 4 points on 0, 0, 1
            {
                pointsEdges.Add(DiePoint(pointColor, -0.4, -0.4, 1, pointSize));
                pointsEdges.Add(DiePoint(pointColor, 0.4, 0.4, 1, pointSize));
                pointsEdges.Add(DiePoint(pointColor, -0.4, 0.4, 1, pointSize));
                pointsEdges.Add(DiePoint(pointColor, 0.4, -0.4, 1, pointSize));
            }
            // 3 points on 0, 0, -1
            {
                pointsEdges.Add(DiePoint(pointColor, -0.4, -0.4, -1, pointSize));
                pointsEdges.Add(DiePoint(pointColor, 0, 0, -1, pointSize));
                pointsEdges.Add(DiePoint(pointColor, 0.4, 0.4, -1, pointSize));
            }

            // rounding the edges
            {
                pointsEdges.Add(DieEdge(cubeColor, 'z', 0, 1, 1));
                pointsEdges.Add(DieEdge(cubeColor, 'z', 0, 1, -1));
                pointsEdges.Add(DieEdge(cubeColor, 'z', 0, -1, 1));
                pointsEdges.Add(DieEdge(cubeColor, 'z', 0, -1, -1));

                pointsEdges.Add(DieEdge(cubeColor, 'x', 1, 1, 0));
                pointsEdges.Add(DieEdge(cubeColor, 'x', -1, 1, 0));
                pointsEdges.Add(DieEdge(cubeColor, 'x', 1, -1, 0));
                pointsEdges.Add(DieEdge(cubeColor, 'x', -1, -1, 0));

                pointsEdges.Add(DieEdge(cubeColor, 'y', 1, 0, 1));
                pointsEdges.Add(DieEdge(cubeColor, 'y', 1, 0, -1));
                pointsEdges.Add(DieEdge(cubeColor, 'y', -1, 0, 1));
                pointsEdges.Add(DieEdge(cubeColor, 'y', -1, 0, -1));
            }

            var vertices = CrtFactory.ShapeFactory.Group();
            // rounding the vertices
            {
                vertices.Add(DieVertice(cubeColor, 1, 1, 1));
                vertices.Add(DieVertice(cubeColor, 1, 1, -1));
                vertices.Add(DieVertice(cubeColor, 1, -1, 1));
                vertices.Add(DieVertice(cubeColor, 1, -1, -1));

                vertices.Add(DieVertice(cubeColor, -1, 1, 1));
                vertices.Add(DieVertice(cubeColor, -1, 1, -1));
                vertices.Add(DieVertice(cubeColor, -1, -1, 1));
                vertices.Add(DieVertice(cubeColor, -1, -1, -1));
            }



            var result = CrtFactory.ShapeFactory.Csg(CrtCSG.DIFFERENCE, cube, vertices);
            result = CrtFactory.ShapeFactory.Csg(CrtCSG.DIFFERENCE, result, pointsEdges);

            return result;
        }

        private void PrepareWorld(int hSize, int vSize)
        {
            //
            _world = CrtFactory.EngineFactory.World();
            // the room
            var room = CrtFactory.ShapeFactory.Cube()
                .WithTransformationMatrix(
                    CrtFactory.TransformationFactory.ScalingMatrix(60, 60, 60)
                )
                .WithMaterial(
                    CrtFactory.MaterialFactory.DefaultMaterial
                );
            _world.Add(room);
            //
            // Add six-sided dies
            var dies = CrtFactory.ShapeFactory.Group();
            var d1 = SixSidedDie(CrtColor.COLOR_GREEN, CrtColor.COLOR_WHITE)
                .WithTransformationMatrix(
                    CrtFactory.TransformationFactory.TranslationMatrix(-4, 0, 1)
                    *
                    CrtFactory.TransformationFactory.ScalingMatrix(3.5, 3.5, 3.5)
                );
            dies.Add(d1);
            var d2 = SixSidedDie(CrtColor.COLOR_RED, CrtColor.COLOR_WHITE)
                .WithTransformationMatrix(
                    CrtFactory.TransformationFactory.TranslationMatrix(4.5, 0, -4)
                    *
                    CrtFactory.TransformationFactory.YRotationMatrix(Math.PI/3)
                    *
                    CrtFactory.TransformationFactory.ZRotationMatrix(Math.PI / 2)
                    *
                    CrtFactory.TransformationFactory.ScalingMatrix(3.5, 3.5, 3.5)
                );
            dies.Add(d2);
            var d3 = SixSidedDie(CrtColor.COLOR_BLUE, CrtColor.COLOR_WHITE)
                .WithTransformationMatrix(
                    CrtFactory.TransformationFactory.TranslationMatrix(0, 7, 0)
                    *
                    CrtFactory.TransformationFactory.YRotationMatrix(Math.PI/6)
                    *
                    CrtFactory.TransformationFactory.ScalingMatrix(3.5, 3.5, 3.5)
                );
            dies.Add(d3);
            _world.Add(dies);
            //
            // add a light
            _world.Add(
                CrtFactory.LightFactory.PointLight(
                    CrtFactory.CoreFactory.Point(30, 40, -45),
                    CrtFactory.CoreFactory.Color(1, 1, 1)
                )
            );
            //
            _eyePosition = CrtFactory.CoreFactory.Point(0, 30, -30);
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
                _world.Objects[1].WithTransformationMatrix(
                    CrtFactory.TransformationFactory.YRotationMatrix(Math.PI / 6)
                    *
                    _world.Objects[1].TransformMatrix
                );
                mustRender = true;
            }

            if (state.IsKeyDown(Keys.Left))
            {
                _world.Objects[1].WithTransformationMatrix(
                    CrtFactory.TransformationFactory.YRotationMatrix(-Math.PI / 6)
                    *
                    _world.Objects[1].TransformMatrix
                );
                mustRender = true;
            }

            if (state.IsKeyDown(Keys.Up))
            {
                _world.Objects[1].WithTransformationMatrix(
                    CrtFactory.TransformationFactory.XRotationMatrix(Math.PI / 6)
                    *
                    _world.Objects[1].TransformMatrix
                );
                mustRender = true;
            }

            if (state.IsKeyDown(Keys.Down))
            {
                _world.Objects[1].WithTransformationMatrix(
                    CrtFactory.TransformationFactory.XRotationMatrix(-Math.PI / 6)
                    *
                    _world.Objects[1].TransformMatrix
                );
                mustRender = true;
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
