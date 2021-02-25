using System;
using ccml.raytracer.math.core;
using chapter01.exercise.Models;
using Environment = chapter01.exercise.Models.Environment;

namespace chapter01.exercise
{
    class Program
    {
        private static Projectile Tick(Projectile proj, Environment env)
        {
            var position = (CrtPoint)(proj.Position + proj.Velocity);
            var velocity = (CrtVector)(proj.Velocity + env.Gravity + env.Wind);
            return new Projectile { Position = position, Velocity = velocity };
        }

        static void Main(string[] args)
        {
            // projectile starts one unit above the origin.
            // velocity is normalized to 1 unit/tick.
            var p = new Projectile
            {
                Position = CrtTupleFactory.Point(0, 1, 0), 
                Velocity = ~CrtTupleFactory.Vector(1, 1, 0)
            };
            // gravity -0.1 unit/tick, and wind is -0.01 unit/tick.
            var e = new Environment
            {
                Gravity = CrtTupleFactory.Vector(0, -0.1, 0),
                Wind = CrtTupleFactory.Vector(-0.01, 0, 0)
            };

            while (p.Position.Y > 0)
            {
                p = Tick(p, e);
                Console.WriteLine(p.Position.Y);
            }
        }
    }
}
