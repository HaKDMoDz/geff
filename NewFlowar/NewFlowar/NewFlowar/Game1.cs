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
        public List<Cell> Cells { get; set; }

        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            graphics.PreferredBackBufferWidth = 600;
            graphics.PreferredBackBufferHeight = 600;
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
            // TODO: Add your initialization logic here

            base.Initialize();

            CreateNewMap();
            CreateCamera();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            // TODO: use this.Content to load your game content here
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

            if (keyState.IsKeyDown(Keys.Left))
            {
                CameraPosition.X -= (float)(gameTime.ElapsedGameTime.TotalMilliseconds * 0.1);
                moveCamera = true;
            }

            if (keyState.IsKeyDown(Keys.Right))
            {
                CameraPosition.X += (float)(gameTime.ElapsedGameTime.TotalMilliseconds * 0.1);
                moveCamera = true;
            }

            if (keyState.IsKeyDown(Keys.Up))
            {
                CameraPosition.Y -= (float)(gameTime.ElapsedGameTime.TotalMilliseconds * 0.1);
                moveCamera = true;
            }

            if (keyState.IsKeyDown(Keys.Down))
            {
                CameraPosition.Y += (float)(gameTime.ElapsedGameTime.TotalMilliseconds * 0.1);
                moveCamera = true;
            }


            if (moveCamera)
            {
                View = Matrix.CreateLookAt(CameraPosition, CameraTarget, CameraUp);
                effect.View = View;
            }

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);


            foreach (EffectPass pass in effect.CurrentTechnique.Passes)
            {
                pass.Apply();
                graphics.GraphicsDevice.DrawPrimitives(PrimitiveType.TriangleList, 0, Map.Cells.Count * 12);
            }



            base.Draw(gameTime);
        }

        BasicEffect effect;
        VertexBuffer vBuffer;

        Matrix View;
        Matrix Projection;
        Matrix World;

        Vector3 CameraPosition = new Vector3(15f, 15f, -80.0f);
        Vector3 CameraTarget = new Vector3(15f, 15f, 0f);
        Vector3 CameraUp = Vector3.Up;

        //Création de la caméra
        private void CreateCamera()
        {
            View = Matrix.CreateLookAt(CameraPosition, CameraTarget, CameraUp);
            Projection = Matrix.CreatePerspectiveFieldOfView(MathHelper.Pi / 4f, this.GraphicsDevice.Viewport.Width / this.GraphicsDevice.Viewport.Height, 0.01f, 1000.0f);
            //Projection = Matrix.CreatePerspective(this.GraphicsDevice.Viewport.Width, this.GraphicsDevice.Viewport.Height, 0.01f, 1000.0f);
            World = Matrix.Identity;

            double r = 2;

            vertexHexagon = new List<Vector3>();
            for (double i = 0; i < 6; i++)
            {
                Vector3 vec = new Vector3();
                vec.X = (float)(Math.Cos(Math.PI / 3 * i) * r);
                vec.Y = (float)(Math.Sin(Math.PI / 3 * i) * r);

                vertexHexagon.Add(vec);
            }


            vBuffer = new VertexBuffer(GraphicsDevice, typeof(VertexPositionColor), Map.Cells.Count * 12, BufferUsage.None);

            List<VertexPositionColor> vertex = new List<VertexPositionColor>();

            foreach (Cell cell in Map.Cells)
            {
                for (int i = 0; i < 4; i++)
                {
                    vertex.Add(new VertexPositionColor(new Vector3(vertexHexagon[0].X + cell.Coord.X, vertexHexagon[0].Y + cell.Coord.Y, cell.Height), Color.White));
                    vertex.Add(new VertexPositionColor(new Vector3(vertexHexagon[1 + i].X + cell.Coord.X, vertexHexagon[1 + i].Y + cell.Coord.Y, cell.Height), Color.Gray));
                    vertex.Add(new VertexPositionColor(new Vector3(vertexHexagon[2 + i].X + cell.Coord.X, vertexHexagon[2 + i].Y + cell.Coord.Y, cell.Height), Color.Black));
                }
            }


            vBuffer.SetData<VertexPositionColor>(vertex.ToArray());

            GraphicsDevice.SetVertexBuffer(vBuffer);


            effect = new BasicEffect(GraphicsDevice);
            //effect.World = Matrix.CreateRotationY((float)gameTime.TotalGameTime.TotalMilliseconds/800);
            effect.View = View;
            effect.Projection = Projection;
            effect.World = World;

            effect.PreferPerPixelLighting = true;
            effect.VertexColorEnabled = true;
        }

        List<Vector3> vertexHexagon = new List<Vector3>();
        public Map Map { get; set; }

        private void CreateNewMap()
        {
            this.Map = new Map(3,3);
            this.Map.CreateGrid();
        }

    }
}
