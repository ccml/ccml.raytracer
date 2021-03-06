using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ccml.raytracer.engine.core;
using NUnit.Framework;

namespace ccml.raytracer.tests.math.core
{
    public class CrtSphereTests
    {
        [SetUp]
        public void Setup()
        {

        }

        #region Sphere intersection

        //Scenario: A ray intersects a sphere at two points
        [Test]
        public void ARayIntersectsASphereAtTwoPoints()
        {
            // Given r ← ray(point(0, 0, -5), vector(0, 0, 1))
            var r = CrtFactory.Ray(CrtFactory.Point(0, 0, -5), CrtFactory.Vector(0,0,1));
            // And s ← sphere()
            var s = CrtFactory.Sphere();
            // When xs ← intersect(s, r)
            var xs = s.Intersect(r);
            // Then xs.count = 2
            Assert.AreEqual(2, xs.Count);
            // And xs[0] = 4.0
            Assert.IsTrue(CrtReal.AreEquals(xs[0].T, 4.0));
            // And xs[1] = 6.0
            Assert.IsTrue(CrtReal.AreEquals(xs[1].T, 6.0));
        }

        // Scenario: A ray intersects a sphere at a tangent
        [Test]
        public void ARayIntersectsASphereAtATangent()
        {
            // Given r ← ray(point(0, 1, -5), vector(0, 0, 1))
            var r = CrtFactory.Ray(CrtFactory.Point(0, 1, -5), CrtFactory.Vector(0, 0, 1));
            // And s ← sphere()
            var s = CrtFactory.Sphere();
            // When xs ← intersect(s, r)
            var xs = s.Intersect(r);
            // Then xs.count = 2
            Assert.AreEqual(2, xs.Count);
            // And xs[0] = 5.0
            Assert.IsTrue(CrtReal.AreEquals(xs[0].T, 5.0));
            // And xs[1] = 5.0
            Assert.IsTrue(CrtReal.AreEquals(xs[1].T, 5.0));
        }

        // Scenario: A ray originates inside a sphere
        [Test]
        public void ARayOriginatesInsideASphere()
        {
            // Given r ← ray(point(0, 0, 0), vector(0, 0, 1))
            var r = CrtFactory.Ray(CrtFactory.Point(0, 0, 0), CrtFactory.Vector(0, 0, 1));
            // And s ← sphere()
            var s = CrtFactory.Sphere();
            // When xs ← intersect(s, r)
            var xs = s.Intersect(r);
            // Then xs.count = 2
            Assert.AreEqual(2, xs.Count);
            // And xs[0] = -1.0
            Assert.IsTrue(CrtReal.AreEquals(xs[0].T, -1.0));
            // And xs[1] = 1.0
            Assert.IsTrue(CrtReal.AreEquals(xs[1].T, 1.0));
        }

        // Scenario: A sphere is behind a ray
        [Test]
        public void ASphereIsBehindARay()
        {
            // Given r ← ray(point(0, 0, 5), vector(0, 0, 1))
            var r = CrtFactory.Ray(CrtFactory.Point(0, 0, 5), CrtFactory.Vector(0, 0, 1));
            // And s ← sphere()
            var s = CrtFactory.Sphere();
            // When xs ← intersect(s, r)
            var xs = s.Intersect(r);
            // Then xs.count = 2
            Assert.AreEqual(2, xs.Count);
            // And xs[0] = -6.0
            Assert.IsTrue(CrtReal.AreEquals(xs[0].T, -6.0));
            // And xs[1] = -4.0
            Assert.IsTrue(CrtReal.AreEquals(xs[1].T, -4.0));
        }

        #endregion

        // Scenario: A sphere's default transformation
        [Test]
        public void ASphereDefaultTransformation()
        {
            // Given s ← sphere()
            var s = CrtFactory.Sphere();
            // Then s.transform = identity_matrix
            Assert.IsTrue(s.TransformMatrix == CrtFactory.IdentityMatrix(4,4));
        }

        // Scenario: Changing a sphere's transformation
        [Test]
        public void ChangingASphereTransformation()
        {
            // Given s ← sphere()
            var s = CrtFactory.Sphere();
            // And t ← translation(2, 3, 4)
            var t = CrtFactory.TranslationMatrix(2, 3, 4);
            // When set_transform(s, t)
            s.SetTransformMatrix(t);
            // Then s.transform = t
            Assert.AreSame(s.TransformMatrix, t);
        }

        #region Sphere normals

        // Scenario: The normal on a sphere at a point on the x axis
        [Test]
        public void TheNormalOnASphereAtAPointOnTheXAxis()
        {
            // Given s ← sphere()
            var s = CrtFactory.Sphere();
            // When n ← normal_at(s, point(1, 0, 0))
            var n = s.NormalAt(CrtFactory.Point(1, 0, 0));
            // Then n = vector(1, 0, 0)
            Assert.IsTrue(n == CrtFactory.Vector(1,0,0));
        }

        // Scenario: The normal on a sphere at a point on the y axis
        [Test]
        public void TheNormalOnASphereAtAPointOnTheYAxis()
        {
            // Given s ← sphere()
            var s = CrtFactory.Sphere();
            // When n ← normal_at(s, point(0, 1, 0))
            var n = s.NormalAt(CrtFactory.Point(0, 1, 0));
            // Then n = vector(0, 1, 0)
            Assert.IsTrue(n == CrtFactory.Vector(0, 1, 0));
        }

        // Scenario: The normal on a sphere at a point on the z axis
        [Test]
        public void TheNormalOnASphereAtAPointOnTheZAxis()
        {
            // Given s ← sphere()
            var s = CrtFactory.Sphere();
            // When n ← normal_at(s, point(0, 0, 1))
            var n = s.NormalAt(CrtFactory.Point(0, 0, 1));
            // Then n = vector(0, 0, 1)
            Assert.IsTrue(n == CrtFactory.Vector(0, 0, 1));
        }

        // Scenario: The normal on a sphere at a nonaxial point
        [Test]
        public void TheNormalOnASphereAtANonAxialPPoint()
        {
            // Given s ← sphere()
            var s = CrtFactory.Sphere();
            // When n ← normal_at(s, point(√3/3, √3/3, √3/3))
            var n = s.NormalAt(CrtFactory.Point(Math.Sqrt(3)/3, Math.Sqrt(3) / 3, Math.Sqrt(3) / 3));
            // Then n = vector(√3 / 3, √3 / 3, √3 / 3)
            Assert.IsTrue(n == CrtFactory.Vector(Math.Sqrt(3) / 3, Math.Sqrt(3) / 3, Math.Sqrt(3) / 3));
        }

        // Scenario: The normal is a normalized vector
        [Test]
        public void TheNormalIsANormalizedVector()
        {
            // Given s ← sphere()
            var s = CrtFactory.Sphere();
            // When n ← normal_at(s, point(√3/3, √3/3, √3/3))
            var n = s.NormalAt(CrtFactory.Point(Math.Sqrt(3) / 3, Math.Sqrt(3) / 3, Math.Sqrt(3) / 3));
            // Then n = normalize(n)
            Assert.IsTrue(n == ~n);
        }

        // Scenario: Computing the normal on a translated sphere
        [Test]
        public void ComputingTheNormalOnATranslatedSphere()
        {
            // Given s ← sphere()
            var s = CrtFactory.Sphere();
            // And set_transform(s, translation(0, 1, 0))
            s.SetTransformMatrix(CrtFactory.TranslationMatrix(0,1,0));
            // When n ← normal_at(s, point(0, 1.70711, -0.70711))
            var n = s.NormalAt(CrtFactory.Point(0, 1.70711, -0.70711));
            // Then n = vector(0, 0.70711, -0.70711)
            Assert.IsTrue(n == CrtFactory.Vector(0, 0.70711, -0.70711));
        }

        // Scenario: Computing the normal on a transformed sphere
        [Test]
        public void ComputingTheNormalOnATransformedSphere()
        {
            // Given s ← sphere()
            var s = CrtFactory.Sphere();
            // And m ← scaling(1, 0.5, 1) * rotation_z(π/5)
            var m = CrtFactory.ScalingMatrix(1, 0.5, 1) * CrtFactory.ZRotationMatrix(Math.PI / 5);
            // And set_transform(s, m)
            s.SetTransformMatrix(m);
            // When n ← normal_at(s, point(0, √2/2, -√2/2))
            var n = s.NormalAt(CrtFactory.Point(0, Math.Sqrt(2.0)/2, -Math.Sqrt(2.0)/2));
            // Then n = vector(0, 0.97014, -0.24254)
            Assert.IsTrue(n == CrtFactory.Vector(0, 0.97014, -0.24254));
        }


        #endregion
    }
}
