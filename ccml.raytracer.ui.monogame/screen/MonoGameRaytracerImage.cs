using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ccml.raytracer.engine.core;
using ccml.raytracer.ui.screen;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ccml.raytracer.ui.monogame.screen
{
    public class MonoGameRaytracerImage : IRaytracerImage
    {
        MonoGameRaytracerContext _context;
        Texture2D _image;

        public object Image => _image;

        public int Width => _context.Width;

        public int Heigth => _context.Height;

        internal MonoGameRaytracerImage(MonoGameRaytracerContext context)
        {
            _context = context;
            InitializeImage();
        }

        private void InitializeImage()
        {
            Color[] data = new Color[_context.Width * _context.Height];
            for (int pixel = 0; pixel < data.Length; pixel++)
            {
                data[pixel] = Color.Black;
            }
            _image = new Texture2D(_context.GraphicsDevice, _context.Width, _context.Height);
            _image.SetData(data);
        }

        public void RefreshPointsColors(CrtCanvas canvas)
        {
            Color[] data = new Color[_context.Width * _context.Height];
            for (int w = 0; w < _context.Width; w++)
            {
                for (int h = 0; h < _context.Height; h++)
                {
                    var pointColor = canvas[w,h];
                    data[h * _image.Width + w] = Color.FromNonPremultiplied((int)(255 * pointColor.Red), (int)(255 * pointColor.Green), (int)(255 * pointColor.Blue), 255);
                }
            }
            _image = new Texture2D(_context.GraphicsDevice, _context.Width, _context.Height);
            _image.SetData(data);
        }
    }
}
