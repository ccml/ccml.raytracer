using NUnit.Framework;

namespace ccml.raytracer.tests.impl
{
    public class CrtViewTransformationsTests
    {
        [SetUp]
        public void Setup()
        {

        }

        // Scenario: The transformation matrix for the default orientation
        [Test]
        public void TheTransformationMatrixForTheDefaultOrientation()
        {
            // Given from ← point(0, 0, 0)
            var from = CrtFactory.CoreFactory.Point(0, 0, 0);
            // And to ← point(0, 0, -1)
            var to = CrtFactory.CoreFactory.Point(0, 0, -1);
            // And up ← vector(0, 1, 0)
            var up = CrtFactory.CoreFactory.Vector(0, 1, 0);
            // When t ← view_transform(from, to, up)
            var t = CrtFactory.EngineFactory.ViewTransformation(from, to, up);
            // Then t = identity_matrix
            Assert.IsTrue(t == CrtFactory.TransformationFactory.IdentityMatrix(4,4));
        }

        // Scenario: A view transformation matrix looking in positive z direction
        [Test]
        public void AViewTransformationMatrixLookingInPositiveZDirection()
        {
            // Given from ← point(0, 0, 0)
            var from = CrtFactory.CoreFactory.Point(0, 0, 0);
            // And to ← point(0, 0, 1)
            var to = CrtFactory.CoreFactory.Point(0, 0, 1);
            // And up ← vector(0, 1, 0)
            var up = CrtFactory.CoreFactory.Vector(0, 1, 0);
            // When t ← view_transform(from, to, up)
            var t = CrtFactory.EngineFactory.ViewTransformation(from, to, up);
            // Then t = scaling(-1, 1, -1)
            Assert.IsTrue(t == CrtFactory.TransformationFactory.ScalingMatrix(-1,1,-1));
        }

        // Scenario: The view transformation moves the world
        [Test]
        public void TheViewTransformationMovesTheWorld()
        {
            // Given from ← point(0, 0, 8)
            var from = CrtFactory.CoreFactory.Point(0, 0, 8);
            // And to ← point(0, 0, 0)
            var to = CrtFactory.CoreFactory.Point(0, 0, 0);
            // And up ← vector(0, 1, 0)
            var up = CrtFactory.CoreFactory.Vector(0, 1, 0);
            // When t ← view_transform(from, to, up)
            var t = CrtFactory.EngineFactory.ViewTransformation(from, to, up);
            // Then t = translation(0, 0, -8)
            Assert.IsTrue(t == CrtFactory.TransformationFactory.TranslationMatrix(0, 0, -8));
        }

        // Scenario: An arbitrary view transformation
        [Test]
        public void AnArbitraryViewTransformation()
        {
            // Given from ← point(1, 3, 2)
            var from = CrtFactory.CoreFactory.Point(1, 3, 2);
            // And to ← point(4, -2, 8)
            var to = CrtFactory.CoreFactory.Point(4, -2, 8);
            // And up ← vector(1, 1, 0)
            var up = CrtFactory.CoreFactory.Vector(1, 1, 0);
            // When t ← view_transform(from, to, up)
            var t = CrtFactory.EngineFactory.ViewTransformation(from, to, up);
            // Then t is the following 4x4 matrix:
            //   | -0.50709 | 0.50709 | 0.67612 | -2.36643 |
            //   | 0.76772 | 0.60609 | 0.12122 | -2.82843 |
            //   | -0.35857 | 0.59761 | -0.71714 | 0.00000 |
            //   | 0.00000 | 0.00000 | 0.00000 | 1.00000 |
            Assert.IsTrue(
                t == CrtFactory.CoreFactory.Matrix(
                    new double[] { -0.50709, 0.50709, 0.67612, -2.36643 },
                    new double[] { 0.76772, 0.60609, 0.12122, -2.82843 },
                    new double[] { -0.35857, 0.59761, -0.71714, 0.00000 },
                    new double[] { 0.00000, 0.00000, 0.00000, 1.00000 }
                )
            );
        }

    }
}
