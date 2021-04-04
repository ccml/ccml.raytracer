using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ccml.raytracer.Core;

namespace ccml.raytracer.Shapes
{
    public class CrtBoundingBox
    {
        public CrtPoint Minimum { get; set; }
        public CrtPoint Maximum { get; set; }
    }
}
