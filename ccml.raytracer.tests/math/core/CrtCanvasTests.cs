using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ccml.raytracer.engine.core;
using NUnit.Framework;

namespace ccml.raytracer.tests.math.core
{
    public class CrtCanvasTests
    {
        [SetUp]
        public void Setup()
        {

        }

        // Scenario: Creating a canvas
        [Test]
        public void CreatingACanvasTest()
        {
            // Given c ← canvas(10, 20)
            var c = CrtFactory.Canvas(10, 20);
            // Then c.width = 10
            Assert.AreEqual(10, c.Width);
            // And c.height = 20
            Assert.AreEqual(20, c.Height);
            // And every pixel of c is color(0, 0, 0)
            var black = CrtFactory.Color(0, 0, 0);
            for (int h = 0; h < 10; h++)
            {
                for (int w = 0; w < 20; w++)
                {
                    Assert.IsTrue(c[w, h] == black);
                }
            }
        }

        // Scenario: Writing pixels to a canvas
        [Test]
        public void WritingPixelsToACanvasTest()
        {
            // Given c ← canvas(10, 20)
            var c = CrtFactory.Canvas(10, 20);
            // And red ← color(1, 0, 0)
            var red = CrtFactory.Color(1, 0, 0);
            // When write_pixel(c, 2, 3, red)
            c[2, 3] = red;
            // Then pixel_at(c, 2, 3) = red
            Assert.IsTrue(c[2, 3] == red);
        }
    }
}
