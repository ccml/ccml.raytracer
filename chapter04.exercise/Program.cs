using System;
using System.IO;
using ccml.raytracer.engine.core;

namespace chapter04.exercise
{
    class Program
    {
        static void Main(string[] args)
        {
            var canvas = CrtFactory.Canvas(200, 200);
            var clockDot = CrtFactory.Point(1, 0, 0);
            var dotRotation = CrtFactory.ZRotationMatrix(Math.PI / 6);
            var dotTransformation = CrtFactory.TranslationMatrix(100, 100, 0) * CrtFactory.ScalingMatrix(75,75,1);
            for (int i = 0; i < 12; i++)
            {
                clockDot = dotRotation * clockDot;
                var dotToDraw = dotTransformation * clockDot;
                canvas[(int)dotToDraw.X, (int)dotToDraw.Y] = CrtFactory.Color(0, 1, 0);
            }
            using (var sw = new StreamWriter(File.Create(@"D:\Temp\TheRayTracerChallenge\output\chapter04\clock.ppm")))
            {
                canvas.ToPPM(sw);
            }
            Console.WriteLine("Done !");
        }
    }
}
