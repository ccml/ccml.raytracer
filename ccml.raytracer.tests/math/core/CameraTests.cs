using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ccml.raytracer.engine.core;
using NUnit.Framework;

namespace ccml.raytracer.tests.math.core
{
    public class CameraTests
    {
        [SetUp]
        public void Setup()
        {

        }

        #region Construct a camera

        // Scenario: Constructing a camera
        [Test]
        public void ConstructingACamera()
        {
            // Given hsize ← 160
            var hsize = 160;
            // And vsize ← 120
            var vsize = 120;
            // And field_of_view ← π/2
            var fieldOfView = Math.PI / 2.0;
            // When c ← camera(hsize, vsize, field_of_view)
            var c = CrtFactory.Camera(hsize, vsize, fieldOfView);
            // Then c.hsize = 160
            Assert.AreEqual(160, c.HSize);
            // And c.vsize = 120
            Assert.AreEqual(120, c.VSize);
            // And c.field_of_view = π / 2
            Assert.IsTrue(CrtReal.AreEquals(c.FieldOfView, Math.PI / 2.0));
            // And c.transform = identity_matrix
            Assert.IsTrue(c.ViewTransformMatrix == CrtFactory.IdentityMatrix(4, 4));
        }

        // Scenario: The pixel size for a horizontal canvas
        [Test]
        public void ThePixelSizeForAHorizontalCanvas()
        {
            // Given c ← camera(200, 125, π/2)
            var c = CrtFactory.Camera(200, 125, Math.PI / 2.0);
            // Then c.pixel_size = 0.01
            Assert.IsTrue(CrtReal.AreEquals(c.PixelSize, 0.01));
        }

        // Scenario: The pixel size for a vertical canvas
        [Test]
        public void ThePixelSizeForAVerticalCanvas()
        {
            // Given c ← camera(125, 200, π/2)
            var c = CrtFactory.Camera(125, 200, Math.PI / 2.0);
            // Then c.pixel_size = 0.01
            Assert.IsTrue(CrtReal.AreEquals(c.PixelSize, 0.01));
        }

        #endregion

        #region Construct a ray for the camera

        // Scenario: Constructing a ray through the center of the canvas
        [Test]
        public void ConstructingARayThroughTheCenterOfTheCanvas()
        {
            // Given c ← camera(201, 101, π/2)
            var c = CrtFactory.Camera(201, 101, Math.PI / 2.0);
            // When r ← ray_for_pixel(c, 100, 50)
            var r = c.RayForPixel(100, 50);
            // Then r.origin = point(0, 0, 0)
            Assert.IsTrue(r.Origin == CrtFactory.Point(0, 0, 0));
            // And r.direction = vector(0, 0, -1)
            Assert.IsTrue(r.Direction == CrtFactory.Vector(0, 0, -1));
        }

        // Scenario: Constructing a ray through a corner of the canvas
        [Test]
        public void ConstructingARayThroughACornerOfTheCanvas()
        {
            // Given c ← camera(201, 101, π/2)
            var c = CrtFactory.Camera(201, 101, Math.PI / 2.0);
            // When r ← ray_for_pixel(c, 0, 0)
            var r = c.RayForPixel(0, 0);
            // Then r.origin = point(0, 0, 0)
            Assert.IsTrue(r.Origin == CrtFactory.Point(0, 0, 0));
            // And r.direction = vector(0.66519, 0.33259, -0.66851)
            Assert.IsTrue(r.Direction == CrtFactory.Vector(0.66519, 0.33259, -0.66851));
        }

        // Scenario: Constructing a ray when the camera is transformed
        [Test]
        public void ConstructingARayWhenTheCameraIsTransformed()
        {
            // Given c ← camera(201, 101, π/2)
            var c = CrtFactory.Camera(201, 101, Math.PI / 2.0);
            // When c.transform ← rotation_y(π/4) * translation(0, -2, 5)
            c.ViewTransformMatrix = 
                CrtFactory.YRotationMatrix(Math.PI / 4.0)
                *
                CrtFactory.TranslationMatrix(0, -2, 5);
            // And r ← ray_for_pixel(c, 100, 50)
            var r = c.RayForPixel(100, 50);
            // Then r.origin = point(0, 2, -5)
            Assert.IsTrue(r.Origin == CrtFactory.Point(0, 2, -5));
            // And r.direction = vector(√2/2, 0, -√2/2)
            Assert.IsTrue(r.Direction == CrtFactory.Vector(Math.Sqrt(2.0) / 2.0, 0.0, -Math.Sqrt(2.0) / 2.0));
        }

        #endregion

        #region Rendering a world

        //Scenario: Rendering a world with a camera
        [Test]
        public void RenderingAWorldWithACamera()
        {
            // Given w ← default_world()
            var w = CrtFactory.DefaultWorld();
            // And c ← camera(11, 11, π/2)
            var c = CrtFactory.Camera(11, 11, Math.PI / 2.0);
            // And from ← point(0, 0, -5)
            var from = CrtFactory.Point(0, 0, -5);
            // And to ← point(0, 0, 0)
            var to = CrtFactory.Point(0, 0, 0);
            // And up ← vector(0, 1, 0)
            var up = CrtFactory.Vector(0, 1, 0);
            // And c.transform ← view_transform(from, to, up)
            c.ViewTransformMatrix = CrtFactory.ViewTransformation(from, to, up);
            // When image ← render(c, w)
            var image = c.Render(w);
            // Then pixel_at(image, 5, 5) = color(0.38066, 0.47583, 0.2855)
            Assert.IsTrue(image[5, 5] == CrtFactory.Color(0.38066, 0.47583, 0.2855));
        }

        #endregion
    }
}
