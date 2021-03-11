using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ccml.raytracer.ui.screen;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ccml.raytracer.ui.monogame.screen
{
    public class MonoGameRaytracerWindow : Game
    {
        private readonly int _width;
        private readonly int _height;
        private readonly Action _renderImage;
        private readonly Action _updateImage;
        MonoGameRaytracerContext _context;

        private IRaytracerImage _image;
        public IRaytracerImage Image => _image;

        public MonoGameRaytracerWindow(
            int width, 
            int height,
            Action renderImage,
            Action updateImage
        )
        {
            _width = width;
            _height = height;
            _renderImage = renderImage;
            _updateImage = updateImage;
            _context = new MonoGameRaytracerContext
            {
                Graphics = new GraphicsDeviceManager(this)

            };
            Content.RootDirectory = "Content";
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            _context.GraphicsDevice = GraphicsDevice;
            _context.SpriteBatch = new SpriteBatch(GraphicsDevice);
            _context.Width = _width;
            _context.Height = _height;
            _image = new MonoGameRaytracerImage(_context);
            Task.Run(() => _renderImage());
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            _updateImage();

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            _context.SpriteBatch.Begin();
            _context.SpriteBatch.Draw((Texture2D)_image.Image, Vector2.Zero, Color.White);
            _context.SpriteBatch.End();

            base.Draw(gameTime);
        }

    }
}
