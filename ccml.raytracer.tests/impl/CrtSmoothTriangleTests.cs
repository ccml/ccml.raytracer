using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ccml.raytracer.Core;
using ccml.raytracer.Shapes;
using NUnit.Framework;

namespace ccml.raytracer.tests.impl
{
    public class CrtSmoothTriangleTests
    {
        private CrtPoint p1, p2, p3;
        private CrtVector n1, n2, n3;
        private CrtSmoothTriangle tri;

        // Background:
        [SetUp]
        public void Setup()
        {
            // Given p1 ← point(0, 1, 0)
            p1 = CrtFactory.CoreFactory.Point(0, 1, 0);
            // And p2 ← point(-1, 0, 0)
            p2 = CrtFactory.CoreFactory.Point(-1, 0, 0);
            // And p3 ← point(1, 0, 0)
            p3 = CrtFactory.CoreFactory.Point(1, 0, 0);
            // And n1 ← vector(0, 1, 0)
            n1 = CrtFactory.CoreFactory.Vector(0, 1, 0);
            // And n2 ← vector(-1, 0, 0)
            n2 = CrtFactory.CoreFactory.Vector(-1, 0, 0);
            // And n3 ← vector(1, 0, 0)
            n3 = CrtFactory.CoreFactory.Vector(1, 0, 0);
            // When tri ← smooth_triangle(p1, p2, p3, n1, n2, n3)
            tri = CrtFactory.ShapeFactory.SmoothTriangle(p1, p2, p3, n1, n2, n3);
        }

        // Scenario: Constructing a smooth triangle
        [Test]
        public void ConstructingASmoothTriangle()
        {
            // Then tri.p1 = p1
            Assert.IsTrue(tri.P1 == p1);
            // And tri.p2 = p2
            Assert.IsTrue(tri.P2 == p2);
            // And tri.p3 = p3
            Assert.IsTrue(tri.P3 == p3);
            // And tri.n1 = n1
            Assert.IsTrue(tri.N1 == n1);
            // And tri.n2 = n2
            Assert.IsTrue(tri.N2 == n2);
            // And tri.n3 = n3
            Assert.IsTrue(tri.N3 == n3);
        }

        // Scenario: An intersection can encapsulate `u` and `v`
        [Test]
        public void AnIntersectionCanEncapsulateUAndV()
        {
            // Given s ← triangle(point(0, 1, 0), point(-1, 0, 0), point(1, 0, 0))
            var s = CrtFactory.ShapeFactory.Triangle(
                CrtFactory.CoreFactory.Point(0, 1, 0),
                CrtFactory.CoreFactory.Point(-1, 0, 0),
                CrtFactory.CoreFactory.Point(1, 0, 0)
            );
            // When i ← intersection_with_uv(3.5, s, 0.2, 0.4)
            var i = CrtFactory.EngineFactory.Intersection(3.5, s, 0.2, 0.4);
            // Then i.u = 0.2
            Assert.IsTrue(CrtReal.AreEquals(i.U, 0.2));
            // And i.v = 0.4
            Assert.IsTrue(CrtReal.AreEquals(i.V, 0.4));
        }

        // Scenario: An intersection with a smooth triangle stores u/v
        [Test]
        public void AnIntersectionWithASmoothTriangleStoresUV()
        {
            // When r ← ray(point(-0.2, 0.3, -2), vector(0, 0, 1))
            var r = CrtFactory.EngineFactory.Ray(
                CrtFactory.CoreFactory.Point(-0.2, 0.3, -2),
                CrtFactory.CoreFactory.Vector(0, 0, 1)
            );
            // And xs ← local_intersect(tri, r)
            var xs = tri.LocalIntersect(r);
            // Then xs[0].u = 0.45
            Assert.IsTrue(CrtReal.AreEquals(xs[0].U, 0.45));
            // And xs[0].v = 0.25
            Assert.IsTrue(CrtReal.AreEquals(xs[0].V, 0.25));
        }

        // Scenario: A smooth triangle uses u/v to interpolate the normal
        [Test]
        public void ASmoothTriangleUsesUVToInterpolateTheNormal()
        {
            // When i ← intersection_with_uv(1, tri, 0.45, 0.25)
            var i = CrtFactory.EngineFactory.Intersection(1, tri, 0.45, 0.25);
            // And n ← normal_at(tri, point(0, 0, 0), i)
            var n = tri.NormalAt(CrtFactory.CoreFactory.Point(0, 0, 0), i);
            // Then n = vector(-0.5547, 0.83205, 0)
            Assert.IsTrue(n == CrtFactory.CoreFactory.Vector(-0.5547, 0.83205, 0));
        }

        // Scenario: Preparing the normal on a smooth triangle
        [Test]
        public void PreparingTheNormalOnASmoothTriangle()
        {
            // When i ← intersection_with_uv(1, tri, 0.45, 0.25)
            var i = CrtFactory.EngineFactory.Intersection(1, tri, 0.45, 0.25);
            // And r ← ray(point(-0.2, 0.3, -2), vector(0, 0, 1))
            var r = CrtFactory.EngineFactory.Ray(
                CrtFactory.CoreFactory.Point(-0.2, 0.3, -2),
                CrtFactory.CoreFactory.Vector(0, 0, 1)
            );
            // And xs ← intersections(i)
            var xs = CrtFactory.EngineFactory.Intersections(i);
            // And comps ← prepare_computations(i, r, xs)
            var comps = CrtFactory.EngineFactory.Engine().PrepareComputations(i, r, xs);
            // Then comps.normalv = vector(-0.5547, 0.83205, 0)
            Assert.IsTrue(comps.NormalVector == CrtFactory.CoreFactory.Vector(-0.5547, 0.83205, 0));
        }

    }
}
