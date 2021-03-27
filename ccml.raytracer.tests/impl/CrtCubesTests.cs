using ccml.raytracer.Core;
using ccml.raytracer.Engine;
using NUnit.Framework;

namespace ccml.raytracer.tests.impl
{
    public class CrtCubesTests
    {
        [SetUp]
        public void Setup()
        {

        }

        // Scenario Outline: A ray intersects a cube
        [Test]
        public void ARayIntersectsACube()
        {
            //
            //          Examples:
            //            |        | origin            | direction        | t1 | t2 |
            var rays = new CrtRay[]
            {
                //            |     +x | point(5, 0.5, 0)  | vector(-1, 0, 0) | 4  | 6  |
                CrtFactory.EngineFactory.Ray(
                    CrtFactory.CoreFactory.Point(5, 0.5, 0),
                    CrtFactory.CoreFactory.Vector(-1, 0, 0)
                ),
                //            |     -x | point(-5, 0.5, 0) | vector(1, 0, 0)  | 4  | 6  |
                CrtFactory.EngineFactory.Ray(
                    CrtFactory.CoreFactory.Point(-5, 0.5, 0),
                    CrtFactory.CoreFactory.Vector(1, 0, 0)
                ),
                //            |     +y | point(0.5, 5, 0)  | vector(0, -1, 0) | 4  | 6  |
                CrtFactory.EngineFactory.Ray(
                    CrtFactory.CoreFactory.Point(0.5, 5, 0),
                    CrtFactory.CoreFactory.Vector(0, -1, 0)
                ),
                //            |     -y | point(0.5, -5, 0) | vector(0, 1, 0)  | 4  | 6  |
                CrtFactory.EngineFactory.Ray(
                    CrtFactory.CoreFactory.Point(0.5, -5, 0),
                    CrtFactory.CoreFactory.Vector(0, 1, 0)
                ),
                //            |     +z | point(0.5, 0, 5)  | vector(0, 0, -1) | 4  | 6  |
                CrtFactory.EngineFactory.Ray(
                    CrtFactory.CoreFactory.Point(0.5, 0, 5),
                    CrtFactory.CoreFactory.Vector(0, 0, -1)
                ),
                //            |     -z | point(0.5, 0, -5) | vector(0, 0, 1)  | 4  | 6  |
                CrtFactory.EngineFactory.Ray(
                    CrtFactory.CoreFactory.Point(0.5, 0, -5),
                    CrtFactory.CoreFactory.Vector(0, 0, 1)
                ),
                //            | inside | point(0, 0.5, 0)  | vector(0, 0, 1)  | -1 | 1  |
                CrtFactory.EngineFactory.Ray(
                    CrtFactory.CoreFactory.Point(0, 0.5, 0),
                    CrtFactory.CoreFactory.Vector(0, 0, 1)
                ),
            };
            var t1s = new double[] { 4, 4, 4, 4, 4, 4, -1 };
            var t2s = new double[] { 6, 6, 6, 6, 6, 6, 1 };
            //
            for (int i = 0; i < rays.Length; i++)
            {
                // Given c ← cube()
                var c = CrtFactory.ShapeFactory.Cube();
                // And r ← ray(<origin>, <direction>)
                var r = rays[i];
                // When xs ← local_intersect(c, r)
                var xs = c.LocalIntersect(r);
                // Then xs.count = 2
                Assert.AreEqual(2, xs.Count);
                // And xs[0].t = < t1 >
                Assert.IsTrue(CrtReal.AreEquals(xs[0].T, t1s[i]));
                // And xs[1].t = < t2 >
                Assert.IsTrue(CrtReal.AreEquals(xs[1].T, t2s[i]));
            }
        }

        // Scenario Outline: A ray misses a cube
        [Test]
        public void ARayMissesACube()
        {
            //          Examples:
            //            | origin          | direction                      |
            var rays = new CrtRay[]
            {
                //            | point(-2, 0, 0) | vector(0.2673, 0.5345, 0.8018) |
                CrtFactory.EngineFactory.Ray(
                    CrtFactory.CoreFactory.Point(-2, 0, 0),
                    CrtFactory.CoreFactory.Vector(0.2673, 0.5345, 0.8018)
                ),
                //            | point(0, -2, 0) | vector(0.8018, 0.2673, 0.5345) |
                CrtFactory.EngineFactory.Ray(
                    CrtFactory.CoreFactory.Point(0, -2, 0),
                    CrtFactory.CoreFactory.Vector(0.8018, 0.2673, 0.5345)
                ),
                //            | point(0, 0, -2) | vector(0.5345, 0.8018, 0.2673) |
                CrtFactory.EngineFactory.Ray(
                    CrtFactory.CoreFactory.Point(0, 0, -2),
                    CrtFactory.CoreFactory.Vector(0.5345, 0.8018, 0.2673)
                ),
                //            | point(2, 0, 2)  | vector(0, 0, -1)               |
                CrtFactory.EngineFactory.Ray(
                    CrtFactory.CoreFactory.Point(2, 0, 2),
                    CrtFactory.CoreFactory.Vector(0, 0, -1)
                ),
                //            | point(0, 2, 2)  | vector(0, -1, 0)               |
                CrtFactory.EngineFactory.Ray(
                    CrtFactory.CoreFactory.Point(0, 2, 2),
                    CrtFactory.CoreFactory.Vector(0, -1, 0)
                ),
                //            | point(2, 2, 0)  | vector(-1, 0, 0)               |
                CrtFactory.EngineFactory.Ray(
                    CrtFactory.CoreFactory.Point(2, 2, 0),
                    CrtFactory.CoreFactory.Vector(-1, 0, 0)
                ),
            };
            //
            for (int i = 0; i < rays.Length; i++)
            {
                // Given c ← cube()
                var c = CrtFactory.ShapeFactory.Cube();
                // And r ← ray(<origin>, <direction>)
                var r = rays[i];
                // When xs ← local_intersect(c, r)
                var xs = c.LocalIntersect(r);
                // Then xs.count = 0
                Assert.AreEqual(0, xs.Count);
            }
        }

        // Scenario Outline: The normal on the surface of a cube
        [Test]
        public void TheNormalOnTheSurfaceOfACube()
        {
            //          Examples:
            //            | point                | normal           |
            var pointNormals = new CrtRay[]
            {
                //            | point(1, 0.5, -0.8)  | vector(1, 0, 0)  |
                CrtFactory.EngineFactory.Ray(
                    CrtFactory.CoreFactory.Point(1, 0.5, -0.8),
                    CrtFactory.CoreFactory.Vector(1, 0, 0)
                ),
                //            | point(-1, -0.2, 0.9) | vector(-1, 0, 0) |
                CrtFactory.EngineFactory.Ray(
                    CrtFactory.CoreFactory.Point(-1, -0.2, 0.9),
                    CrtFactory.CoreFactory.Vector(-1, 0, 0)
                ),
                //            | point(-0.4, 1, -0.1) | vector(0, 1, 0)  |
                CrtFactory.EngineFactory.Ray(
                    CrtFactory.CoreFactory.Point(-0.4, 1, -0.1),
                    CrtFactory.CoreFactory.Vector(0, 1, 0)
                ),
                //            | point(0.3, -1, -0.7) | vector(0, -1, 0) |
                CrtFactory.EngineFactory.Ray(
                    CrtFactory.CoreFactory.Point(0.3, -1, -0.7),
                    CrtFactory.CoreFactory.Vector(0, -1, 0)
                ),
                //            | point(-0.6, 0.3, 1)  | vector(0, 0, 1)  |
                CrtFactory.EngineFactory.Ray(
                    CrtFactory.CoreFactory.Point(-0.6, 0.3, 1),
                    CrtFactory.CoreFactory.Vector(0, 0, 1)
                ),
                //            | point(0.4, 0.4, -1)  | vector(0, 0, -1) |
                CrtFactory.EngineFactory.Ray(
                    CrtFactory.CoreFactory.Point(0.4, 0.4, -1),
                    CrtFactory.CoreFactory.Vector(0, 0, -1)
                ),
                //            | point(1, 1, 1)       | vector(1, 0, 0)  |
                CrtFactory.EngineFactory.Ray(
                    CrtFactory.CoreFactory.Point(1, 1, 1),
                    CrtFactory.CoreFactory.Vector(1, 0, 0)
                ),
                //            | point(-1, -1, -1)    | vector(-1, 0, 0) |
                CrtFactory.EngineFactory.Ray(
                    CrtFactory.CoreFactory.Point(-1, -1, -1),
                    CrtFactory.CoreFactory.Vector(-1, 0, 0)
                ),
            };
            //
            for (int i = 0; i < pointNormals.Length; i++)
            {
                // Given c ← cube()
                var c = CrtFactory.ShapeFactory.Cube();
                // And p ← <point>
                var p = pointNormals[i].Origin;
                // When normal ← local_normal_at(c, p)
                var normal = c.LocalNormalAt(p);
                // Then normal = < normal >
                Assert.AreEqual(normal, pointNormals[i].Direction);
            }
        }

    }
}
