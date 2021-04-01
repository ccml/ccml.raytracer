using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ccml.raytracer.Core;
using ccml.raytracer.Engine;
using NUnit.Framework;

namespace ccml.raytracer.tests.impl
{
    public class CrtCylindersTests
    {
        [SetUp]
        public void Setup()
        {

        }

        #region Cylinder side intersections

        // Scenario Outline: A ray misses a cylinder
        [Test]
        public void ARayMissesACylinder()
        {
            // Given cyl ← cylinder()
            var cyl = CrtFactory.ShapeFactory.Cylinder();
            // And direction ← normalize(<direction>)
            // And r ← ray(<origin>, direction)
            //      Examples:
            //        | origin          | direction       |
            var rays = new CrtRay[]
            {
                //    | point(1, 0, 0)  | vector(0, 1, 0) |
                CrtFactory.EngineFactory.Ray(
                    CrtFactory.CoreFactory.Point(1, 0, 0),
                    ~CrtFactory.CoreFactory.Vector(0, 1, 0)
                ),
                //    | point(0, 0, 0)  | vector(0, 1, 0) |
                CrtFactory.EngineFactory.Ray(
                    CrtFactory.CoreFactory.Point(0, 0, 0),
                    ~CrtFactory.CoreFactory.Vector(0, 1, 0)
                ),
                //    | point(0, 0, -5) | vector(1, 1, 1) |
                CrtFactory.EngineFactory.Ray(
                    CrtFactory.CoreFactory.Point(0, 0, -5),
                    ~CrtFactory.CoreFactory.Vector(1, 1, 1)
                ),
            };
            foreach (var r in rays)
            {
                // When xs ← local_intersect(cyl, r)
                var xs = cyl.LocalIntersect(r);
                // Then xs.count = 0
                Assert.AreEqual(0, xs.Count);
            }
        }

        // Scenario Outline: A ray strikes a cylinder
        [Test]
        public void ARayStrikesACylinder()
        {
            // Given cyl ← cylinder()
            var cyl = CrtFactory.ShapeFactory.Cylinder();
            // And direction ← normalize(<direction>)
            // And r ← ray(<origin>, direction)
            //      Examples:
            //          | origin            | direction         | t0      | t1      |
            var rays = new CrtRay[]
            {
                //      | point(1, 0, -5)   | vector(0, 0, 1)   | 5       | 5       |
                CrtFactory.EngineFactory.Ray(
                    CrtFactory.CoreFactory.Point(1, 0, -5),
                    ~CrtFactory.CoreFactory.Vector(0, 0, 1)
                ),
                //      | point(0, 0, -5)   | vector(0, 0, 1)   | 4       | 6       |
                CrtFactory.EngineFactory.Ray(
                    CrtFactory.CoreFactory.Point(0, 0, -5),
                    ~CrtFactory.CoreFactory.Vector(0, 0, 1)
                ),
                //      | point(0.5, 0, -5) | vector(0.1, 1, 1) | 6.80798 | 7.08872 |
                CrtFactory.EngineFactory.Ray(
                    CrtFactory.CoreFactory.Point(0.5, 0, -5),
                    ~CrtFactory.CoreFactory.Vector(0.1, 1, 1)
                ),
            };
            var t0s = new double[] { 5, 4, 6.80798 };
            var t1s = new double[] { 5, 6, 7.08872 };
            for (int i = 0; i < rays.Length; i++)
            {
                var r = rays[i];
                var t0 = t0s[i];
                var t1 = t1s[i];
                // When xs ← local_intersect(cyl, r)
                var xs = cyl.LocalIntersect(r);
                // Then xs.count = 2
                Assert.AreEqual(2, xs.Count);
                // And xs[0].t = < t0 >
                Assert.IsTrue(CrtReal.AreEquals(xs[0].T, t0));
                // And xs[1].t = < t1 >
                Assert.IsTrue(CrtReal.AreEquals(xs[1].T, t1));
            }
        }

        // Scenario Outline: Normal vector on a cylinder
        [Test]
        public void NormalVectorOnACylinder()
        {
            // Given cyl ← cylinder()
            var cyl = CrtFactory.ShapeFactory.Cylinder();
            //       Examples:
            //          | point           | normal           |
            var pointsNormals = new CrtRay[]
            {
                //          | point(1, 0, 0)  | vector(1, 0, 0)  |
                CrtFactory.EngineFactory.Ray(
                    CrtFactory.CoreFactory.Point(1, 0, 0),
                    ~CrtFactory.CoreFactory.Vector(1, 0, 0)
                ),
                //          | point(0, 5, -1) | vector(0, 0, -1) |
                CrtFactory.EngineFactory.Ray(
                    CrtFactory.CoreFactory.Point(0, 5, -1),
                    ~CrtFactory.CoreFactory.Vector(0, 0, -1)
                ),
                //          | point(0, -2, 1) | vector(0, 0, 1)  |
                CrtFactory.EngineFactory.Ray(
                    CrtFactory.CoreFactory.Point(0, -2, 1),
                    ~CrtFactory.CoreFactory.Vector(0, 0, 1)
                ),
                //          | point(-1, 1, 0) | vector(-1, 0, 0) |
                CrtFactory.EngineFactory.Ray(
                    CrtFactory.CoreFactory.Point(-1, 1, 0),
                    ~CrtFactory.CoreFactory.Vector(-1, 0, 0)
                ),
            };
            foreach (var pointNormal in pointsNormals)
            {
                var point = pointNormal.Origin;
                var normal = pointNormal.Direction;
                // When n ← local_normal_at(cyl, <point>)
                var n = cyl.LocalNormalAt(point);
                // Then n = < normal >
                Assert.IsTrue(n == normal);
            }
        }

        #endregion

        #region Truncated cylinders

        // Scenario: The default minimum and maximum for a cylinder
        [Test]
        public void TheDefaultMinimumAndMaximumForACylinder()
        {
            // Given cyl ← cylinder()
            var cyl = CrtFactory.ShapeFactory.Cylinder();
            // Then cyl.minimum = -infinity
            Assert.AreEqual(double.MinValue, cyl.Minimum);
            // And cyl.maximum = infinity
            Assert.AreEqual(double.MaxValue, cyl.Maximum);
        }

        // Scenario Outline: Intersecting a constrained cylinder
        [Test]
        public void IntersectingAConstrainedCylinder()
        {
            // Given cyl ← cylinder()
            // And cyl.minimum ← 1
            // And cyl.maximum ← 2
            // And direction ← normalize(<direction>)
            var cyl = CrtFactory.ShapeFactory.Cylinder().WithMinimum(1).WithMaximum(2);
            // Examples:
            //      |   | point             | direction         | count |
            var rays = new CrtRay[]
            {
                //      | 1 | point(0, 1.5, 0)  | vector(0.1, 1, 0) | 0     |
                CrtFactory.EngineFactory.Ray(
                    CrtFactory.CoreFactory.Point(0, 1.5, 0),
                    ~CrtFactory.CoreFactory.Vector(0.1, 1, 0)
                ),
                //      | 2 | point(0, 3, -5)   | vector(0, 0, 1)   | 0     |
                CrtFactory.EngineFactory.Ray(
                    CrtFactory.CoreFactory.Point(0, 3, -5),
                    ~CrtFactory.CoreFactory.Vector(0, 0, 1)
                ),
                //      | 3 | point(0, 0, -5)   | vector(0, 0, 1)   | 0     |
                CrtFactory.EngineFactory.Ray(
                    CrtFactory.CoreFactory.Point(0, 0, -5),
                    ~CrtFactory.CoreFactory.Vector(0, 0, 1)
                ),
                //      | 4 | point(0, 2, -5)   | vector(0, 0, 1)   | 0     |
                CrtFactory.EngineFactory.Ray(
                    CrtFactory.CoreFactory.Point(0, 2, -5),
                    ~CrtFactory.CoreFactory.Vector(0, 0, 1)
                ),
                //      | 5 | point(0, 1, -5)   | vector(0, 0, 1)   | 0     |
                CrtFactory.EngineFactory.Ray(
                    CrtFactory.CoreFactory.Point(0, 1, -5),
                    ~CrtFactory.CoreFactory.Vector(0, 0, 1)
                ),
                //      | 6 | point(0, 1.5, -2) | vector(0, 0, 1)   | 2     |
                CrtFactory.EngineFactory.Ray(
                    CrtFactory.CoreFactory.Point(0, 1.5, -2),
                    ~CrtFactory.CoreFactory.Vector(0, 0, 1)
                ),
            };
            var counts = new int[] { 0, 0, 0, 0, 0, 2 };
            for (int i = 0; i < rays.Length; i++)
            {
                // And r ← ray(<point>, direction)
                var r = rays[i];
                // When xs ← local_intersect(cyl, r)
                var xs = cyl.LocalIntersect(r);
                // Then xs.count = <count>
                Assert.AreEqual(counts[i], xs.Count);
            }
        }

        #endregion

        #region Closed cylinders

        // Scenario: The default closed value for a cylinder
        [Test]
        public void TheDefaultClosedValueForACylinder()
        {
            // Given cyl ← cylinder()
            var cyl = CrtFactory.ShapeFactory.Cylinder();
            // Then cyl.closed = false
            Assert.IsFalse(cyl.MinimumClosed);
            Assert.IsFalse(cyl.MaximumClosed);
        }

        // Scenario Outline: Intersecting the caps of a closed cylinder
        [Test]
        public void IntersectingTheCapsOfAClosedCylinder()
        {
            // Given cyl ← cylinder()
            // And cyl.minimum ← 1
            // And cyl.maximum ← 2
            // And cyl.closed ← true
            var cyl =
                CrtFactory.ShapeFactory.Cylinder()
                    .WithMinimum(1)
                    .WithMaximum(2)
                    .WithMinimumClosed()
                    .WithMaximumClosed();
            // And direction ← normalize(<direction>)
            // Examples:
            //      |   | point            | direction        | count |
            var rays = new CrtRay[]
            {
                //      | 1 | point(0, 3, 0)   | vector(0, -1, 0) | 2     |
                CrtFactory.EngineFactory.Ray(
                    CrtFactory.CoreFactory.Point(0, 3, 0),
                    ~CrtFactory.CoreFactory.Vector(0, -1, 0)
                ),
                //      | 2 | point(0, 3, -2)  | vector(0, -1, 2) | 2     |
                CrtFactory.EngineFactory.Ray(
                    CrtFactory.CoreFactory.Point(0, 3, -2),
                    ~CrtFactory.CoreFactory.Vector(0, -1, 2)
                ),
                //      | 3 | point(0, 4, -2)  | vector(0, -1, 1) | 2     | # corner case
                CrtFactory.EngineFactory.Ray(
                    CrtFactory.CoreFactory.Point(0, 4, -2),
                    ~CrtFactory.CoreFactory.Vector(0, -1, 1)
                ),
                //      | 4 | point(0, 0, -2)  | vector(0, 1, 2)  | 2     |
                CrtFactory.EngineFactory.Ray(
                    CrtFactory.CoreFactory.Point(0, 0, -2),
                    ~CrtFactory.CoreFactory.Vector(0, 1, 2)
                ),
                //      | 5 | point(0, -1, -2) | vector(0, 1, 1)  | 2     | # corner case
                CrtFactory.EngineFactory.Ray(
                    CrtFactory.CoreFactory.Point(0, -1, -2),
                    ~CrtFactory.CoreFactory.Vector(0, 1, 1)
                ),
            };
            var counts = new int[] { 2, 2, 2, 2, 2 };
            for (int i = 0; i < rays.Length; i++)
            {
                // And r ← ray(<point>, direction)
                var r = rays[i];
                // When xs ← local_intersect(cyl, r)
                var xs = cyl.LocalIntersect(r);
                // Then xs.count = <count>
                Assert.AreEqual(counts[i], xs.Count);
            }
        }

        // Scenario Outline: The normal vector on a cylinder's end caps
        [Test]
        public void TheNormalVectorOnACylindersEndCaps()
        {
            // Given cyl ← cylinder()
            // And cyl.minimum ← 1
            // And cyl.maximum ← 2
            // And cyl.closed ← true
            var cyl =
                CrtFactory.ShapeFactory.Cylinder()
                    .WithMinimum(1)
                    .WithMaximum(2)
                    .WithMinimumClosed()
                    .WithMaximumClosed();
            //       Examples:
            //          | point            | normal           |
            var pointsNormals = new CrtRay[]
            {
                //          | point(0, 1, 0)   | vector(0, -1, 0) |
                CrtFactory.EngineFactory.Ray(
                    CrtFactory.CoreFactory.Point(0, 1, 0),
                    ~CrtFactory.CoreFactory.Vector(0, -1, 0)
                ),
                //          | point(0.5, 1, 0) | vector(0, -1, 0) |
                CrtFactory.EngineFactory.Ray(
                    CrtFactory.CoreFactory.Point(0.5, 1, 0),
                    ~CrtFactory.CoreFactory.Vector(0, -1, 0)
                ),
                //          | point(0, 1, 0.5) | vector(0, -1, 0) |
                CrtFactory.EngineFactory.Ray(
                    CrtFactory.CoreFactory.Point(0, 1, 0.5),
                    ~CrtFactory.CoreFactory.Vector(0, -1, 0)
                ),
                //          | point(0, 2, 0)   | vector(0, 1, 0)  |
                CrtFactory.EngineFactory.Ray(
                    CrtFactory.CoreFactory.Point(0, 2, 0),
                    ~CrtFactory.CoreFactory.Vector(0, 1, 0)
                ),
                //          | point(0.5, 2, 0) | vector(0, 1, 0)  |
                CrtFactory.EngineFactory.Ray(
                    CrtFactory.CoreFactory.Point(0.5, 2, 0),
                    ~CrtFactory.CoreFactory.Vector(0, 1, 0)
                ),
                //          | point(0, 2, 0.5) | vector(0, 1, 0)  |
                CrtFactory.EngineFactory.Ray(
                    CrtFactory.CoreFactory.Point(0, 2, 0.5),
                    ~CrtFactory.CoreFactory.Vector(0, 1, 0)
                ),
            };
            foreach (var pointNormal in pointsNormals)
            {
                var point = pointNormal.Origin;
                var normal = pointNormal.Direction;
                // When n ← local_normal_at(cyl, <point>)
                var n = cyl.LocalNormalAt(point);
                // Then n = < normal >
                Assert.IsTrue(n == normal);
            }
        }

        #endregion
    }
}
