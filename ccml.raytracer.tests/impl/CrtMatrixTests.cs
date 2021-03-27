using ccml.raytracer.Core;
using NUnit.Framework;

namespace ccml.raytracer.tests.impl
{
    public class CrtMatrixTests
    {
        [SetUp]
        public void Setup()
        {

        }


        // Scenario: Constructing and inspecting a 4x4 matrix
        [Test]
        public void ConstructingAndInspectingA4x4Matrix()
        {
            // Given the following 4x4 matrix M:
            //    | 1 | 2 | 3 | 4 |
            //    | 5.5 | 6.5 | 7.5 | 8.5 |
            //    | 9 | 10 | 11 | 12 |
            //    | 13.5 | 14.5 | 15.5 | 16.5 |
            var m = CrtFactory.CoreFactory.Matrix(
                new double[] {1, 2, 3, 4 },
                new double[] {5.5, 6.5, 7.5, 8.5 },
                new double[] {9, 10, 11, 12 },
                new double[] {13.5, 14.5, 15.5, 16.5 }
            );
            // Then M[0, 0] = 1
            Assert.IsTrue(CrtReal.AreEquals(1, m[0, 0]));
            // And M[0, 3] = 4
            Assert.IsTrue(CrtReal.AreEquals(4, m[0, 3]));
            // And M[1, 0] = 5.5
            Assert.IsTrue(CrtReal.AreEquals(5.5, m[1, 0]));
            // And M[1, 2] = 7.5
            Assert.IsTrue(CrtReal.AreEquals(7.5, m[1, 2]));
            // And M[2, 2] = 11
            Assert.IsTrue(CrtReal.AreEquals(11, m[2, 2]));
            // And M[3, 0] = 13.5
            Assert.IsTrue(CrtReal.AreEquals(13.5, m[3, 0]));
            // And M[3, 2] = 15.5
            Assert.IsTrue(CrtReal.AreEquals(15.5, m[3, 2]));
        }

        //Scenario: A 2x2 matrix ought to be representable
        [Test]
        public void A2x2MatrixOughtToBeRepresentable()
        {
            // Given the following 2x2 matrix M:
            //    | -3 | 5 |
            //    | 1 | -2 |
            var m = CrtFactory.CoreFactory.Matrix(
                new double[] { -3, 5 },
                new double[] { 1, -2 }
            );
            // Then M[0, 0] = -3
            Assert.IsTrue(CrtReal.AreEquals(-3, m[0, 0]));
            // And M[0, 1] = 5
            Assert.IsTrue(CrtReal.AreEquals(5, m[0, 1]));
            // And M[1, 0] = 1
            Assert.IsTrue(CrtReal.AreEquals(1, m[1, 0]));
            // And M[1, 1] = -2
            Assert.IsTrue(CrtReal.AreEquals(-2, m[1, 1]));
        }


        //Scenario: A 3x3 matrix ought to be representable
        [Test]
        public void A3x3MatrixOughtToBeRepresentable()
        {
            // Given the following 3x3 matrix M:
            //    | -3 | 5  | 0  |
            //    | 1  | -2 | -7 |
            //    | 0  | 1  | 1  |
            var m = CrtFactory.CoreFactory.Matrix(
                new double[] { -3, 5, 0 },
                new double[] { 1, -2, -7 },
                new double[] { 0, 1, 1 }
            );
            // Then M[0, 0] = -3
            Assert.IsTrue(CrtReal.AreEquals(-3, m[0, 0]));
            // And M[1, 1] = -2
            Assert.IsTrue(CrtReal.AreEquals(-2, m[1, 1]));
            // And M[2, 2] = 1
            Assert.IsTrue(CrtReal.AreEquals(1, m[2, 2]));
        }

        //Scenario: Matrix equality with identical matrices
        [Test]
        public void MatrixEqualityWithIdenticalMatrices()
        {
            // Given the following matrix A:
            //    | 1 | 2 | 3 | 4 |
            //    | 5 | 6 | 7 | 8 |
            //    | 9 | 8 | 7 | 6 |
            //    | 5 | 4 | 3 | 2 |
            var a = CrtFactory.CoreFactory.Matrix(
                new double[] { 1, 2, 3, 4 },
                new double[] { 5, 6, 7, 8 },
                new double[] { 9, 8, 7, 6 },
                new double[] { 5, 4, 3, 2 }
            );
            // And the following matrix B:
            //    | 1 | 2 | 3 | 4 |
            //    | 5 | 6 | 7 | 8 |
            //    | 9 | 8 | 7 | 6 |
            //    | 5 | 4 | 3 | 2 |
            var b = CrtFactory.CoreFactory.Matrix(
                new double[] { 1, 2, 3, 4 },
                new double[] { 5, 6, 7, 8 },
                new double[] { 9, 8, 7, 6 },
                new double[] { 5, 4, 3, 2 }
            );
            // Then A = B
            Assert.IsTrue(a == b);
            Assert.IsFalse(a != b);
        }

        //Scenario: Matrix equality with different matrices
        [Test]
        public void MatrixEqualityWithDifferentMatrices()
        {
            // Given the following matrix A:
            //    | 1 | 2 | 3 | 4 |
            //    | 5 | 6 | 7 | 8 |
            //    | 9 | 8 | 7 | 6 |
            //    | 5 | 4 | 3 | 2 |
            var a = CrtFactory.CoreFactory.Matrix(
                new double[] { 1, 2, 3, 4 },
                new double[] { 5, 6, 7, 8 },
                new double[] { 9, 8, 7, 6 },
                new double[] { 5, 4, 3, 2 }
            );
            // And the following matrix B:
            //    | 2 | 3 | 4 | 5 |
            //    | 6 | 7 | 8 | 9 |
            //    | 8 | 7 | 6 | 5 |
            //    | 4 | 3 | 2 | 1 |
            var b = CrtFactory.CoreFactory.Matrix(
                new double[] { 2, 3, 4, 5 },
                new double[] { 6, 7, 8, 9 },
                new double[] { 8, 7, 6, 5 },
                new double[] { 4, 3, 2, 1 }
            );
            // Then A != B
            Assert.IsTrue(a != b);
            Assert.IsFalse(a == b);
        }


        // Scenario: Multiplying two matrices
        [Test]
        public void MultiplyingTwoMatrices()
        {
            // Given the following matrix A:
            //    | 1 | 2 | 3 | 4 |
            //    | 5 | 6 | 7 | 8 |
            //    | 9 | 8 | 7 | 6 |
            //    | 5 | 4 | 3 | 2 |
            var a = CrtFactory.CoreFactory.Matrix(
                new double[] { 1, 2, 3, 4 },
                new double[] { 5, 6, 7, 8 },
                new double[] { 9, 8, 7, 6 },
                new double[] { 5, 4, 3, 2 }
            );
            // And the following matrix B:
            //    | -2 | 1 | 2 | 3 |
            //    | 3 | 2 | 1 | -1 |
            //    | 4 | 3 | 6 | 5 |
            //    | 1 | 2 | 7 | 8 |
            var b = CrtFactory.CoreFactory.Matrix(
                new double[] { -2, 1, 2, 3 },
                new double[] { 3, 2, 1, -1 },
                new double[] { 4, 3, 6, 5 },
                new double[] { 1, 2, 7, 8 }
            );
            // Then A * B is the following 4x4 matrix:
            //    | 20| 22 | 50 | 48 |
            //    | 44| 54 | 114 | 108 |
            //    | 40| 58 | 110 | 102 |
            //    | 16| 26 | 46 | 42 |
            var expectedResult = CrtFactory.CoreFactory.Matrix(
                new double[] { 20, 22, 50, 48 },
                new double[] { 44, 54, 114, 108 },
                new double[] { 40, 58, 110, 102 },
                new double[] { 16, 26, 46, 42 }
            );
            Assert.IsTrue(a * b == expectedResult);
        }

        //Scenario: A matrix multiplied by a tuple
        [Test]
        public void AMatrixMultipliedByATuple()
        {
            // Given the following matrix A:
            //    | 1 | 2 | 3 | 4 |
            //    | 2 | 4 | 4 | 2 |
            //    | 8 | 6 | 4 | 1 |
            //    | 0 | 0 | 0 | 1 |
            var a = CrtFactory.CoreFactory.Matrix(
                new double[] { 1, 2, 3, 4 },
                new double[] { 2, 4, 4, 2 },
                new double[] { 8, 6, 4, 1 },
                new double[] { 0, 0, 0, 1 }
            );
            // And b ← tuple(1, 2, 3, 1)
            var b = CrtFactory.CoreFactory.Tuple(1, 2, 3, 1);
            // Then A * b = tuple(18, 24, 33, 1)
            Assert.IsTrue(a * b == CrtFactory.CoreFactory.Tuple(18, 24, 33, 1));
        }

        //Scenario: Multiplying a matrix by the identity matrix
        [Test]
        public void MultiplyingAMatrixByTheIdentityMatrix()
        {
            // Given the following matrix A:
            //    | 0 | 1 | 2 | 4 |
            //    | 1 | 2 | 4 | 8 |
            //    | 2 | 4 | 8 | 16 |
            //    | 4 | 8 | 16 | 32 |
            var a = CrtFactory.CoreFactory.Matrix(
                new double[] { 0, 1, 2, 4 },
                new double[] { 1, 2, 4, 8 },
                new double[] { 2, 4, 8, 16 },
                new double[] { 4, 8, 16, 32 }
            );
            //Then A * identity_matrix = A
            Assert.IsTrue(a * CrtFactory.TransformationFactory.IdentityMatrix(4,4) == a);
        }

        // Scenario: Multiplying the identity matrix by a tuple
        [Test]
        public void MultiplyingTheIdentityMatrixByATuple()
        {
            // Given a ← tuple(1, 2, 3, 4)
            var a = CrtFactory.CoreFactory.Tuple(1, 2, 3, 4);
            // Then identity_matrix * a = a
            Assert.IsTrue(CrtFactory.TransformationFactory.IdentityMatrix(4,4) * a == a);
        }

        // Scenario: Transposing a matrix
        [Test]
        public void TransposingAMatrix()
        {
            // Given the following matrix A:
            //    | 0 | 9 | 3 | 0 |
            //    | 9 | 8 | 0 | 8 |
            //    | 1 | 8 | 5 | 3 |
            //    | 0 | 0 | 5 | 8 |
            var a = CrtFactory.CoreFactory.Matrix(
                new double[] { 0, 9, 3, 0 },
                new double[] { 9, 8, 0, 8 },
                new double[] { 1, 8, 5, 3 },
                new double[] { 0, 0, 5, 8 }
            );
            // Then transpose(A) is the following matrix:
            //    | 0 | 9 | 1 | 0 |
            //    | 9 | 8 | 8 | 0 |
            //    | 3 | 0 | 5 | 5 |
            //    | 0 | 8 | 3 | 8 |
            var expectedResult = CrtFactory.CoreFactory.Matrix(
                new double[] { 0, 9, 1, 0 },
                new double[] { 9, 8, 8, 0 },
                new double[] { 3, 0, 5, 5 },
                new double[] { 0, 8, 3, 8 }
            );
            Assert.IsTrue(a.Transpose() == expectedResult);
        }

        // Scenario: Transposing the identity matrix
        [Test]
        public void TransposingTheIdentityMatrix()
        {
            // Given A ← transpose(identity_matrix)
            var a = CrtFactory.TransformationFactory.IdentityMatrix(4, 4).Transpose();
            // Then A = identity_matrix
            Assert.IsTrue(a == CrtFactory.TransformationFactory.IdentityMatrix(4,4));
        }

        // Scenario: Calculating the determinant of a 2x2 matrix
        [Test]
        public void CalculatingTheDeterminantOfA2x2Matrix()
        {
            // Given the following 2x2 matrix A:
            //    | 1 | 5 |
            //    | -3 | 2 |
            var a = CrtFactory.CoreFactory.Matrix(
                new double[] { 1, 5 },
                new double[] { -3, 2 }
            );
            // Then determinant(A) = 17
            Assert.IsTrue(CrtReal.AreEquals(a.Det(), 17));
        }

        //Scenario: A submatrix of a 3x3 matrix is a 2x2 matrix
        [Test]
        public void ASubmatrixOfA3x3MatrixIsA2x2Matrix()
        {
            // Given the following 3x3 matrix A:
            //    | 1 | 5 | 0 |
            //    | -3 | 2 | 7 |
            //    | 0 | 6 | -3 |
            var a = CrtFactory.CoreFactory.Matrix(
                new double[] { 1, 5, 0 },
                new double[] { -3, 2, 7 },
                new double[] { 0, 6, -3 }
            );
            // Then submatrix(A, 0, 2) is the following 2x2 matrix:
            //   | -3 | 2 |
            //   | 0 | 6 |
            var expectedResult = CrtFactory.CoreFactory.Matrix(
                new double[] { -3, 2 },
                new double[] { 0, 6 }
            );
            Assert.IsTrue(a.SubMatrix(0, 2) == expectedResult);
        }

        //Scenario: A submatrix of a 4x4 matrix is a 3x3 matrix
        [Test]
        public void ASubmatrixOfA4x4MatrixIsA3x3Matrix()
        {
            //Given the following 4x4 matrix A:
            //   | -6 | 1 | 1 | 6 |
            //   | -8 | 5 | 8 | 6 |
            //   | -1 | 0 | 8 | 2 |
            //   | -7 | 1 | -1 | 1 |
            var a = CrtFactory.CoreFactory.Matrix(
                new double[] { -6, 1, 1, 6 },
                new double[] { -8, 5, 8, 6 },
                new double[] { -1, 0, 8, 2 },
                new double[] { -7, 1, -1, 1 }
            );
            // Then submatrix(A, 2, 1) is the following 3x3 matrix:
            //   | -6 | 1 | 6 |
            //   | -8 | 8 | 6 |
            //  | -7 | -1 | 1 |
            var expectedResult = CrtFactory.CoreFactory.Matrix(
                new double[] { -6, 1, 6 },
                new double[] { -8, 8, 6 },
                new double[] { -7, -1, 1 }
            );
            Assert.IsTrue(a.SubMatrix(2, 1) == expectedResult);
        }

        //Scenario: Calculating a minor of a 3x3 matrix
        [Test]
        public void CalculatingAMinorOfA3x3Matrix()
        {
            // Given the following 3x3 matrix A:
            //    | 3 | 5 | 0 |
            //    | 2 | -1 | -7 |
            //    | 6 | -1 | 5 |
            var a = CrtFactory.CoreFactory.Matrix(
                new double[] { 3, 5, 0 },
                new double[] { 2, -1, -7 },
                new double[] { 6, -1, 5 }
            );
            // And B ← submatrix(A, 1, 0)
            var b = a.SubMatrix(1, 0);
            // Then determinant(B) = 25
            Assert.IsTrue(CrtReal.AreEquals(b.Det(), 25));
            // And minor(A, 1, 0) = 25
            Assert.IsTrue(CrtReal.AreEquals(a.Minor(1,0), 25));
        }

        //Scenario: Calculating a cofactor of a 3x3 matrix
        [Test]
        public void CalculatingACofactorOfA3x3Matrix()
        {
            // Given the following 3x3 matrix A:
            //    | 3 | 5 | 0 |
            //    | 2 | -1 | -7 |
            //    | 6 | -1 | 5 |
            var a = CrtFactory.CoreFactory.Matrix(
                new double[] { 3, 5, 0 },
                new double[] { 2, -1, -7 },
                new double[] { 6, -1, 5 }
            );
            // Then minor(A, 0, 0) = -12
            Assert.IsTrue(CrtReal.AreEquals(a.Minor(0,0), -12));
            // And cofactor(A, 0, 0) = -12
            Assert.IsTrue(CrtReal.AreEquals(a.Cofactor(0, 0), -12));
            // And minor(A, 1, 0) = 25
            Assert.IsTrue(CrtReal.AreEquals(a.Minor(1, 0), 25));
            // And cofactor(A, 1, 0) = -25
            Assert.IsTrue(CrtReal.AreEquals(a.Cofactor(1, 0), -25));
        }

        //Scenario: Calculating the determinant of a 3x3 matrix
        [Test]
        public void CalculatingTheDeterminantOfA3x3Matrix()
        {
            // Given the following 3x3 matrix A:
            //    | 1 | 2 | 6 |
            //    | -5 | 8 | -4 |
            //    | 2 | 6 | 4 |
            var a = CrtFactory.CoreFactory.Matrix(
                new double[] { 1, 2, 6 },
                new double[] { -5, 8, -4 },
                new double[] { 2, 6, 4 }
            );
            // Then cofactor(A, 0, 0) = 56
            Assert.IsTrue(CrtReal.AreEquals(a.Cofactor(0,0), 56));
            // And cofactor(A, 0, 1) = 12
            Assert.IsTrue(CrtReal.AreEquals(a.Cofactor(0, 1), 12));
            // And cofactor(A, 0, 2) = -46
            Assert.IsTrue(CrtReal.AreEquals(a.Cofactor(0, 2), -46));
            // And determinant(A) = -196
            Assert.IsTrue(CrtReal.AreEquals(a.Det(), -196));
        }

        //Scenario: Calculating the determinant of a 4x4 matrix
        [Test]
        public void CalculatingTheDeterminantOfA4x4Matrix()
        {
            // Given the following 4x4 matrix A:
            //    | -2 | -8 | 3 | 5 |
            //    | -3 | 1 | 7 | 3 |
            //    | 1 | 2 | -9 | 6 |
            //    | -6 | 7 | 7 | -9 |
            var a = CrtFactory.CoreFactory.Matrix(
                new double[] { -2, -8, 3, 5 },
                new double[] { -3, 1, 7, 3 },
                new double[] { 1, 2, -9, 6 },
                new double[] { -6, 7, 7, -9 }
            );
            // Then cofactor(A, 0, 0) = 690
            Assert.IsTrue(CrtReal.AreEquals(a.Cofactor(0, 0), 690));
            // And cofactor(A, 0, 1) = 447
            Assert.IsTrue(CrtReal.AreEquals(a.Cofactor(0, 1), 447));
            // And cofactor(A, 0, 2) = 210
            Assert.IsTrue(CrtReal.AreEquals(a.Cofactor(0, 2), 210));
            // And cofactor(A, 0, 3) = 51
            Assert.IsTrue(CrtReal.AreEquals(a.Cofactor(0, 3), 51));
            // And determinant(A) = -4071
            Assert.IsTrue(CrtReal.AreEquals(a.Det(), -4071));
        }

        // Scenario: Testing an invertible matrix for invertibility
        [Test]
        public void TestingAnInvertibleMatrixForInvertibility()
        {
            // Given the following 4x4 matrix A:
            //    | 6 | 4 | 4 | 4 |
            //    | 5 | 5 | 7 | 6 |
            //    | 4 | -9 | 3 | -7 |
            //    | 9 | 1 | 7 | -6 |
            var a = CrtFactory.CoreFactory.Matrix(
                new double[] { 6, 4, 4 ,4 },
                new double[] { 5, 5, 7, 6 },
                new double[] { 4, -9, 3, -7 },
                new double[] { 9, 1, 7, -6 }
            );
            // Then determinant(A) = -2120
            Assert.IsTrue(CrtReal.AreEquals(a.Det(), -2120));
            // And A is invertible
            Assert.IsTrue(a.IsInvertible());
        }

        // Scenario: Testing a noninvertible matrix for invertibility
        [Test]
        public void TestingANoninvertibleMatrixForInvertibility()
        {
            // Given the following 4x4 matrix A:
            //    | -4 | 2 | -2 | -3 |
            //    | 9 | 6 | 2 | 6 |
            //    | 0 | -5 | 1 | -5 |
            //    | 0 | 0 | 0 | 0 |
            var a = CrtFactory.CoreFactory.Matrix(
                new double[] { -4, 2, -2, -3 },
                new double[] { 9, 6, 2, 6 },
                new double[] { 0, -5, 1, -5 },
                new double[] { 0, 0, 0, 0 }
            );
            //Then determinant(A) = 0
            Assert.IsTrue(CrtReal.AreEquals(a.Det(), 0));
            //And A is not invertible
            Assert.IsFalse(a.IsInvertible());
        }

        // Scenario: Calculating the inverse of a matrix
        [Test]
        public void CalculatingTheInverseOfAMatrix()
        {
            // Given the following 4x4 matrix A:
            //    | -5 | 2 | 6 | -8 |
            //    | 1 | -5 | 1 | 8 |
            //    | 7 | 7 | -6 | -7 |
            //    | 1 | -3 | 7 | 4 |
            var a = CrtFactory.CoreFactory.Matrix(
                new double[] { -5, 2, 6, -8 },
                new double[] { 1, -5, 1, 8 },
                new double[] { 7, 7, -6, -7 },
                new double[] { 1, -3, 7, 4 }
            );
            // And B ← inverse(A)
            var b = a.Inverse();
            // Then determinant(A) = 532
            Assert.IsTrue(CrtReal.AreEquals(a.Det(), 532));
            // And cofactor(A, 2, 3) = -160
            Assert.IsTrue(CrtReal.AreEquals(a.Cofactor(2, 3), -160));
            // And B[3, 2] = -160 / 532
            Assert.IsTrue(CrtReal.AreEquals(b[3, 2], -160.0 / 532.0));
            // And cofactor(A, 3, 2) = 105
            Assert.IsTrue(CrtReal.AreEquals(a.Cofactor(3, 2), 105));
            // And B[2, 3] = 105 / 532
            Assert.IsTrue(CrtReal.AreEquals(b[2, 3], 105.0 / 532.0));
            // And B is the following 4x4 matrix:
            //    | 0.21805 | 0.45113 | 0.24060 | -0.04511 |
            //    | -0.80827 | -1.45677 | -0.44361 | 0.52068 |
            //    | -0.07895 | -0.22368 | -0.05263 | 0.19737 |
            //    | -0.52256 | -0.81391 | -0.30075 | 0.30639 |
            var expectedResult = CrtFactory.CoreFactory.Matrix(
                new double[] { 0.21805, 0.45113, 0.24060, -0.04511 },
                new double[] { -0.80827, -1.45677, -0.44361, 0.52068 },
                new double[] { -0.07895, -0.22368, -0.05263, 0.19737 },
                new double[] { -0.52256, -0.81391, -0.30075, 0.30639 }
            );
            Assert.IsTrue(b == expectedResult);
        }

        // Scenario: Calculating the inverse of another matrix
        [Test]
        public void CalculatingTheInverseOfAnotherMatrix()
        {
            // Given the following 4x4 matrix A:
            //    | 8 | -5 | 9 | 2 |
            //    | 7 | 5 | 6 | 1 |
            //    | -6 | 0 | 9 | 6 |
            //    | -3 | 0 | -9 | -4 |
            var a = CrtFactory.CoreFactory.Matrix(
                new double[] { 8, -5, 9, 2 },
                new double[] { 7, 5, 6, 1 },
                new double[] { -6, 0, 9, 6 },
                new double[] { -3, 0, -9, -4 }
            );
            // Then inverse(A) is the following 4x4 matrix:
            //    | -0.15385 | -0.15385 | -0.28205 | -0.53846 |
            //    | -0.07692 | 0.12308 | 0.02564 | 0.03077 |
            //    | 0.35897 | 0.35897 | 0.43590 | 0.92308 |
            //    | -0.69231 | -0.69231 | -0.76923 | -1.92308 |
            var expectedResult = CrtFactory.CoreFactory.Matrix(
                new double[] { -0.15385, -0.15385, -0.28205, -0.53846 },
                new double[] { -0.07692, 0.12308, 0.02564, 0.03077 },
                new double[] { 0.35897, 0.35897, 0.43590, 0.92308 },
                new double[] { -0.69231, -0.69231, -0.76923, -1.92308 }
            );
            Assert.IsTrue(a.Inverse() == expectedResult);
        }

        // Scenario: Calculating the inverse of a third matrix
        [Test]
        public void CalculatingTheInverseOfAThirdMatrix()
        {
            // Given the following 4x4 matrix A:
            //    | 9 | 3 | 0 | 9 |
            //    | -5 | -2 | -6 | -3 |
            //    | -4 | 9 | 6 | 4 |
            //    | -7 | 6 | 6 | 2 |
            var a = CrtFactory.CoreFactory.Matrix(
                new double[] { 9, 3, 0, 9 },
                new double[] { -5, -2, -6, -3 },
                new double[] { -4, 9, 6, 4 },
                new double[] { -7, 6, 6, 2 }
            );
            // Then inverse(A) is the following 4x4 matrix:
            //    | -0.04074 | -0.07778 | 0.14444 | -0.22222 |
            //    | -0.07778 | 0.03333 | 0.36667 | -0.33333 |
            //    | -0.02901 | -0.14630 | -0.10926 | 0.12963 |
            //    | 0.17778 | 0.06667 | -0.26667 | 0.33333 |
            var expectedResult = CrtFactory.CoreFactory.Matrix(
                new double[] { -0.04074, -0.07778, 0.14444, -0.22222 },
                new double[] { -0.07778, 0.03333, 0.36667, -0.33333 },
                new double[] { -0.02901, -0.14630, -0.10926, 0.12963 },
                new double[] { 0.17778, 0.06667, -0.26667, 0.33333 }
            );
            Assert.IsTrue(a.Inverse() == expectedResult);
        }

        // Scenario: Multiplying a product by its inverse
        [Test]
        public void MultiplyingAProductByItsInverse()
        {
            //Given the following 4x4 matrix A:
            //    | 3 | -9 | 7 | 3 |
            //    | 3 | -8 | 2 | -9 |
            //    | -4 | 4 | 4 | 1 |
            //    | -6 | 5 | -1 | 1 |
            var a = CrtFactory.CoreFactory.Matrix(
                new double[] { 3, -9, 7, 3 },
                new double[] { 3, -8, 2, -9 },
                new double[] { -4, 4, 4, 1 },
                new double[] { -6, 5, -1, 1 }
            );
            //And the following 4x4 matrix B:
            //    | 8 | 2 | 2 | 2 |
            //    | 3 | -1 | 7 | 0 |
            //    | 7 | 0 | 5 | 4 |
            //    | 6 | -2 | 0 | 5 |
            var b = CrtFactory.CoreFactory.Matrix(
                new double[] { 8, 2, 2, 2 },
                new double[] { 3, -1, 7, 0 },
                new double[] { 7, 0, 5, 4 },
                new double[] { 6, -2, 0, 5 }
            );
            // And C ← A* B
            var c = a * b;
            // Then C * inverse(B) = A
            Assert.IsTrue(c * b.Inverse() == a);
        }

        // Additional scenarios

        // Scenario: Multiplying a matrix by its inverse
        [Test]
        public void MultiplyingAMatrixByItsInverse()
        {
            //Given the following 4x4 matrix A:
            //    | 3 | -9 | 7 | 3 |
            //    | 3 | -8 | 2 | -9 |
            //    | -4 | 4 | 4 | 1 |
            //    | -6 | 5 | -1 | 1 |
            var a = CrtFactory.CoreFactory.Matrix(
                new double[] { 3, -9, 7, 3 },
                new double[] { 3, -8, 2, -9 },
                new double[] { -4, 4, 4, 1 },
                new double[] { -6, 5, -1, 1 }
            );
            //And B = inverse(A)
            var b = a.Inverse();
            // Then A * B = Identity Matrix
            Assert.IsTrue(a * b == CrtFactory.TransformationFactory.IdentityMatrix(4, 4));
        }

        // Scenario: Compare inverse of transpose and transpose of inverse
        [Test]
        public void CompareInverseOfTransposeAndTransposeOfInverse()
        {
            //Given the following 4x4 matrix A:
            //    | 3 | -9 | 7 | 3 |
            //    | 3 | -8 | 2 | -9 |
            //    | -4 | 4 | 4 | 1 |
            //    | -6 | 5 | -1 | 1 |
            var a = CrtFactory.CoreFactory.Matrix(
                new double[] { 3, -9, 7, 3 },
                new double[] { 3, -8, 2, -9 },
                new double[] { -4, 4, 4, 1 },
                new double[] { -6, 5, -1, 1 }
            );
            //And B <- inverse(transpose(A))
            var b = a.Transpose().Inverse();
            // And C <- transpose(inverse(A))
            var c = a.Inverse().Transpose();
            // Then A * B = Identity Matrix
            Assert.IsTrue(b == c);
        }

        // Scenario: Multiply Matrix by tuple to scale one component of the vector
        [Test]
        public void MultiplyMatrixByTupleToScaleOneComponentOfTheVector()
        {
            //Given the following 4x4 matrix A:
            //    | 1 | 0 | 0 | 0 |
            //    | 0 | 5 | 0 | 0 |
            //    | 0 | 0 | 1 | 0 |
            //    | 0 | 0 | 0 | 1 |
            var a = CrtFactory.TransformationFactory.IdentityMatrix(4,4);
            a[1, 1] = 5;
            //And B <- tuple(1,2,3,4)
            var b = CrtFactory.CoreFactory.Tuple(1,2,3,4);
            var c = a.Inverse().Transpose();
            // Then A * B = tuple(1, 10, 3, 4)
            Assert.IsTrue(a * b == CrtFactory.CoreFactory.Tuple(1, 10, 3, 4));
        }

    }
}
