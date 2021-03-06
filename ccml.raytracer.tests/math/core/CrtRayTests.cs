using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ccml.raytracer.engine.core;
using NUnit.Framework;

namespace ccml.raytracer.tests.math.core
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
            var origin = CrtFactory.Point(1, 2, 3);
            // And direction ← vector(4, 5, 6)
            var direction = CrtFactory.Vector(4, 5, 6);
            // When r ← ray(origin, direction)
            var r = CrtFactory.Ray(origin, direction);
            // Then r.origin = origin
            Assert.IsTrue(origin == r.Origin);
            Assert.IsTrue(r.Origin == CrtFactory.Point(1, 2, 3));
            // And r.direction = direction
            Assert.IsTrue(direction == r.Direction);
            Assert.IsTrue(r.Direction == CrtFactory.Vector(4, 5, 6));
        }

        // Scenario: Computing a point from a distance
        [Test]
        public void ComputingAPointFromADistance()
        {
            // Given r ← ray(point(2, 3, 4), vector(1, 0, 0))
            var r = CrtFactory.Ray(CrtFactory.Point(2, 3, 4), CrtFactory.Vector(1, 0, 0));
            // Then position(r, 0) = point(2, 3, 4)
            Assert.IsTrue(r.PositionAtTime(0) == CrtFactory.Point(2,3,4));
            // And position(r, 1) = point(3, 3, 4)
            Assert.IsTrue(r.PositionAtTime(1) == CrtFactory.Point(3, 3, 4));
            // And position(r, -1) = point(1, 3, 4)
            Assert.IsTrue(r.PositionAtTime(-1) == CrtFactory.Point(1, 3, 4));
            // And position(r, 2.5) = point(4.5, 3, 4)
            Assert.IsTrue(r.PositionAtTime(2.5) == CrtFactory.Point(4.5, 3, 4));
        }
    }
}
