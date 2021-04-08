using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ccml.raytracer.Core;
using ccml.raytracer.Engine;

namespace ccml.raytracer.Shapes
{
    public class CrtCSG : CrtGroup
    {
        public const string UNION = "union";
        public const string INTERSECTION = "intersection";
        public const string DIFFERENCE = "difference";

        public string Operation { get; }
        public CrtShape Left => Childs[0];
        public CrtShape Right => Childs[1];

        private static Dictionary<int, bool> _unionTruthTable = new Dictionary<int, bool>();
        private static Dictionary<int, bool> _intersectionTruthTable = new Dictionary<int, bool>();
        private static Dictionary<int, bool> _differenceTruthTable = new Dictionary<int, bool>();

        static CrtCSG()
        {
            //    | op           | lhit  | inl   | inr   | result |
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
                    while ((line = sr.ReadLine()) != null)
                    {
                        if (string.IsNullOrWhiteSpace(line)) continue;
                        line = line.Trim();
                        line = line.Substring(1);
                        line = line.Substring(0, line.Length - 1);
                        var parts = line.Split('|');
                        var operation = parts[0].Trim();
                        var lhit = bool.Parse(parts[1].Trim());
                        var inl = bool.Parse(parts[2].Trim());
                        var inr = bool.Parse(parts[3].Trim());
                        var operationResult = bool.Parse(parts[4].Trim());
                        switch (operation)
                        {
                            case UNION:
                                _unionTruthTable[ToLhit(lhit) + ToInL(inl) + ToInR(inr)] = operationResult;
                                break;
                            case INTERSECTION:
                                _intersectionTruthTable[ToLhit(lhit) + ToInL(inl) + ToInR(inr)] = operationResult;
                                break;
                            case DIFFERENCE:
                                _differenceTruthTable[ToLhit(lhit) + ToInL(inl) + ToInR(inr)] = operationResult;
                                break;
                        }
                    }
                }
            }
        }

        public static int ToLhit(bool b) => b ? 1 : 0;
        public static int ToInL(bool b) => b ? 2 : 0;
        public static int ToInR(bool b) => b ? 4 : 0;

        public static bool IntersectionAllowed(string operation, bool lhit, bool inl, bool inr)
        {
            switch (operation)
            {
                case UNION:
                    return _unionTruthTable[ToLhit(lhit) + ToInL(inl) + ToInR(inr)];
                case INTERSECTION:
                    return _intersectionTruthTable[ToLhit(lhit) + ToInL(inl) + ToInR(inr)];
                case DIFFERENCE:
                    return _differenceTruthTable[ToLhit(lhit) + ToInL(inl) + ToInR(inr)];
                default:
                    throw new Exception($"CrtCSG : unknown operation '{operation}'");
            }
        }

        public CrtCSG(string operation, CrtShape left, CrtShape right)
        {
            this.Operation = operation;
            Add(left);
            Add(right);
        }

        public override IList<CrtIntersection> LocalIntersect(CrtRay r)
        {
            var leftXs = Left.Intersect(r);
            var rightXs = Right.Intersect(r);
            var allXs = new List<CrtIntersection>();
            allXs.AddRange(leftXs);
            allXs.AddRange(rightXs);
            var xs = CrtFactory.EngineFactory.Intersections(allXs.ToArray());
            return FilterIntersections(xs);
        }

        public IList<CrtIntersection> FilterIntersections(List<CrtIntersection> xs)
        {
            // begin outside of both children
            var inl = false;
            var inr = false;
            // prepare a list to receive the filtered intersections
            var result = new List<CrtIntersection>();
            foreach (var intersection in xs)
            {
                // if i.object is part of the "left" child, then lhit is true
                var lhit = Left.Includes(intersection.TheObject);
                if (IntersectionAllowed(Operation, lhit, inl, inr))
                {
                    result.Add(intersection);
                }
                // depending on which object was hit, toggle either inl or inr
                if (lhit)
                {
                    inl = !inl;
                }
                else
                {
                    inr = !inr;
                }
            }
            return result;
        }
    }
}
