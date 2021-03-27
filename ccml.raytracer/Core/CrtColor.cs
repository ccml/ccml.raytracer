using System;

namespace ccml.raytracer.Core
{
    public class CrtColor
    {
        public static CrtColor COLOR_BLACK => CrtFactory.CoreFactory.Color(0.0, 0.0, 0.0);
        public static CrtColor COLOR_WHITE => CrtFactory.CoreFactory.Color(1.0, 1.0, 1.0);
        public static CrtColor COLOR_RED => CrtFactory.CoreFactory.Color(1.0, 0.0, 0.0);
        public static CrtColor COLOR_GREEN => CrtFactory.CoreFactory.Color(0.0, 1.0, 0.0);
        public static CrtColor COLOR_BLUE => CrtFactory.CoreFactory.Color(0.0, 0.0, 1.0);

        public double Red { get; set; }
        public double Green { get; set; }
        public double Blue { get; set; }

        internal CrtColor()
        {
            
        }

        /// <summary>
        /// Adding two colors
        /// </summary>
        /// <param name="c1">first color</param>
        /// <param name="c2">second color</param>
        /// <returns>The resulting color</returns>
        public static CrtColor operator +(CrtColor c1, CrtColor c2)
        {
            return CrtFactory.CoreFactory.Color(
                c1.Red + c2.Red, 
                c1.Green + c2.Green, 
                c1.Blue + c2.Blue
            );
        }

        /// <summary>
        /// Subtracting two colors
        /// </summary>
        /// <param name="c1">first color</param>
        /// <param name="c2">second color</param>
        /// <returns>The resulting color</returns>
        public static CrtColor operator -(CrtColor c1, CrtColor c2)
        {
            return CrtFactory.CoreFactory.Color(
                c1.Red - c2.Red,
                c1.Green - c2.Green,
                c1.Blue - c2.Blue
            );
        }

        /// <summary>
        /// Multiplying a color by a scalar
        /// </summary>
        /// <param name="c">a color</param>
        /// <param name="scalar">a scalar</param>
        /// <returns>The resulting color</returns>
        public static CrtColor operator *(CrtColor c, double scalar)
        {
            return CrtFactory.CoreFactory.Color(
                c.Red * scalar,
                c.Green * scalar,
                c.Blue * scalar
            );
        }

        /// <summary>
        /// Multiplying a color by a scalar
        /// </summary>
        /// <param name="scalar">a scalar</param>
        /// <param name="c">a color</param>
        /// <returns>The resulting color</returns>
        public static CrtColor operator *(double scalar, CrtColor c) => c * scalar;

        /// <summary>
        /// Multiplying two colors (Hadamard product)
        ///    Is used to blend two colors together ==>
        ///    To know the visible color of a colored surface is illuminated by a colored light
        /// </summary>
        /// <param name="c1">first color</param>
        /// <param name="c2">second color</param>
        /// <returns>The resulting color</returns>
        public static CrtColor operator *(CrtColor c1, CrtColor c2)
        {
            return CrtFactory.CoreFactory.Color(
                c1.Red * c2.Red,
                c1.Green * c2.Green,
                c1.Blue * c2.Blue
            );
        }

        public static bool operator ==(CrtColor c1, CrtColor c2)
        {
            if (c1 is null) throw new ArgumentException();
            if (c2 is null) throw new ArgumentException();
            return
                CrtReal.AreEquals(c1.Red, c2.Red)
                &&
                CrtReal.AreEquals(c1.Green, c2.Green)
                &&
                CrtReal.AreEquals(c1.Blue, c2.Blue);
        }

        public static bool operator !=(CrtColor c1, CrtColor c2)
        {
            if (c1 is null) throw new ArgumentException();
            if (c2 is null) throw new ArgumentException();
            return
                !CrtReal.AreEquals(c1.Red, c2.Red)
                ||
                !CrtReal.AreEquals(c1.Green, c2.Green)
                ||
                !CrtReal.AreEquals(c1.Blue, c2.Blue);
        }

        protected bool Equals(CrtColor other)
        {
            return Red.Equals(other.Red) && Green.Equals(other.Green) && Blue.Equals(other.Blue);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((CrtColor) obj);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Red, Green, Blue);
        }
    }
}
