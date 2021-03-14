using ccml.raytracer.engine.core.Lights;
using ccml.raytracer.engine.core.Materials;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ccml.raytracer.engine.core.Shapes;
using ccml.raytracer.tests.math.core;

namespace ccml.raytracer.engine.core.Engine
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
            CrtColor diffuse = CrtFactory.Color(0, 0, 0);
            CrtColor specular = CrtFactory.Color(0, 0, 0);
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
                    diffuse = CrtFactory.Color(0, 0, 0);
                    specular = CrtFactory.Color(0, 0, 0);
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
                        specular = CrtFactory.Color(0, 0, 0);
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

        public CrtIntersectionComputation PrepareComputations(CrtIntersection intersection, CrtRay r)
        {
            var comps = new CrtIntersectionComputation();
            comps.T = intersection.T;
            comps.TheObject = intersection.TheObject;
            comps.HitPoint = r.PositionAtTime(intersection.T);
            comps.EyeVector = -r.Direction;
            comps.NormalVector = intersection.TheObject.NormalAt(comps.HitPoint);
            if (CrtReal.CompareTo(comps.NormalVector * comps.EyeVector, 0) < 0)
            {
                comps.IsInside = true;
                comps.NormalVector = -comps.NormalVector;
            }
            else
            {
                comps.IsInside = false;
            }
            comps.OverPoint = comps.HitPoint + comps.NormalVector * CrtReal.EPSILON;
            return comps;
        }

        public CrtColor ShadeHit(CrtWorld w, CrtIntersectionComputation comps)
        {
            var shadowed = w.IsShadowed(comps.OverPoint);
            return Lighting(comps.TheObject.Material, comps.TheObject, w.Lights[0], comps.OverPoint, comps.EyeVector, comps.NormalVector, shadowed);
        }
    }
}
