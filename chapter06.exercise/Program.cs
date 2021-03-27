using System;
using System.IO;
using ccml.raytracer;
using ccml.raytracer.Core;
using ccml.raytracer.Shapes;

namespace chapter06.exercise
{
    class Program
    {
        private static void Process(
            int canvasSize,
            CrtPoint origin,
            double wallSize,
            double wallZ,
            CrtSphere sphere,
            string fileName
        )
        {
            var lightPosition = CrtFactory.CoreFactory.Point(-10, 10, -10);
            var lightColor = CrtFactory.CoreFactory.Color(1, 1, 1);
            var light = CrtFactory.LightFactory.PointLight(lightPosition, lightColor);
            //
            var canvas = CrtFactory.EngineFactory.Canvas(canvasSize, canvasSize);
            var canvasToWallFactor = wallSize / canvasSize;
            //
            var canvasHalfSize = canvasSize / 2;
            for (int h = 0; h < canvasSize; h++)
            {
                for (int w = 0; w < canvasSize; w++)
                {
                    var cx = (w - canvasHalfSize) * canvasToWallFactor;
                    var cy = -(h - canvasHalfSize) * canvasToWallFactor;
                    var r = CrtFactory.EngineFactory.Ray(origin, ~(CrtFactory.CoreFactory.Point(cx, cy, wallZ) - origin));
                    var xs = sphere.Intersect(r);
                    var intersection = CrtFactory.EngineFactory.Engine().Hit(xs);
                    if(intersection != null)
                    {
                        var hitPoint = r.PositionAtTime(intersection.T);
                        var normalVector = intersection.TheObject.NormalAt(hitPoint);
                        var eyeVector = -r.Direction;
                        var color = CrtFactory.EngineFactory.Engine().Lighting(
                            sphere.Material,
                            sphere,
                            light,
                            hitPoint,
                            eyeVector,
                            normalVector
                        );
                        canvas[w, h] = color;
                    }
                    else
                    {
                        canvas[w, h] = CrtFactory.CoreFactory.Color(0, 0, 0);
                    }
                }
            }
            //
            using (var sw = new StreamWriter(File.Create(fileName)))
            {
                canvas.ToPPM(sw);
            }
        }

        static void Main(string[] args)
        {
            //  A canvas 400x400
            var canvasSize = 400;
            //  put the wall at z = 10
            var wallZ = 10.0;
            var wallSize = 7.0;
            //
            // start the ray at z = -5
            var origin = CrtFactory.CoreFactory.Point(0, 0, -5);
            //
            // trying to cast the shadow of a unit sphere onto some wall behind it
            var s = CrtFactory.ShapeFactory.Sphere();
            s.Material.Color = CrtFactory.CoreFactory.Color(1, 0.2, 1);
            s.Material.Ambient = 0.05;
            s.Material.Diffuse = 0.9;
            s.Material.Specular = 0.9;
            s.Material.Shininess = 75.0;
            Process(canvasSize, origin, wallSize, wallZ, s, @"D:\Temp\TheRayTracerChallenge\output\chapter06\unit_sphere.ppm");
            //
            // trying to cast the shadow of a Y scaled sphere onto some wall behind it
            s = CrtFactory.ShapeFactory.Sphere();
            s.Material.Color = CrtFactory.CoreFactory.Color(1, 0.2, 1);
            s.TransformMatrix = CrtFactory.TransformationFactory.ScalingMatrix(1, 0.5, 1);
            Process(canvasSize, origin, wallSize, wallZ, s, @"D:\Temp\TheRayTracerChallenge\output\chapter06\yscaled_sphere.ppm");
            //
            // trying to cast the shadow of a X scaled sphere onto some wall behind it
            s = CrtFactory.ShapeFactory.Sphere();
            s.Material.Color = CrtFactory.CoreFactory.Color(1, 0.2, 1);
            s.TransformMatrix = CrtFactory.TransformationFactory.ScalingMatrix(0.5, 1, 1);
            Process(canvasSize, origin, wallSize, wallZ, s, @"D:\Temp\TheRayTracerChallenge\output\chapter06\xscaled_sphere.ppm");
            //
            // trying to cast the shadow of a x scaled and z rotated sphere onto some wall behind it
            s = CrtFactory.ShapeFactory.Sphere();
            s.Material.Color = CrtFactory.CoreFactory.Color(1, 0.2, 1);
            s.TransformMatrix = CrtFactory.TransformationFactory.ZRotationMatrix(Math.PI / 4) * CrtFactory.TransformationFactory.ScalingMatrix(0.5, 1, 1);
            Process(canvasSize, origin, wallSize, wallZ, s, @"D:\Temp\TheRayTracerChallenge\output\chapter06\xscaled_zrotation_sphere.ppm");
            //
            // trying to cast the shadow of a x scaled and skewed sphere onto some wall behind it
            s = CrtFactory.ShapeFactory.Sphere();
            s.Material.Color = CrtFactory.CoreFactory.Color(1, 0.2, 1);
            s.TransformMatrix = CrtFactory.TransformationFactory.ShearingMatrix(1, 0, 0, 0, 0, 0) * CrtFactory.TransformationFactory.ScalingMatrix(0.5, 1, 1);
            Process(canvasSize, origin, wallSize, wallZ, s, @"D:\Temp\TheRayTracerChallenge\output\chapter06\xscaled_skewed_sphere.ppm");
            //
            //
            Console.WriteLine("Done !");
        }
    }
}
