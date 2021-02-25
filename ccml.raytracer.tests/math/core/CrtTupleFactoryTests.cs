using ccml.raytracer.math.core;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ccml.raytracer.tests.math.core
{
    public class CrtTupleFactoryTests
    {
        [SetUp]
        public void Setup()
        {
        }

        // Scenario: A tuple with w=1.0 is a point
        [Test]
        public void Tuple_W_1_Is_Point_Test()
        {
            // Given a ← tuple(4.3, -4.2, 3.1, 1.0)
            var a = CrtTupleFactory.Tuple(4.3, -4.2, 3.1, 1.0);
            //  Then a.x = 4.3
            Assert.AreEqual(a.X, 4.3);
            //   And a.y = -4.2
            Assert.AreEqual(a.Y, -4.2);
            //   And a.z = 3.1
            Assert.AreEqual(a.Z, 3.1);
            //   And a.w = 1.0
            Assert.AreEqual(a.W, 1.0);
            //   And a is a point
            Assert.IsTrue(a is CrtPoint);
            //   And a is not a vector
            Assert.IsFalse(a is CrtVector);
        }

        // Scenario: A tuple with w=0 is a vector
        [Test]
        public void Tuple_W_0_Is_Vector_Test()
        {
            // Given a ← tuple(4.3, -4.2, 3.1, 0.0)
            var a = CrtTupleFactory.Tuple(4.3, -4.2, 3.1, 0.0);
            //  Then a.x = 4.3
            Assert.AreEqual(a.X, 4.3);
            //   And a.y = -4.2
            Assert.AreEqual(a.Y, -4.2);
            //   And a.z = 3.1
            Assert.AreEqual(a.Z, 3.1);
            //   And a.w = 0.0
            Assert.AreEqual(a.W, 0.0);
            //   And a is not a point
            Assert.IsFalse(a is CrtPoint);
            //   And a is a vector
            Assert.IsTrue(a is CrtVector);
        }


        // Scenario: point() creates tuples with w = 1
        [Test]
        public void Point_Create_Tuple_W_1_Test()
        {
            // Given p ← point(4, -4, 3)
            var p = CrtTupleFactory.Point(4, -4, 3);
            // Then p = tuple(4, -4, 3, 1)
            Assert.AreEqual(p.X, 4);
            Assert.AreEqual(p.Y, -4);
            Assert.AreEqual(p.Z, 3);
            Assert.AreEqual(p.W, 1);
        }

        // Scenario: vector() creates tuples with w = 0
        [Test]
        public void Point_Create_Tuple_W_0_Test()
        {
            // Given v ← vector(4, -4, 3)
            var p = CrtTupleFactory.Vector(4, -4, 3);
            // Then v = tuple(4, -4, 3, 0)
            Assert.AreEqual(p.X, 4);
            Assert.AreEqual(p.Y, -4);
            Assert.AreEqual(p.Z, 3);
            Assert.AreEqual(p.W, 0);
        }

    }
}
