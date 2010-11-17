using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace NewFlowar
{
    public class Render
    {
        public SpriteBatch SpriteBatch;

        public Map Map { get; set; }
        public Game Game { get; set; }

        BasicEffect effect;
        VertexBuffer vBuffer;

        Matrix View;
        Matrix Projection;
        Matrix World;

        public Vector3 CameraPosition = new Vector3(15f, 15f, -80.0f);
        public Vector3 CameraTarget = new Vector3(15f, 15f, 0f);
        public Vector3 CameraUp = Vector3.Up;

        public Render(Game game, Map map)
        {
            this.Game = game;
            this.Map = map;
        }

        public void InitRender()
        {
            CreateCamera();
            CreateShader();

            CreateVertex();

            SpriteBatch = new SpriteBatch(Game.GraphicsDevice);
        }

        public void UpdateCamera()
        {
            View = Matrix.CreateLookAt(CameraPosition, CameraTarget, CameraUp);
            effect.View = View;
        }

        //Création de la caméra
        private void CreateCamera()
        {
            View = Matrix.CreateLookAt(CameraPosition, CameraTarget, CameraUp);
            Projection = Matrix.CreatePerspectiveFieldOfView(MathHelper.Pi / 4f, Game.GraphicsDevice.Viewport.Width / Game.GraphicsDevice.Viewport.Height, 0.01f, 1000.0f);
            //Projection = Matrix.CreatePerspective(this.GraphicsDevice.Viewport.Width, this.GraphicsDevice.Viewport.Height, 0.01f, 1000.0f);
            World = Matrix.Identity;
        }

        private void CreateShader()
        {
            effect = new BasicEffect(Game.GraphicsDevice);
            //effect.World = Matrix.CreateRotationY((float)gameTime.TotalGameTime.TotalMilliseconds/800);
            effect.View = View;
            effect.Projection = Projection;
            effect.World = World;

            effect.PreferPerPixelLighting = true;
            effect.VertexColorEnabled = true;
        }

        public void CreateVertex()
        {
            vBuffer = new VertexBuffer(Game.GraphicsDevice, typeof(VertexPositionColor), Map.Cells.Count * 12, BufferUsage.None);

            List<VertexPositionColor> vertex = new List<VertexPositionColor>();

            foreach (Cell cell in Map.Cells)
            {
                vertex.Add(new VertexPositionColor(cell.Points[1], Color.White));
                if(cell == Map.Cells[0])
                    vertex.Add(new VertexPositionColor(cell.Points[2], Color.White));
                else if(cell == Map.Cells[1])
                    vertex.Add(new VertexPositionColor(cell.Points[2], Color.White));
                else
                    vertex.Add(new VertexPositionColor(cell.Points[2], Color.White));

                vertex.Add(new VertexPositionColor(cell.Points[3], Color.White));

                vertex.Add(new VertexPositionColor(cell.Points[3], Color.White));
                vertex.Add(new VertexPositionColor(cell.Points[4], Color.White));
                vertex.Add(new VertexPositionColor(cell.Points[6], Color.White));

                vertex.Add(new VertexPositionColor(cell.Points[4], Color.White));
                vertex.Add(new VertexPositionColor(cell.Points[5], Color.White));
                vertex.Add(new VertexPositionColor(cell.Points[6], Color.White));

                vertex.Add(new VertexPositionColor(cell.Points[1], Color.White));
                vertex.Add(new VertexPositionColor(cell.Points[3], Color.White));
                vertex.Add(new VertexPositionColor(cell.Points[6], Color.White));
            }


            vBuffer.SetData<VertexPositionColor>(vertex.ToArray());

            Game.GraphicsDevice.SetVertexBuffer(vBuffer);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        public void Draw(GameTime gameTime)
        {
            Game.GraphicsDevice.Clear(Color.Black);

            foreach (EffectPass pass in effect.CurrentTechnique.Passes)
            {
                pass.Apply();
                Game.GraphicsDevice.DrawPrimitives(PrimitiveType.TriangleList, 0, Map.Cells.Count * 12);
            }
        }
    }
}
