using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace NewFlowar
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        public Map Map { get; set; }
        public Render Render { get; set; }
        public GraphicsDeviceManager Graphics;

        public Game1()
        {
            Graphics = new GraphicsDeviceManager(this);
            Graphics.PreferredBackBufferWidth = 600;
            Graphics.PreferredBackBufferHeight = 600;

            Content.RootDirectory = "Content";

            Map = new Map(10, 10);
            Map.CreateGrid();

            Render = new Render(this, Map);
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            base.Initialize();
            Render.InitRender();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            KeyboardState keyState = Keyboard.GetState();

            // Allows the game to exit
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || keyState.IsKeyDown(Keys.Space))
                this.Exit();

            bool moveCamera = false;

            if (keyState.IsKeyDown(Keys.M))
            {
                Map.CreateGrid();
                Render.CreateVertex();
            }

            if (keyState.IsKeyDown(Keys.Left))
            {
                Render.CameraPosition.X -= (float)(gameTime.ElapsedGameTime.TotalMilliseconds * 0.1);
                moveCamera = true;
            }

            if (keyState.IsKeyDown(Keys.Right))
            {
                Render.CameraPosition.X += (float)(gameTime.ElapsedGameTime.TotalMilliseconds * 0.1);
                moveCamera = true;
            }

            if (keyState.IsKeyDown(Keys.Up))
            {
                Render.CameraPosition.Y -= (float)(gameTime.ElapsedGameTime.TotalMilliseconds * 0.1);
                moveCamera = true;
            }

            if (keyState.IsKeyDown(Keys.Down))
            {
                Render.CameraPosition.Y += (float)(gameTime.ElapsedGameTime.TotalMilliseconds * 0.1);
                moveCamera = true;
            }

            if (moveCamera)
            {
                Render.UpdateCamera();
            }

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            Render.Draw(gameTime);

            base.Draw(gameTime);
        }
    }
}
