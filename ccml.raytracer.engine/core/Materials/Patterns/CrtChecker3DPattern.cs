﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ccml.raytracer.engine.core.Materials.Patterns
{
    public class CrtChecker3DPattern : CrtPattern
    {
        public CrtColor ColorA { get; private set; }
        public CrtColor ColorB { get; private set; }

        internal CrtChecker3DPattern(CrtColor colorA, CrtColor colorB)
        {
            ColorA = colorA;
            ColorB = colorB;
        }

        public override CrtColor PatternAt(CrtPoint point)
        {
            return CrtReal.AreEquals(
                (Math.Floor(point.X) + Math.Floor(point.Y) + Math.Floor(point.Z)) % 2.0,
                0.0
            ) ? ColorA : ColorB;
        }
    }
}
