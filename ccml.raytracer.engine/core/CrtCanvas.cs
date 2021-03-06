using System.IO;
using System.Text;

namespace ccml.raytracer.engine.core
{
    public class CrtCanvas
    {
        private CrtColor[] _buffer;

        public int Width { get; set; }
        public int Height { get; set; }

        internal CrtCanvas(int width, int height)
        {
            Width = width;
            Height = height;
            //
            _buffer = new CrtColor[width * height];
            for (int h = 0; h < height; h++)
            {
                for (int w = 0; w < width; w++)
                {
                    _buffer[h * width + w] = CrtFactory.Color(0, 0, 0);
                }
            }
        }

        public CrtColor this[int w, int h]
        {
            get => _buffer[h * Width + w]; // pixel_at
            set => _buffer[h * Width + w] = value; // write_pixel
        }

        private int ToIntColor(double colorComponent)
        {
            var result = (int)(colorComponent * 255);
            if (result < 0) return 0;
            if (result > 255) return 255;
            return result;
        }

        public void ToPPM(StreamWriter sw)
        {
            // Constructing the PPM header
            //   P3
            //   <Width> <Height>
            //   <the maximum color value>
            sw.WriteLine("P3");
            sw.WriteLine($"{Width} {Height}");
            sw.WriteLine("255");
            //
            // NB) Splitting long lines in PPM files (max 70 characters)
            // NB) The string "<R> <G> <B> " is max length is 12 characters
            //     because 3 chiffers mas + 1 blanc for each color component => 4 * 3 = 12
            var splitLineLength = 70 - 12;
            for (int h = 0; h < Height; h++)
            {
                var line = new StringBuilder(70);
                for (int w = 0; w < Width; w++)
                {
                    // NB) The string "<R> <G> <B> " is max length is 12 characters
                    //     because 3 chiffers mas + 1 blanc for each color component => 4 * 3 = 12
                    if (line.Length >= splitLineLength)
                    {
                        sw.WriteLine(line);
                        line = new StringBuilder(70);
                    }
                    var color = this[w, h];
                    line.Append($"{ToIntColor(color.Red)} {ToIntColor(color.Green)} {ToIntColor(color.Blue)} ");
                }
                sw.WriteLine(line);
            }
            sw.Flush();
        }
    }
}
