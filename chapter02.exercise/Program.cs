using System;
using System.IO;
using ccml.raytracer.engine.core;
using chapter02.exercise.Models;
using Environment = chapter02.exercise.Models.Environment;

namespace chapter02.exercise
{
    class Program
    {
        private static Projectile Tick(Projectile proj, Environment env)
        {
            var position = proj.Position + proj.Velocity;
            var velocity = proj.Velocity + env.Gravity + env.Wind;
            return new Projectile { Position = position, Velocity = velocity };
        }

        static void Main(string[] args)
        {
            // projectile starts one unit above the origin.
            // velocity is normalized to 1 unit/tick.
            var p = new Projectile
            {
                Position = CrtFactory.Point(0, 1, 0),
                Velocity = (~CrtFactory.Vector(1, 1.8, 0)) * 11.25
            };
            // gravity -0.1 unit/tick, and wind is -0.01 unit/tick.
            var e = new Environment
            {
                Gravity = CrtFactory.Vector(0, -0.1, 0),
                Wind = CrtFactory.Vector(-0.01, 0, 0)
            };
            var c = CrtFactory.Canvas(900, 550);
            var pointColor = CrtFactory.Color(0.75, 1, 0.5);
            while (p.Position.Y > 0)
            {
                p = Tick(p, e);
                var w = (int)p.Position.X;
                var h = c.Height - (int)p.Position.Y;
                if (w >= 0 && w < c.Width && h >= 0 && h < c.Height)
                {
                    c[w, h] = pointColor;
                    Console.WriteLine($"{w} , {h}");
                }
            }
            using (var sw = new StreamWriter(File.Create(@"D:\Temp\TheRayTracerChallenge\output\chapter02\projectile.ppm")))
            {
                c.ToPPM(sw);
            }
            Console.WriteLine("Done !");
        }
    }
}
