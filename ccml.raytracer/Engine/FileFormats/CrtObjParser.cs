using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using ccml.raytracer.Core;
using ccml.raytracer.Shapes;

namespace ccml.raytracer.Engine.FileFormats
{
    public class CrtObjParser
    {
        public int NbrIgnoredLines { get; private set; }

        public List<CrtPoint> Vertices { get; } = new List<CrtPoint>()
        {
            CrtFactory.CoreFactory.Point(0, 0, 0)
        };

        public List<CrtVector> Normals { get; } = new List<CrtVector>()
        {
            CrtFactory.CoreFactory.Vector(0, 0, 0)
        };

        public CrtGroup DefaultGroup = new CrtGroup();

        private CrtGroup _currentGroup = null;

        public CrtObjParser()
        {
            _currentGroup = DefaultGroup;
        }

        public void LoadFile(string filePath)
        {
            using var fs = File.Open(filePath, FileMode.Open, FileAccess.Read);
            Parse(fs);
        }

        public void Parse(Stream fileContent)
        {
            using var sr = new StreamReader(fileContent);
            string line = null;
            while ((line = sr.ReadLine()) != null)
            {
                ParseLine(line);
            }
        }

        private void ParseLine(string line)
        {
            if(string.IsNullOrWhiteSpace(line)) return;
            line = line.Trim();
            if (line.StartsWith("v ")) ParseVerticeLine(line);
            if (line.StartsWith("vn ")) ParseNormalLine(line);
            if (line.StartsWith("f ")) ParseFaceLine(line);
            if (line.StartsWith("g ")) ParseGroupLine(line);
            NbrIgnoredLines++;
        }

        private void ParseGroupLine(string line)
        {
            var groupName = line.Substring(1).Trim();
            var newGroup = CrtFactory.ShapeFactory.Group();
            newGroup.Name = groupName;
            DefaultGroup.Add(newGroup);
            _currentGroup = newGroup;
        }

        private (bool, double, string) ExtractDoubleFromString(string s)
        {
            var pos = s.IndexOf(' ');
            string number = (pos == -1) ? s : s.Substring(0, pos);
            try
            {
                var d = double.Parse(number, CultureInfo.InvariantCulture);
                return (true, d, ((pos == -1) ? "" : s.Substring(pos).Trim()));
            }
            catch
            {
                return (false, 0, s);
            }
        }

        private (bool, int, int, int, string) ExtractFaceVerticeInfoIntFromString(string s)
        {
            var pos = s.IndexOf(' ');
            var isLastVertice = pos == -1;
            string verticeNumber = isLastVertice ? s : s.Substring(0, pos);
            string textureNumber = null;
            string normalNumber = null;
            //
            var pos1Separator = verticeNumber.IndexOf('/');
            if (pos1Separator != -1)
            {
                textureNumber = verticeNumber.Substring(pos1Separator+1);
                verticeNumber = verticeNumber.Substring(0, pos1Separator);
                //
                var pos2Separator = textureNumber.IndexOf('/');
                if (pos2Separator != -1)
                {
                    normalNumber = textureNumber.Substring(pos2Separator + 1);
                    textureNumber = textureNumber.Substring(0, pos2Separator);
                }
            }
            try
            {
                var verticeIndex = int.Parse(verticeNumber, CultureInfo.InvariantCulture);
                var textureIndex = string.IsNullOrWhiteSpace(textureNumber) ? 0 : int.Parse(textureNumber, CultureInfo.InvariantCulture);
                var normalIndex = string.IsNullOrWhiteSpace(normalNumber) ? 0 : int.Parse(normalNumber, CultureInfo.InvariantCulture);
                return (true, verticeIndex, textureIndex, normalIndex, (isLastVertice ? "" : s.Substring(pos).Trim()));
            }
            catch
            {
                return (false, 0, 0, 0, s);
            }
        }

        private void ParseVerticeLine(string line)
        {
            var xyzStr = line.Substring(1).Trim();

            (bool xOk, double x, string yzStr) = ExtractDoubleFromString(xyzStr);
            if (!xOk)
            {
                throw new Exception($"OBJ PARSER : Vertice line bad format : {line}");
            }
            (bool yOk, double y, string zStr) = ExtractDoubleFromString(yzStr);
            if (!yOk)
            {
                throw new Exception($"OBJ PARSER : Vertice line bad format : {line}");
            }
            (bool zOk, double z, string emptyStr) = ExtractDoubleFromString(zStr);
            if (!zOk)
            {
                throw new Exception($"OBJ PARSER : Vertice line bad format : {line}");
            }

            try
            {
                Vertices.Add(CrtFactory.CoreFactory.Point(x, y, z));
            }
            catch (Exception e)
            {
                Console.WriteLine(e.StackTrace);
                throw new Exception($"OBJ PARSER : Vertice line bad format : {line}");
            }
        }

        private void ParseNormalLine(string line)
        {
            var xyzStr = line.Substring(2).Trim();

            (bool xOk, double x, string yzStr) = ExtractDoubleFromString(xyzStr);
            if (!xOk)
            {
                throw new Exception($"OBJ PARSER : Normal line bad format : {line}");
            }
            (bool yOk, double y, string zStr) = ExtractDoubleFromString(yzStr);
            if (!yOk)
            {
                throw new Exception($"OBJ PARSER : Normal line bad format : {line}");
            }
            (bool zOk, double z, string emptyStr) = ExtractDoubleFromString(zStr);
            if (!zOk)
            {
                throw new Exception($"OBJ PARSER : Normal line bad format : {line}");
            }

            try
            {
                Normals.Add(CrtFactory.CoreFactory.Vector(x, y, z));
            }
            catch (Exception e)
            {
                Console.WriteLine(e.StackTrace);
                throw new Exception($"OBJ PARSER : Normal line bad format : {line}");
            }
        }

        private void ParseFaceLine(string line)
        {
            var edgesStr = line.Substring(1).Trim();

            try
            {
                CrtPoint p1 = null;
                CrtPoint p2 = null;
                CrtPoint p3 = null;
                CrtVector n1 = null;
                CrtVector n2 = null;
                CrtVector n3 = null;
                var ended = false;
                do
                {
                    (bool ok, int verticeIndex, int textureIndex, int normalIndex, string remaining) = ExtractFaceVerticeInfoIntFromString(edgesStr);
                    if (!ok)
                    {
                        throw new Exception($"OBJ PARSER : Face line bad format : {line}");
                    }
                    if (p1 is null)
                    {
                        p1 = Vertices[verticeIndex];
                        n1 = normalIndex == 0 ? null : Normals[normalIndex];
                    }
                    else if (p2 is null)
                    {
                        p2 = Vertices[verticeIndex];
                        n2 = normalIndex == 0 ? null : Normals[normalIndex];
                    }
                    else if (p3 is null)
                    {
                        p3 = Vertices[verticeIndex];
                        n3 = normalIndex == 0 ? null : Normals[normalIndex];
                    }
                    else
                    {
                        throw new Exception("Bad algo ==> correct the code !");
                    }

                    if (!(p3 is null))
                    {
                        _currentGroup.Add(
                            n3 is null ?
                                CrtFactory.ShapeFactory.Triangle(p1, p2, p3)
                                :
                                CrtFactory.ShapeFactory.SmoothTriangle(p1, p2, p3, n1, n2, n3)
                        );
                        p2 = p3;
                        p3 = null;
                        n2 = n3;
                        n3 = null;
                    }
                    ended = string.IsNullOrWhiteSpace(remaining);
                    edgesStr = remaining;
                } while (!ended);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.StackTrace);
                // throw new Exception($"OBJ PARSER : Face line bad format : {line}");
            }
        }

        public CrtGroup ObjToGroup() => DefaultGroup;
    }
}
