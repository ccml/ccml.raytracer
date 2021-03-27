using System;
using ccml.raytracer;
using chapter01.exercise.Models;
using Environment = chapter01.exercise.Models.Environment;

namespace chapter01.exercise
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
                Position = CrtFactory.CoreFactory.Point(0, 1, 0), 
                Velocity = ~CrtFactory.CoreFactory.Vector(1, 1, 0)
            };
            // gravity -0.1 unit/tick, and wind is -0.01 unit/tick.
            var e = new Environment
            {
                Gravity = CrtFactory.CoreFactory.Vector(0, -0.1, 0),
                Wind = CrtFactory.CoreFactory.Vector(-0.01, 0, 0)
            };

            while (p.Position.Y > 0)
            {
                p = Tick(p, e);
                Console.WriteLine(p.Position.Y);
            }
        }
    }
}
