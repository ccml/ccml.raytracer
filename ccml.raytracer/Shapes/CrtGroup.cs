using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ccml.raytracer.Core;
using ccml.raytracer.Engine;
using ccml.raytracer.Shapes.Helpers;

namespace ccml.raytracer.Shapes
{
    public class CrtGroup : CrtShape
    {

        private bool IntersectBoundingBox(CrtRay r)
        {
            // No bounding box == infinit bounding box ==> intersection
            if (ObjectBoundingBox == null) return true;

            // check if intersection with bounding box
            (double tmin, double tmax) = BoxIntersectionHelper.IntersectBoundinBox(r, ObjectBoundingBox);
            return CrtReal.CompareTo(tmin, tmax) <= 0;
        }

        public override IList<CrtIntersection> LocalIntersect(CrtRay r)
        {
            // check if intersection with bounding box
            if (!IntersectBoundingBox(r))
            {
                return new List<CrtIntersection>();
            }

            var xs = new List<CrtIntersection>();
            if (IsEmpty) return xs;
            foreach (var child in Childs)
            {
                xs.AddRange(child.Intersect(r));
            }
            return CrtFactory.EngineFactory.Intersections(xs.ToArray());
        }

        public override CrtVector LocalNormalAt(CrtPoint point)
        {
            throw new NotImplementedException();
        }

        public IList<CrtShape> Childs { get; } = new List<CrtShape>();

        public bool IsEmpty => !Childs.Any();
        public bool Contains(CrtShape shape) => Childs.Contains(shape);

        public CrtGroup Add(params CrtShape[] shapes)
        {
            foreach (var shape in shapes)
            {
                Childs.Add(shape);
                shape.Parent = this;
            }
            ResetBounds();
            return this;
        }

        public override CrtBoundingBox ObjectBounds()
        {
            CrtBoundingBox bbox = null;
            foreach (var child in Childs)
            {
                var childBbox = child.Bounds();
                if (childBbox == null) return null;
                if (bbox != null)
                {
                    bbox.Minimum.X = Math.Min(bbox.Minimum.X, childBbox.Minimum.X);
                    bbox.Minimum.Y = Math.Min(bbox.Minimum.Y, childBbox.Minimum.Y);
                    bbox.Minimum.Z = Math.Min(bbox.Minimum.Z, childBbox.Minimum.Z);

                    bbox.Maximum.X = Math.Max(bbox.Maximum.X, childBbox.Maximum.X);
                    bbox.Maximum.Y = Math.Max(bbox.Maximum.Y, childBbox.Maximum.Y);
                    bbox.Maximum.Z = Math.Max(bbox.Maximum.Z, childBbox.Maximum.Z);
                }

                if (bbox == null)
                {
                    bbox = new CrtBoundingBox()
                    {
                        Minimum = CrtFactory.CoreFactory.Point(childBbox.Minimum.X, childBbox.Minimum.Y, childBbox.Minimum.Z),
                        Maximum = CrtFactory.CoreFactory.Point(childBbox.Maximum.X, childBbox.Maximum.Y, childBbox.Maximum.Z)
                    };
                }
            }
            return bbox;
        }
    }
}
