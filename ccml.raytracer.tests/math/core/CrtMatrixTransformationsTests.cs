using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ccml.raytracer.engine.core;
using NUnit.Framework;

namespace ccml.raytracer.tests.math.core
{
    public class CrtMatrixTransformationsTests
    {
        [SetUp]
        public void Setup()
        {

        }

        #region Translation

        // Scenario: Multiplying by a translation matrix
        [Test]
        public void MultiplyingByATranslationMatrix()
        {
            // Given transform ← translation(5, -3, 2)
            var transform = CrtFactory.TranslationMatrix(5, -3, 2);
            // And p ← point(-3, 4, 5)
            var p = CrtFactory.Point(-3, 4, 5);
            // Then transform * p = point(2, 1, 7)
            Assert.IsTrue(transform * p == CrtFactory.Point(2, 1, 7));
        }

        // Scenario: Multiplying by the inverse of a translation matrix
        [Test]
        public void MultiplyingByTheInverseOfATranslationMatrix()
        {
            // Given transform ← translation(5, -3, 2)
            var transform = CrtFactory.TranslationMatrix(5, -3, 2);
            // And inv ← inverse(transform)
            var inv = transform.Inverse();
            // And p ← point(-3, 4, 5)
            var p = CrtFactory.Point(-3, 4, 5);
            // Then inv * p = point(-8, 7, 3)
            Assert.IsTrue(inv * p == CrtFactory.Point(-8, 7, 3));
        }

        //Scenario: Translation does not affect vectors
        [Test]
        public void TranslationDoesNotAffectVectors()
        {
            // Given transform ← translation(5, -3, 2)
            var transform = CrtFactory.TranslationMatrix(5, -3, 2);
            // And v ← vector(-3, 4, 5)
            var v = CrtFactory.Vector(-3, 4, 5);
            // Then transform * v = v
            Assert.IsTrue(transform * v == v);
        }

        #endregion

        #region Scaling

        // Scenario: A scaling matrix applied to a point
        [Test]
        public void AScalingMatrixAppliedToAPoint()
        {
            // Given transform ← scaling(2, 3, 4)
            var transform = CrtFactory.ScalingMatrix(2, 3, 4);
            // And p ← point(-4, 6, 8)
            var p = CrtFactory.Point(-4, 6, 8);
            // Then transform * p = point(-8, 18, 32)
            Assert.IsTrue(transform * p == CrtFactory.Point(-8, 18, 32));
        }

        // Scenario: A scaling matrix applied to a vector
        [Test]
        public void AScalingMatrixAppliedToAVector()
        {
            // Given transform ← scaling(2, 3, 4)
            var transform = CrtFactory.ScalingMatrix(2, 3, 4);
            // And v ← vector(-4, 6, 8)
            var v = CrtFactory.Vector(-4, 6, 8);
            // Then transform * v = vector(-8, 18, 32)
            Assert.IsTrue(transform * v == CrtFactory.Vector(-8, 18, 32));
        }

        // Scenario: Multiplying by the inverse of a scaling matrix
        [Test]
        public void MultiplyingByTheInverseOfAScalingMatrixAppliedToAVector()
        {
            // Given transform ← scaling(2, 3, 4)
            var transform = CrtFactory.ScalingMatrix(2, 3, 4);
            // And inv ← inverse(transform)
            var inv = transform.Inverse();
            // And v ← vector(-4, 6, 8)
            var v = CrtFactory.Vector(-4, 6, 8);
            // Then inv * v = vector(-2, 2, 2)
            Assert.IsTrue(inv * v == CrtFactory.Vector(-2, 2, 2));
        }

        // Scenario: Reflection is scaling by a negative value
        [Test]
        public void ReflectionIsScalingByANegativeValue()
        {
            // Given transform ← scaling(-1, 1, 1)
            var transform = CrtFactory.ScalingMatrix(-1, 1, 1);
            // And p ← point(2, 3, 4)
            var p = CrtFactory.Point(2, 3, 4);
            // Then transform * p = point(-2, 3, 4)
            Assert.IsTrue(transform * p == CrtFactory.Point(-2, 3, 4));
        }

        #endregion

        #region Rotation

        //Scenario: Rotating a point around the x axis
        [Test]
        public void RotatingAPointAroundTheXAxis()
        {
            // Given p ← point(0, 1, 0)
            var p = CrtFactory.Point(0, 1, 0);
            // And half_quarter ← rotation_x(π / 4)
            var halfQuarter = CrtFactory.XRotationMatrix(Math.PI / 4);
            // And full_quarter ← rotation_x(π / 2)
            var fullQuarter = CrtFactory.XRotationMatrix(Math.PI / 2);
            // Then half_quarter * p = point(0, √2/2, √2/2)
            Assert.IsTrue(halfQuarter * p == CrtFactory.Point(0, Math.Sqrt(2.0)/2.0, Math.Sqrt(2.0)/2.0));
            // And full_quarter * p = point(0, 0, 1)
            Assert.IsTrue(fullQuarter * p == CrtFactory.Point(0, 0, 1));
        }

        // Scenario: Rotating a point around the y axis
        [Test]
        public void RotatingAPointAroundTheYAxis()
        {
            // Given p ← point(0, 0, 1)
            var p = CrtFactory.Point(0, 0, 1);
            // And half_quarter ← rotation_y(π / 4)
            var halfQuarter = CrtFactory.YRotationMatrix(Math.PI / 4);
            // And full_quarter ← rotation_y(π / 2)
            var fullQuarter = CrtFactory.YRotationMatrix(Math.PI / 2);
            // Then half_quarter * p = point(√2/2, 0, √2/2)
            Assert.IsTrue(halfQuarter * p == CrtFactory.Point(Math.Sqrt(2.0) / 2.0, 0, Math.Sqrt(2.0) / 2.0));
            // And full_quarter * p = point(1, 0, 0)
            Assert.IsTrue(fullQuarter * p == CrtFactory.Point(1, 0, 0));
        }

        //Scenario: Rotating a point around the z axis
        [Test]
        public void RotatingAPointAroundTheZAxis()
        {
            // Given p ← point(0, 1, 0)
            var p = CrtFactory.Point(0, 1, 0);
            // And half_quarter ← rotation_y(π / 4)
            var halfQuarter = CrtFactory.ZRotationMatrix(Math.PI / 4);
            // And full_quarter ← rotation_y(π / 2)
            var fullQuarter = CrtFactory.ZRotationMatrix(Math.PI / 2);
            // Then half_quarter * p = point(-√2/2, √2/2, 0)
            Assert.IsTrue(halfQuarter * p == CrtFactory.Point(-Math.Sqrt(2.0) / 2.0, Math.Sqrt(2.0) / 2.0, 0));
            // And full_quarter * p = point(-1, 0, 0)
            Assert.IsTrue(fullQuarter * p == CrtFactory.Point(-1, 0, 0));
        }

        #endregion

        #region Shearing

        //Scenario: A shearing transformation moves x in proportion to y
        [Test]
        public void AShearingTransformationMovesXInProportionToY()
        {
            // Given transform ← shearing(1, 0, 0, 0, 0, 0)
            var transform = CrtFactory.ShearingMatrix(1, 0, 0, 0, 0, 0);
            // And p ← point(2, 3, 4)
            var p = CrtFactory.Point(2, 3, 4);
            // Then transform * p = point(5, 3, 4)
            Assert.IsTrue(transform * p == CrtFactory.Point(5, 3, 4));
        }

        // Scenario: A shearing transformation moves x in proportion to z
        [Test]
        public void AShearingTransformationMovesXInProportionToZ()
        {
            // Given transform ← shearing(0, 1, 0, 0, 0, 0)
            var transform = CrtFactory.ShearingMatrix(0, 1, 0, 0, 0, 0);
            // And p ← point(2, 3, 4)
            var p = CrtFactory.Point(2, 3, 4);
            // Then transform * p = point(6, 3, 4)
            Assert.IsTrue(transform * p == CrtFactory.Point(6, 3, 4));
        }

        // Scenario: A shearing transformation moves y in proportion to x
        [Test]
        public void AShearingTransformationMovesYInProportionToX()
        {
            // Given transform ← shearing(0, 0, 1, 0, 0, 0)
            var transform = CrtFactory.ShearingMatrix(0, 0, 1, 0, 0, 0);
            // And p ← point(2, 3, 4)
            var p = CrtFactory.Point(2, 3, 4);
            // Then transform * p = point(2, 5, 4)
            Assert.IsTrue(transform * p == CrtFactory.Point(2, 5, 4));
        }

        // Scenario: A shearing transformation moves y in proportion to z
        [Test]
        public void AShearingTransformationMovesYInProportionToZ()
        {
            // Given transform ← shearing(0, 0, 0, 1, 0, 0)
            var transform = CrtFactory.ShearingMatrix(0, 0, 0, 1, 0, 0);
            // And p ← point(2, 3, 4)
            var p = CrtFactory.Point(2, 3, 4);
            // Then transform * p = point(2, 7, 4)
            Assert.IsTrue(transform * p == CrtFactory.Point(2, 7, 4));
        }

        // Scenario: A shearing transformation moves z in proportion to x
        [Test]
        public void AShearingTransformationMovesZInProportionToX()
        {
            // Given transform ← shearing(0, 0, 0, 0, 1, 0)
            var transform = CrtFactory.ShearingMatrix(0, 0, 0, 0, 1, 0);
            // And p ← point(2, 3, 4)
            var p = CrtFactory.Point(2, 3, 4);
            // Then transform * p = point(2, 3, 6)
            Assert.IsTrue(transform * p == CrtFactory.Point(2, 3, 6));
        }

        // Scenario: A shearing transformation moves z in proportion to y
        [Test]
        public void AShearingTransformationMovesZInProportionToY()
        {
            // Given transform ← shearing(0, 0, 0, 0, 0, 1)
            var transform = CrtFactory.ShearingMatrix(0, 0, 0, 0, 0, 1);
            // And p ← point(2, 3, 4)
            var p = CrtFactory.Point(2, 3, 4);
            // Then transform * p = point(2, 3, 7)
            Assert.IsTrue(transform * p == CrtFactory.Point(2, 3, 7));
        }

        #endregion

        #region Chaining transformation

        // Scenario: Individual transformations are applied in sequence
        [Test]
        public void IndividualTransformationsAreAppliedInSequence()
        {
            // Given p ← point(1, 0, 1)
            var p = CrtFactory.Point(1, 0, 1);
            // And A ← rotation_x(π / 2)
            var a = CrtFactory.XRotationMatrix(Math.PI / 2);
            // And B ← scaling(5, 5, 5)
            var b = CrtFactory.ScalingMatrix(5, 5, 5);
            // And C ← translation(10, 5, 7)
            var c = CrtFactory.TranslationMatrix(10, 5, 7);
            // apply rotation first
            // When p2 ← A* p
            var p2 = a * p;
            // Then p2 = point(1, -1, 0)
            Assert.IsTrue(p2 == CrtFactory.Point(1, -1, 0));
            //# then apply scaling
            // When p3 ← B* p2
            var p3 = b * p2;
            // Then p3 = point(5, -5, 0)
            Assert.IsTrue(p3 == CrtFactory.Point(5, -5, 0));
            //# then apply translation
            // When p4 ← C* p3
            var p4 = c * p3;
            // Then p4 = point(15, 0, 7)
            Assert.IsTrue(p4 == CrtFactory.Point(15, 0, 7));
        }

        // Scenario: Chained transformations must be applied in reverse order
        [Test]
        public void ChainedTransformationsMustBeAppliedInReverseOrder()
        {
            // Given p ← point(1, 0, 1)
            var p = CrtFactory.Point(1, 0, 1);
            // And A ← rotation_x(π / 2)
            var a = CrtFactory.XRotationMatrix(Math.PI / 2);
            // And B ← scaling(5, 5, 5)
            var b = CrtFactory.ScalingMatrix(5, 5, 5);
            // And C ← translation(10, 5, 7)
            var c = CrtFactory.TranslationMatrix(10, 5, 7);
            // When T ← C* B * A
            var t = c * b * a;
            // Then T* p = point(15, 0, 7)
            Assert.IsTrue(t * p == CrtFactory.Point(15, 0, 7));
        }

        #endregion

    }
}
