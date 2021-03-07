using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ccml.raytracer.engine.core;

namespace ccml.raytracer.tests.math.core
{
    public class CrtLightsTests
    {
        [SetUp]
        public void Setup()
        {

        }

        #region PointLight

        // Scenario: A point light has a position and intensity
        [Test]
        public void APointLightHasAPositionAndIntensity()
        {
            // Given intensity ← color(1, 1, 1)
            var intensity = CrtFactory.Color(1, 1, 1);
            // And position ← point(0, 0, 0)
            var position = CrtFactory.Point(0, 0, 0);
            // When light ← point_light(position, intensity)
            var light = CrtFactory.PointLight(position, intensity);
            // Then light.position = position
            Assert.AreSame(light.Position, position);
            // And light.intensity = intensity
            Assert.AreSame(light.Intensity, intensity);
        }

        #endregion
    }
}
