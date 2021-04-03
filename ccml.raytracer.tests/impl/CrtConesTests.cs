using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ccml.raytracer.Core;
using ccml.raytracer.Engine;
using NUnit.Framework;
using NUnit.Framework.Internal;

namespace ccml.raytracer.tests.impl
{
    public class CrtConesTests
    {
        [SetUp]
        public void Setup()
        {

        }

        // Scenario Outline: Intersecting a cone with a ray
        [Test]
        public void IntersectingAConeWithARay()
        {
            // Given shape ← cone()
            var shape = CrtFactory.ShapeFactory.Cone();
            // And direction ← normalize(<direction>)
            // And r ← ray(<origin>, direction)
            // Examples:
            //      | origin          | direction           | t0      | t1       |
            var rays = new CrtRay[]
            {
                //      | point(0, 0, -5) | vector(0, 0, 1)     | 5       | 5        |
                CrtFactory.EngineFactory.Ray(
                    CrtFactory.CoreFactory.Point(0, 0, -5),
                    ~CrtFactory.CoreFactory.Vector(0, 0, 1)
                ),
                //      | point(0, 0, -5) | vector(1, 1, 1)     | 8.66025 | 8.66025  |
                CrtFactory.EngineFactory.Ray(
                    CrtFactory.CoreFactory.Point(0, 0, -5),
                    ~CrtFactory.CoreFactory.Vector(1, 1, 1)
                ),
                //      | point(1, 1, -5) | vector(-0.5, -1, 1) | 4.55006 | 49.44994 |
                CrtFactory.EngineFactory.Ray(
                    CrtFactory.CoreFactory.Point(1, 1, -5),
                    ~CrtFactory.CoreFactory.Vector(-0.5, -1, 1)
                )
            };
            var t0s = new double[] { 5, 8.66025, 4.55006 };
            var t1s = new double[] { 5, 8.66025, 49.44994 };
            for (int i=0; i < rays.Length; i++)
            {
                // When xs ← local_intersect(shape, r)
                var xs = shape.LocalIntersect(rays[i]);
                // Then xs.count = 2
                Assert.AreEqual(2, xs.Count);
                // And xs[0].t = < t0 >
                Assert.IsTrue(CrtReal.AreEquals(xs[0].T, t0s[i]));
                // And xs[1].t = < t1 >
                Assert.IsTrue(CrtReal.AreEquals(xs[1].T, t1s[i]));
            }
        }

        // Scenario: Intersecting a cone with a ray parallel to one of its halves
        [Test]
        public void IntersectingAConeWithARayParallelToOneOfItsHalves()
        {
            // Given shape ← cone()
            var shape = CrtFactory.ShapeFactory.Cone();
            // And direction ← normalize(vector(0, 1, 1))
            // And r ← ray(point(0, 0, -1), direction)
            var r = CrtFactory.EngineFactory.Ray(
                CrtFactory.CoreFactory.Point(0, 0, -1),
                ~CrtFactory.CoreFactory.Vector(0, 1, 1)
            );
            // When xs ← local_intersect(shape, r)
            var xs = shape.LocalIntersect(r);
            // Then xs.count = 1
            Assert.AreEqual(1, xs.Count);
            // And xs[0].t = 0.35355
            Assert.IsTrue(CrtReal.AreEquals(xs[0].T, 0.35355));
        }

        // Scenario Outline: Intersecting a cone's end caps
        [Test]
        public void IntersectingAConesEndCaps()
        {
            // Given shape ← cone()
            // And shape.minimum ← -0.5
            // And shape.maximum ← 0.5
            // And shape.closed ← true
            var shape = 
                CrtFactory.ShapeFactory.Cone()
                    .WithMinimum(-0.5).WithMaximum(0.5)
                    .WithMinimumClosed().WithMaximumClosed();
            // And direction ← normalize(<direction>)
            // And r ← ray(<origin>, direction)
            // Examples:
            //      | origin             | direction       | count |
            //      | point(0, 0, -5)    | vector(0, 1, 0) | 0     |
            //      | point(0, 0, -0.25) | vector(0, 1, 1) | 2     |
            //      | point(0, 0, -0.25) | vector(0, 1, 0) | 4     |
            var rays = new CrtRay[]
            {
                //      | point(0, 0, -5)    | vector(0, 1, 0) | 0     |
                CrtFactory.EngineFactory.Ray(
                    CrtFactory.CoreFactory.Point(0, 0, -5),
                    ~CrtFactory.CoreFactory.Vector(0, 1, 0)
                ),
                //      | point(0, 0, -0.25) | vector(0, 1, 1) | 2     |
                CrtFactory.EngineFactory.Ray(
                    CrtFactory.CoreFactory.Point(0, 0, -0.25),
                    ~CrtFactory.CoreFactory.Vector(0, 1, 1)
                ),
                //      | point(0, 0, -0.25) | vector(0, 1, 0) | 4     |
                CrtFactory.EngineFactory.Ray(
                    CrtFactory.CoreFactory.Point(0, 0, -0.25),
                    ~CrtFactory.CoreFactory.Vector(0, 1, 0)
                ),
            };
            var counts = new int[] { 0, 2, 4 };
            for(int i=0; i < rays.Length; i++)
            {
                // When xs ← local_intersect(shape, r)
                var xs = shape.LocalIntersect(rays[i]);
                // Then xs.count = <count>
                Assert.AreEqual(counts[i], xs.Count);
            }
        }

        // Scenario Outline: Computing the normal vector on a cone
        [Test]
        public void ComputingTheNormalVectorOnACone()
        {
            // Given shape ← cone()
            var shape = CrtFactory.ShapeFactory.Cone();
            // Examples:
            //      | point            | normal            |
            //      | point(0, 0, 0)   | vector(0, 0, 0)   |
            //      | point(1, 1, 1)   | vector(1, -√2, 1) |
            //      | point(-1, -1, 0) | vector(-1, 1, 0)  |
            var pointsNormals = new CrtRay[]
            {
                //      | point(0, 0, 0)   | vector(0, 0, 0)   |
                CrtFactory.EngineFactory.Ray(
                    CrtFactory.CoreFactory.Point(0, 0, 0),
                    CrtFactory.CoreFactory.Vector(0, 0, 0)
                ),
                //      | point(1, 1, 1)   | vector(1, -√2, 1) |
                CrtFactory.EngineFactory.Ray(
                    CrtFactory.CoreFactory.Point(1, 1, 1),
                    CrtFactory.CoreFactory.Vector(1, -Math.Sqrt(2.0), 1)
                ),
                //      | point(-1, -1, 0) | vector(-1, 1, 0)  |
                CrtFactory.EngineFactory.Ray(
                    CrtFactory.CoreFactory.Point(-1, -1, 0),
                    CrtFactory.CoreFactory.Vector(-1, 1, 0)
                ),
            };
            foreach (var pointNormal in pointsNormals)
            {
                // When n ← local_normal_at(shape, <point>)
                var n = shape.LocalNormalAt(pointNormal.Origin);
                // Then n = < normal >
                Assert.IsTrue(n == pointNormal.Direction);
            }
        }
    }
}
