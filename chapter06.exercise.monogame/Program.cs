using System;
using System.Threading.Tasks;
using ccml.raytracer.engine.core;
using ccml.raytracer.engine.core.Materials;
using ccml.raytracer.engine.core.Shapes;
using ccml.raytracer.ui.monogame.screen;
using ccml.raytracer.ui.screen;

namespace chapter06.exercise.monogame
{
    class Program
    {
        private static bool _isDirty = false;
        private static CrtCanvas _canvas;
        private static MonoGameRaytracerWindow _window;

        private static void RenderSphere(
            int canvasSize,
            CrtPoint origin,
            double wallSize,
            double wallZ,
            CrtSphere sphere
        )
        {
            var lightPosition = CrtFactory.Point(-10, 10, -10);
            var lightColor = CrtFactory.Color(1, 1, 1);
            var light = CrtFactory.PointLight(lightPosition, lightColor);
            //
            _canvas = CrtFactory.Canvas(canvasSize, canvasSize);
            var canvasToWallFactor = wallSize / canvasSize;
            //
            var canvasHalfSize = canvasSize / 2;

            Parallel.For(0, canvasSize, new ParallelOptions {MaxDegreeOfParallelism = Environment.ProcessorCount - 2},
                h =>
                {
                    for (int w = 0; w < canvasSize; w++)
                    {
                        var cx = (w - canvasHalfSize) * canvasToWallFactor;
                        var cy = -(h - canvasHalfSize) * canvasToWallFactor;
                        var r = CrtFactory.Ray(origin, ~(CrtFactory.Point(cx, cy, wallZ) - origin));
                        var xs = sphere.Intersect(r);
                        var intersection = CrtFactory.Engine().Hit(xs);
                        if (intersection != null)
                        {
                            var hitPoint = r.PositionAtTime(intersection.T);
                            var normalVector = intersection.TheObject.NormalAt(hitPoint);
                            var eyeVector = -r.Direction;
                            var color = CrtFactory.Engine().Lighting(
                                sphere.Material,
                                sphere,
                                light,
                                hitPoint,
                                eyeVector,
                                normalVector
                            );
                            _canvas[w, h] = color;
                        }
                        else
                        {
                            _canvas[w, h] = CrtFactory.Color(0, 0, 0);
                        }
                    }
                }
            );
        }

        private static async Task Render(int canvasSize)
        {
            //  put the wall at z = 10
            var wallZ = 10.0;
            var wallSize = 7.0;
            //
            // start the ray at z = -5
            var origin = CrtFactory.Point(0, 0, -5);
            //
            // trying to cast the shadow of a unit sphere onto some wall behind it
            for (int i = 10; i < 200; i++)
            {
                while (_isDirty)
                {
                    await Task.Delay(5);
                }
                var s = CrtFactory.Sphere();
                s.Material.Color = CrtFactory.Color(1, 0.2, 1);
                s.Material.Ambient = 0.2;
                s.Material.Diffuse = 0.9;
                s.Material.Specular = 0.9;
                s.Material.Shininess = i;
                // s.SetTransformMatrix(CrtFactory.ZRotationMatrix(i * 2 * Math.PI / 360) * CrtFactory.ScalingMatrix(0.5, 1, 1));
                //
                RenderSphere(canvasSize, origin, wallSize, wallZ, s);
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
            //  A canvas 400x400
            var canvasSize = 400;
            //
            _window = new MonoGameRaytracerWindow(
                canvasSize, 
                canvasSize,
                async () => await Render(canvasSize),
                async () => await UpdateImage()
            );
            _window.Run();
        }
    }
}
