using System;
using ccml.raytracer.Tests;
using NUnit.Framework;

namespace ccml.raytracer.tests.impl
{
    class CrtShapeTests
    {
        [SetUp]
        public void Setup()
        {
        }

        // Scenario: The default transformation
        [Test]
        public void TheDefaultTransformation()
        {
            // Given s ← test_shape()
            var s = CrtFactory.TestsFactory.TestShape();
            // Then s.transform = identity_matrix
            Assert.IsTrue(s.TransformMatrix == CrtFactory.TransformationFactory.IdentityMatrix(4, 4));
        }

        // Scenario: Assigning a transformation
        [Test]
        public void AssigningATransformation()
        {
            // Given s ← test_shape()
            var s = CrtFactory.TestsFactory.TestShape();
            // When set_transform(s, translation(2, 3, 4))
            s.TransformMatrix = CrtFactory.TransformationFactory.TranslationMatrix(2, 3, 4);
            // Then s.transform = t
            Assert.IsTrue(s.TransformMatrix == CrtFactory.TransformationFactory.TranslationMatrix(2, 3, 4));
        }

        // Scenario: Assigning a material
        [Test]
        public void ASphereMayBeAssignedAMaterial()
        {
            // Given s ← test_shape()
            var s = CrtFactory.TestsFactory.TestShape();
            // And m ← material()
            var m = CrtFactory.MaterialFactory.DefaultMaterial;
            // And m.ambient ← 1
            m.Ambient = 1;
            // When s.material ← m
            s.Material = m;
            // Then s.material = m
            Assert.IsTrue(s.Material == m);
        }

        // Scenario: Intersecting a scaled shape with a ray
        [Test]
        public void IntersectingAScaledShapeWithARay()
        {
            // Given r ← ray(point(0, 0, -5), vector(0, 0, 1))
            var r = CrtFactory.EngineFactory.Ray(CrtFactory.CoreFactory.Point(0, 0, -5), CrtFactory.CoreFactory.Vector(0, 0, 1));
            // And s ← test_shape()
            var s = CrtFactory.TestsFactory.TestShape();
            // When set_transform(s, scaling(2, 2, 2))
            s.TransformMatrix = CrtFactory.TransformationFactory.ScalingMatrix(2, 2, 2);
            // And xs ← intersect(s, r)
            var xs = s.Intersect(r);
            // Then s.saved_ray.origin = point(0, 0, -2.5)
            Assert.IsTrue(((CrtTestShape)s).SavedRay.Origin == CrtFactory.CoreFactory.Point(0, 0, -2.5));
            // And s.saved_ray.direction = vector(0, 0, 0.5)
            Assert.IsTrue(((CrtTestShape)s).SavedRay.Direction == CrtFactory.CoreFactory.Vector(0, 0, 0.5));
        }

        // Scenario: Intersecting a translated shape with a ray
        [Test]
        public void IntersectingATranslatedShapeWithARay()
        {
            // Given r ← ray(point(0, 0, -5), vector(0, 0, 1))
            var r = CrtFactory.EngineFactory.Ray(CrtFactory.CoreFactory.Point(0, 0, -5), CrtFactory.CoreFactory.Vector(0, 0, 1));
            // And s ← test_shape()
            var s = CrtFactory.TestsFactory.TestShape();
            // When set_transform(s, translation(5, 0, 0))
            s.TransformMatrix = CrtFactory.TransformationFactory.TranslationMatrix(5, 0, 0);
            // And xs ← intersect(s, r)
            var xs = s.Intersect(r);
            // Then s.saved_ray.origin = point(-5, 0, -5)
            Assert.IsTrue(((CrtTestShape)s).SavedRay.Origin == CrtFactory.CoreFactory.Point(-5, 0, -5));
            // And s.saved_ray.direction = vector(0, 0, 1)
            Assert.IsTrue(((CrtTestShape)s).SavedRay.Direction == CrtFactory.CoreFactory.Vector(0, 0, 1));
        }

        // Scenario: Computing the normal on a translated shape
        [Test]
        public void ComputingTheNormalOnATranslatedShape()
        {
            // Given s ← test_shape()
            var s = CrtFactory.TestsFactory.TestShape();
            // When set_transform(s, translation(0, 1, 0))
            s.TransformMatrix = CrtFactory.TransformationFactory.TranslationMatrix(0, 1, 0);
            // And n ← normal_at(s, point(0, 1.70711, -0.70711))
            var n = s.NormalAt(CrtFactory.CoreFactory.Point(0, 1.70711, -0.70711));
            // Then n = vector(0, 0.70711, -0.70711)
            Assert.IsTrue(n == CrtFactory.CoreFactory.Vector(0, 0.70711, -0.70711));
        }

        // Scenario: Computing the normal on a transformed shape
        [Test]
        public void ComputingTheNormalOnATransformedShape()
        {
            // Given s ← test_shape()
            var s = CrtFactory.TestsFactory.TestShape();
            // And m ← scaling(1, 0.5, 1) * rotation_z(π/5)
            var m = CrtFactory.TransformationFactory.ScalingMatrix(1, 0.5, 1) * CrtFactory.TransformationFactory.ZRotationMatrix(Math.PI / 5);
            // When set_transform(s, m)
            s.TransformMatrix = m;
            // And n ← normal_at(s, point(0, √2/2, -√2/2))
            var n = s.NormalAt(CrtFactory.CoreFactory.Point(0, Math.Sqrt(2.0) / 2, -Math.Sqrt(2.0) / 2));
            // Then n = vector(0, 0.97014, -0.24254)
            Assert.IsTrue(n == CrtFactory.CoreFactory.Vector(0, 0.97014, -0.24254));
        }

    }
}
