using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ccml.raytracer.Engine;

namespace ccml.raytracer.ui.screen
{
    public interface IRaytracerImage
    {
        int Width { get; }
        int Heigth { get; }
        object Image { get;  }
        void RefreshPointsColors(CrtCanvas canvas);
    }
}
