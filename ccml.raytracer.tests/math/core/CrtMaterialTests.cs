using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ccml.raytracer.engine.core;
using ccml.raytracer.engine.core.Materials;
using NUnit.Framework;

namespace ccml.raytracer.tests.math.core
{
    public class CrtMaterialTests
    {
        private CrtMaterial _sharedMaterial;
        private CrtPoint _sharedHitPoint;

        [SetUp]
        public void Setup()
        {
            // Background:
            //    Given m ← material()
            //    And position ← point(0, 0, 0)
            _sharedMaterial = CrtFactory.Material();
            _sharedHitPoint = CrtFactory.Point(0, 0, 0);
        }

        // Scenario: The default material
        [Test]
        public void TheDefaultMaterial()
        {
            // Given m ← material()
            var m = CrtFactory.Material();
            // Then m.color = color(1, 1, 1)
            Assert.IsTrue(m.Color == CrtFactory.Color(1,1,1));
            // And m.ambient = 0.1
            Assert.IsTrue(CrtReal.AreEquals(m.Ambient, 0.1));
            // And m.diffuse = 0.9
            Assert.IsTrue(CrtReal.AreEquals(m.Diffuse, 0.9));
            // And m.specular = 0.9
            Assert.IsTrue(CrtReal.AreEquals(m.Specular, 0.9));
            // And m.shininess = 200.0
            Assert.IsTrue(CrtReal.AreEquals(m.Shininess, 200.0));
        }

        #region Shapes and materials

        // Scenario: A sphere has a default material
        [Test]
        public void ASphereHasADefaultMaterial()
        {
            // Given s ← sphere()
            var s = CrtFactory.Sphere();
            // When m ← s.material
            var m = CrtFactory.Material();
            // Then m = material()
            Assert.IsTrue(s.Material == m);
        }

        // Scenario: A sphere may be assigned a material
        [Test]
        public void ASphereMayBeAssignedAMaterial()
        {
            // Given s ← sphere()
            var s = CrtFactory.Sphere();
            // And m ← material()
            var m = CrtFactory.Material();
            // And m.ambient ← 1
            m.Ambient = 1;
            // When s.material ← m
            s.Material = m;
            // Then s.material = m
            Assert.IsTrue(s.Material == m);
        }

        #endregion

        #region Lighting

        // Scenario: Lighting with the eye between the light and the surface
        [Test]
        public void LightingWithTheEyeBetweenTheLightAndTheSurface()
        {
            // Given eyev ← vector(0, 0, -1)
            var eyev = CrtFactory.Vector(0, 0, -1);
            // And normalv ← vector(0, 0, -1)
            var normalv = CrtFactory.Vector(0, 0, -1);
            // And light ← point_light(point(0, 0, -10), color(1, 1, 1))
            var light = CrtFactory.PointLight(CrtFactory.Point(0, 0, -10), CrtFactory.Color(1, 1, 1));
            // When result ← lighting(m, light, position, eyev, normalv)
            var result = CrtFactory.Engine().Lighting(_sharedMaterial, CrtFactory.Sphere(), light, _sharedHitPoint, eyev, normalv);
            // Then result = color(1.9, 1.9, 1.9)
            Assert.IsTrue(result == CrtFactory.Color(1.9,1.9,1.9));
        }

        // Scenario: Lighting with the eye between light and surface, eye offset 45°
        [Test]
        public void LightingWithTheEyeBetweenTheLightAndTheSurfaceEyeOffset45Deg()
        {
            // Given eyev ← vector(0, √2/2, -√2/2)
            var eyev = CrtFactory.Vector(0, Math.Sqrt(2.0)/2, -Math.Sqrt(2.0) / 2);
            // And normalv ← vector(0, 0, -1)
            var normalv = CrtFactory.Vector(0, 0, -1);
            // And light ← point_light(point(0, 0, -10), color(1, 1, 1))
            var light = CrtFactory.PointLight(CrtFactory.Point(0, 0, -10), CrtFactory.Color(1, 1, 1));
            // When result ← lighting(m, light, position, eyev, normalv)
            var result = CrtFactory.Engine().Lighting(_sharedMaterial, CrtFactory.Sphere(), light, _sharedHitPoint, eyev, normalv);
            // Then result = color(1.0, 1.0, 1.0)
            Assert.IsTrue(result == CrtFactory.Color(1.0, 1.0, 1.0));
        }

        // Scenario: Lighting with eye opposite surface, light offset 45°
        [Test]
        public void LightingWithTheEyeBetweenTheLightAndTheSurfaceLightOffset45Deg()
        {
            // Given eyev ← vector(0, 0, -1)
            var eyev = CrtFactory.Vector(0, 0, -1);
            // And normalv ← vector(0, 0, -1)
            var normalv = CrtFactory.Vector(0, 0, -1);
            // And light ← point_light(point(0, 10, -10), color(1, 1, 1))
            var light = CrtFactory.PointLight(CrtFactory.Point(0, 10, -10), CrtFactory.Color(1, 1, 1));
            // When result ← lighting(m, light, position, eyev, normalv)
            var result = CrtFactory.Engine().Lighting(_sharedMaterial, CrtFactory.Sphere(), light, _sharedHitPoint, eyev, normalv);
            // Then result = color(0.7364, 0.7364, 0.7364)
            Assert.IsTrue(result == CrtFactory.Color(0.7364, 0.7364, 0.7364));
        }

        // Scenario: Lighting with eye in the path of the reflection vector
        [Test]
        public void LightingWithTheEyeInThePathOfTheReflectionVector()
        {
            // Given eyev ← vector(0, -√2/2, -√2/2)
            var eyev = CrtFactory.Vector(0, -Math.Sqrt(2.0) / 2, -Math.Sqrt(2.0) / 2);
            // And normalv ← vector(0, 0, -1)
            var normalv = CrtFactory.Vector(0, 0, -1);
            // And light ← point_light(point(0, 10, -10), color(1, 1, 1))
            var light = CrtFactory.PointLight(CrtFactory.Point(0, 10, -10), CrtFactory.Color(1, 1, 1));
            // When result ← lighting(m, light, position, eyev, normalv)
            var result = CrtFactory.Engine().Lighting(_sharedMaterial, CrtFactory.Sphere(), light, _sharedHitPoint, eyev, normalv);
            // Then result = color(1.6364, 1.6364, 1.6364)
            Assert.IsTrue(result == CrtFactory.Color(1.6364, 1.6364, 1.6364));
        }

        // Scenario: Lighting with the light behind the surface
        [Test]
        public void LightingWithTheLightBehindTheSurface()
        {
            // Given eyev ← vector(0, 0, -1)
            var eyev = CrtFactory.Vector(0, 0, -1);
            // And normalv ← vector(0, 0, -1)
            var normalv = CrtFactory.Vector(0, 0, -1);
            // And light ← point_light(point(0, 0, 10), color(1, 1, 1))
            var light = CrtFactory.PointLight(CrtFactory.Point(0, 0, 10), CrtFactory.Color(1, 1, 1));
            // When result ← lighting(m, light, position, eyev, normalv)
            var result = CrtFactory.Engine().Lighting(_sharedMaterial, CrtFactory.Sphere(), light, _sharedHitPoint, eyev, normalv);
            // Then result = color(0.1, 0.1, 0.1)
            Assert.IsTrue(result == CrtFactory.Color(0.1, 0.1, 0.1));
        }

        #endregion

        #region Shadowing

        // Scenario: Lighting with the surface in shadow
        [Test]
        public void LightingWithThSurfaceInShadow()
        {
            // Given eyev ← vector(0, 0, -1)
            var eyev = CrtFactory.Vector(0, 0, -1);
            // And normalv ← vector(0, 0, -1)
            var normalv = CrtFactory.Vector(0, 0, -1);
            // And light ← point_light(point(0, 0, -10), color(1, 1, 1))
            var light = CrtFactory.PointLight(CrtFactory.Point(0, 0, -10), CrtFactory.Color(1, 1, 1));
            // And in_shadow ← true
            var inShadow = true;
            // When result ← lighting(m, light, position, eyev, normalv, in_shadow)
            var result = CrtFactory.Engine().Lighting(_sharedMaterial, CrtFactory.Sphere(), light, _sharedHitPoint, eyev, normalv, inShadow);
            // Then result = color(0.1, 0.1, 0.1)
            Assert.IsTrue(result == CrtFactory.Color(0.1, 0.1, 0.1));
        }

        #endregion

        #region Reflection

        // Scenario: Reflectivity for the default material
        [Test]
        public void ReflectivityForTheDefaultMaterial()
        {
            // Given m ← material()
            var m = CrtFactory.Material();
            // Then m.reflective = 0.0
            Assert.IsTrue(CrtReal.AreEquals(m.Reflective, 0.0));
        }

        // Scenario: Precomputing the reflection vector
        [Test]
        public void PrecomputingTheReflectionVector()
        {
            // Given shape ← plane()
            var shape = CrtFactory.Plane();
            // And r ← ray(point(0, 1, -1), vector(0, -√2/2, √2/2))
            var r = CrtFactory.Ray(
                CrtFactory.Point(0, 1, -1),
                CrtFactory.Vector(0, -Math.Sqrt(2.0) / 2.0, Math.Sqrt(2.0) / 2.0)
            );
            // And i ← intersection(√2, shape)
            var i = CrtFactory.Intersection(Math.Sqrt(2.0), shape);
            // When comps ← prepare_computations(i, r)
            var comps = CrtFactory.Engine().PrepareComputations(i, r);
            // Then comps.reflectv = vector(0, √2/2, √2/2)
            Assert.IsTrue(comps.ReflectVector == CrtFactory.Vector(0, Math.Sqrt(2.0) / 2.0, Math.Sqrt(2.0) / 2.0));
        }

        #endregion
    }
}
