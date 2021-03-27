using System;
using System.IO;
using ccml.raytracer;
using ccml.raytracer.Core;
using ccml.raytracer.Shapes;

namespace chapter05.exercise
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
                    var r = CrtFactory.EngineFactory.Ray(origin, CrtFactory.CoreFactory.Point(cx, cy, wallZ) - origin);
                    var xs = sphere.Intersect(r);
                    if (xs.Count > 0)
                    {
                        canvas[w, h] = CrtFactory.CoreFactory.Color(1, 0, 0);
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
            //  A canvas 100x100
            var canvasSize = 100;
            //  put the wall at z = 10
            var wallZ = 10.0;
            var wallSize = 7.0;
            //
            // start the ray at z = -5
            var origin = CrtFactory.CoreFactory.Point(0, 0, -5);
            //
            // trying to cast the shadow of a unit sphere onto some wall behind it
            var s = CrtFactory.ShapeFactory.Sphere();
            Process(canvasSize, origin, wallSize, wallZ, s, @"D:\Temp\TheRayTracerChallenge\output\chapter05\unit_sphere_shadow.ppm");
            //
            // trying to cast the shadow of a Y scaled sphere onto some wall behind it
            s = CrtFactory.ShapeFactory.Sphere();
            s.TransformMatrix = CrtFactory.TransformationFactory.ScalingMatrix(1,0.5,1);
            Process(canvasSize, origin, wallSize, wallZ, s, @"D:\Temp\TheRayTracerChallenge\output\chapter05\yscaled_sphere_shadow.ppm");
            //
            // trying to cast the shadow of a X scaled sphere onto some wall behind it
            s = CrtFactory.ShapeFactory.Sphere();
            s.TransformMatrix = CrtFactory.TransformationFactory.ScalingMatrix(0.5, 1, 1);
            Process(canvasSize, origin, wallSize, wallZ, s, @"D:\Temp\TheRayTracerChallenge\output\chapter05\xscaled_sphere_shadow.ppm");
            //
            // trying to cast the shadow of a x scaled and z rotated sphere onto some wall behind it
            s = CrtFactory.ShapeFactory.Sphere();
            s.TransformMatrix = CrtFactory.TransformationFactory.ZRotationMatrix(Math.PI/4) * CrtFactory.TransformationFactory.ScalingMatrix(0.5, 1, 1);
            Process(canvasSize, origin, wallSize, wallZ, s, @"D:\Temp\TheRayTracerChallenge\output\chapter05\xscaled_zrotation_sphere_shadow.ppm");
            //
            // trying to cast the shadow of a x scaled and skewed sphere onto some wall behind it
            s = CrtFactory.ShapeFactory.Sphere();
            s.TransformMatrix = CrtFactory.TransformationFactory.ShearingMatrix(1,0,0,0,0,0) * CrtFactory.TransformationFactory.ScalingMatrix(0.5, 1, 1);
            Process(canvasSize, origin, wallSize, wallZ, s, @"D:\Temp\TheRayTracerChallenge\output\chapter05\xscaled_skewed_sphere_shadow.ppm");
            //
            //
            Console.WriteLine("Done !");
        }
    }
}
