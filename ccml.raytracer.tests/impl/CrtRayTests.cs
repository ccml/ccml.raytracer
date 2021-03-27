using NUnit.Framework;

namespace ccml.raytracer.tests.impl
{
    public class CrtRayTests
    {
        [SetUp]
        public void Setup()
        {

        }

        // Scenario: Creating and querying a ray
        [Test]
        public void CreatingAndQueryingARay()
        {
            // Given origin ← point(1, 2, 3)
            var origin = CrtFactory.CoreFactory.Point(1, 2, 3);
            // And direction ← vector(4, 5, 6)
            var direction = CrtFactory.CoreFactory.Vector(4, 5, 6);
            // When r ← ray(origin, direction)
            var r = CrtFactory.EngineFactory.Ray(origin, direction);
            // Then r.origin = origin
            Assert.IsTrue(origin == r.Origin);
            Assert.IsTrue(r.Origin == CrtFactory.CoreFactory.Point(1, 2, 3));
            // And r.direction = direction
            Assert.IsTrue(direction == r.Direction);
            Assert.IsTrue(r.Direction == CrtFactory.CoreFactory.Vector(4, 5, 6));
        }

        // Scenario: Computing a point from a distance
        [Test]
        public void ComputingAPointFromADistance()
        {
            // Given r ← ray(point(2, 3, 4), vector(1, 0, 0))
            var r = CrtFactory.EngineFactory.Ray(CrtFactory.CoreFactory.Point(2, 3, 4), CrtFactory.CoreFactory.Vector(1, 0, 0));
            // Then position(r, 0) = point(2, 3, 4)
            Assert.IsTrue(r.PositionAtTime(0) == CrtFactory.CoreFactory.Point(2,3,4));
            // And position(r, 1) = point(3, 3, 4)
            Assert.IsTrue(r.PositionAtTime(1) == CrtFactory.CoreFactory.Point(3, 3, 4));
            // And position(r, -1) = point(1, 3, 4)
            Assert.IsTrue(r.PositionAtTime(-1) == CrtFactory.CoreFactory.Point(1, 3, 4));
            // And position(r, 2.5) = point(4.5, 3, 4)
            Assert.IsTrue(r.PositionAtTime(2.5) == CrtFactory.CoreFactory.Point(4.5, 3, 4));
        }

        // Scenario: Translating a ray
        [Test]
        public void TranslatingARay()
        {
            // Given r ← ray(point(1, 2, 3), vector(0, 1, 0))
            var r = CrtFactory.EngineFactory.Ray(CrtFactory.CoreFactory.Point(1, 2, 3), CrtFactory.CoreFactory.Vector(0, 1, 0));
            // And m ← translation(3, 4, 5)
            var m = CrtFactory.TransformationFactory.TranslationMatrix(3, 4, 5);
            // When r2 ← transform(r, m)
            var r2 = r.Transform(m);
            // Then r2.origin = point(4, 6, 8)
            Assert.IsTrue(r2.Origin == CrtFactory.CoreFactory.Point(4,6,8));
            // And r2.direction = vector(0, 1, 0)
            Assert.IsTrue(r2.Direction == CrtFactory.CoreFactory.Vector(0,1,0));
        }

        // Scenario: Scaling a ray
        [Test]
        public void ScalingARay()
        {
            // Given r ← ray(point(1, 2, 3), vector(0, 1, 0))
            var r = CrtFactory.EngineFactory.Ray(CrtFactory.CoreFactory.Point(1, 2, 3), CrtFactory.CoreFactory.Vector(0, 1, 0));
            // And m ← scaling(2, 3, 4)
            var m = CrtFactory.TransformationFactory.ScalingMatrix(2,3,4);
            // When r2 ← transform(r, m)
            var r2 = r.Transform(m);
            // Then r2.origin = point(2, 6, 12)
            Assert.IsTrue(r2.Origin == CrtFactory.CoreFactory.Point(2, 6, 12));
            // And r2.direction = vector(0, 3, 0)
            Assert.IsTrue(r2.Direction == CrtFactory.CoreFactory.Vector(0, 3, 0));
        }
    }
}
