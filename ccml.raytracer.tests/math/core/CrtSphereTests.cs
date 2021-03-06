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

        // Scenario: An intersection encapsulates t and object
        [Test]
        public void AnIntersectionEncapsulatesTAndObject()
        {
            // Given s ← sphere()
            var s = CrtFactory.Sphere();
            // When i ← intersection(3.5, s)
            var i = CrtFactory.Intersection(3.5, s);
            // Then i.t = 3.5
            Assert.IsTrue(CrtReal.AreEquals(i.T, 3.5));
            // And i.object = s
            Assert.AreSame(i.TheObject, s);
        }

        // Scenario: Intersect sets the object on the intersection
        [Test]
        public void IntersectSetsTheObjectOnTheIntersection()
        {
            // Given r ← ray(point(0, 0, -5), vector(0, 0, 1))
            var r = CrtFactory.Ray(CrtFactory.Point(0, 0, -5), CrtFactory.Vector(0, 0, 1));
            // And s ← sphere()
            var s = CrtFactory.Sphere();
            // When xs ← intersect(s, r)
            var xs = s.Intersect(r);
            // Then xs.count = 2
            Assert.AreEqual(2, xs.Count);
            // And xs[0].object = s
            Assert.AreSame(xs[0].TheObject, s);
            // And xs[1].object = s
            Assert.AreSame(xs[1].TheObject, s);
        }

    }
}
