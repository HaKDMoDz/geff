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
using TheGrid.Model;
using TheGrid.Logic.Render;
using TheGrid.Logic.Controller;
using TheGrid.Logic.GamePlay;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using TheGrid.Common;
using TheGrid.Logic.UI;
using TheGrid.Logic.Sound;

namespace TheGrid
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class GameEngine : Microsoft.Xna.Framework.Game
    {
        [DllImport("user32.dll")]
        static extern bool MoveWindow(IntPtr hWnd, int X, int Y, int nWidth, int nHeight, bool bRepaint);

        public RenderLogic Render { get; set; }
        public ControllerLogic Controller { get; set; }
        public GamePlayLogic GamePlay { get; set; }
        public UILogic UI { get; set; }
        public SoundLogic Sound { get; set; }

        public GraphicsDeviceManager Graphics;
        public bool Mini = false;

        private TimeSpan lastElaspedReset = TimeSpan.Zero;
        private int count = 0;

        public GameEngine()
        {
            Mini = (Environment.MachineName == "P64L03BIB69");

            Graphics = new GraphicsDeviceManager(this);

            if (Mini)
            {
                Graphics.PreferredBackBufferWidth = 600;
                Graphics.PreferredBackBufferHeight = 600;
            }
            else
            {
                Graphics.PreferredBackBufferWidth = 800;
                Graphics.PreferredBackBufferHeight = 800;
                
                //Graphics.PreferredBackBufferWidth = 1680;
                //Graphics.PreferredBackBufferHeight = 1050;

                //Graphics.PreferredBackBufferWidth = 1360;
                //Graphics.PreferredBackBufferHeight = 768;

                Graphics.PreferredBackBufferWidth = 1600;
                Graphics.PreferredBackBufferHeight = 900;
                
                //Graphics.IsFullScreen = true;
            }

            IsFixedTimeStep = false;
            Graphics.SynchronizeWithVerticalRetrace = false;
            Graphics.ApplyChanges();

            this.IsMouseVisible = true;
            this.Window.Title = "Analyse";
            Content.RootDirectory = "Content";
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            if (Mini)
            {
                Control window = Control.FromHandle(this.Window.Handle);
                window.Location = new System.Drawing.Point(1950, 350);
                window.Location = new System.Drawing.Point(1950, 350);

                //window.Location = new System.Drawing.Point(1950, 850);
                //window.Location = new System.Drawing.Point(1950, 850);
            }

            base.Initialize();

            Context.GameEngine = this;
            //VisualStyle.OpenVisualStyle("LightGray");
            VisualStyle.OpenVisualStyle("AlmostDarkGrayBlue");
            
            Render = new RenderLogic(this);
            Render.InitRender();
            UI = new UILogic(this);
            Sound = new SoundLogic(this);
            GamePlay = new GamePlayLogic(this);
            Controller = new ControllerLogic(this);
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
            if (IsActive)
            {
                Controller.UpdateBegin(gameTime);

                UI.Update(gameTime);

                Controller.UpdateEnd(gameTime);
            }

            GamePlay.Update(gameTime);

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            Render.Draw(gameTime);

            UI.Draw(gameTime);

            base.Draw(gameTime);

            count++;
            if (gameTime.TotalGameTime.Subtract(lastElaspedReset).TotalMilliseconds >= 1000)
            {
                lastElaspedReset = gameTime.TotalGameTime;
                Window.Title = count.ToString();
                count = 0;
            }
        }
    }
}
