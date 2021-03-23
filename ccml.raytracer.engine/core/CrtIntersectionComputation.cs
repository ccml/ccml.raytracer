using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ccml.raytracer.engine.core;
using ccml.raytracer.engine.core.Shapes;

namespace ccml.raytracer.tests.math.core
{
    public class CrtIntersectionComputation
    {
        /// <summary>
        /// Intant t when the ray hit the object
        /// </summary>
        public double T { get; internal set; }
        
        /// <summary>
        /// The object being hit by the ray
        /// </summary>
        public CrtShape TheObject { get; internal set; }
        
        /// <summary>
        /// The point where the ray hit the object
        /// </summary>
        public CrtPoint HitPoint { get; internal set; }
        
        /// <summary>
        /// The vector from the hit point to the ray origin
        /// </summary>
        public CrtVector EyeVector { get; internal set; }
        
        /// <summary>
        /// The normal at the hit point
        /// </summary>
        public CrtVector NormalVector { get; internal set; }
        
        /// <summary>
        /// The reflected vector (where the ray vector reflects compared to the normal)
        /// </summary>
        public CrtVector ReflectVector { get; internal set; }
        
        /// <summary>
        /// Say if the hit is inside the object (the ray comes from the inside)
        /// </summary>
        public bool IsInside { get; internal set; }
        
        /// <summary>
        /// Origin of the reflected ray (a little above the hit point to manage calculation comparisons errors effects)
        /// </summary>
        public CrtPoint OverPoint { get; internal set; }

        /// <summary>
        /// Origin of the refracted ray (a little below the hit point to manage calculation comparisons errors effects)
        /// </summary>
        public CrtPoint UnderPoint { get; internal set; }

        /// <summary>
        ///  Refractive indice belonging to the material being exited
        /// </summary>
        public double N1 { get; internal set; }

        /// <summary>
        ///  Refractive indice belonging to the material being entered
        /// </summary>
        public double N2 { get; internal set; }
    }
}
