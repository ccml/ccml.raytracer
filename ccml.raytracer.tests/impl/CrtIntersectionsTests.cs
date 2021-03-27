using System;
using ccml.raytracer.Core;
using NUnit.Framework;

namespace ccml.raytracer.tests.impl
{
    public class CrtIntersectionsTests
    {
        [SetUp]
        public void Setup()
        {

        }

        // Scenario: An intersection encapsulates t and object
        [Test]
        public void AnIntersectionEncapsulatesTAndObject()
        {
            // Given s ← sphere()
            var s = CrtFactory.ShapeFactory.Sphere();
            // When i ← intersection(3.5, s)
            var i = CrtFactory.EngineFactory.Intersection(3.5, s);
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
            var r = CrtFactory.EngineFactory.Ray(CrtFactory.CoreFactory.Point(0, 0, -5), CrtFactory.CoreFactory.Vector(0, 0, 1));
            // And s ← sphere()
            var s = CrtFactory.ShapeFactory.Sphere();
            // When xs ← intersect(s, r)
            var xs = s.Intersect(r);
            // Then xs.count = 2
            Assert.AreEqual(2, xs.Count);
            // And xs[0].object = s
            Assert.AreSame(xs[0].TheObject, s);
            // And xs[1].object = s
            Assert.AreSame(xs[1].TheObject, s);
        }

        // Scenario: The hit, when all intersections have positive t
        [Test]
        public void TheHitWhenAllIntersectionsHavePositiveT()
        {
            // Given s ← sphere()
            var s = CrtFactory.ShapeFactory.Sphere();
            // And i1 ← intersection(1, s)
            var i1 = CrtFactory.EngineFactory.Intersection(1, s);
            // And i2 ← intersection(2, s)
            var i2 = CrtFactory.EngineFactory.Intersection(2, s);
            // And xs ← intersections(i2, i1)
            var xs = CrtFactory.EngineFactory.Intersections(i1, i2);
            // When i ← hit(xs)
            var i = CrtFactory.EngineFactory.Engine().Hit(xs);
            // Then i = i1
            Assert.AreSame(i, i1);
        }

        // Scenario: The hit, when some intersections have negative t
        [Test]
        public void TheHitWhenSomeIntersectionsHaveNegativeT()
        {
            // Given s ← sphere()
            var s = CrtFactory.ShapeFactory.Sphere();
            // And i1 ← intersection(-1, s)
            var i1 = CrtFactory.EngineFactory.Intersection(-1, s);
            // And i2 ← intersection(1, s)
            var i2 = CrtFactory.EngineFactory.Intersection(1, s);
            // And xs ← intersections(i2, i1)
            var xs = CrtFactory.EngineFactory.Intersections(i1, i2);
            // When i ← hit(xs)
            var i = CrtFactory.EngineFactory.Engine().Hit(xs);
            // Then i = i2
            Assert.AreSame(i, i2);
        }

        // Scenario: The hit, when all intersections have negative t
        [Test]
        public void TheHitWhenAllIntersectionsHaveNegativeT()
        {
            // Given s ← sphere()
            var s = CrtFactory.ShapeFactory.Sphere();
            // And i1 ← intersection(-2, s)
            var i1 = CrtFactory.EngineFactory.Intersection(-2, s);
            // And i2 ← intersection(-1, s)
            var i2 = CrtFactory.EngineFactory.Intersection(-1, s);
            // And xs ← intersections(i2, i1)
            var xs = CrtFactory.EngineFactory.Intersections(i1, i2);
            // When i ← hit(xs)
            var i = CrtFactory.EngineFactory.Engine().Hit(xs);
            // Then i is nothing
            Assert.IsNull(i);
        }

        // Scenario: The hit is always the lowest nonnegative intersection
        [Test]
        public void TheHitIsAlwaysTheLowestNonnegativeIntersection()
        {
            // Given s ← sphere()
            var s = CrtFactory.ShapeFactory.Sphere();
            // And i1 ← intersection(5, s)
            var i1 = CrtFactory.EngineFactory.Intersection(5, s);
            // And i2 ← intersection(7, s)
            var i2 = CrtFactory.EngineFactory.Intersection(7, s);
            // And i3 ← intersection(-3, s)
            var i3 = CrtFactory.EngineFactory.Intersection(-3, s);
            // And i4 ← intersection(2, s)
            var i4 = CrtFactory.EngineFactory.Intersection(2, s);
            // And xs ← intersections(i1, i2, i3, i4)
            var xs = CrtFactory.EngineFactory.Intersections(i1, i2, i3, i4);
            // When i ← hit(xs)
            var i = CrtFactory.EngineFactory.Engine().Hit(xs);
            // Then i = i4
            Assert.AreSame(i, i4);
        }

        // Scenario: Intersecting a scaled sphere with a ray
        [Test]
        public void IntersectingAScaledSphereWithARay()
        {
            // Given r ← ray(point(0, 0, -5), vector(0, 0, 1))
            var r = CrtFactory.EngineFactory.Ray(CrtFactory.CoreFactory.Point(0, 0, -5), CrtFactory.CoreFactory.Vector(0, 0, 1));
            // And s ← sphere()
            var s = CrtFactory.ShapeFactory.Sphere();
            // When set_transform(s, scaling(2, 2, 2))
            s.TransformMatrix = CrtFactory.TransformationFactory.ScalingMatrix(2,2,2);
            // And xs ← intersect(s, r)
            var xs = s.Intersect(r);
            // Then xs.count = 2
            Assert.AreEqual(2, xs.Count);
            // And xs[0].t = 3
            Assert.IsTrue(CrtReal.AreEquals(xs[0].T, 3));
            // And xs[1].t = 7
            Assert.IsTrue(CrtReal.AreEquals(xs[1].T, 7));
        }

        // Scenario: Intersecting a translated sphere with a ray
        [Test]
        public void IntersectingATranslatedSphereWithARay()
        {
            // Given r ← ray(point(0, 0, -5), vector(0, 0, 1))
            var r = CrtFactory.EngineFactory.Ray(CrtFactory.CoreFactory.Point(0, 0, -5), CrtFactory.CoreFactory.Vector(0, 0, 1));
            // And s ← sphere()
            var s = CrtFactory.ShapeFactory.Sphere();
            // When set_transform(s, translation(5, 0, 0))
            s.TransformMatrix = CrtFactory.TransformationFactory.TranslationMatrix(5,0,0);
            // And xs ← intersect(s, r)
            var xs = s.Intersect(r);
            // Then xs.count = 0
            Assert.AreEqual(0, xs.Count);
        }

        #region PreComputeHits

        // Scenario: Precomputing the state of an intersection
        [Test]
        public void PrecomputingTheStateOfAnIntersection()
        {
            // Given r ← ray(point(0, 0, -5), vector(0, 0, 1))
            var r = CrtFactory.EngineFactory.Ray(CrtFactory.CoreFactory.Point(0, 0, -5), CrtFactory.CoreFactory.Vector(0, 0, 1));
            // And shape ← sphere()
            var shape = CrtFactory.ShapeFactory.Sphere();
            // And i ← intersection(4, shape)
            var i = CrtFactory.EngineFactory.Intersection(4.0, shape);
            // When comps ← prepare_computations(i, r)
            var comps = CrtFactory.EngineFactory.Engine().PrepareComputations(i, r);
            // Then comps.t = i.t
            Assert.IsTrue(CrtReal.AreEquals(comps.T, i.T));
            // And comps.object = i.object
            Assert.AreSame(comps.TheObject, i.TheObject);
            // And comps.point = point(0, 0, -1)
            Assert.IsTrue(comps.HitPoint == CrtFactory.CoreFactory.Point(0, 0, -1));
            // And comps.eyev = vector(0, 0, -1)
            Assert.IsTrue(comps.EyeVector == CrtFactory.CoreFactory.Vector(0, 0, -1));
            // And comps.normalv = vector(0, 0, -1)
            Assert.IsTrue(comps.NormalVector == CrtFactory.CoreFactory.Vector(0, 0, -1));
        }

        // Scenario: The hit, when an intersection occurs on the outside
        [Test]
        public void TheHitWhenAnIntersectionOccursOnTheOutside()
        {
            // Given r ← ray(point(0, 0, -5), vector(0, 0, 1))
            var r = CrtFactory.EngineFactory.Ray(CrtFactory.CoreFactory.Point(0, 0, -5), CrtFactory.CoreFactory.Vector(0, 0, 1));
            // And shape ← sphere()
            var shape = CrtFactory.ShapeFactory.Sphere();
            // And i ← intersection(4, shape)
            var i = CrtFactory.EngineFactory.Intersection(4.0, shape);
            // When comps ← prepare_computations(i, r)
            var comps = CrtFactory.EngineFactory.Engine().PrepareComputations(i, r);
            // Then comps.inside = false
            Assert.IsFalse(comps.IsInside);
        }

        // Scenario: The hit, when an intersection occurs on the inside
        [Test]
        public void TheHitWhenAnIntersectionOccursOnTheInside()
        {
            // Given r ← ray(point(0, 0, 0), vector(0, 0, 1))
            var r = CrtFactory.EngineFactory.Ray(CrtFactory.CoreFactory.Point(0, 0, 0), CrtFactory.CoreFactory.Vector(0, 0, 1));
            // And shape ← sphere()
            var shape = CrtFactory.ShapeFactory.Sphere();
            // And i ← intersection(1, shape)
            var i = CrtFactory.EngineFactory.Intersection(1.0, shape);
            // When comps ← prepare_computations(i, r)
            var comps = CrtFactory.EngineFactory.Engine().PrepareComputations(i, r);
            // Then comps.point = point(0, 0, 1)
            Assert.IsTrue(comps.HitPoint == CrtFactory.CoreFactory.Point(0, 0, 1));
            // And comps.eyev = vector(0, 0, -1)
            Assert.IsTrue(comps.EyeVector == CrtFactory.CoreFactory.Vector(0, 0, -1));
            // And comps.inside = true
            Assert.IsTrue(comps.IsInside);
            // # normal would have been (0, 0, 1), but is inverted!
            // And comps.normalv = vector(0, 0, -1)
            Assert.IsTrue(comps.NormalVector == CrtFactory.CoreFactory.Vector(0, 0, -1));
        }

        #endregion

        #region Transparency and refraction

        // Scenario Outline: Finding n1 and n2 at various intersections
        [Test]
        public void FindingN1AndN2AtVariousIntersections()
        {
            // Given A ← glass_sphere() with:
            //    | transform | scaling(2, 2, 2) |
            //    | material.refractive_index | 1.5 |
            var a =
                CrtFactory
                    .ShapeFactory
                    .Sphere()
                    .WithMaterial(CrtFactory.MaterialFactory.Glass.WithRefractiveIndex(1.5))
                    .WithTransformationMatrix(CrtFactory.TransformationFactory.ScalingMatrix(2, 2, 2));
            // And B ← glass_sphere() with:
            //    | transform | translation(0, 0, -0.25) |
            //    | material.refractive_index | 2.0 |
            var b =
                CrtFactory
                    .ShapeFactory
                    .Sphere()
                    .WithMaterial(CrtFactory.MaterialFactory.Glass.WithRefractiveIndex(2.0))
                    .WithTransformationMatrix(CrtFactory.TransformationFactory.TranslationMatrix(0, 0, -0.25));
            // And C ← glass_sphere() with:
            //    | transform | translation(0, 0, 0.25) |
            //    | material.refractive_index | 2.5 |
            var c =
                CrtFactory
                    .ShapeFactory
                    .Sphere()
                    .WithMaterial(CrtFactory.MaterialFactory.Glass.WithRefractiveIndex(2.5))
                    .WithTransformationMatrix(CrtFactory.TransformationFactory.TranslationMatrix(0, 0, 0.25));
            // And r ← ray(point(0, 0, -4), vector(0, 0, 1))
            var r = CrtFactory.EngineFactory.Ray(CrtFactory.CoreFactory.Point(0, 0, -4), CrtFactory.CoreFactory.Vector(0, 0, 1));
            // And xs ← intersections(2:A, 2.75:B, 3.25:C, 4.75:B, 5.25:C, 6:A)
            var xs = CrtFactory.EngineFactory.Intersections(
                CrtFactory.EngineFactory.Intersection(2, a),
                CrtFactory.EngineFactory.Intersection(2.75, b),
                CrtFactory.EngineFactory.Intersection(3.25, c),
                CrtFactory.EngineFactory.Intersection(4.75, b),
                CrtFactory.EngineFactory.Intersection(5.25, c),
                CrtFactory.EngineFactory.Intersection(6, a)
            );
            double[] expectedN1 = { 1.0, 1.5, 2.0, 2.5, 2.5, 1.5 };
            double[] expectedN2 = { 1.5, 2.0, 2.5, 2.5, 1.5, 1.0 };
            for (int i = 0; i < 6; i++)
            {
                // When comps ← prepare_computations(xs[< index >], r, xs)
                var comps = CrtFactory.EngineFactory.Engine().PrepareComputations(xs[i], r, xs);
                // Then comps.n1 = <n1>
                // And comps.n2 = < n2 >
                //    (with)
                //    | index | n1 | n2 |
                //    | 0 | 1.0 | 1.5 |
                //    | 1 | 1.5 | 2.0 |
                //    | 2 | 2.0 | 2.5 |
                //    | 3 | 2.5 | 2.5 |
                //    | 4 | 2.5 | 1.5 |
                //    | 5 | 1.5 | 1.0 |
                Assert.IsTrue(CrtReal.AreEquals(comps.N1, expectedN1[i]));
                Assert.IsTrue(CrtReal.AreEquals(comps.N2, expectedN2[i]));
            }
        }

        // Scenario: The under point is offset below the surface
        [Test]
        public void TheUnderPointIsOffsetBelowTheSurface()
        {
            // Given r ← ray(point(0, 0, -5), vector(0, 0, 1))
            var r = CrtFactory.EngineFactory.Ray(CrtFactory.CoreFactory.Point(0, 0, -5), CrtFactory.CoreFactory.Vector(0, 0, 1));
            // And shape ← glass_sphere() with:
            //   | transform | translation(0, 0, 1) |
            var shape =
                CrtFactory
                    .ShapeFactory
                    .Sphere()
                    .WithMaterial(CrtFactory.MaterialFactory.Glass)
                    .WithTransformationMatrix(CrtFactory.TransformationFactory.TranslationMatrix(0, 0, 1));
            // And i ← intersection(5, shape)
            var i = CrtFactory.EngineFactory.Intersection(5, shape);
            // And xs ← intersections(i)
            var xs = CrtFactory.EngineFactory.Intersections(i);
            // When comps ← prepare_computations(i, r, xs)
            var comps = CrtFactory.EngineFactory.Engine().PrepareComputations(i, r, xs);
            // Then comps.under_point.z > EPSILON/2
            Assert.IsTrue(comps.UnderPoint.Z > CrtReal.EPSILON / 2.0);
            // And comps.point.z<comps.under_point.z
            Assert.IsTrue(CrtReal.CompareTo(comps.HitPoint.Z, comps.UnderPoint.Z) < 0);
        }

        // Scenario: The Schlick approximation under total internal reflection
        [Test]
        public void TheSchlickApproximationUnderTotalInternalReflection()
        {
            // Given shape ← glass_sphere()
            var shape = CrtFactory.ShapeFactory.Sphere().WithMaterial(CrtFactory.MaterialFactory.Glass);
            // And r ← ray(point(0, 0, √2/2), vector(0, 1, 0))
            var r = CrtFactory.EngineFactory.Ray(CrtFactory.CoreFactory.Point(0, 0, Math.Sqrt(2.0)/2.0), CrtFactory.CoreFactory.Vector(0, 1, 0));
            // And xs ← intersections(-√2/2:shape, √2/2:shape)
            var xs = CrtFactory.EngineFactory.Intersections(
                CrtFactory.EngineFactory.Intersection(-Math.Sqrt(2.0) / 2.0, shape),
                CrtFactory.EngineFactory.Intersection(Math.Sqrt(2.0) / 2.0, shape)
            );
            // When comps ← prepare_computations(xs[1], r, xs)
            var comps = CrtFactory.EngineFactory.Engine().PrepareComputations(xs[1], r, xs);
            // And reflectance ← schlick(comps)
            var reflectance = CrtFactory.EngineFactory.Engine().Schlick(comps);
            // Then reflectance = 1.0
            Assert.IsTrue(CrtReal.AreEquals(reflectance, 1.0));
        }

        // Scenario: The Schlick approximation with a perpendicular viewing angle
        [Test]
        public void TheSchlickApproximationWithAPerpendicularViewingAngle()
        {
            // Given shape ← glass_sphere()
            var shape = CrtFactory.ShapeFactory.Sphere().WithMaterial(CrtFactory.MaterialFactory.Glass);
            // And r ← ray(point(0, 0, 0), vector(0, 1, 0))
            var r = CrtFactory.EngineFactory.Ray(CrtFactory.CoreFactory.Point(0, 0, 0), CrtFactory.CoreFactory.Vector(0, 1, 0));
            // And xs ← intersections(-1:shape, 1:shape)
            var xs = CrtFactory.EngineFactory.Intersections(
                CrtFactory.EngineFactory.Intersection(-1, shape),
                CrtFactory.EngineFactory.Intersection(1, shape)
            );
            // When comps ← prepare_computations(xs[1], r, xs)
            var comps = CrtFactory.EngineFactory.Engine().PrepareComputations(xs[1], r, xs);
            // And reflectance ← schlick(comps)
            var reflectance = CrtFactory.EngineFactory.Engine().Schlick(comps);
            // Then reflectance = 0.04
            Assert.IsTrue(CrtReal.AreEquals(reflectance, 0.04));
        }

        // Scenario: The Schlick approximation with small angle and n2 > n1
        [Test]
        public void TheSchlickApproximationWithSmallAngleAndN2GreaterThanN1()
        {
            // Given shape ← glass_sphere()
            var shape = CrtFactory.ShapeFactory.Sphere().WithMaterial(CrtFactory.MaterialFactory.Glass);
            // And r ← ray(point(0, 0.99, -2), vector(0, 0, 1))
            var r = CrtFactory.EngineFactory.Ray(CrtFactory.CoreFactory.Point(0, 0.99, -2), CrtFactory.CoreFactory.Vector(0, 0, 1));
            // And xs ← intersections(1.8589:shape)
            var xs = CrtFactory.EngineFactory.Intersections(
                CrtFactory.EngineFactory.Intersection(1.8589, shape)
            );
            // When comps ← prepare_computations(xs[0], r, xs)
            var comps = CrtFactory.EngineFactory.Engine().PrepareComputations(xs[0], r, xs);
            // And reflectance ← schlick(comps)
            var reflectance = CrtFactory.EngineFactory.Engine().Schlick(comps);
            // Then reflectance = 0.48873
            Assert.IsTrue(CrtReal.AreEquals(reflectance, 0.48873));
        }


        #endregion
    }
}
