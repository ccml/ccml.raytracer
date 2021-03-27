using System;
using System.IO;
using ccml.raytracer;

namespace chapter04.exercise
{
    class Program
    {
        static void Main(string[] args)
        {
            var canvas = CrtFactory.EngineFactory.Canvas(200, 200);
            var clockDot = CrtFactory.CoreFactory.Point(1, 0, 0);
            var dotRotation = CrtFactory.TransformationFactory.ZRotationMatrix(Math.PI / 6);
            var dotTransformation = CrtFactory.TransformationFactory.TranslationMatrix(100, 100, 0) * CrtFactory.TransformationFactory.ScalingMatrix(75,75,1);
            for (int i = 0; i < 12; i++)
            {
                clockDot = dotRotation * clockDot;
                var dotToDraw = dotTransformation * clockDot;
                canvas[(int)dotToDraw.X, (int)dotToDraw.Y] = CrtFactory.CoreFactory.Color(0, 1, 0);
            }
            using (var sw = new StreamWriter(File.Create(@"D:\Temp\TheRayTracerChallenge\output\chapter04\clock.ppm")))
            {
                canvas.ToPPM(sw);
            }
            Console.WriteLine("Done !");
        }
    }
}
