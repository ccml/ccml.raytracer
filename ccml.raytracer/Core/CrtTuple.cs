using System;

namespace ccml.raytracer.Core
{
    /// <summary>
    /// A tuple allow to store a point or a vector.
    /// Points have W = 1.0
    /// Vectors have W = 0.0
    /// </summary>
    public class CrtTuple
    {
        public double X { get; set; }
        public double Y { get; set; }
        public double Z { get; set; }
        public double W { get; set; }

        internal CrtTuple(double x, double y, double z, double w)
        {
            X = x;
            Y = y;
            Z = z;
            W = w;
        }

        public static CrtTuple operator +(CrtTuple tuple1, CrtTuple tuple2)
        {
            if (tuple1 is null) throw new ArgumentException();
            if (tuple2 is null) throw new ArgumentException();
            if (CrtReal.AreEquals(tuple1.W, 1) && CrtReal.AreEquals(tuple2.W, 1)) throw new ArgumentException("Can't add 2 points");
            return CrtFactory.CoreFactory.Tuple(
                tuple1.X + tuple2.X,
                tuple1.Y + tuple2.Y,
                tuple1.Z + tuple2.Z,
                tuple1.W + tuple2.W
            );
        }

        public static CrtTuple operator -(CrtTuple tuple1, CrtTuple tuple2)
        {
            if (tuple1 is null) throw new ArgumentException();
            if (tuple2 is null) throw new ArgumentException();
            if (CrtReal.AreEquals(tuple1.W, 0) && CrtReal.AreEquals(tuple2.W, 1)) throw new ArgumentException("Can't subtract a point from a vector");
            return CrtFactory.CoreFactory.Tuple(
                tuple1.X - tuple2.X,
                tuple1.Y - tuple2.Y,
                tuple1.Z - tuple2.Z,
                tuple1.W - tuple2.W
            );
        }

        public static CrtTuple operator -(CrtTuple tuple)
        {
            if (tuple is null) throw new ArgumentException();
            return CrtFactory.CoreFactory.Tuple(-tuple.X, -tuple.Y, -tuple.Z, -tuple.W);
        }

        public static CrtTuple operator *(CrtTuple tuple, double scalar)
        {
            if (tuple is null) throw new ArgumentException();
            return CrtFactory.CoreFactory.Tuple(
                tuple.X * scalar,
                tuple.Y * scalar,
                tuple.Z * scalar,
                tuple.W * scalar
            );
        }

        public static CrtTuple operator *(double scalar, CrtTuple tuple)
        {
            if (tuple is null) throw new ArgumentException();
            return CrtFactory.CoreFactory.Tuple(
                tuple.X * scalar,
                tuple.Y * scalar,
                tuple.Z * scalar,
                tuple.W * scalar
            );
        }

        public static CrtTuple operator /(CrtTuple tuple, double scalar)
        {
            if (tuple is null) throw new ArgumentException();
            if(CrtReal.AreEquals(scalar, 0)) throw new DivideByZeroException();
            return CrtFactory.CoreFactory.Tuple(
                tuple.X / scalar,
                tuple.Y / scalar,
                tuple.Z / scalar,
                tuple.W / scalar
            );
        }

        public static bool operator ==(CrtTuple tuple1, CrtTuple tuple2)
        {
            if (tuple1 is null) throw new ArgumentException();
            if (tuple2 is null) throw new ArgumentException();
            return
                CrtReal.AreEquals(tuple1.X, tuple2.X)
                &&
                CrtReal.AreEquals(tuple1.Y, tuple2.Y)
                &&
                CrtReal.AreEquals(tuple1.Z, tuple2.Z)
                &&
                CrtReal.AreEquals(tuple1.W, tuple2.W);
        }

        public static bool operator !=(CrtTuple tuple1, CrtTuple tuple2)
        {
            if (tuple1 is null) throw new ArgumentException();
            if (tuple2 is null) throw new ArgumentException();
            return
                !CrtReal.AreEquals(tuple1.X, tuple2.X)
                ||
                !CrtReal.AreEquals(tuple1.Y, tuple2.Y)
                ||
                !CrtReal.AreEquals(tuple1.Z, tuple2.Z)
                ||
                !CrtReal.AreEquals(tuple1.W, tuple2.W);
        }

        protected bool Equals(CrtTuple other)
        {
            return CrtReal.AreEquals(X, other.X) && CrtReal.AreEquals(Y, other.Y) && CrtReal.AreEquals(Z, other.Z) && CrtReal.AreEquals(W, other.W);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((CrtTuple)obj);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(X, Y, Z, W);
        }

    }
}
