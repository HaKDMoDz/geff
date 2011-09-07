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
    //string clipboard = String.Empty;

    //        int max = 6;
    //        int count = 0;
    //        for (int n0 = 0; n0 <= max; n0++)
    //        {
    //            for (int b0 = 0; b0 <= max; b0++)
    //            {
    //                for (int n1 = 0; n1 <= max; n1++)
    //                {
    //                    for (int b1 = 0; b1 <= max; b1++)
    //                    {
    //                        if (n0 + b0 <= max && n1 + b1 <= max &&
    //                            b0 != 1 && n1 != 1 && b1 != 1)
    //                        {
    //                            count++;

    //                            Console.WriteLine("=== " + count + " ===");


    //                            float val = n0;
                                
    //                            if (b0 != 0) 
    //                                val += (float)Math.Pow(2, b0);
    //                            if (n1 != 0)
    //                                val += 1f / (float)n1;
    //                            if (b1 != 0)
    //                                val += 1f / (float)Math.Pow(2, b1);

    //                            Console.WriteLine(n0.ToString() + " + 2^" + b0.ToString() + " + 1/" + n1.ToString() + " + 1/2^" + b1.ToString() + " = \t" + val.ToString());

    //                            clipboard += val.ToString() + "\r\n";
    //                        }
    //                    }
    //                }
    //            }
    //        }

    //        Console.ReadKey(true);

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
        public Vector2 NativeResolution { get; set; }
        public bool IsNativeResolution { get; set; }

        public GameEngine()
        {
            Mini = (Environment.MachineName == "P64L03BIB69");

            Graphics = new GraphicsDeviceManager(this);

            if (Mini)
            {
                NativeResolution = new Vector2(600, 600);
            }
            else
            {
                //--- Plein écran PC
                //NativeResolution = new Vector2(1680, 1050);

                ////--- Plein écran TV
                //NativeResolution = new Vector2(1360, 768);

                //--- Plein écran tablette
                NativeResolution = new Vector2(1280,720);

                //--- Plein écran PC Portable parents élo
                //NativeResolution = new Vector2(1600, 900);


                Graphics.PreferredBackBufferWidth = 1440;
                Graphics.PreferredBackBufferHeight = 900;

                Graphics.IsFullScreen = false;
            }

            IsNativeResolution = true;
            Graphics.PreferredBackBufferWidth = (int)NativeResolution.X;
            Graphics.PreferredBackBufferHeight = (int)NativeResolution.Y;
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

        public void ToggleScreen()
        {
            Graphics.IsFullScreen = !Graphics.IsFullScreen;

            Graphics.ApplyChanges();
            Render.InitRender();
        }

        public void ToogleResolution()
        {
            IsNativeResolution = !IsNativeResolution;

            if (IsNativeResolution)
            {
                Graphics.PreferredBackBufferWidth = (int)NativeResolution.X;
                Graphics.PreferredBackBufferHeight = (int)NativeResolution.Y;
            }
            else
            {
                Graphics.PreferredBackBufferWidth = Screen.GetBounds(new System.Drawing.Point(this.Window.ClientBounds.Location.X, this.Window.ClientBounds.Location.Y)).Width;
                Graphics.PreferredBackBufferHeight = Screen.GetBounds(new System.Drawing.Point(this.Window.ClientBounds.Location.X, this.Window.ClientBounds.Location.Y)).Height;
            }

            Graphics.ApplyChanges();
            Render.InitRender();
            UI.Ribbon.Init();
        }
    }
}
