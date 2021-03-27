using ccml.raytracer.Core;
using NUnit.Framework;

namespace ccml.raytracer.tests.impl
{
    public class CrtPlanesTests
    {
        [SetUp]
        public void Setup()
        {

        }

        // Scenario: The normal of a plane is constant everywhere
        [Test]
        public void TheNormalOfAPlaneIsConstantEverywhere()
        {
            // Given p ← plane()
            var p = CrtFactory.ShapeFactory.Plane();
            // When n1 ← local_normal_at(p, point(0, 0, 0))
            var n1 = p.LocalNormalAt(CrtFactory.CoreFactory.Point(0, 0, 0));
            // And n2 ← local_normal_at(p, point(10, 0, -10))
            var n2 = p.LocalNormalAt(CrtFactory.CoreFactory.Point(10, 0, -10));
            // And n3 ← local_normal_at(p, point(-5, 0, 150))
            var n3 = p.LocalNormalAt(CrtFactory.CoreFactory.Point(-5, 0, 150));
            // Then n1 = vector(0, 1, 0)
            Assert.IsTrue(n1 == CrtFactory.CoreFactory.Vector(0,1,0));
            // And n2 = vector(0, 1, 0)
            Assert.IsTrue(n1 == CrtFactory.CoreFactory.Vector(0, 1, 0));
            // And n3 = vector(0, 1, 0)
            Assert.IsTrue(n1 == CrtFactory.CoreFactory.Vector(0, 1, 0));
        }

        // Scenario: Intersect with a ray parallel to the plane
        [Test]
        public void IntersectWithARayParallelToThePlane()
        {
            // Given p ← plane()
            var p = CrtFactory.ShapeFactory.Plane();
            // And r ← ray(point(0, 10, 0), vector(0, 0, 1))
            var r = CrtFactory.EngineFactory.Ray(CrtFactory.CoreFactory.Point(0, 10, 0), CrtFactory.CoreFactory.Vector(0, 0, 1));
            // When xs ← local_intersect(p, r)
            var xs = p.LocalIntersect(r);
            // Then xs is empty
            Assert.IsEmpty(xs);
        }

        // Scenario: Intersect with a coplanar ray
        [Test]
        public void IntersectWithACoplanarRay()
        {
            // Given p ← plane()
            var p = CrtFactory.ShapeFactory.Plane();
            // And r ← ray(point(0, 0, 0), vector(0, 0, 1))
            var r = CrtFactory.EngineFactory.Ray(CrtFactory.CoreFactory.Point(0, 0, 0), CrtFactory.CoreFactory.Vector(0, 0, 1));
            // When xs ← local_intersect(p, r)
            var xs = p.LocalIntersect(r);
            // Then xs is empty
            Assert.IsEmpty(xs);
        }

        // Scenario: A ray intersecting a plane from above
        [Test]
        public void ARayIntersectingAPlaneFromAbove()
        {
            // Given p ← plane()
            var p = CrtFactory.ShapeFactory.Plane();
            // And r ← ray(point(0, 1, 0), vector(0, -1, 0))
            var r = CrtFactory.EngineFactory.Ray(CrtFactory.CoreFactory.Point(0, 1, 0), CrtFactory.CoreFactory.Vector(0, -1, 0));
            // When xs ← local_intersect(p, r)
            var xs = p.LocalIntersect(r);
            // Then xs.count = 1
            Assert.AreEqual(1, xs.Count);
            // And xs[0].t = 1
            Assert.IsTrue(CrtReal.AreEquals(xs[0].T, 1));
            // And xs[0].object = p
            Assert.AreSame(xs[0].TheObject, p);
        }

        // Scenario: A ray intersecting a plane from below
        [Test]
        public void ARayIntersectingAPlaneFromBelow()
        {
            // Given p ← plane()
            var p = CrtFactory.ShapeFactory.Plane();
            // And r ← ray(point(0, -1, 0), vector(0, 1, 0))
            var r = CrtFactory.EngineFactory.Ray(CrtFactory.CoreFactory.Point(0, -1, 0), CrtFactory.CoreFactory.Vector(0, 1, 0));
            // When xs ← local_intersect(p, r)
            var xs = p.LocalIntersect(r);
            // Then xs.count = 1
            Assert.AreEqual(1, xs.Count);
            // And xs[0].t = 1
            Assert.IsTrue(CrtReal.AreEquals(xs[0].T, 1));
            // And xs[0].object = p
            Assert.AreSame(xs[0].TheObject, p);
        }
    }
}
