using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ccml.raytracer.engine.core;
using ccml.raytracer.engine.core.Materials;
using NUnit.Framework;

namespace ccml.raytracer.tests.math.core
{
    public class CrtWorldTests
    {
        [SetUp]
        public void Setup()
        {

        }

        // Scenario: Creating a world
        [Test]
        public void CreatingAWorld()
        {
            // Given w ← world()
            var w = CrtFactory.World();
            // Then w contains no objects
            Assert.IsEmpty(w.Objects);
            // And w has no light source
            Assert.IsEmpty(w.Lights);
        }

        // Scenario: The default world
        [Test]
        public void TheDefaultWorld()
        {
            // Given light ← point_light(point(-10, 10, -10), color(1, 1, 1))
            var light = CrtFactory.PointLight(CrtFactory.Point(-10, 10, -10), CrtFactory.Color(1, 1, 1));
            // And s1 ← sphere() with:
            //      | material.color | (0.8, 1.0, 0.6) |
            //      | material.diffuse | 0.7 |
            //      | material.specular | 0.2 |
            var s1 = CrtFactory.Sphere();
            s1.Material = CrtFactory.UniformColorMaterial(CrtFactory.Color(0.8, 1.0, 0.6), diffuse:0.7, specular:0.2);
            // And s2 ← sphere() with:
            //      | transform | scaling(0.5, 0.5, 0.5) |
            var s2 = CrtFactory.Sphere();
            s2.TransformMatrix = CrtFactory.ScalingMatrix(0.5, 0.5, 0.5);
            // When w ← default_world()
            var w = CrtFactory.DefaultWorld();
            // Then w.light = light
            Assert.IsTrue(w.Lights.First() == light);
            // And w contains s1
            Assert.IsTrue(w.Objects[0] == s1);
            // And w contains s2
            Assert.IsTrue(w.Objects[1] == s2);
        }

        // Scenario: Intersect a world with a ray
        [Test]
        public void IntersectAWorldWithARay()
        {
            // Given w ← default_world()
            var w = CrtFactory.DefaultWorld();
            // And r ← ray(point(0, 0, -5), vector(0, 0, 1))
            var r = CrtFactory.Ray(CrtFactory.Point(0, 0, -5), CrtFactory.Vector(0, 0, 1));
            // When xs ← intersect_world(w, r)
            var xs = r.Intersect(w);
            // Then xs.count = 4
            Assert.AreEqual(4, xs.Count);
            // And xs[0].t = 4
            Assert.IsTrue(CrtReal.AreEquals(xs[0].T, 4));
            // And xs[1].t = 4.5
            Assert.IsTrue(CrtReal.AreEquals(xs[1].T, 4.5));
            // And xs[2].t = 5.5
            Assert.IsTrue(CrtReal.AreEquals(xs[2].T, 5.5));
            // And xs[3].t = 6
            Assert.IsTrue(CrtReal.AreEquals(xs[3].T, 6));
        }

        #region Shading an intersection

        // Scenario: Shading an intersection
        [Test]
        public void ShadingAnIntersection()
        {
            // Given w ← default_world()
            var w = CrtFactory.DefaultWorld();
            // And r ← ray(point(0, 0, -5), vector(0, 0, 1))
            var r = CrtFactory.Ray(CrtFactory.Point(0, 0, -5), CrtFactory.Vector(0, 0, 1));
            // And shape ← the first object in w
            var shape = w.Objects[0];
            // And i ← intersection(4, shape)
            var i = shape.Intersect(r).Single(ie => CrtReal.AreEquals(ie.T, 4));
            // When comps ← prepare_computations(i, r)
            var comps = CrtFactory.Engine().PrepareComputations(i, r);
            // And c ← shade_hit(w, comps)
            var c = CrtFactory.Engine().ShadeHit(w, comps);
            // Then c = color(0.38066, 0.47583, 0.2855)
            Assert.IsTrue(c == CrtFactory.Color(0.38066, 0.47583, 0.2855));
        }

        // Scenario: Shading an intersection from the inside
        [Test]
        public void ShadingAnIntersectionFromTheInside()
        {
            // Given w ← default_world()
            var w = CrtFactory.DefaultWorld();
            w.Lights.Clear();
            // And w.light ← point_light(point(0, 0.25, 0), color(1, 1, 1))
            w.Add(CrtFactory.PointLight(CrtFactory.Point(0, 0.25, 0), CrtFactory.Color(1, 1, 1)));
            // And r ← ray(point(0, 0, 0), vector(0, 0, 1))
            var r = CrtFactory.Ray(CrtFactory.Point(0, 0, 0), CrtFactory.Vector(0, 0, 1));
            // And shape ← the second object in w
            var shape = w.Objects[1];
            // And i ← intersection(0.5, shape)
            var i = shape.Intersect(r).Single(ie => CrtReal.AreEquals(ie.T, 0.5));
            // When comps ← prepare_computations(i, r)
            var comps = CrtFactory.Engine().PrepareComputations(i, r);
            // And c ← shade_hit(w, comps)
            var c = CrtFactory.Engine().ShadeHit(w, comps);
            // Then c = color(0.90498, 0.90498, 0.90498)
            Assert.IsTrue(c == CrtFactory.Color(0.90498, 0.90498, 0.90498));
        }

        // Scenario: The color when a ray misses
        [Test]
        public void TheColorWhenARayMisses()
        {
            // Given w ← default_world()
            var w = CrtFactory.DefaultWorld();
            // And r ← ray(point(0, 0, -5), vector(0, 1, 0))
            var r = CrtFactory.Ray(CrtFactory.Point(0, 0, -5), CrtFactory.Vector(0, 1, 0));
            // When c ← color_at(w, r)
            var c = w.ColorAt(r);
            // Then c = color(0, 0, 0)
            Assert.IsTrue(c == CrtFactory.Color(0,0,0));
        }

        // Scenario: The color when a ray hits
        [Test]
        public void TheColorWhenARayHits()
        {
            // Given w ← default_world()
            var w = CrtFactory.DefaultWorld();
            // And r ← ray(point(0, 0, -5), vector(0, 0, 1))
            var r = CrtFactory.Ray(CrtFactory.Point(0, 0, -5), CrtFactory.Vector(0, 0, 1));
            // When c ← color_at(w, r)
            var c = w.ColorAt(r);
            // Then c = color(0.38066, 0.47583, 0.2855)
            Assert.IsTrue(c == CrtFactory.Color(0.38066, 0.47583, 0.2855));
        }

        // Scenario: The color with an intersection behind the ray
        [Test]
        public void TheColorWithAnIntersectionBehindTheRay()
        {
            // Given w ← default_world()
            var w = CrtFactory.DefaultWorld();
            // And outer ← the first object in w
            var outer = w.Objects[0];
            // And outer.material.ambient ← 1
            outer.Material.Ambient = 1;
            // And inner ← the second object in w
            var inner = w.Objects[1];
            // And inner.material.ambient ← 1
            inner.Material.Ambient = 1;
            // And r ← ray(point(0, 0, 0.75), vector(0, 0, -1))
            var r = CrtFactory.Ray(CrtFactory.Point(0, 0, 0.75), CrtFactory.Vector(0, 0, -1));
            // When c ← color_at(w, r)
            var c = w.ColorAt(r);
            // Then c = inner.material.color
            Assert.IsTrue(c == ((CrtUniformColorMaterial)inner.Material).Color);
        }

        #endregion
    }
}
