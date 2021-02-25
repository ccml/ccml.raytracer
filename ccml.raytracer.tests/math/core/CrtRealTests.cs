using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ccml.raytracer.math.core;

namespace ccml.raytracer.tests.math.core
{
    public class CrtRealTests
    {
        [SetUp]
        public void Setup()
        {

        }

        // Scenario: 2 reals with less than 0.00001 of difference are equals
        [Test]
        public void reals_Are_Equals()
        {
            // Given a ← 4
            var a = 4;
            // Given b ← 4.000001
            var b = 4.000001;
            // Then a == b
            Assert.IsTrue(CrtReal.AreEquals(a, b));
        }

        // Scenario: 2 reals with more (or equals) than 0.00001 of difference are differents
        [Test]
        public void reals_Are_Not_Equals()
        {
            // Given a ← 4
            var a = 4;
            // Given b ← 4.00002
            var b = 4.00002;
            // Then a != b
            Assert.IsFalse(CrtReal.AreEquals(a, b));
        }
    }
}
