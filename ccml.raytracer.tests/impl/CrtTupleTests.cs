using System;
using ccml.raytracer.Core;
using NUnit.Framework;

namespace ccml.raytracer.tests.impl
{
    public class CrtTupleTests
    {
        [SetUp]
        public void Setup()
        {
        }

        #region Equallity
        
        // Scenario: 2 tuples are equals if their components are equals
        [Test]
        public void TuplesAreEqualsTest()
        {
            // Given a ← tuple(4.3, -4.2, 3.1, 1.0)
            var a = CrtFactory.CoreFactory.Tuple(4.3, -4.2, 3.1, 1.0);
            // And b ← tuple(4.3, -4.2, 3.1, 1.0)
            var b = CrtFactory.CoreFactory.Tuple(4.3, -4.2, 3.1, 1.0);
            // Then a == b is true
            Assert.IsTrue(a == b);
            // And a != b is false
            Assert.IsFalse(a != b);
        }

        // Scenario: 2 tuples are different if a least one their components are different
        [Test]
        public void TuplesAreNotEqualsTest()
        {
            // Given a ← tuple(4.3, 2.4, 3.1, 1.0)
            var a = CrtFactory.CoreFactory.Tuple(4.3, 2.4, 3.1, 1.0);
            // And b ← tuple(4.3, -4.2, 3.1, 1.0)
            var b = CrtFactory.CoreFactory.Tuple(4.3, -4.2, 3.1, 1.0);
            // Then a != b is true
            Assert.IsTrue(a != b);
            // And a == b is false
            Assert.IsFalse(a == b);
        }

        #endregion

        #region Adding two tuples

        //Scenario: Adding two tuples
        [Test]
        public void AddingTwoTuplesTest()
        {
            // Given a1 ← tuple(3, -2, 5, 1)
            var a1 = CrtFactory.CoreFactory.Tuple(3, -2, 5, 1);
            // And a2 ← tuple(-2, 3, 1, 0)
            var a2 = CrtFactory.CoreFactory.Tuple(-2, 3, 1, 0);
            // Then a1 +a2 = tuple(1, 1, 6, 1)
            Assert.IsTrue((a1 + a2) == CrtFactory.CoreFactory.Tuple(1, 1, 6, 1));
        }

        //Scenario: Adding a point and a vector gives a point
        [Test]
        public void AddingAPointAndAVectorGivesAPointTest()
        {
            // Given a1 ← tuple(3, -2, 5, 1)
            var a1 = CrtFactory.CoreFactory.Tuple(3, -2, 5, 1);
            // And a2 ← tuple(-2, 3, 1, 0)
            var a2 = CrtFactory.CoreFactory.Tuple(-2, 3, 1, 0);
            // When b = a1 + a2
            var b = a1 + a2;
            // Then b = tuple(1, 1, 6, 1)
            Assert.IsTrue(b == CrtFactory.CoreFactory.Tuple(1, 1, 6, 1));
            // And b is a Point
            Assert.IsInstanceOf<CrtPoint>(b);

        }

        //Scenario: Adding 2 vectors gives a vector
        [Test]
        public void AddingTwoVectorsGivesAVectorTest()
        {
            //Given a1 ← tuple(3, -2, 5, 0)
            var a1 = CrtFactory.CoreFactory.Tuple(3, -2, 5, 0);
            //And a2 ← tuple(-2, 3, 1, 0)
            var a2 = CrtFactory.CoreFactory.Tuple(-2, 3, 1, 0);
            //And b = a1 + a2
            var b = a1 + a2;
            //Then b = tuple(1, 1, 6, 0)
            Assert.IsTrue(b == CrtFactory.CoreFactory.Tuple(1, 1, 6, 0));
            //And b is a Vector
            Assert.IsInstanceOf<CrtVector>(b);
        }

        //Scenario: Adding 2 points gives an ArgumentException
        [Test]
        public void AddingTwoPointsThrowAnExceptionTest()
        {
            //Given a1 ← tuple(3, -2, 5, 1)
            var a1 = CrtFactory.CoreFactory.Tuple(3, -2, 5, 1);
            //And a2 ← tuple(-2, 3, 1, 1)
            var a2 = CrtFactory.CoreFactory.Tuple(-2, 3, 1, 1);
            //Then a1 + a2 throw an ArgumentException("Can't add 2 points")
            Assert.Throws<ArgumentException>(() =>
            {
                var b = a1 + a2;
            }, "Can't add 2 points");
        }

        #endregion

        #region Subtracting two tuples

        // Scenario: Subtracting two points
        [Test]
        public void SubtractingTwoPointsTest()
        {
            // Given p1 ← point(3, 2, 1)
            var p1 = CrtFactory.CoreFactory.Point(3, 2, 1);
            // And p2 ← point(5, 6, 7)
            var p2 = CrtFactory.CoreFactory.Point(5, 6, 7);
            // Then p1 - p2 = vector(-2, -4, -6)
            Assert.IsTrue((p1 - p2) == CrtFactory.CoreFactory.Vector(-2, -4, -6));

        }

        // Scenario: Subtracting a vector from a point
        [Test]
        public void SubtractingAVectorFromAPointTest()
        {
            // Given p ← point(3, 2, 1)
            var p = CrtFactory.CoreFactory.Point(3, 2, 1);
            // And v ← vector(5, 6, 7)
            var v = CrtFactory.CoreFactory.Vector(5, 6, 7);
            //Then p - v = point(-2, -4, -6)
            Assert.IsTrue((p - v) == CrtFactory.CoreFactory.Point(-2, -4, -6));
        }

        //Scenario: Subtracting two vectors
        [Test]
        public void SubtractingTwoVectorsTest()
        {
            // Given v1 ← vector(3, 2, 1)
            var v1 = CrtFactory.CoreFactory.Vector(3, 2, 1);
            // And v2 ← vector(5, 6, 7)
            var v2 = CrtFactory.CoreFactory.Vector(5, 6, 7);
            // Then v1 - v2 = vector(-2, -4, -6)
            Assert.IsTrue((v1 - v2) == CrtFactory.CoreFactory.Vector(-2, -4, -6));
        }

        //  Scenario: Subtracting a vector from a point
        [Test]
        public void SubtractingAPontFromAVectorTest()
        {
            //Given v ← vector(3, 2, 1)
            var v = CrtFactory.CoreFactory.Vector(3, 2, 1);
            //And p ← point(5, 6, 7)
            var p = CrtFactory.CoreFactory.Point(5, 6, 7);
            //Then a1 - a2 throw an ArgumentException("Can't subtract a point from a vector")
            Assert.Throws<ArgumentException>(() =>
            {
                var b = v - p;
            }, "Can't subtract a point from a vector");
        }

        #endregion

        #region Negating tuples

        // Scenario: Negating a tuple
        [Test]
        public void NegatingTupleTest()
        {
            // Given a ← tuple(1, -2, 3, -4)
            var a = CrtFactory.CoreFactory.Tuple(1, -2, 3, -4);
            //Then -a = tuple(-1, 2, -3, 4)
            Assert.IsTrue(-a == CrtFactory.CoreFactory.Tuple(-1, 2, -3, 4));
        }

        #endregion


        #region Scalar multiplication

        // Scenario: Multiplying a tuple by a scalar
        [Test]
        public void MultiplyingATupleByAScalarTest()
        {
            // Given a ← tuple(1, -2, 3, -4)
            var a = CrtFactory.CoreFactory.Tuple(1, -2, 3, -4);
            // Then a * 3.5 = tuple(3.5, -7, 10.5, -14)
            Assert.IsTrue(a * 3.5 == CrtFactory.CoreFactory.Tuple(3.5, -7, 10.5, -14));
        }

        //Scenario: Multiplying a scalar by a tuple
        [Test]
        public void MultiplyingAScalarByATupleTest()
        {
            // Given a ← tuple(1, -2, 3, -4)
            var a = CrtFactory.CoreFactory.Tuple(1, -2, 3, -4);
            // Then 3.5 * a = tuple(3.5, -7, 10.5, -14)
            Assert.IsTrue(3.5 * a == CrtFactory.CoreFactory.Tuple(3.5, -7, 10.5, -14));
        }

        #endregion

        #region Divide tuple by scalar

        // Scenario: Dividing a tuple by a scalar
        [Test]
        public void DividingATupleByAScalarTest()
        {
            // Given a ← tuple(1, -2, 3, -4)
            var a = CrtFactory.CoreFactory.Tuple(1, -2, 3, -4);
            // Then a / 2 = tuple(0.5, -1, 1.5, -2)
            Assert.IsTrue(a / 2 == CrtFactory.CoreFactory.Tuple(0.5, -1, 1.5, -2));
        }

        #endregion


    }
}
