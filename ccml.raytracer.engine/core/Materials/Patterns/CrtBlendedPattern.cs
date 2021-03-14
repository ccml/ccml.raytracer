﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ccml.raytracer.engine.core.Materials.Patterns
{
    public class CrtBlendedPattern : CrtPattern
    {
        public const string BLENDING_METHOD_AVERAGE = "Average";
        public const string BLENDING_METHOD_INVERT_XY_AVERAGE = "InvertXYAverage";
        public const string BLENDING_METHOD_INVERT_XZ_AVERAGE = "InvertXZAverage";
        public const string BLENDING_METHOD_INVERT_YZ_AVERAGE = "InvertYZAverage";

        public string BlendingMethod { get; private set; }

        public CrtPattern PatternA { get; private set; }
        public CrtPattern PatternB { get; private set; }

        internal CrtBlendedPattern(CrtPattern patternA, CrtPattern patternB, string blendingMethod = BLENDING_METHOD_AVERAGE)
        {
            BlendingMethod = blendingMethod;
            PatternA = patternA;
            PatternB = patternB;
        }

        public override CrtColor PatternAt(CrtPoint point)
        {
            if (
                (BlendingMethod == BLENDING_METHOD_AVERAGE)
            )
            {
                var colorA = PatternA.PatternAt(point);
                var colorB = PatternB.PatternAt(point);
                return CrtFactory.Color(
                    (colorA.Red + colorB.Red) / 2.0,
                    (colorA.Green + colorB.Green) / 2.0,
                    (colorA.Blue + colorB.Blue) / 2.0
                );
            }
            else if (
                (BlendingMethod == BLENDING_METHOD_INVERT_XY_AVERAGE)
            )
            {
                var colorA = PatternA.PatternAt(point);
                var colorB = PatternB.PatternAt(CrtFactory.Point(point.Y, point.X, point.Z));
                return CrtFactory.Color(
                    (colorA.Red + colorB.Red) / 2.0,
                    (colorA.Green + colorB.Green) / 2.0,
                    (colorA.Blue + colorB.Blue) / 2.0
                );
            }
            else if (
                (BlendingMethod == BLENDING_METHOD_INVERT_XZ_AVERAGE)
            )
            {
                var colorA = PatternA.PatternAt(point);
                var colorB = PatternB.PatternAt(CrtFactory.Point(point.Z, point.Y, point.X));
                return CrtFactory.Color(
                    (colorA.Red + colorB.Red) / 2.0,
                    (colorA.Green + colorB.Green) / 2.0,
                    (colorA.Blue + colorB.Blue) / 2.0
                );
            }
            else if (
                (BlendingMethod == BLENDING_METHOD_INVERT_YZ_AVERAGE)
            )
            {
                var colorA = PatternA.PatternAt(point);
                var colorB = PatternB.PatternAt(CrtFactory.Point(point.X, point.Z, point.Y));
                return CrtFactory.Color(
                    (colorA.Red + colorB.Red) / 2.0,
                    (colorA.Green + colorB.Green) / 2.0,
                    (colorA.Blue + colorB.Blue) / 2.0
                );
            }
            throw new Exception("CrtBlendedPattern : unknown blending method : " + BlendingMethod);
        }
    }
}
