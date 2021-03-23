using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ccml.raytracer.engine.core;
using ccml.raytracer.engine.core.Materials.Patterns;
using NUnit.Framework;

namespace ccml.raytracer.tests.math.core
{
    public class CrtPatternsTests
    {
        // Background:
        // Given black ← color(0, 0, 0)
        private CrtColor _black = CrtFactory.Color(0, 0, 0);
        // And white ← color(1, 1, 1)
        private CrtColor _white = CrtFactory.Color(1, 1, 1);

        [SetUp]
        public void Setup()
        {

        }

        // Scenario: Creating a stripe pattern
        [Test]
        public void CreatingAStripePattern()
        {
            // Given pattern ← stripe_pattern(white, black)
            var pattern = CrtFactory.StripePattern(_white, _black);
            // Then pattern.a = white
            Assert.IsTrue(((CrtStripedPattern)pattern).ColorA == _white);
            // And pattern.b = black
            Assert.IsTrue(((CrtStripedPattern)pattern).ColorB == _black);
        }

        // Scenario: A stripe pattern is constant in y
        [Test]
        public void AStripePatternIsConstantInY()
        {
            // Given pattern ← stripe_pattern(white, black)
            var pattern = CrtFactory.StripePattern(_white, _black);
            // Then stripe_at(pattern, point(0, 0, 0)) = white
            Assert.IsTrue(pattern.PatternAt(CrtFactory.Point(0, 0, 0)) == _white);
            // And stripe_at(pattern, point(0, 1, 0)) = white
            Assert.IsTrue(pattern.PatternAt(CrtFactory.Point(0, 1, 0)) == _white);
            // And stripe_at(pattern, point(0, 2, 0)) = white
            Assert.IsTrue(pattern.PatternAt(CrtFactory.Point(0, 2, 0)) == _white);
        }

        // Scenario: A stripe pattern is constant in z
        [Test]
        public void AStripePatternIsConstantInZ()
        {
            // Given pattern ← stripe_pattern(white, black)
            var pattern = CrtFactory.StripePattern(_white, _black);
            // Then stripe_at(pattern, point(0, 0, 0)) = white
            Assert.IsTrue(pattern.PatternAt(CrtFactory.Point(0, 0, 0)) == _white);
            // And stripe_at(pattern, point(0, 0, 1)) = white
            Assert.IsTrue(pattern.PatternAt(CrtFactory.Point(0, 0, 1)) == _white);
            // And stripe_at(pattern, point(0, 0, 2)) = white
            Assert.IsTrue(pattern.PatternAt(CrtFactory.Point(0, 0, 2)) == _white);
        }

        // Scenario: A stripe pattern alternates in x
        [Test]
        public void AStripePatternAlternatesInX()
        {
            // Given pattern ← stripe_pattern(white, black)
            var pattern = CrtFactory.StripePattern(_white, _black);
            // Then stripe_at(pattern, point(0, 0, 0)) = white
            Assert.IsTrue(pattern.PatternAt(CrtFactory.Point(0, 0, 0)) == _white);
            // And stripe_at(pattern, point(0.9, 0, 0)) = white
            Assert.IsTrue(pattern.PatternAt(CrtFactory.Point(0.9, 0, 0)) == _white);
            // And stripe_at(pattern, point(1, 0, 0)) = black
            Assert.IsTrue(pattern.PatternAt(CrtFactory.Point(1, 0, 0)) == _black);
            // And stripe_at(pattern, point(-0.1, 0, 0)) = black
            Assert.IsTrue(pattern.PatternAt(CrtFactory.Point(-0.1, 0, 0)) == _black);
            // And stripe_at(pattern, point(-1, 0, 0)) = black
            Assert.IsTrue(pattern.PatternAt(CrtFactory.Point(-1, 0, 0)) == _black);
            // And stripe_at(pattern, point(-1.1, 0, 0)) = white
            Assert.IsTrue(pattern.PatternAt(CrtFactory.Point(-1.1, 0, 0)) == _white);
        }

        // Scenario: Lighting with a pattern applied
        [Test]
        public void LightingWithAPatternApplied()
        {
            var shape = CrtFactory.Sphere();
            var m = CrtFactory.MaterialFactory.DefaultMaterial;
            shape.Material = m;
            // Given m.pattern ← stripe_pattern(color(1, 1, 1), color(0, 0, 0))
            m.Pattern = CrtFactory.StripePattern(_white, _black);
            // And m.ambient ← 1
            m.Ambient = 1;
            // And m.diffuse ← 0
            m.Diffuse = 0;
            // And m.specular ← 0
            m.Specular = 0;
            // And eyev ← vector(0, 0, -1)
            var eyev = CrtFactory.Vector(0, 0, -1);
            // And normalv ← vector(0, 0, -1)
            var normalv = CrtFactory.Vector(0, 0, -1);
            // And light ← point_light(point(0, 0, -10), color(1, 1, 1))
            var light = CrtFactory.PointLight(CrtFactory.Point(0, 0, -10), CrtFactory.Color(1, 1, 1));
            // When c1 ← lighting(m, light, point(0.9, 0, 0), eyev, normalv, false)
            var c1 = CrtFactory.Engine().Lighting(m, shape, light, CrtFactory.Point(0.9, 0, 0), eyev, normalv, false);
            // And c2 ← lighting(m, light, point(1.1, 0, 0), eyev, normalv, false)
            var c2 = CrtFactory.Engine().Lighting(m, shape, light, CrtFactory.Point(1.1, 0, 0), eyev, normalv, false);
            // Then c1 = color(1, 1, 1)
            Assert.IsTrue(c1 == _white);
            // And c2 = color(0, 0, 0)
            Assert.IsTrue(c2 == _black);
        }

        #region Pattern transformation

        // Scenario: Stripes with an object transformation
        [Test]
        public void StripesWithAnObjectTransformation()
        {
            // Given object ← sphere()
            var s = CrtFactory.Sphere();
            // And set_transform(object, scaling(2, 2, 2))
            s.TransformMatrix = CrtFactory.ScalingMatrix(2, 2, 2);
            // And pattern ← stripe_pattern(white, black)
            var pattern = CrtFactory.StripePattern(_white, _black);
            // When c ← stripe_at_object(pattern, object, point(1.5, 0, 0))
            var c = pattern.PatternAt(s, CrtFactory.Point(1.5, 0, 0));
            // Then c = white
            Assert.IsTrue(c == _white);
        }

        // Scenario: Stripes with a pattern transformation
        [Test]
        public void StripesWithAPatternTransformation()
        {
            // Given object ← sphere()
            var s = CrtFactory.Sphere();
            // And pattern ← stripe_pattern(white, black)
            var pattern = CrtFactory.StripePattern(_white, _black);
            // And set_pattern_transform(pattern, scaling(2, 2, 2))
            pattern.TransformMatrix = CrtFactory.ScalingMatrix(2, 2, 2);
            // When c ← stripe_at_object(pattern, object, point(1.5, 0, 0))
            var c = pattern.PatternAt(s, CrtFactory.Point(1.5, 0, 0));
            // Then c = white
            Assert.IsTrue(c == _white);
        }

        // Scenario: Stripes with both an object and a pattern transformation
        [Test]
        public void StripesWithBothAnObjectAndAPatternTransformation()
        {
            // Given object ← sphere()
            var s = CrtFactory.Sphere();
            // And set_transform(object, scaling(2, 2, 2))
            s.TransformMatrix = CrtFactory.ScalingMatrix(2, 2, 2);
            // And pattern ← stripe_pattern(white, black)
            var pattern = CrtFactory.StripePattern(_white, _black);
            // And set_pattern_transform(pattern, translation(0.5, 0, 0))
            pattern.TransformMatrix = CrtFactory.TranslationMatrix(0.5, 0, 0);
            // When c ← stripe_at_object(pattern, object, point(2.5, 0, 0))
            var c = pattern.PatternAt(s, CrtFactory.Point(2.5, 0, 0));
            // Then c = white
            Assert.IsTrue(c == _white);
        }

        #endregion

        #region TestShape

        // Scenario: The default pattern transformation
        [Test]
        public void TheDefaultPatternTransformation_TestShape()
        {
            // Given pattern ← test_pattern()
            var pattern = CrtFactory.TestPattern();
            // Then pattern.transform = identity_matrix
            Assert.IsTrue(pattern.TransformMatrix == CrtFactory.IdentityMatrix(4, 4));
        }

        // Scenario: Assigning a transformation
        [Test]
        public void AssigningATransformation_TestShape()
        {
            // Given pattern ← test_pattern()
            var pattern = CrtFactory.TestPattern();
            // When set_pattern_transform(pattern, translation(1, 2, 3))
            pattern.TransformMatrix = CrtFactory.TranslationMatrix(1, 2, 3);
            // Then pattern.transform = translation(1, 2, 3)
            Assert.IsTrue(pattern.TransformMatrix == CrtFactory.TranslationMatrix(1, 2, 3));
        }

        // Scenario: A pattern with an object transformation
        [Test]
        public void APatternWithAnObjectTransformation_TestShape()
        {
            // Given shape ← sphere()
            var shape = CrtFactory.Sphere();
            // And set_transform(shape, scaling(2, 2, 2))
            shape.TransformMatrix = CrtFactory.ScalingMatrix(2, 2, 2);
            // And pattern ← test_pattern()
            var pattern = CrtFactory.TestPattern();
            // When c ← pattern_at_shape(pattern, shape, point(2, 3, 4))
            var c = pattern.PatternAt(shape, CrtFactory.Point(2, 3, 4));
            // Then c = color(1, 1.5, 2)
            Assert.IsTrue(c == CrtFactory.Color(1,1.5,2));
        }

        // Scenario: A pattern with a pattern transformation
        [Test]
        public void APatternWithAPatternTransformation_TestShape()
        {
            // Given shape ← sphere()
            var shape = CrtFactory.Sphere();
            // And pattern ← test_pattern()
            var pattern = CrtFactory.TestPattern();
            // And set_pattern_transform(pattern, scaling(2, 2, 2))
            pattern.TransformMatrix = CrtFactory.ScalingMatrix(2, 2, 2);
            // When c ← pattern_at_shape(pattern, shape, point(2, 3, 4))
            var c = pattern.PatternAt(shape, CrtFactory.Point(2, 3, 4));
            // Then c = color(1, 1.5, 2)
            Assert.IsTrue(c == CrtFactory.Color(1, 1.5, 2));
        }

        // Scenario: A pattern with both an object and a pattern transformation
        [Test]
        public void APatternWithBothAnObjectAndAPatternTransformation_TestShape()
        {
            // Given shape ← sphere()
            var shape = CrtFactory.Sphere();
            // And set_transform(shape, scaling(2, 2, 2))
            shape.TransformMatrix = CrtFactory.ScalingMatrix(2, 2, 2);
            // And pattern ← test_pattern()
            var pattern = CrtFactory.TestPattern();
            // And set_pattern_transform(pattern, translation(0.5, 1, 1.5))
            pattern.TransformMatrix = CrtFactory.TranslationMatrix(0.5, 1, 1.5);
            // When c ← pattern_at_shape(pattern, shape, point(2.5, 3, 3.5))
            var c = pattern.PatternAt(shape, CrtFactory.Point(2.5, 3, 3.5));
            // Then c = color(0.75, 0.5, 0.25)
            Assert.IsTrue(c == CrtFactory.Color(0.75, 0.5, 0.25));
        }

        #endregion

        #region Gradient pattern

        // Scenario: A gradient linearly interpolates between colors
        [Test]
        public void AGradientLinearlyInterpolatesBetweenColors()
        {
            // Given pattern ← gradient_pattern(white, black)
            var pattern = CrtFactory.GradientPattern(_white, _black);
            // Then pattern_at(pattern, point(0, 0, 0)) = white
            Assert.IsTrue(pattern.PatternAt(CrtFactory.Point(0,0,0)) == _white);
            // And pattern_at(pattern, point(0.25, 0, 0)) = color(0.75, 0.75, 0.75)
            Assert.IsTrue(pattern.PatternAt(CrtFactory.Point(0.25, 0, 0)) == CrtFactory.Color(0.75, 0.75, 0.75));
            // And pattern_at(pattern, point(0.5, 0, 0)) = color(0.5, 0.5, 0.5)
            Assert.IsTrue(pattern.PatternAt(CrtFactory.Point(0.5, 0, 0)) == CrtFactory.Color(0.5, 0.5, 0.5));
            // And pattern_at(pattern, point(0.75, 0, 0)) = color(0.25, 0.25, 0.25)
            Assert.IsTrue(pattern.PatternAt(CrtFactory.Point(0.75, 0, 0)) == CrtFactory.Color(0.25, 0.25, 0.25));
        }

        #endregion

        #region Ring pattern

        // Scenario: A ring should extend in both x and z
        [Test]
        public void ARingShouldExtendInBothXAndZ()
        {
            // Given pattern ← ring_pattern(white, black)
            var pattern = CrtFactory.RingPattern(_white, _black);
            // Then pattern_at(pattern, point(0, 0, 0)) = white
            Assert.IsTrue(pattern.PatternAt(CrtFactory.Point(0, 0, 0)) == _white);
            // And pattern_at(pattern, point(1, 0, 0)) = black
            Assert.IsTrue(pattern.PatternAt(CrtFactory.Point(1, 0, 0)) == _black);
            // And pattern_at(pattern, point(0, 0, 1)) = black
            Assert.IsTrue(pattern.PatternAt(CrtFactory.Point(0, 0, 1)) == _black);
            // # 0.708 = just slightly more than √2/2
            // And pattern_at(pattern, point(0.708, 0, 0.708)) = black
            Assert.IsTrue(pattern.PatternAt(CrtFactory.Point(0.708, 0, 0.708)) == _black);
        }

        #endregion

        #region 3D Checker Pattern

        // Scenario: Checkers should repeat in x
        [Test]
        public void CheckersShouldRepeatInX()
        {
            // Given pattern ← checkers_pattern(white, black)
            var pattern = CrtFactory.Checker3DPattern(_white, _black);
            // Then pattern_at(pattern, point(0, 0, 0)) = white
            Assert.IsTrue(pattern.PatternAt(CrtFactory.Point(0, 0, 0)) == _white);
            // And pattern_at(pattern, point(0.99, 0, 0)) = white
            Assert.IsTrue(pattern.PatternAt(CrtFactory.Point(0.99, 0, 0)) == _white);
            // And pattern_at(pattern, point(1.01, 0, 0)) = black
            Assert.IsTrue(pattern.PatternAt(CrtFactory.Point(1.01, 0, 0)) == _black);
        }

        // Scenario: Checkers should repeat in y
        [Test]
        public void CheckersShouldRepeatInY()
        {
            // Given pattern ← checkers_pattern(white, black)
            var pattern = CrtFactory.Checker3DPattern(_white, _black);
            // Then pattern_at(pattern, point(0, 0, 0)) = white
            Assert.IsTrue(pattern.PatternAt(CrtFactory.Point(0, 0, 0)) == _white);
            // And pattern_at(pattern, point(0, 0.99, 0)) = white
            Assert.IsTrue(pattern.PatternAt(CrtFactory.Point(0, 0.99, 0)) == _white);
            // And pattern_at(pattern, point(0, 1.01, 0)) = black
            Assert.IsTrue(pattern.PatternAt(CrtFactory.Point(0, 1.01, 0)) == _black);
        }

        // Scenario: Checkers should repeat in z
        [Test]
        public void CheckersShouldRepeatInZ()
        {
            // Given pattern ← checkers_pattern(white, black)
            var pattern = CrtFactory.Checker3DPattern(_white, _black);
            // Then pattern_at(pattern, point(0, 0, 0)) = white
            Assert.IsTrue(pattern.PatternAt(CrtFactory.Point(0, 0, 0)) == _white);
            // And pattern_at(pattern, point(0, 0, 0.99)) = white
            Assert.IsTrue(pattern.PatternAt(CrtFactory.Point(0, 0, 0.99)) == _white);
            // And pattern_at(pattern, point(0, 0, 1.01)) = black
            Assert.IsTrue(pattern.PatternAt(CrtFactory.Point(0, 0, 1.01)) == _black);
        }

        #endregion

    }
}
