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
            s1.Material = CrtFactory.MaterialFactory.SpecificMaterial(CrtFactory.Color(0.8, 1.0, 0.6), diffuse:0.7, specular:0.2);
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
            var i = CrtFactory.Intersection(4.0, shape);
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
            var i = CrtFactory.Intersection(0.5, shape);
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
            Assert.IsTrue(c == inner.Material.Color);
        }

        #endregion

        #region Shadowing

        // Scenario: There is no shadow when nothing is collinear with point and light
        [Test]
        public void ThereIsNoShadowWhenNothingIsCollinearWithPointAndLight()
        {
            // Given w ← default_world()
            var w = CrtFactory.DefaultWorld();
            // And p ← point(0, 10, 0)
            var p = CrtFactory.Point(0, 10, 0);
            // Then is_shadowed(w, p) is false
            Assert.IsFalse(w.IsShadowed(p));
        }

        // Scenario: The shadow when an object is between the point and the light
        [Test]
        public void TheShadowWhenAnObjectIsBetweenThePointAndTheLight()
        {
            // Given w ← default_world()
            var w = CrtFactory.DefaultWorld();
            // And p ← point(10, -10, 10)
            var p = CrtFactory.Point(10, -10, 10);
            // Then is_shadowed(w, p) is true
            Assert.IsTrue(w.IsShadowed(p));
        }

        // Scenario: There is no shadow when an object is behind the light
        [Test]
        public void ThereIsNoShadowWhenAnObjectIsBehindTheLight()
        {
            // Given w ← default_world()
            var w = CrtFactory.DefaultWorld();
            // And p ← point(-20, 20, -20)
            var p = CrtFactory.Point(-20, 20, -20);
            // Then is_shadowed(w, p) is false
            Assert.IsFalse(w.IsShadowed(p));
        }

        // Scenario: There is no shadow when an object is behind the point
        [Test]
        public void ThereIsNoShadowWhenAnObjectIsBehindThePoint()
        {
            // Given w ← default_world()
            var w = CrtFactory.DefaultWorld();
            // And p ← point(-2, 2, -2)
            var p = CrtFactory.Point(-2, 2, -2);
            // Then is_shadowed(w, p) is false
            Assert.IsFalse(w.IsShadowed(p));
        }

        // Scenario: shade_hit() is given an intersection in shadow
        [Test]
        public void ShadeHitIsGivenAnIntersectionInShadow()
        {
            // Given w ← world()
            var w = CrtFactory.World();
            // And w.light ← point_light(point(0, 0, -10), color(1, 1, 1))
            w.Add(CrtFactory.PointLight(CrtFactory.Point(0, 0, -10), CrtFactory.Color(1, 1, 1)));
            // And s1 ← sphere()
            var s1 = CrtFactory.Sphere();
            // And s1 is added to w
            w.Add(s1);
            // And s2 ← sphere() with:
            //      | transform | translation(0, 0, 10) |
            var s2 = CrtFactory.Sphere();
            s2.TransformMatrix = CrtFactory.TranslationMatrix(0, 0, 10);
            // And s2 is added to w
            w.Add(s2);
            // And r ← ray(point(0, 0, 5), vector(0, 0, 1))
            var r = CrtFactory.Ray(CrtFactory.Point(0, 0, 5), CrtFactory.Vector(0, 0, 1));
            // And i ← intersection(4, s2)
            var i = CrtFactory.Intersection(4.0, s2);
            // When comps ← prepare_computations(i, r)
            var comps = CrtFactory.Engine().PrepareComputations(i, r);
            // And c ← shade_hit(w, comps)
            var c = CrtFactory.Engine().ShadeHit(w, comps);
            // Then c = color(0.1, 0.1, 0.1)
            Assert.IsTrue(c == CrtFactory.Color(0.1, 0.1, 0.1));
        }

        // Scenario: The hit should offset the point
        [Test]
        public void TheHitShouldOffsetThePoint()
        {
            // Given r ← ray(point(0, 0, -5), vector(0, 0, 1))
            var r = CrtFactory.Ray(CrtFactory.Point(0, 0, -5), CrtFactory.Vector(0, 0, 1));
            // And shape ← sphere() with:
            //      | transform | translation(0, 0, 1) |
            var shape = CrtFactory.Sphere();
            shape.TransformMatrix = CrtFactory.TranslationMatrix(0, 0, 1);
            // And i ← intersection(5, shape)
            var i = CrtFactory.Intersection(5.0, shape);
            // When comps ← prepare_computations(i, r)
            var comps = CrtFactory.Engine().PrepareComputations(i, r);
            // Then comps.over_point.z< -EPSILON/2
            Assert.IsTrue(comps.OverPoint.Z < -CrtReal.EPSILON/2.0);
            // And comps.point.z > comps.over_point.z
            Assert.IsTrue(CrtReal.CompareTo(comps.HitPoint.Z, comps.OverPoint.Z) > 0);
        }

        #endregion

        #region Reflection

        // Scenario: The reflected color for a nonreflective material
        [Test]
        public void TheReflectedColorForANonreflectiveMaterial()
        {
            // Given w ← default_world()
            var w = CrtFactory.DefaultWorld();
            // And r ← ray(point(0, 0, 0), vector(0, 0, 1))
            var r = CrtFactory.Ray(CrtFactory.Point(0, 0, 0), CrtFactory.Vector(0, 0, 1));
            // And shape ← the second object in w
            var shape = w.Objects[1];
            // And shape.material.ambient ← 1
            shape.Material.Ambient = 1;
            // And i ← intersection(1, shape)
            var i = CrtFactory.Intersection(1.0, shape);
            // When comps ← prepare_computations(i, r)
            var comps = CrtFactory.Engine().PrepareComputations(i, r);
            // And color ← reflected_color(w, comps)
            var color = w.ReflectedColor(comps);
            // Then color = color(0, 0, 0)
            Assert.IsTrue(color == CrtColor.COLOR_BLACK);
        }

        // Scenario: The reflected color for a reflective material
        [Test]
        public void TheReflectedColorForAReflectiveMaterial()
        {
            // Given w ← default_world()
            var w = CrtFactory.DefaultWorld();
            // And shape ← plane() with:
            //      | material.reflective | 0.5 |
            //      | transform | translation(0, -1, 0) |
            var shape = CrtFactory.Plane();
            shape.Material.Reflective = 0.5;
            shape.TransformMatrix = CrtFactory.TranslationMatrix(0, -1, 0);
            // And shape is added to w
            w.Add(shape);
            // And r ← ray(point(0, 0, -3), vector(0, -√2/2, √2/2))
            var r = CrtFactory.Ray(CrtFactory.Point(0, 0, -3), CrtFactory.Vector(0, -Math.Sqrt(2.0)/2.0, Math.Sqrt(2.0) / 2.0));
            // And i ← intersection(√2, shape)
            var i = CrtFactory.Intersection(Math.Sqrt(2.0), shape);
            // When comps ← prepare_computations(i, r)
            var comps = CrtFactory.Engine().PrepareComputations(i, r);
            // And color ← reflected_color(w, comps)
            var color = w.ReflectedColor(comps);
            // Then color = color(0.19032, 0.2379, 0.14274)
            Assert.IsTrue(CrtReal.AreEquals(color.Red, 0.19032, 1e-4));
            Assert.IsTrue(CrtReal.AreEquals(color.Green, 0.2379, 1e-4));
            Assert.IsTrue(CrtReal.AreEquals(color.Blue, 0.14274, 1e-4));
        }

        // Scenario: shade_hit() with a reflective material
        [Test]
        public void ShadeHitWithAReflectiveMaterial()
        {
            // Given w ← default_world()
            var w = CrtFactory.DefaultWorld();
            // And shape ← plane() with:
            //      | material.reflective | 0.5 |
            //      | transform | translation(0, -1, 0) |
            var shape = CrtFactory.Plane();
            shape.Material.Reflective = 0.5;
            shape.TransformMatrix = CrtFactory.TranslationMatrix(0, -1, 0);
            // And shape is added to w
            w.Add(shape);
            // And r ← ray(point(0, 0, -3), vector(0, -√2/2, √2/2))
            var r = CrtFactory.Ray(CrtFactory.Point(0, 0, -3), CrtFactory.Vector(0, -Math.Sqrt(2.0) / 2.0, Math.Sqrt(2.0) / 2.0));
            // And i ← intersection(√2, shape)
            var i = CrtFactory.Intersection(Math.Sqrt(2.0), shape);
            // When comps ← prepare_computations(i, r)
            var comps = CrtFactory.Engine().PrepareComputations(i, r);
            // And color ← shade_hit(w, comps)
            var color = CrtFactory.Engine().ShadeHit(w, comps);
            // Then color = color(0.87677, 0.92436, 0.82918)
            Assert.IsTrue(CrtReal.AreEquals(color.Red, 0.87677, 1e-4));
            Assert.IsTrue(CrtReal.AreEquals(color.Green, 0.92436, 1e-4));
            Assert.IsTrue(CrtReal.AreEquals(color.Blue, 0.82918, 1e-4));
        }

        // Scenario: color_at() with mutually reflective surfaces
        [Test]
        public void ColorAtWithMutuallyReflectiveSurfaces()
        {
            // Given w ← world()
            var w = CrtFactory.World();
            // And w.light ← point_light(point(0, 0, 0), color(1, 1, 1))
            w.Add(CrtFactory.PointLight(CrtFactory.Point(0, 0, 0), CrtColor.COLOR_WHITE));
            // And lower ← plane() with:
            //      | material.reflective | 1 |
            //      | transform | translation(0, -1, 0) |
            var lower = CrtFactory.Plane();
            lower.Material.Reflective = 1;
            lower.TransformMatrix = CrtFactory.TranslationMatrix(0, -1, 0);
            // And lower is added to w
            w.Add(lower);
            // And upper ← plane() with:
            //      | material.reflective | 1 |
            //      | transform | translation(0, 1, 0) |
            var upper = CrtFactory.Plane();
            upper.Material.Reflective = 1;
            upper.TransformMatrix = CrtFactory.TranslationMatrix(0, 1, 0);
            // And upper is added to w
            w.Add(upper);
            // And r ← ray(point(0, 0, 0), vector(0, 1, 0))
            var r = CrtFactory.Ray(CrtFactory.Point(0, 0, 0), CrtFactory.Vector(0, 1, 0));
            // Then color_at(w, r) should terminate successfully
            var c = w.ColorAt(r);
            Assert.IsNotNull(c);
        }

        // Scenario: The reflected color at the maximum recursive depth
        [Test]
        public void TheReflectedColorAtTheMaximumRecursiveDepth()
        {
            // Given w ← default_world()
            var w = CrtFactory.DefaultWorld();
            // And shape ← plane() with:
            //       | material.reflective | 0.5 |
            //       | transform | translation(0, -1, 0) |
            var shape = CrtFactory.Plane();
            shape.Material.Reflective = 1;
            shape.TransformMatrix = CrtFactory.TranslationMatrix(0, -1, 0);
            // And shape is added to w
            w.Add(shape);
            // And r ← ray(point(0, 0, -3), vector(0, -√2/2, √2/2))
            var r = CrtFactory.Ray(CrtFactory.Point(0, 0, -3), CrtFactory.Vector(0, -Math.Sqrt(2.0) / 2.0, Math.Sqrt(2.0) / 2.0));
            // And i ← intersection(√2, shape)
            var i = CrtFactory.Intersection(Math.Sqrt(2.0), shape);
            // When comps ← prepare_computations(i, r)
            var comps = CrtFactory.Engine().PrepareComputations(i, r);
            // And color ← reflected_color(w, comps, 0)
            var color = w.ReflectedColor(comps, 0);
            // Then color = color(0, 0, 0)
            Assert.IsTrue(color == CrtColor.COLOR_BLACK);
        }

        #endregion

        #region Transparency and Refraction

        // Scenario: The refracted color with an opaque surface
        [Test]
        public void TheRefractedColorWithAnOpaqueSurface()
        {
            // Given w ← default_world()
            var w = CrtFactory.DefaultWorld();
            // And shape ← the first object in w
            var shape = w.Objects[0];
            // And r ← ray(point(0, 0, -5), vector(0, 0, 1))
            var r = CrtFactory.Ray(CrtFactory.Point(0, 0, -5), CrtFactory.Vector(0, 0, 1));
            // And xs ← intersections(4:shape, 6:shape)
            var xs = CrtFactory.Intersections(
                CrtFactory.Intersection(4, shape),
                CrtFactory.Intersection(6, shape)
            );
            // When comps ← prepare_computations(xs[0], r, xs)
            var comps = CrtFactory.Engine().PrepareComputations(xs[0], r, xs);
            // And c ← refracted_color(w, comps, 5)
            var c = w.RefractedColor(comps, 5);
            // Then c = color(0, 0, 0)
            Assert.IsTrue(c == CrtColor.COLOR_BLACK);
        }

        // Scenario: The refracted color at the maximum recursive depth
        [Test]
        public void TheRefractedColorAtTheMaximumRecursiveDepth()
        {
            // Given w ← default_world()
            var w = CrtFactory.DefaultWorld();
            // And shape ← the first object in w
            var shape = w.Objects[0];
            // And shape has:
            //      | material.transparency | 1.0 |
            //      | material.refractive_index | 1.5 |
            shape.Material.WithTransparency(1.0).WithRefractiveIndex(1.5);
            // And r ← ray(point(0, 0, -5), vector(0, 0, 1))
            var r = CrtFactory.Ray(CrtFactory.Point(0, 0, -5), CrtFactory.Vector(0, 0, 1));
            // And xs ← intersections(4:shape, 6:shape)
            var xs = CrtFactory.Intersections(
                CrtFactory.Intersection(4, shape),
                CrtFactory.Intersection(6, shape)
            );
            // When comps ← prepare_computations(xs[0], r, xs)
            var comps = CrtFactory.Engine().PrepareComputations(xs[0], r, xs);
            // And c ← refracted_color(w, comps, 0)
            var c = w.RefractedColor(comps, 0);
            // Then c = color(0, 0, 0)
            Assert.IsTrue(c == CrtColor.COLOR_BLACK);
        }

        // Scenario: The refracted color under total internal reflection
        [Test]
        public void TheRefractedColorUnderTotalInternalReflection()
        {
            // Given w ← default_world()
            var w = CrtFactory.DefaultWorld();
            // And shape ← the first object in w
            var shape = w.Objects[0];
            // And shape has:
            //      | material.transparency | 1.0 |
            //      | material.refractive_index | 1.5 |
            shape.Material.WithTransparency(1.0).WithRefractiveIndex(1.5);
            // And r ← ray(point(0, 0, √2/2), vector(0, 1, 0))
            var r = CrtFactory.Ray(CrtFactory.Point(0, 0, Math.Sqrt(2.0)/2.0), CrtFactory.Vector(0, 1, 0));
            // And xs ← intersections(-√2/2:shape, √2/2:shape)
            var xs = CrtFactory.Intersections(
                CrtFactory.Intersection(-Math.Sqrt(2.0) / 2.0, shape),
                CrtFactory.Intersection(Math.Sqrt(2.0) / 2.0, shape)
            );
            //    # NOTE: this time you're inside the sphere, so you need
            //    # to look at the second intersection, xs[1], not xs[0]
            // When comps ← prepare_computations(xs[1], r, xs)
            var comps = CrtFactory.Engine().PrepareComputations(xs[1], r, xs);
            // And c ← refracted_color(w, comps, 5)
            var c = w.RefractedColor(comps, 5);
            // Then c = color(0, 0, 0)
            Assert.IsTrue(c == CrtColor.COLOR_BLACK);
        }

        // Scenario: The refracted color with a refracted ray
        [Test]
        public void TheRefractedColorWithARefractedRay()
        {
            // Given w ← default_world()
            var w = CrtFactory.DefaultWorld();
            // And A ← the first object in w
            var a = w.Objects[0];
            // And A has:
            //       | material.ambient | 1.0 |
            //       | material.pattern | test_pattern() |
            a.Material.WithAmbient(1.0).WithPattern(CrtFactory.TestPattern());
            // And B ← the second object in w
            var b = w.Objects[1];
            // And B has:
            //       | material.transparency | 1.0 |
            //       | material.refractive_index | 1.5 |
            b.Material.WithTransparency(1.0).WithRefractiveIndex(1.5);
            // And r ← ray(point(0, 0, 0.1), vector(0, 1, 0))
            var r = CrtFactory.Ray(CrtFactory.Point(0, 0, 0.1), CrtFactory.Vector(0, 1, 0));
            // And xs ← intersections(-0.9899:A, -0.4899:B, 0.4899:B, 0.9899:A)
            var xs = CrtFactory.Intersections(
                CrtFactory.Intersection(-0.9899, a),
                CrtFactory.Intersection(-0.4899, b),
                CrtFactory.Intersection(0.4899, b),
                CrtFactory.Intersection(0.9899, a)
            );
            // When comps ← prepare_computations(xs[2], r, xs)
            var comps = CrtFactory.Engine().PrepareComputations(xs[2], r, xs);
            // And c ← refracted_color(w, comps, 5)
            var c = w.RefractedColor(comps, 5);
            // Then c = color(0, 0.99888, 0.04725)
            Assert.IsTrue(CrtReal.AreEquals(c.Red, 0.0, 1e-4));
            Assert.IsTrue(CrtReal.AreEquals(c.Green, 0.99888, 1e-4));
            Assert.IsTrue(CrtReal.AreEquals(c.Blue, 0.04725, 1e-4));
        }

        // Scenario: shade_hit() with a transparent material
        [Test]
        public void ShadeHitWithATransparentMaterial()
        {
            // Given w ← default_world()
            var w = CrtFactory.DefaultWorld();
            // And floor ← plane() with:
            //   | transform | translation(0, -1, 0) |
            //   | material.transparency | 0.5 |
            //   | material.refractive_index | 1.5 |
            var floor = CrtFactory.Plane();
            floor.WithTransformationMatrix(CrtFactory.TranslationMatrix(0, -1, 0))
                 .Material.WithTransparency(0.5).WithRefractiveIndex(1.5);
            // And floor is added to w
            w.Add(floor);
            // And ball ← sphere() with:
            //   | material.color | (1, 0, 0) |
            //   | material.ambient | 0.5 |
            //   | transform | translation(0, -3.5, -0.5) |
            var ball = CrtFactory.Sphere();
            ball.WithTransformationMatrix(CrtFactory.TranslationMatrix(0, -3.5, -0.5))
                .Material.WithColor(CrtColor.COLOR_RED).WithAmbient(0.5);
            // And ball is added to w
            w.Add(ball);
            // And r ← ray(point(0, 0, -3), vector(0, -√2/2, √2/2))
            var r = CrtFactory.Ray(CrtFactory.Point(0, 0, -3), CrtFactory.Vector(0, -Math.Sqrt(2.0)/2.0, Math.Sqrt(2.0) / 2.0));
            // And xs ← intersections(√2:floor)
            var xs = CrtFactory.Intersections(
                CrtFactory.Intersection(Math.Sqrt(2.0), floor)
            );
            // When comps ← prepare_computations(xs[0], r, xs)
            var comps = CrtFactory.Engine().PrepareComputations(xs[0], r, xs);
            // And color ← shade_hit(w, comps, 5)
            var c = CrtFactory.Engine().ShadeHit(w, comps, 5);
            // Then color = color(0.93642, 0.68642, 0.68642)
            Assert.IsTrue(CrtReal.AreEquals(c.Red, 0.93642, 1e-4));
            Assert.IsTrue(CrtReal.AreEquals(c.Green, 0.68642, 1e-4));
            Assert.IsTrue(CrtReal.AreEquals(c.Blue, 0.68642, 1e-4));
        }

        // Scenario: shade_hit() with a reflective, transparent material
        [Test]
        public void ShadeHitWithAReflectiveTransparentMaterial()
        {
            // Given w ← default_world()
            var w = CrtFactory.DefaultWorld();
            // And r ← ray(point(0, 0, -3), vector(0, -√2/2, √2/2))
            var r = CrtFactory.Ray(CrtFactory.Point(0, 0, -3), CrtFactory.Vector(0, -Math.Sqrt(2.0) / 2.0, Math.Sqrt(2.0) / 2.0));
            // And floor ← plane() with:
            //       | transform | translation(0, -1, 0) |
            //       | material.reflective | 0.5 |
            //       | material.transparency | 0.5 |
            //       | material.refractive_index | 1.5 |
            var floor = CrtFactory.Plane();
            floor.WithTransformationMatrix(CrtFactory.TranslationMatrix(0, -1, 0))
                .Material.WithReflective(0.5).WithTransparency(0.5).WithRefractiveIndex(1.5);
            // And floor is added to w
            w.Add(floor);
            // And ball ← sphere() with:
            //       | material.color | (1, 0, 0) |
            //       | material.ambient | 0.5 |
            //       | transform | translation(0, -3.5, -0.5) |
            var ball = CrtFactory.Sphere();
            ball.WithTransformationMatrix(CrtFactory.TranslationMatrix(0, -3.5, -0.5))
                .Material.WithColor(CrtColor.COLOR_RED).WithAmbient(0.5);
            // And ball is added to w
            w.Add(ball);
            // And xs ← intersections(√2:floor)
            var xs = CrtFactory.Intersections(
                CrtFactory.Intersection(Math.Sqrt(2.0), floor)
            );
            // When comps ← prepare_computations(xs[0], r, xs)
            var comps = CrtFactory.Engine().PrepareComputations(xs[0], r, xs);
            // And color ← shade_hit(w, comps, 5)
            var c = CrtFactory.Engine().ShadeHit(w, comps, 5);
            // Then color = color(0.93391, 0.69643, 0.69243)
            Assert.IsTrue(CrtReal.AreEquals(c.Red, 0.93391, 1e-4));
            Assert.IsTrue(CrtReal.AreEquals(c.Green, 0.69643, 1e-4));
            Assert.IsTrue(CrtReal.AreEquals(c.Blue, 0.69243, 1e-4));
        }

        #endregion
    }
}
