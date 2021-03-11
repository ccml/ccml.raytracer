using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ccml.raytracer.engine.core.Lights
{
    public class CrtPointLight
    {
        public CrtPoint Position { get; private set; }
        public CrtColor Intensity { get; private set; }

        internal CrtPointLight(CrtPoint position, CrtColor intensity)
        {
            Position = position;
            Intensity = intensity;
        }

        public static bool operator ==(CrtPointLight l1, CrtPointLight l2)
        {
            return l1.Position == l2.Position
                   &&
                   l1.Intensity == l2.Intensity;
        }

        public static bool operator !=(CrtPointLight l1, CrtPointLight l2)
        {
            return l1.Position != l2.Position
                   ||
                   l1.Intensity != l2.Intensity;
        }

        protected bool Equals(CrtPointLight other)
        {
            return Equals(Position, other.Position) && Equals(Intensity, other.Intensity);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((CrtPointLight) obj);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Position, Intensity);
        }
    }
}
