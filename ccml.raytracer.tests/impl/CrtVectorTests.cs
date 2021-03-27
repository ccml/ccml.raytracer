using System;
using NUnit.Framework;

namespace ccml.raytracer.tests.impl
{
    public class CrtVectorTests
    {
        [SetUp]
        public void Setup()
        {
        }

        #region Magnitude of a vector

        // Scenario: Computing the magnitude of vector(0, 1, 0)
        [Test]
        public void ComputingTheMagnitude01Test()
        {
            // Given v ← vector(0, 1, 0)
            var v = CrtFactory.CoreFactory.Vector(0, 1, 0);
            // Then magnitude(v) = 1
            Assert.AreEqual(1.0, !v);
        }

        // Scenario: Computing the magnitude of vector(0, 0, 1)
        [Test]
        public void ComputingTheMagnitude02Test()
        {
            // Given v ← vector(0, 0, 1)
            var v = CrtFactory.CoreFactory.Vector(0, 0, 1);
            // Then magnitude(v) = 1
            Assert.AreEqual(1.0, !v);
        }

        // Scenario: Computing the magnitude of vector(1, 2, 3)
        [Test]
        public void ComputingTheMagnitude03Test()
        {
            // Given v ← vector(1, 2, 3)
            var v = CrtFactory.CoreFactory.Vector(1, 2, 3);
            // Then magnitude(v) = √14
            Assert.AreEqual(Math.Sqrt(14), !v);
        }

        // Scenario: Computing the magnitude of vector(-1, -2, -3)
        [Test]
        public void ComputingTheMagnitude04Test()
        {
            // Given v ← vector(-1, -2, -3)
            var v = CrtFactory.CoreFactory.Vector(-1, -2, -3);
            // Then magnitude(v) = √14
            Assert.AreEqual(Math.Sqrt(14), !v);
        }

        #endregion

        #region Normalizing vectors

        // Scenario: Normalizing vector(4, 0, 0) gives(1, 0, 0)
        [Test]
        public void NormalizingVector01Test()
        {
            // Given v ← vector(4, 0, 0)
            var v = CrtFactory.CoreFactory.Vector(4, 0, 0);
            // Then normalize(v) = vector(1, 0, 0)
            Assert.IsTrue(~v == CrtFactory.CoreFactory.Vector(1, 0, 0));
        }

        // Scenario: Normalizing vector(1, 2, 3)
        [Test]
        public void NormalizingVector02Test()
        {
            // Given v ← vector(1, 2, 3)
            var v = CrtFactory.CoreFactory.Vector(1, 2, 3);
            //                                 # vector(1/√14,   2/√14,   3/√14)
            // Then normalize(v) = approximately vector(0.26726, 0.53452, 0.80178)
            Assert.IsTrue(~v == CrtFactory.CoreFactory.Vector(1.0 / Math.Sqrt(14), 2.0 / Math.Sqrt(14), 3.0 / Math.Sqrt(14)));
        }

        // Scenario: The magnitude of a normalized vector
        [Test]
        public void NormalizingVector03Test()
        {
            // Given v ← vector(1, 2, 3)
            var v = CrtFactory.CoreFactory.Vector(1, 2, 3);
            // When norm ← normalize(v)
            var vn = ~v;
            // Then magnitude(norm) = 1
            Assert.AreEqual(1.0, !vn);
        }

        #endregion

        #region Dot product (also called scalar product or inner product)

        // Scenario: The dot product of two vectors
        [Test]
        public void DotProductOfTwoVectorsTest()
        {
            // Given a ← vector(1, 2, 3)
            var a = CrtFactory.CoreFactory.Vector(1, 2, 3);
            // And b ← vector(2, 3, 4)
            var b = CrtFactory.CoreFactory.Vector(2, 3, 4);
            // Then dot(a, b) = 20
            Assert.AreEqual(20, a * b);
        }


        #endregion

        #region Cross product

        //Scenario: The cross product of two vectors
        [Test]
        public void CrossProductOfTwoVectorsTest()
        {
            //Given a ← vector(1, 2, 3)
            var a = CrtFactory.CoreFactory.Vector(1, 2, 3);
            //And b ← vector(2, 3, 4)
            var b = CrtFactory.CoreFactory.Vector(2, 3, 4);
            //Then cross(a, b) = vector(-1, 2, -1)
            Assert.IsTrue((a ^ b) == CrtFactory.CoreFactory.Vector(-1, 2, -1));
            //And cross(b, a) = vector(1, -2, 1)
            Assert.IsTrue((b ^ a) == CrtFactory.CoreFactory.Vector(1, -2, 1));
        }

        #endregion

        #region Refecting a vector

        // Scenario: Reflecting a vector approaching at 45°
        [Test]
        public void ReflectingAVectorApproachingAt45Deg()
        {
            // Given v ← vector(1, -1, 0)
            var v = CrtFactory.CoreFactory.Vector(1, -1, 0);
            // And n ← vector(0, 1, 0)
            var n = CrtFactory.CoreFactory.Vector(0,1,0);
            // When r ← reflect(v, n)
            var r = v.ReflectBy(n);
            // Then r = vector(1, 1, 0)
            Assert.IsTrue(r == CrtFactory.CoreFactory.Vector(1,1,0));
        }

        // Scenario: Reflecting a vector off a slanted surface
        [Test]
        public void ReflectingAVectorOffASlantedSurface()
        {
            // Given v ← vector(0, -1, 0)
            var v = CrtFactory.CoreFactory.Vector(0, -1, 0);
            // And n ← vector(√2/2, √2/2, 0)
            var n = CrtFactory.CoreFactory.Vector(Math.Sqrt(2.0)/2, Math.Sqrt(2.0) / 2, 0);
            // When r ← reflect(v, n)
            var r = v.ReflectBy(n);
            // Then r = vector(1, 0, 0)
            Assert.IsTrue(r == CrtFactory.CoreFactory.Vector(1, 0, 0));
        }

        #endregion
    }
}
