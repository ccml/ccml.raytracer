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
    public class CrtGroupTests
    {
        [SetUp]
        public void Setup()
        {

        }

        // Scenario: Creating a new group
        [Test]
        public void CreatingANewGroup()
        {
            // Given g ← group()
            var g = CrtFactory.ShapeFactory.Group();
            // Then g.transform = identity_matrix
            Assert.IsTrue(g.TransformMatrix == CrtFactory.TransformationFactory.IdentityMatrix(4, 4));
            // And g is empty
            Assert.IsTrue(g.IsEmpty);
        }

        // Scenario: A shape has a parent attribute
        [Test]
        public void AShapeHasAParentAttribute()
        {
            // Given s ← test_shape()
            var s = CrtFactory.TestsFactory.TestShape();
            // Then s.parent is nothing
            Assert.IsNull(s.Parent);
        }

        // Scenario: Adding a child to a group
        [Test]
        public void AddingAChildToAGroup()
        {
            // Given g ← group()
            var g = CrtFactory.ShapeFactory.Group();
            // And s ← test_shape()
            var s = CrtFactory.TestsFactory.TestShape();
            // When add_child(g, s)
            g.Add(s);
            // Then g is not empty
            Assert.IsFalse(g.IsEmpty);
            // And g includes s
            Assert.IsTrue(g.Contains(s));
            // And s.parent = g
            Assert.AreEqual(s.Parent, g);
        }

        // Scenario: Intersecting a ray with an empty group
        [Test]
        public void IntersectingARayWithAnEmptyGroup()
        {
            // Given g ← group()
            var g = CrtFactory.ShapeFactory.Group();
            // And r ← ray(point(0, 0, 0), vector(0, 0, 1))
            var r = CrtFactory.EngineFactory.Ray(
                CrtFactory.CoreFactory.Point(0, 0, 0),
                CrtFactory.CoreFactory.Vector(0, 0 , 1)
            );
            // When xs ← local_intersect(g, r)
            var xs = g.LocalIntersect(r);
            // Then xs is empty
            Assert.IsEmpty(xs);
        }

        // Scenario: Intersecting a ray with a nonempty group
        [Test]
        public void IntersectingARayWithANonEmptyGroup()
        {
            // Given g ← group()
            var g = CrtFactory.ShapeFactory.Group();
            // And s1 ← sphere()
            var s1 = CrtFactory.ShapeFactory.Sphere();
            // And s2 ← sphere()
            // And set_transform(s2, translation(0, 0, -3))
            var s2 = CrtFactory.ShapeFactory.Sphere()
                .WithTransformationMatrix(CrtFactory.TransformationFactory.TranslationMatrix(0, 0, -3)
            );
            // And s3 ← sphere()
            // And set_transform(s3, translation(5, 0, 0))
            var s3 = CrtFactory.ShapeFactory.Sphere()
                .WithTransformationMatrix(CrtFactory.TransformationFactory.TranslationMatrix(5, 0, 0)
                );
            // And add_child(g, s1)
            // And add_child(g, s2)
            // And add_child(g, s3)
            g.Add(s1, s2, s3);
            // When r ← ray(point(0, 0, -5), vector(0, 0, 1))
            var r = CrtFactory.EngineFactory.Ray(
                CrtFactory.CoreFactory.Point(0, 0, -5),
                CrtFactory.CoreFactory.Vector(0, 0, 1)
            );
            // And xs ← local_intersect(g, r)
            var xs = g.LocalIntersect(r);
            // Then xs.count = 4
            Assert.AreEqual(4, xs.Count);
            // And xs[0].object = s2
            Assert.IsTrue(xs[0].TheObject == s2);
            // And xs[1].object = s2
            Assert.IsTrue(xs[1].TheObject == s2);
            // And xs[2].object = s1
            Assert.IsTrue(xs[2].TheObject == s1);
            // And xs[3].object = s1
            Assert.IsTrue(xs[3].TheObject == s1);
        }

        // Scenario: Intersecting a transformed group
        [Test]
        public void IntersectingATransformedGroup()
        {
            // Given g ← group()
            // And set_transform(g, scaling(2, 2, 2))
            var g = (CrtGroup)CrtFactory.ShapeFactory.Group()
                .WithTransformationMatrix(CrtFactory.TransformationFactory.ScalingMatrix(2, 2, 2));
            // And s ← sphere()
            // And set_transform(s, translation(5, 0, 0))
            var s = CrtFactory.ShapeFactory.Sphere()
                .WithTransformationMatrix(CrtFactory.TransformationFactory.TranslationMatrix(5, 0, 0)
                );
            // And add_child(g, s)
            g.Add(s);
            // When r ← ray(point(10, 0, -10), vector(0, 0, 1))
            var r = CrtFactory.EngineFactory.Ray(
                CrtFactory.CoreFactory.Point(10, 0, -10),
                CrtFactory.CoreFactory.Vector(0, 0, 1)
            );
            // And xs ← intersect(g, r)
            var xs = g.Intersect(r);
            // Then xs.count = 2
            Assert.AreEqual(2, xs.Count);
        }

        // Scenario: Converting a point from world to object space
        [Test]
        public void ConvertingAPointFromWorldToObjectSpace()
        {
            // Given g1 ← group()
            // And set_transform(g1, rotation_y(π/2))
            var g1 = (CrtGroup) CrtFactory.ShapeFactory.Group()
                .WithTransformationMatrix(CrtFactory.TransformationFactory.YRotationMatrix(Math.PI / 2.0));
            // And g2 ← group()
            // And set_transform(g2, scaling(2, 2, 2))
            var g2 = (CrtGroup)CrtFactory.ShapeFactory.Group()
                .WithTransformationMatrix(CrtFactory.TransformationFactory.ScalingMatrix(2, 2, 2));
            // And add_child(g1, g2)
            g1.Add(g2);
            // And s ← sphere()
            // And set_transform(s, translation(5, 0, 0))
            var s = CrtFactory.ShapeFactory.Sphere()
                .WithTransformationMatrix(CrtFactory.TransformationFactory.TranslationMatrix(5, 0, 0));
            // And add_child(g2, s)
            g2.Add(s);
            // When p ← world_to_object(s, point(-2, 0, -10))
            var p = s.WorldToObject(CrtFactory.CoreFactory.Point(-2, 0, -10));
            // Then p = point(0, 0, -1)
            Assert.IsTrue(p == CrtFactory.CoreFactory.Point(0, 0, -1));
        }

        // Scenario: Converting a normal from object to world space
        [Test]
        public void ConvertingANormalFromObjectToWorldSpace()
        {
            // Given g1 ← group()
            // And set_transform(g1, rotation_y(π/2))
            var g1 = (CrtGroup)CrtFactory.ShapeFactory.Group()
                .WithTransformationMatrix(CrtFactory.TransformationFactory.YRotationMatrix(Math.PI / 2.0));
            // And g2 ← group()
            // And set_transform(g2, scaling(1, 2, 3))
            var g2 = (CrtGroup)CrtFactory.ShapeFactory.Group()
                .WithTransformationMatrix(CrtFactory.TransformationFactory.ScalingMatrix(1, 2, 3));
            // And add_child(g1, g2)
            g1.Add(g2);
            // And s ← sphere()
            // And set_transform(s, translation(5, 0, 0))
            var s = CrtFactory.ShapeFactory.Sphere()
                .WithTransformationMatrix(CrtFactory.TransformationFactory.TranslationMatrix(5, 0, 0));
            // And add_child(g2, s)
            g2.Add(s);
            // When n ← normal_to_world(s, vector(√3/3, √3/3, √3/3))
            var n = s.NormalToWorld(
                CrtFactory.CoreFactory.Vector(
                    Math.Sqrt(3.0) / 3.0, 
                    Math.Sqrt(3.0) / 3.0,
                    Math.Sqrt(3.0) / 3.0)
                );
            // Then n = vector(0.2857, 0.4286, -0.8571)
            Assert.IsTrue(CrtReal.AreEquals(n.X, 0.2857, 1e-4));
            Assert.IsTrue(CrtReal.AreEquals(n.Y, 0.4286, 1e-4));
            Assert.IsTrue(CrtReal.AreEquals(n.Z, -0.8571, 1e-4));
        }

        // Scenario: Finding the normal on a child object
        [Test]
        public void FindingTheNormalOnAChildObject()
        {
            // Given g1 ← group()
            // And set_transform(g1, rotation_y(π/2))
            var g1 = (CrtGroup)CrtFactory.ShapeFactory.Group()
                .WithTransformationMatrix(CrtFactory.TransformationFactory.YRotationMatrix(Math.PI / 2.0));
            // And g2 ← group()
            // And set_transform(g2, scaling(1, 2, 3))
            var g2 = (CrtGroup)CrtFactory.ShapeFactory.Group()
                .WithTransformationMatrix(CrtFactory.TransformationFactory.ScalingMatrix(1, 2, 3));
            // And add_child(g1, g2)
            g1.Add(g2);
            // And s ← sphere()
            // And set_transform(s, translation(5, 0, 0))
            var s = CrtFactory.ShapeFactory.Sphere()
                .WithTransformationMatrix(CrtFactory.TransformationFactory.TranslationMatrix(5, 0, 0));
            // And add_child(g2, s)
            g2.Add(s);
            // When n ← normal_at(s, point(1.7321, 1.1547, -5.5774))
            var n = s.NormalAt(CrtFactory.CoreFactory.Point(1.7321, 1.1547, -5.5774));
            // Then n = vector(0.2857, 0.4286, -0.8571)
            Assert.IsTrue(CrtReal.AreEquals(n.X, 0.2857, 1e-4));
            Assert.IsTrue(CrtReal.AreEquals(n.Y, 0.4286, 1e-4));
            Assert.IsTrue(CrtReal.AreEquals(n.Z, -0.8571, 1e-4));
        }

    }
}
