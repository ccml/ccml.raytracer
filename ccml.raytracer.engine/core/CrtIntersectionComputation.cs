using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ccml.raytracer.engine.core;
using ccml.raytracer.engine.core.Shapes;

namespace ccml.raytracer.tests.math.core
{
    public class CrtIntersectionComputation
    {
        public double T { get; internal set; }
        public CrtShape TheObject { get; internal set; }
        public CrtPoint HitPoint { get; internal set; }
        public CrtVector EyeVector { get; internal set; }
        public CrtVector NormalVector { get; internal set; }
        public bool IsInside { get; internal set; }
        public CrtPoint OverPoint { get; set; }
    }
}
