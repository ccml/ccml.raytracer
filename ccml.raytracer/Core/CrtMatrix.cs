using System;

namespace ccml.raytracer.Core
{
    public class CrtMatrix
    {
        private readonly int _nbrRows;
        public int NbrRows => _nbrRows;

        private readonly int _nbrCols;
        public int NbrCols => _nbrCols;

        private readonly double[][] _matrix;

        internal CrtMatrix(int nbrRows, int nbrCols)
        {
            _nbrRows = nbrRows;
            _nbrCols = nbrCols;
            _matrix = new double[NbrRows][];
            // Initialize as an Identity matrix
            for (int r = 0; r < NbrRows; r++)
            {
                _matrix[r] = new double[NbrCols];
                for (int c = 0; c < NbrCols; c++)
                {
                    _matrix[r][c] = (r == c) ? 1.0 : 0.0;
                }
            }
        }

        public double this[int row, int col]
        {
            get => _matrix[row][col];
            set => _matrix[row][col] = value;
        }

        public static bool operator ==(CrtMatrix m1, CrtMatrix m2)
        {
            if (m1 is null || m2 is null) throw new ArgumentException();
            if ((m1.NbrRows != m2.NbrRows) || (m1.NbrCols != m2.NbrCols)) return false;
            for (int r = 0; r < m1.NbrRows; r++)
            {
                for (int c = 0; c < m1.NbrCols; c++)
                {
                    if (!CrtReal.AreEquals(m1[r, c], m2[r, c])) return false;
                }
            }
            return true;
        }

        public static bool operator !=(CrtMatrix m1, CrtMatrix m2)
        {
            if (m1 is null || m2 is null) throw new ArgumentException();
            if ((m1.NbrRows != m2.NbrRows) || (m1.NbrCols != m2.NbrCols)) return true;
            for (int r = 0; r < m1.NbrRows; r++)
            {
                for (int c = 0; c < m1.NbrCols; c++)
                {
                    if (!CrtReal.AreEquals(m1[r, c], m2[r, c])) return true;
                }
            }
            return false;
        }

        public static CrtMatrix operator *(CrtMatrix m1, CrtMatrix m2)
        {
            if (m1 is null || m2 is null) throw new ArgumentException();
            if(m1.NbrCols != m2.NbrRows) throw new ArgumentException();
            var result = new CrtMatrix(m1.NbrRows, m2.NbrCols);
            for (int r1 = 0; r1 < m1.NbrRows; r1++)
            {
                for (int c2 = 0; c2 < m2.NbrCols; c2++)
                {
                    double value = 0;
                    for (int i = 0; i < m1.NbrCols; i++)
                    {
                        value += m1[r1, i] * m2[i, c2];
                    }
                    result[r1, c2] = value;
                }
            }
            return result;
        }

        public static CrtTuple operator *(CrtMatrix m, CrtTuple t)
        {
            if (m is null || t is null) throw new ArgumentException();
            if (m.NbrRows != 4) throw new ArgumentException();
            if (m.NbrCols != 4) throw new ArgumentException();
            var resultComponents = new double[4];
            for (int i = 0; i < 4; i++)
            {
                double value = m[i, 0] * t.X + m[i, 1] * t.Y + m[i, 2] * t.Z + m[i, 3] * t.W;
                resultComponents[i] = value;
            }
            return CrtFactory.CoreFactory.Tuple(resultComponents[0], resultComponents[1], resultComponents[2], resultComponents[3]);
        }

        public static CrtPoint operator *(CrtMatrix m, CrtPoint p)
        {
            return (m * ((CrtTuple)p)) as CrtPoint;
        }

        public static CrtVector operator *(CrtMatrix m, CrtVector v)
        {
            return (m * ((CrtTuple)v)) as CrtVector;
        }

        public CrtMatrix Transpose()
        {
            var result = new CrtMatrix(NbrCols, NbrRows);
            for (int r = 0; r < NbrRows; r++)
            {
                for (int c = 0; c < NbrCols; c++)
                {
                    result._matrix[c][r] = _matrix[r][c];
                }
            }
            return result;
        }

        public double Det()
        {
            double result = 0;
            if (NbrRows == 2 & NbrCols == 2)
            {
                result = _matrix[0][0] * _matrix[1][1] - _matrix[0][1] * _matrix[1][0];
            }
            else
            {
                for (int c = 0; c < NbrCols; c++)
                {
                    result += _matrix[0][c] * Cofactor(0, c);
                }
            }
            return result;
        }

        public CrtMatrix SubMatrix(int row, int col)
        {
            var result = new CrtMatrix(NbrRows - 1, NbrCols - 1);
            for (int r = 0; r < NbrRows; r++)
            {
                for (int c = 0; c < NbrCols; c++)
                {
                    if ((r != row) && (c != col))
                    {
                        result._matrix[r < row ? r : r -1][c < col ? c : c -1] = _matrix[r][c];
                    }
                }
            }
            return result;
        }

        public double Minor(int row, int col)
        {
            return SubMatrix(row, col).Det();
        }

        public double Cofactor(int row, int col)
        {
            if ((row + col) % 2 == 0)
            {
                return Minor(row, col);
            }
            else
            {
                return -Minor(row, col);
            }
        }

        public bool IsInvertible()
        {
            return !CrtReal.AreEquals(Det(), 0.0);
        }

        public CrtMatrix Inverse()
        {
            if (!IsInvertible()) throw new Exception("Not invertible matrix !");
            var result = new CrtMatrix(NbrRows, NbrCols);
            var det = Det();
            for (int r = 0; r < NbrRows; r++)
            {
                for (int c = 0; c < NbrCols; c++)
                {
                    result._matrix[c][r] = Cofactor(r, c) / det;
                }
            }
            return result;
        }

        // -------------------------------------------------

        protected bool Equals(CrtMatrix other)
        {
            return Equals(_matrix, other._matrix) && NbrRows == other.NbrRows && NbrCols == other.NbrCols;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((CrtMatrix)obj);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(_matrix, NbrRows, NbrCols);
        }

    }
}
