using ccml.raytracer.Core;
using NUnit.Framework;

namespace ccml.raytracer.tests.impl
{
    public class CrtColorTest
    {
        [SetUp]
        public void Setup()
        {

        }

        //Scenario: Colors are(red, green, blue) tuples
        [Test]
        public void ColorsAreRedGreenBlueTuplesTest()
        {
            // Given c ← color(-0.5, 0.4, 1.7)
            var c = CrtFactory.CoreFactory.Color(-0.5, 0.4, 1.7);
            // Then c.red = -0.5
            Assert.IsTrue(CrtReal.AreEquals(c.Red, -0.5));
            // And c.green = 0.4
            Assert.IsTrue(CrtReal.AreEquals(c.Green, 0.4));
            // And c.blue = 1.7
            Assert.IsTrue(CrtReal.AreEquals(c.Blue, 1.7));
        }


        #region Color operations

        //Scenario: Adding colors
        [Test]
        public void AddingColorsTest()
        {
            // Given c1 ← color(0.9, 0.6, 0.75)
            var c1 = CrtFactory.CoreFactory.Color(0.9, 0.6, 0.75);
            // And c2 ← color(0.7, 0.1, 0.25)
            var c2 = CrtFactory.CoreFactory.Color(0.7, 0.1, 0.25);
            // Then c1 + c2 = color(1.6, 0.7, 1.0)
            Assert.IsTrue(c1 + c2 == CrtFactory.CoreFactory.Color(1.6, 0.7, 1.0));
        }

        //Scenario: Subtracting colors
        [Test]
        public void SubtractingColorsTest()
        {
            // Given c1 ← color(0.9, 0.6, 0.75)
            var c1 = CrtFactory.CoreFactory.Color(0.9, 0.6, 0.75);
            // And c2 ← color(0.7, 0.1, 0.25)
            var c2 = CrtFactory.CoreFactory.Color(0.7, 0.1, 0.25);
            // Then c1 - c2 = color(0.2, 0.5, 0.5)
            Assert.IsTrue(c1 - c2 == CrtFactory.CoreFactory.Color(0.2, 0.5, 0.5));
        }

        //Scenario: Multiplying a color by a scalar
        [Test]
        public void MultiplyingAColorByAScalarTest()
        {
            // Given c ← color(0.2, 0.3, 0.4)
            var c = CrtFactory.CoreFactory.Color(0.2, 0.3, 0.4);
            // Then c * 2 = color(0.4, 0.6, 0.8)
            Assert.IsTrue(c * 2 == CrtFactory.CoreFactory.Color(0.4, 0.6, 0.8));
            // And 2 * c = color(0.4, 0.6, 0.8)
            Assert.IsTrue(2 * c == CrtFactory.CoreFactory.Color(0.4, 0.6, 0.8));
        }

        // Scenario: Multiplying colors
        [Test]
        public void MultiplyingTwoColorsTest()
        {
            // Given c1 ← color(1, 0.2, 0.4)
            var c1 = CrtFactory.CoreFactory.Color(1, 0.2, 0.4);
            // And c2 ← color(0.9, 1, 0.1)
            var c2 = CrtFactory.CoreFactory.Color(0.9, 1, 0.1);
            // Then c1 * c2 = color(0.9, 0.2, 0.04)
            Assert.IsTrue(c1 * c2 == CrtFactory.CoreFactory.Color(0.9, 0.2, 0.04));
        }

        #endregion

    }
}
