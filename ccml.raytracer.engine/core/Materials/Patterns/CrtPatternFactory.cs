using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ccml.raytracer.engine.core.Materials.Patterns.Noises;

namespace ccml.raytracer.engine.core.Materials.Patterns
{
    public class CrtPatternFactory
    {
        /// <summary>
        /// Create a solid color pattern
        ///   => returns always the same color
        /// </summary>
        /// <param name="color">the color</param>
        /// <returns>the pattern</returns>
        public CrtPattern SolidColor(CrtColor color) => new CrtSolidColorPattern(color);

        /// <summary>
        /// Create a stripe pattern
        ///   => colorA if floor(x) mod 2 = 0
        ///      colorB otherwise
        /// </summary>
        /// <param name="colorA">the colorA</param>
        /// <param name="colorB">the colorB</param>
        /// <returns>the pattern</returns>
        public CrtPattern StripePattern(CrtColor colorA, CrtColor colorB) => new CrtStripedPattern(colorA, colorB);

        /// <summary>
        /// Create a gradient pattern
        ///    ==> color = gradientA + (gradientB − gradientA) ∗ (x − floor(x))
        /// </summary>
        /// <param name="gradientA">the gradientA color</param>
        /// <param name="gradientB">the gradientB color</param>
        /// <returns>the pattern</returns>
        public CrtPattern GradientPattern(CrtColor gradientA, CrtColor gradientB) => new CrtGradientPattern(gradientA, gradientB);

        /// <summary>
        /// Create a ring pattern (depends on two dimensions, x and z)
        ///   => colorA if floor(sqrt(x*x+z*z) mod 2 = 0
        ///      colorB otherwise
        /// </summary>
        /// <param name="colorA">the colorA</param>
        /// <param name="colorB">the colorB</param>
        /// <returns>the pattern</returns>
        public CrtPattern RingPattern(CrtColor colorA, CrtColor colorB) => new CrtRingPattern(colorA, colorB);

        /// <summary>
        /// Create a 3D Checker pattern (depends on two dimensions, x and z)
        ///   => colorA if (floor(x) + floor(y) + floor(z)) mod 2 = 0
        ///      colorB otherwise
        /// </summary>
        /// <param name="colorA">the colorA</param>
        /// <param name="colorB">the colorB</param>
        /// <returns>the pattern</returns>
        public CrtPattern Checker3DPattern(CrtColor colorA, CrtColor colorB) => new CrtChecker3DPattern(colorA, colorB);

        /// <summary>
        /// Create a gradient pattern (depends on two dimensions, x and z)
        ///    ==> color = gradientA + (gradientB − gradientA) ∗ (sqrt(x*x+z*z) − floor(sqrt(x*x+z*z)))
        /// </summary>
        /// <param name="gradientA">the gradientA color</param>
        /// <param name="gradientB">the gradientB color</param>
        /// <returns>the pattern</returns>
        public CrtPattern RadialGradientPattern(CrtColor gradientA, CrtColor gradientB) => new CrtRadialGradientPattern(gradientA, gradientB);

        /// <summary>
        /// Create a blended pattern
        ///    ==> color = f( (color returned  by patternA), (color returned by patternB))
        ///                f is the blending method
        /// </summary>
        /// <param name="patternA">the pattern A</param>
        /// <param name="patternB">the pattern B</param>
        /// <param name="blendingMethod">the blendingMethod</param>
        /// <returns>the pattern</returns>
        public CrtPattern BlendedPattern(CrtPattern patternA, CrtPattern patternB, string blendingMethod = CrtBlendedPattern.BLENDING_METHOD_AVERAGE) => new CrtBlendedPattern(patternA, patternB, blendingMethod);

        /// <summary>
        /// Create a pattern where point position on the inner pattern is perturbed by a function
        /// </summary>
        /// <param name="pattern">the inner pattern</param>
        /// <param name="perturbationFunction">the function perturbing the point</param>
        /// <returns>the pattern</returns>
        public CrtPattern PointPerturbedPattern(CrtPattern pattern, Func<CrtPoint, CrtPoint> perturbationFunction) =>
            new CrtPointPerturbedPattern(pattern, perturbationFunction);

        /// <summary>
        /// Create a pattern where the returned color returned by an inner pattern is perturbed by a function
        /// </summary>
        /// <param name="pattern">the inner pattern</param>
        /// <param name="perturbationFunction">the function perturbing the color</param>
        /// <returns>the inner pattern</returns>
        public CrtPattern ColorPerturbedPattern(CrtPattern pattern, Func<CrtPoint, CrtColor, CrtColor> perturbationFunction) =>
            new CrtColorPerturbedPattern(pattern, perturbationFunction);

        /// <summary>
        /// Create a perlin noise procedural texture.
        /// </summary>
        /// <param name="colors">Some indiced colors to create the texture</param>
        /// <returns>the pattern</returns>
        public CrtPerlinNoisePattern PerlinNoisePattern(Dictionary<double, CrtColor> colors) =>
            new CrtPerlinNoisePattern(new CrtPerlin(colors));

        /// <summary>
        /// Create a marble procedural texture.
        /// </summary>
        /// <param name="colors">Some indiced colors to create the texture</param>
        /// <returns>the pattern</returns>
        public CrtPerlinNoisePattern MarblePattern(Dictionary<double, CrtColor> colors) =>
            new CrtPerlinNoisePattern(new CrtMarble(colors));
    }
}
