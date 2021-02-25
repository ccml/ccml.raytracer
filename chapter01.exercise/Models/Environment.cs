using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ccml.raytracer.math.core;

namespace chapter01.exercise.Models
{
    public class Environment
    {
        public CrtVector Gravity { get; set; }
        public CrtVector Wind { get; set; }
    }
}
