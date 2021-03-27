using ccml.raytracer.Core;
using ccml.raytracer.Materials.Patterns;

namespace ccml.raytracer.Tests
{
    public class CrtTestPattern : CrtPattern
    {
        public override CrtColor PatternAt(CrtPoint point)
        {
            return CrtFactory.CoreFactory.Color(point.X, point.Y, point.Z);
        }
    }
}
