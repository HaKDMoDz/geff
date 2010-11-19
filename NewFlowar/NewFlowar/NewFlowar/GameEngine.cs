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
using NewFlowar.Model;
using NewFlowar.Logic.Render;
using NewFlowar.Logic.Controller;
using NewFlowar.Logic.GamePlay;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using NewFlowar.Common;

namespace NewFlowar
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
        public GraphicsDeviceManager Graphics;
        public bool Mini = false;

        public GameEngine()
        {
            Mini = (Environment.MachineName == "P64L03BIB69");

            Graphics = new GraphicsDeviceManager(this);

            if (Mini)
            {
                Graphics.PreferredBackBufferWidth = 400;
                Graphics.PreferredBackBufferHeight = 400;
            }
            else
            {
                Graphics.PreferredBackBufferWidth = 1000;
                Graphics.PreferredBackBufferHeight = 1000;
            }

            
            this.IsMouseVisible = true;

            Content.RootDirectory = "Content";

            GamePlay = new GamePlayLogic();
            Render = new RenderLogic(this);
            Controller = new ControllerLogic(this);


            //List<double> l = new List<double>();

            //for (int i = 0; i < 500; i++)
            //{
            //    l.Add(Tools.GetBellCurvePoint((double)i / 500, 0.5));
            //}
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
                window.Location = new System.Drawing.Point(2500, 0);
            }

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
            Controller.UpdateBegin(gameTime);

            Controller.UpdateEnd(gameTime);

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
