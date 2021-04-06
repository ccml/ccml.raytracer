using System;
using System.Collections.Generic;
using System.Linq;
using ccml.raytracer.Core;
using ccml.raytracer.Lights;
using ccml.raytracer.Materials;
using ccml.raytracer.Shapes;

namespace ccml.raytracer.Engine
{
    public class CrtEngine
    {
        internal CrtEngine()
        {
            
        }

        public CrtIntersection Hit(IList<CrtIntersection> intersections)
        {
            return intersections.FirstOrDefault(i => CrtReal.CompareTo(i.T, 0.0) >= 0);
        }

        public CrtColor Lighting(CrtMaterial material, CrtShape theObject, CrtPointLight light, CrtPoint hitPoint, CrtVector eyeVector, CrtVector normalVector, bool inShadow = false)
        {
            var color = material.Color;
            if (material.HasPattern)
            {
                color = material.Pattern.PatternAt(theObject, hitPoint);
            }
            // combine the surface color with the light's color/intensity
            var effectiveColor = color * light.Intensity;
            //
            // find the direction to the light source
            var lightVector = ~(light.Position - hitPoint);
            //
            // compute the ambient contribution
            var ambient = effectiveColor * material.Ambient;
            //
            CrtColor diffuse = CrtColor.COLOR_BLACK;
            CrtColor specular = CrtColor.COLOR_BLACK;
            //
            if (!inShadow)
            {
                //
                // lightDotNormal represents the cosine of the angle between the
                // lightVector and normalVector. A negative number means the
                // light is on the other side of the surface.
                var lightDotNormal = lightVector * normalVector;
                if (CrtReal.CompareTo(lightDotNormal, 0.0) < 0)
                {
                    // They are already black
                    //diffuse = CrtColor.COLOR_BLACK;
                    //specular = CrtColor.COLOR_BLACK;
                }
                else
                {
                    // compute the diffuse contribution
                    diffuse = effectiveColor * material.Diffuse * lightDotNormal;
                    // reflectDotEye represents the cosine of the angle between the
                    // reflectionVector and the eyeVector. A negative number means the
                    // light reflects away from the eye.
                    var reflectVector = (-lightVector).ReflectBy(normalVector);
                    var reflectDotEye = reflectVector * eyeVector;
                    if (CrtReal.CompareTo(reflectDotEye, 0.0) <= 0)
                    {
                        // It's already black
                        // specular = CrtColor.COLOR_BLACK;
                    }
                    else
                    {
                        // compute the specular contribution
                        var factor = Math.Pow(reflectDotEye, material.Shininess);
                        specular = light.Intensity * material.Specular * factor;
                    }
                }
            }
            //
            return ambient + diffuse + specular;
        }

        public CrtIntersectionComputation PrepareComputations(CrtIntersection intersection, CrtRay r, List<CrtIntersection> intersections = null)
        {
            var comps = new CrtIntersectionComputation();
            comps.T = intersection.T;
            comps.TheObject = intersection.TheObject;
            comps.HitPoint = r.PositionAtTime(intersection.T);
            comps.EyeVector = -r.Direction;
            comps.NormalVector = intersection.TheObject.NormalAt(comps.HitPoint, intersection);
            if (CrtReal.CompareTo(comps.NormalVector * comps.EyeVector, 0) < 0)
            {
                comps.IsInside = true;
                comps.NormalVector = -comps.NormalVector;
            }
            else
            {
                comps.IsInside = false;
            }
            comps.ReflectVector = r.Direction.ReflectBy(comps.NormalVector);
            comps.OverPoint = comps.HitPoint + comps.NormalVector * CrtReal.EPSILON;
            comps.UnderPoint = comps.HitPoint - comps.NormalVector * CrtReal.EPSILON;
            //
            intersections ??= new List<CrtIntersection>() { intersection };
            var containers = new List<CrtShape>();
            var hitReached = false;
            var intersectionsIterator = intersections.GetEnumerator();
            while (!hitReached && intersectionsIterator.MoveNext())
            {
                var anIntersection = intersectionsIterator.Current;
                if (anIntersection == intersection)
                {
                    comps.N1 = containers.Any() ? containers[^1].Material.RefractiveIndex : 1.0;
                }
                if (containers.Contains(anIntersection.TheObject))
                {
                    containers.Remove(anIntersection.TheObject);
                }
                else
                {
                    containers.Add(anIntersection.TheObject);
                }
                if (anIntersection == intersection)
                {
                    comps.N2 = containers.Any() ? containers[^1].Material.RefractiveIndex : 1.0;
                    hitReached = true;
                }
            }
            //
            return comps;
        }

        public CrtColor ShadeHit(CrtWorld w, CrtIntersectionComputation comps, int remaining = 4)
        {
            var shadowed = w.IsShadowed(comps.OverPoint);
            var surface = Lighting(comps.TheObject.Material, comps.TheObject, w.Lights[0], comps.OverPoint, comps.EyeVector, comps.NormalVector, shadowed);
            var reflected = w.ReflectedColor(comps, remaining);
            var refracted = w.RefractedColor(comps, remaining);
            if (comps.TheObject.Material.IsReflective && comps.TheObject.Material.IsTransparent)
            {
                var reflectance = Schlick(comps);
                return surface + reflected * reflectance + refracted * (1 - reflectance);
            }
            else
            {
                return surface + reflected + refracted;
            }
        }

        public double Schlick(CrtIntersectionComputation comps)
        {
            // find the cosine of the angle between the eye and normal vectors
            var cosI = comps.EyeVector * comps.NormalVector;
            // total internal reflection can only occur if n1 > n2
            if (CrtReal.CompareTo(comps.N1, comps.N2) > 0)
            {
                var nRatio = comps.N1 / comps.N2;
                var sin2T = nRatio * nRatio * (1.0 - cosI * cosI);
                if (CrtReal.CompareTo(sin2T, 1.0) > 0)
                {
                    return 1.0;
                }
                // compute cosine of theta_t using trig identity
                var cosT = Math.Sqrt(1.0 - sin2T);
                cosI = cosT;
            }
            var r0 = Math.Pow((comps.N1 - comps.N2) / (comps.N1 + comps.N2), 2);
            return r0 + (1 - r0) * Math.Pow(1 - cosI, 5);
        }
    }
}
