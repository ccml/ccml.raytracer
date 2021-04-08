using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using ccml.raytracer.Core;
using ccml.raytracer.Shapes;
using NUnit.Framework;

namespace ccml.raytracer.tests.impl
{
    public class CrtCsgTests
    {
        [SetUp]
        public void Setup()
        {
        }

        // Scenario: CSG is created with an operation and two shapes
        [Test]
        public void CsgIsCreatedWithAnOperationAndTwoShapes()
        {
            // Given s1 ← sphere()
            var s1 = CrtFactory.ShapeFactory.Sphere();
            // And s2 ← cube()
            var s2 = CrtFactory.ShapeFactory.Cube();
            // When c ← csg("union", s1, s2)
            var c = CrtFactory.ShapeFactory.Csg(CrtCSG.UNION, s1, s2);
            // Then c.operation = "union"
            Assert.AreEqual(CrtCSG.UNION, c.Operation);
            // And c.left = s1
            Assert.AreSame(s1, c.Left);
            // And c.right = s2
            Assert.AreSame(s2, c.Right);
            // And s1.parent = c
            Assert.AreSame(c, s1.Parent);
            // And s2.parent = c
            Assert.AreSame(c, s2.Parent);
        }

        // Scenario Outline: Evaluating the rule for a CSG operation
        [Test]
        public void EvaluatingTheRuleForACsgOperation()
        {
            // Examples:
            //  | op           | lhit  | inl   | inr   | result |

            //  | union        | true  | true  | true  | false  |
            //  | union        | true  | true  | false | true   |
            //  | union        | true  | false | true  | false  |
            //  | union        | true  | false | false | true   |
            //  | union        | false | true  | true  | false  |
            //  | union        | false | true  | false | false  |
            //  | union        | false | false | true  | true   |
            //  | union        | false | false | false | true   |

            //  | intersection | true  | true  | true  | true   |
            //  | intersection | true  | true  | false | false  |
            //  | intersection | true  | false | true  | true   |
            //  | intersection | true  | false | false | false  |
            //  | intersection | false | true  | true  | true   |
            //  | intersection | false | true  | false | true   |
            //  | intersection | false | false | true  | false  |
            //  | intersection | false | false | false | false  |

            //  | difference   | true  | true  | true  | false  |
            //  | difference   | true  | true  | false | true   |
            //  | difference   | true  | false | true  | false  |
            //  | difference   | true  | false | false | true   |
            //  | difference   | false | true  | true  | true   |
            //  | difference   | false | true  | false | true   |
            //  | difference   | false | false | true  | false  |
            //  | difference   | false | false | false | false  |

            var ops = @"
                  | union        | true  | true  | true  | false  |
                  | union        | true  | true  | false | true   |
                  | union        | true  | false | true  | false  |
                  | union        | true  | false | false | true   |
                  | union        | false | true  | true  | false  |
                  | union        | false | true  | false | false  |
                  | union        | false | false | true  | true   |
                  | union        | false | false | false | true   |

                  | intersection | true  | true  | true  | true   |
                  | intersection | true  | true  | false | false  |
                  | intersection | true  | false | true  | true   |
                  | intersection | true  | false | false | false  |
                  | intersection | false | true  | true  | true   |
                  | intersection | false | true  | false | true   |
                  | intersection | false | false | true  | false  |
                  | intersection | false | false | false | false  |

                  | difference   | true  | true  | true  | false  |
                  | difference   | true  | true  | false | true   |
                  | difference   | true  | false | true  | false  |
                  | difference   | true  | false | false | true   |
                  | difference   | false | true  | true  | true   |
                  | difference   | false | true  | false | true   |
                  | difference   | false | false | true  | false  |
                  | difference   | false | false | false | false  |
            ";
            using (var stream = new MemoryStream(Encoding.UTF8.GetBytes(ops)))
            {
                using (var sr = new StreamReader(stream))
                {
                    string line = null;
                    while ((line = sr.ReadLine())!=null)
                    {
                        if(string.IsNullOrWhiteSpace(line)) continue;
                        line = line.Trim();
                        line = line.Substring(1);
                        line = line.Substring(0, line.Length - 1);
                        var parts = line.Split('|');
                        var operation = parts[0].Trim();
                        var lhit = bool.Parse(parts[1].Trim());
                        var inl = bool.Parse(parts[2].Trim());
                        var inr = bool.Parse(parts[3].Trim());
                        var operationResult = bool.Parse(parts[4].Trim());
                        // When result ← intersection_allowed("<op>", < lhit >, < inl >, < inr >)
                        var result = CrtCSG.IntersectionAllowed(operation, lhit, inl, inr);
                        // Then result = < result >
                        Assert.AreEqual(operationResult, result);
                    }
                }
            }
        }

        // Scenario Outline: Filtering a list of intersections
        [Test]
        public void FilteringAListOfIntersections()
        {
            //      Examples:
            //          | operation    | x0 | x1 |
            //          | union        | 0  | 3  |
            //          | intersection | 1  | 2  |
            //          | difference   | 0  | 1  |
            var operations = new string[] { "union", "intersection", "difference" };
            var x0s = new int[] { 0, 1, 0 };
            var x1s = new int[] { 3, 2, 1 };
            for (int i = 0; i < operations.Length; i++)
            {
                // Given s1 ← sphere()
                var s1 = CrtFactory.ShapeFactory.Sphere();
                // And s2 ← cube()
                var s2 = CrtFactory.ShapeFactory.Cube();
                // And c ← csg("<operation>", s1, s2)
                var c = CrtFactory.ShapeFactory.Csg(operations[i], s1, s2);
                // And xs ← intersections(1:s1, 2:s2, 3:s1, 4:s2)
                var xs = CrtFactory.EngineFactory.Intersections(
                    CrtFactory.EngineFactory.Intersection(1, s1),
                    CrtFactory.EngineFactory.Intersection(2, s2),
                    CrtFactory.EngineFactory.Intersection(3, s1),
                    CrtFactory.EngineFactory.Intersection(4, s2)
                );
                // When result ← filter_intersections(c, xs)
                var result = c.FilterIntersections(xs);
                // Then result.count = 2
                Assert.AreEqual(2, result.Count);
                // And result[0] = xs[< x0 >]
                Assert.AreSame(xs[x0s[i]], result[0]);
                // And result[1] = xs[< x1 >]
                Assert.AreSame(xs[x1s[i]], result[1]);
            }
        }

        // Scenario: A ray misses a CSG object
        [Test]
        public void ARayMissesACsgObject()
        {
            // Given c ← csg("union", sphere(), cube())
            var c = CrtFactory.ShapeFactory.Csg(
                CrtCSG.UNION,
                CrtFactory.ShapeFactory.Sphere(),
                CrtFactory.ShapeFactory.Cube()
            );
            // And r ← ray(point(0, 2, -5), vector(0, 0, 1))
            var r = CrtFactory.EngineFactory.Ray(
                CrtFactory.CoreFactory.Point(0, 2, -5),
                CrtFactory.CoreFactory.Vector(0, 0, 1)
            );
            // When xs ← local_intersect(c, r)
            var xs = c.LocalIntersect(r);
            // Then xs is empty
            Assert.IsEmpty(xs);
        }

        // Scenario: A ray hits a CSG object
        [Test]
        public void ARayHitsACsgObject()
        {
            // Given s1 ← sphere()
            var s1 = CrtFactory.ShapeFactory.Sphere();
            // And s2 ← sphere()
            // And set_transform(s2, translation(0, 0, 0.5))
            var s2 = CrtFactory.ShapeFactory.Sphere()
                .WithTransformationMatrix(CrtFactory.TransformationFactory.TranslationMatrix(0, 0, 0.5));
            // And c ← csg("union", s1, s2)
            var c = CrtFactory.ShapeFactory.Csg(CrtCSG.UNION, s1, s2);
            // And r ← ray(point(0, 0, -5), vector(0, 0, 1))
            var r = CrtFactory.EngineFactory.Ray(
                CrtFactory.CoreFactory.Point(0, 0, -5),
                CrtFactory.CoreFactory.Vector(0, 0, 1)
            );
            // When xs ← local_intersect(c, r)
            var xs = c.LocalIntersect(r);
            // Then xs.count = 2
            Assert.AreEqual(2, xs.Count);
            // And xs[0].t = 4
            Assert.IsTrue(CrtReal.AreEquals(xs[0].T, 4));
            // And xs[0].object = s1
            Assert.AreSame(s1, xs[0].TheObject);
            // And xs[1].t = 6.5
            Assert.IsTrue(CrtReal.AreEquals(xs[1].T, 6.5));
            // And xs[1].object = s2
            Assert.AreSame(s2, xs[1].TheObject);
        }

    }
}
