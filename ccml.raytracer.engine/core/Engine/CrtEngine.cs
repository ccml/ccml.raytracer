﻿using ccml.raytracer.engine.core.Lights;
using ccml.raytracer.engine.core.Materials;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

        public CrtColor Lighting(CrtMaterial material, CrtPointLight light, CrtPoint hitPoint, CrtVector eyeVector, CrtVector normalVector)
        {
            // combine the surface color with the light's color/intensity
            var effectiveColor = ((CrtUniformColorMaterial) material).Color * light.Intensity;
            //
            // find the direction to the light source
            var lightVector = ~(light.Position - hitPoint);
            //
            // compute the ambient contribution
            var ambient = effectiveColor * material.Ambient;
            //
            CrtColor diffuse = null;
            CrtColor specular = null;
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

            return ambient + diffuse + specular;
        }
    }
}