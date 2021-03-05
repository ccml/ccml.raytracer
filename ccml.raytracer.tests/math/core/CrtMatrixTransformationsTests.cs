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
            var halfQuarter = CrtFactory.XRotation(Math.PI / 4);
            // And full_quarter ← rotation_x(π / 2)
            var fullQuarter = CrtFactory.XRotation(Math.PI / 2);
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
            var halfQuarter = CrtFactory.YRotation(Math.PI / 4);
            // And full_quarter ← rotation_y(π / 2)
            var fullQuarter = CrtFactory.YRotation(Math.PI / 2);
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
            var halfQuarter = CrtFactory.ZRotation(Math.PI / 4);
            // And full_quarter ← rotation_y(π / 2)
            var fullQuarter = CrtFactory.ZRotation(Math.PI / 2);
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
            var transform = CrtFactory.Shearing(1, 0, 0, 0, 0, 0);
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
            var transform = CrtFactory.Shearing(0, 1, 0, 0, 0, 0);
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
            var transform = CrtFactory.Shearing(0, 0, 1, 0, 0, 0);
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
            var transform = CrtFactory.Shearing(0, 0, 0, 1, 0, 0);
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
            var transform = CrtFactory.Shearing(0, 0, 0, 0, 1, 0);
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
            var transform = CrtFactory.Shearing(0, 0, 0, 0, 0, 1);
            // And p ← point(2, 3, 4)
            var p = CrtFactory.Point(2, 3, 4);
            // Then transform * p = point(2, 3, 7)
            Assert.IsTrue(transform * p == CrtFactory.Point(2, 3, 7));
        }

        #endregion

    }
}
