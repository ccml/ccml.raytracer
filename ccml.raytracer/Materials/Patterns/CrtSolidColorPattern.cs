using ccml.raytracer.Core;

namespace ccml.raytracer.Materials.Patterns
{
    public class CrtSolidColorPattern : CrtPattern
    {
        public CrtColor Color { get; private set; }

        internal CrtSolidColorPattern(CrtColor color)
        {
            Color = color;
        }

        public override CrtColor PatternAt(CrtPoint point)
        {
            return Color;
        }
    }
}
