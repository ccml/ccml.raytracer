using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ccml.raytracer.Core;

namespace chapter01.exercise.Models
{
    public class Projectile
    {
        public CrtPoint Position { get; set; }
        public CrtVector Velocity { get; set; }
    }
}
