using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ccml.raytracer.Shapes;
using NUnit.Framework;

namespace ccml.raytracer.tests.impl
{
    public class CrtObjFileTests
    {
        [SetUp]
        public void Setup()
        {

        }

        #region Triangles

        // Scenario: Ignoring unrecognized lines
        [Test]
        public void IgnoringUnrecognizedLines()
        {
            // Given gibberish ← a file containing:
            // """
            // There was a young lady named Bright
            // who traveled much faster than light.
            // She set out one day
            // in a relative way,
            // and came back the previous night.
            // """
            var gibberish =
@"
There was a young lady named Bright
who traveled much faster than light.
She set out one day
in a relative way,
and came back the previous night.
";
            // When parser ← parse_obj_file(gibberish)
            var parser = CrtFactory.FileFormatFactory.ObjParser;
            parser.Parse(new MemoryStream(Encoding.UTF8.GetBytes(gibberish)));
            // Then parser should have ignored 5 lines
            Assert.AreEqual(5, parser.NbrIgnoredLines);
        }

        // Scenario: Vertex records
        [Test]
        public void VertexRecords()
        {
            // Given file ← a file containing:
            // """
            // v -1 1 0
            // v -1.0000 0.5000 0.0000
            // v 1 0 0
            // v 1 1 0  <-- test with some additional whitespaces on last line
            // """
            var file =
                @"
v -1 1 0
v -1.0000 0.5000 0.0000
v 1 0   0
v   1   1 0
";
            // When parser ← parse_obj_file(file)
            var parser = CrtFactory.FileFormatFactory.ObjParser;
            parser.Parse(new MemoryStream(Encoding.UTF8.GetBytes(file)));
            // Then parser.vertices[1] = point(-1, 1, 0)
            Assert.IsTrue(parser.Vertices[1] == CrtFactory.CoreFactory.Point(-1, 1, 0));
            // And parser.vertices[2] = point(-1, 0.5, 0)
            Assert.IsTrue(parser.Vertices[2] == CrtFactory.CoreFactory.Point(-1, 0.5, 0));
            // And parser.vertices[3] = point(1, 0, 0)
            Assert.IsTrue(parser.Vertices[3] == CrtFactory.CoreFactory.Point(1, 0, 0));
            // And parser.vertices[4] = point(1, 1, 0)
            Assert.IsTrue(parser.Vertices[4] == CrtFactory.CoreFactory.Point(1, 1, 0));
        }

        // Scenario: Parsing triangle faces
        [Test]
        public void ParsingTriangleFaces()
        {
            // Given file ← a file containing:
            // """
            // v -1 1 0
            // v -1 0 0
            // v 1 0 0
            // v 1 1 0
            // f 1 2 3
            // f 1 3 4
            // """
            var file =
@"
v -1 1 0
v -1 0 0
v 1 0 0
v 1 1 0
f 1 2 3
f 1 3 4
";
            // When parser ← parse_obj_file(file)
            var parser = CrtFactory.FileFormatFactory.ObjParser;
            parser.Parse(new MemoryStream(Encoding.UTF8.GetBytes(file)));
            // And g ← parser.default_group
            var g = parser.DefaultGroup;
            // And t1 ← first child of g
            var t1 = g.Childs[0] as CrtTriangle;
            // And t2 ← second child of g
            var t2 = g.Childs[1] as CrtTriangle;
            //
            Assert.IsNotNull(t1);
            Assert.IsNotNull(t2);
            //
            // Then t1.p1 = parser.vertices[1]
            Assert.IsTrue(t1.P1 == parser.Vertices[1]);
            // And t1.p2 = parser.vertices[2]
            Assert.IsTrue(t1.P2 == parser.Vertices[2]);
            // And t1.p3 = parser.vertices[3]
            Assert.IsTrue(t1.P3 == parser.Vertices[3]);
            // And t2.p1 = parser.vertices[1]
            Assert.IsTrue(t2.P1 == parser.Vertices[1]);
            // And t2.p2 = parser.vertices[3]
            Assert.IsTrue(t2.P2 == parser.Vertices[3]);
            // And t2.p3 = parser.vertices[4]
            Assert.IsTrue(t2.P3 == parser.Vertices[4]);
        }

        // Scenario: Triangulating polygons
        [Test]
        public void TriangulatingPolygons()
        {
            // Given file ← a file containing:
            // """
            // v -1 1 0
            // v -1 0 0
            // v 1 0 0
            // v 1 1 0
            // v 0 2 0
            // f 1 2 3 4 5
            // """
            var file =
@"
v -1 1 0
v -1 0 0
v 1 0 0
v 1 1 0
v 0 2 0
f 1 2 3 4 5
";
            // When parser ← parse_obj_file(file)
            var parser = CrtFactory.FileFormatFactory.ObjParser;
            parser.Parse(new MemoryStream(Encoding.UTF8.GetBytes(file)));
            // And g ← parser.default_group
            var g = parser.DefaultGroup;
            // And t1 ← first child of g
            var t1 = g.Childs[0] as CrtTriangle;
            // And t2 ← second child of g
            var t2 = g.Childs[1] as CrtTriangle;
            // And t3 ← third child of g
            var t3 = g.Childs[2] as CrtTriangle;
            //
            Assert.IsNotNull(t1);
            Assert.IsNotNull(t2);
            Assert.IsNotNull(t3);
            //
            // Then t1.p1 = parser.vertices[1]
            Assert.IsTrue(t1.P1 == parser.Vertices[1]);
            // And t1.p2 = parser.vertices[2]
            Assert.IsTrue(t1.P2 == parser.Vertices[2]);
            // And t1.p3 = parser.vertices[3]
            Assert.IsTrue(t1.P3 == parser.Vertices[3]);
            // And t2.p1 = parser.vertices[1]
            Assert.IsTrue(t2.P1 == parser.Vertices[1]);
            // And t2.p2 = parser.vertices[3]
            Assert.IsTrue(t2.P2 == parser.Vertices[3]);
            // And t2.p3 = parser.vertices[4]
            Assert.IsTrue(t2.P3 == parser.Vertices[4]);
            // And t3.p1 = parser.vertices[1]
            Assert.IsTrue(t3.P1 == parser.Vertices[1]);
            // And t3.p2 = parser.vertices[4]
            Assert.IsTrue(t3.P2 == parser.Vertices[4]);
            // And t3.p3 = parser.vertices[5]
            Assert.IsTrue(t3.P3 == parser.Vertices[5]);
        }


        private string _trianglesObj =
            @"
v -1 1 0
v -1 0 0
v 1 0 0
v 1 1 0
g FirstGroup
f 1 2 3
g SecondGroup
f 1 3 4
";

        // Scenario: Triangles in groups
        [Test]
        public void TrianglesInGroups()
        {
            // Given file ← the file "triangles.obj"
            var file = _trianglesObj;
            // When parser ← parse_obj_file(file)
            var parser = CrtFactory.FileFormatFactory.ObjParser;
            parser.Parse(new MemoryStream(Encoding.UTF8.GetBytes(file)));
            // And g1 ← "FirstGroup" from parser
            var g1 = parser.DefaultGroup.Childs[0] as CrtGroup;
            // And g2 ← "SecondGroup" from parser
            var g2 = parser.DefaultGroup.Childs[1] as CrtGroup;
            //
            Assert.IsNotNull(g1);
            Assert.IsNotNull(g2);
            //
            // And t1 ← first child of g1
            var t1 = g1.Childs[0] as CrtTriangle;
            // And t2 ← first child of g2
            var t2 = g2.Childs[0] as CrtTriangle;
            //
            Assert.IsNotNull(t1);
            Assert.IsNotNull(t2);
            //
            // Then t1.p1 = parser.vertices[1]
            Assert.IsTrue(t1.P1 == parser.Vertices[1]);
            // And t1.p2 = parser.vertices[2]
            Assert.IsTrue(t1.P2 == parser.Vertices[2]);
            // And t1.p3 = parser.vertices[3]
            Assert.IsTrue(t1.P3 == parser.Vertices[3]);
            // And t2.p1 = parser.vertices[1]
            Assert.IsTrue(t2.P1 == parser.Vertices[1]);
            // And t2.p2 = parser.vertices[3]
            Assert.IsTrue(t2.P2 == parser.Vertices[3]);
            // And t2.p3 = parser.vertices[4]
            Assert.IsTrue(t2.P3 == parser.Vertices[4]);
        }

        // Scenario: Converting an OBJ file to a group
        [Test]
        public void ConvertingAnOBJFileToAGroup()
        {
            // Given file ← the file "triangles.obj"
            var file = _trianglesObj;
            // And parser ← parse_obj_file(file)
            var parser = CrtFactory.FileFormatFactory.ObjParser;
            parser.Parse(new MemoryStream(Encoding.UTF8.GetBytes(file)));
            // When g ← obj_to_group(parser)
            var g = parser.ObjToGroup();
            // Then g includes "FirstGroup" from parser
            Assert.IsTrue(g.Childs.OfType<CrtGroup>().Any(child => child.Name == "FirstGroup"));
            // And g includes "SecondGroup" from parser
            Assert.IsTrue(g.Childs.OfType<CrtGroup>().Any(child => child.Name == "SecondGroup"));
        }

        #endregion

        #region Smooth triangles

        // Scenario: Vertex normal records
        [Test]
        public void VertexNormalRecords()
        {
            // Given file ← a file containing:
            // """
            // vn 0 0 1
            // vn 0.707 0 -0.707
            // vn 1 2 3
            // """
            var file =
@"
vn 0 0 1
vn 0.707 0 -0.707
vn 1 2 3
";
            // When parser ← parse_obj_file(file)
            var parser = CrtFactory.FileFormatFactory.ObjParser;
            parser.Parse(new MemoryStream(Encoding.UTF8.GetBytes(file)));
            // Then parser.normals[1] = vector(0, 0, 1)
            Assert.IsTrue(parser.Normals[1] == CrtFactory.CoreFactory.Vector(0, 0, 1));
            // And parser.normals[2] = vector(0.707, 0, -0.707)
            Assert.IsTrue(parser.Normals[2] == CrtFactory.CoreFactory.Vector(0.707, 0, -0.707));
            // And parser.normals[3] = vector(1, 2, 3)
            Assert.IsTrue(parser.Normals[3] == CrtFactory.CoreFactory.Vector(1, 2, 3));
        }

        // Scenario: Faces with normals
        [Test]
        public void FacesWithNormals()
        {
            // Given file ← a file containing:
            // """
            // v 0 1 0
            // v -1 0 0
            // v 1 0 0
            // vn -1 0 0
            // vn 1 0 0
            // vn 0 1 0
            // f 1//3 2//1 3//2
            // f 1/0/3 2/102/1 3/14/2
            // """
            var file =
@"
v 0 1 0
v -1 0 0
v 1 0 0
vn -1 0 0
vn 1 0 0
vn 0 1 0
f 1//3 2//1 3//2
f 1/0/3 2/102/1 3/14/2
";
            // When parser ← parse_obj_file(file)
            var parser = CrtFactory.FileFormatFactory.ObjParser;
            parser.Parse(new MemoryStream(Encoding.UTF8.GetBytes(file)));
            // And g ← parser.default_group
            var g = parser.DefaultGroup;
            // And t1 ← first child of g
            var t1 = g.Childs[0] as CrtSmoothTriangle;
            // And t2 ← second child of g
            var t2 = g.Childs[1] as CrtSmoothTriangle;
            //
            Assert.IsNotNull(t1);
            Assert.IsNotNull(t2);
            //
            // Then t1.p1 = parser.vertices[1]
            Assert.IsTrue(t1.P1 == parser.Vertices[1]);
            // And t1.p2 = parser.vertices[2]
            Assert.IsTrue(t1.P2 == parser.Vertices[2]);
            // And t1.p3 = parser.vertices[3]
            Assert.IsTrue(t1.P3 == parser.Vertices[3]);
            // And t1.n1 = parser.normals[3]
            Assert.IsTrue(t1.N1 == parser.Normals[3]);
            // And t1.n2 = parser.normals[1]
            Assert.IsTrue(t1.N2 == parser.Normals[1]);
            // And t1.n3 = parser.normals[2]
            Assert.IsTrue(t1.N3 == parser.Normals[2]);
            // And t2 = t1
            Assert.IsTrue(t2.P1 == parser.Vertices[1]);
            Assert.IsTrue(t2.P2 == parser.Vertices[2]);
            Assert.IsTrue(t2.P3 == parser.Vertices[3]);
            Assert.IsTrue(t2.N1 == parser.Normals[3]);
            Assert.IsTrue(t2.N2 == parser.Normals[1]);
            Assert.IsTrue(t2.N3 == parser.Normals[2]);
        }

        #endregion
    }
}
