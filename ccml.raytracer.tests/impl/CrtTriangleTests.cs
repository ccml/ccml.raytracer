using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ccml.raytracer.Core;
using NUnit.Framework;

namespace ccml.raytracer.tests.impl
{
    public class CrtTriangleTests
    {
        [SetUp]
        public void Setup()
        {

        }

        // Scenario: Constructing a triangle
        [Test]
        public void ConstructingATriangle()
        {
            // Given p1 ← point(0, 1, 0)
            var p1 = CrtFactory.CoreFactory.Point(0, 1, 0);
            // And p2 ← point(-1, 0, 0)
            var p2 = CrtFactory.CoreFactory.Point(-1, 0, 0);
            // And p3 ← point(1, 0, 0)
            var p3 = CrtFactory.CoreFactory.Point(1, 0, 0);
            // And t ← triangle(p1, p2, p3)
            var t = CrtFactory.ShapeFactory.Triangle(p1, p2, p3);
            // Then t.p1 = p1
            Assert.IsTrue(t.P1 == p1);
            // And t.p2 = p2
            Assert.IsTrue(t.P2 == p2);
            // And t.p3 = p3
            Assert.IsTrue(t.P3 == p3);
            // And t.e1 = vector(-1, -1, 0)
            Assert.IsTrue(t.E1 == CrtFactory.CoreFactory.Vector(-1, -1, 0));
            // And t.e2 = vector(1, -1, 0)
            Assert.IsTrue(t.E2 == CrtFactory.CoreFactory.Vector(1, -1, 0));
            // And t.normal = vector(0, 0, -1)
            Assert.IsTrue(t.Normal == CrtFactory.CoreFactory.Vector(0, 0, -1));
        }

        // Scenario: Finding the normal on a triangle
        [Test]
        public void FindingTheNormalOnATriangle()
        {
            // Given t ← triangle(point(0, 1, 0), point(-1, 0, 0), point(1, 0, 0))
            var p1 = CrtFactory.CoreFactory.Point(0, 1, 0);
            var p2 = CrtFactory.CoreFactory.Point(-1, 0, 0);
            var p3 = CrtFactory.CoreFactory.Point(1, 0, 0);
            var t = CrtFactory.ShapeFactory.Triangle(p1, p2, p3);
            // When n1 ← local_normal_at(t, point(0, 0.5, 0))
            var n1 = t.LocalNormalAt(CrtFactory.CoreFactory.Point(0, 0.5, 0));
            // And n2 ← local_normal_at(t, point(-0.5, 0.75, 0))
            var n2 = t.LocalNormalAt(CrtFactory.CoreFactory.Point(-0.5, 0.75, 0));
            // And n3 ← local_normal_at(t, point(0.5, 0.25, 0))
            var n3 = t.LocalNormalAt(CrtFactory.CoreFactory.Point(0.5, 0.25, 0));
            // Then n1 = t.normal
            Assert.IsTrue(n1 == t.Normal);
            // And n2 = t.normal
            Assert.IsTrue(n2 == t.Normal);
            // And n3 = t.normal
            Assert.IsTrue(n3 == t.Normal);
        }

        // Scenario: Intersecting a ray parallel to the triangle
        [Test]
        public void IntersectingARayParallelToTheTriangle()
        {
            // Given t ← triangle(point(0, 1, 0), point(-1, 0, 0), point(1, 0, 0))
            var p1 = CrtFactory.CoreFactory.Point(0, 1, 0);
            var p2 = CrtFactory.CoreFactory.Point(-1, 0, 0);
            var p3 = CrtFactory.CoreFactory.Point(1, 0, 0);
            var t = CrtFactory.ShapeFactory.Triangle(p1, p2, p3);
            // And r ← ray(point(0, -1, -2), vector(0, 1, 0))
            var r = CrtFactory.EngineFactory.Ray(
                CrtFactory.CoreFactory.Point(0, -1, -2),
                CrtFactory.CoreFactory.Vector(0, 1, 0)
            );
            // When xs ← local_intersect(t, r)
            var xs = t.LocalIntersect(r);
            // Then xs is empty
            Assert.IsEmpty(xs);
        }

        // Scenario: A ray misses the p1-p3 edge
        [Test]
        public void ARayMissesTheP1P3Edge()
        {
            // Given t ← triangle(point(0, 1, 0), point(-1, 0, 0), point(1, 0, 0))
            var p1 = CrtFactory.CoreFactory.Point(0, 1, 0);
            var p2 = CrtFactory.CoreFactory.Point(-1, 0, 0);
            var p3 = CrtFactory.CoreFactory.Point(1, 0, 0);
            var t = CrtFactory.ShapeFactory.Triangle(p1, p2, p3);
            // And r ← ray(point(1, 1, -2), vector(0, 0, 1))
            var r = CrtFactory.EngineFactory.Ray(
                CrtFactory.CoreFactory.Point(1, 1, -2),
                CrtFactory.CoreFactory.Vector(0, 0, 1)
            );
            // When xs ← local_intersect(t, r)
            var xs = t.LocalIntersect(r);
            // Then xs is empty
            Assert.IsEmpty(xs);
        }

        // Scenario: A ray misses the p1-p2 edge
        [Test]
        public void ARayMissesTheP1P2Edge()
        {
            // Given t ← triangle(point(0, 1, 0), point(-1, 0, 0), point(1, 0, 0))
            var p1 = CrtFactory.CoreFactory.Point(0, 1, 0);
            var p2 = CrtFactory.CoreFactory.Point(-1, 0, 0);
            var p3 = CrtFactory.CoreFactory.Point(1, 0, 0);
            var t = CrtFactory.ShapeFactory.Triangle(p1, p2, p3);
            // And r ← ray(point(-1, 1, -2), vector(0, 0, 1))
            var r = CrtFactory.EngineFactory.Ray(
                CrtFactory.CoreFactory.Point(-1, 1, -2),
                CrtFactory.CoreFactory.Vector(0, 0, 1)
            );
            // When xs ← local_intersect(t, r)
            var xs = t.LocalIntersect(r);
            // Then xs is empty
            Assert.IsEmpty(xs);
        }

        // Scenario: A ray misses the p2-p3 edge
        [Test]
        public void ARayMissesTheP2P3Edge()
        {
            // Given t ← triangle(point(0, 1, 0), point(-1, 0, 0), point(1, 0, 0))
            var p1 = CrtFactory.CoreFactory.Point(0, 1, 0);
            var p2 = CrtFactory.CoreFactory.Point(-1, 0, 0);
            var p3 = CrtFactory.CoreFactory.Point(1, 0, 0);
            var t = CrtFactory.ShapeFactory.Triangle(p1, p2, p3);
            // And r ← ray(point(0, -1, -2), vector(0, 0, 1))
            var r = CrtFactory.EngineFactory.Ray(
                CrtFactory.CoreFactory.Point(0, -1, -2),
                CrtFactory.CoreFactory.Vector(0, 0, 1)
            );
            // When xs ← local_intersect(t, r)
            var xs = t.LocalIntersect(r);
            // Then xs is empty
            Assert.IsEmpty(xs);
        }

        // Scenario: A ray strikes a triangle
        [Test]
        public void ARayStrikesATriangle()
        {
            // Given t ← triangle(point(0, 1, 0), point(-1, 0, 0), point(1, 0, 0))
            var p1 = CrtFactory.CoreFactory.Point(0, 1, 0);
            var p2 = CrtFactory.CoreFactory.Point(-1, 0, 0);
            var p3 = CrtFactory.CoreFactory.Point(1, 0, 0);
            var t = CrtFactory.ShapeFactory.Triangle(p1, p2, p3);
            // And r ← ray(point(0, 0.5, -2), vector(0, 0, 1))
            var r = CrtFactory.EngineFactory.Ray(
                CrtFactory.CoreFactory.Point(0, 0.5, -2),
                CrtFactory.CoreFactory.Vector(0, 0, 1)
            );
            // When xs ← local_intersect(t, r)
            var xs = t.LocalIntersect(r);
            // Then xs.count = 1
            Assert.AreEqual(1, xs.Count);
            // And xs[0].t = 2
            Assert.IsTrue(CrtReal.AreEquals(xs[0].T, 2));
        }

    }
}
