using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ccml.raytracer.Engine.FileFormats
{
    public class CrtFileFormatFactory
    {
        public CrtObjParser ObjParser => new CrtObjParser();
    }
}
