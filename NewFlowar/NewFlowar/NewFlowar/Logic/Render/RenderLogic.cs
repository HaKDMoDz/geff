using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using NewFlowar.Model;

namespace NewFlowar.Logic.Render
{
    public class RenderLogic
    {
        public SpriteBatch SpriteBatch;

        public Map Map
        {
            get
            {
                return this.GameEngine.GamePlay.Map;
            }
        }

        public GameEngine GameEngine { get; set; }

        BasicEffect effect;
        VertexBuffer vBuffer;

        Matrix View;
        Matrix Projection;
        Matrix World;

        public Vector3 CameraPosition = new Vector3(15f, 15f, 0f);
        public Vector3 CameraTarget = new Vector3(15f, 15f, 0f);
        public Vector3 CameraUp = Vector3.Up;
        public float Zoom { get; set; }
        public Vector2 VecTranslation = new Vector2(0, 0);
        public bool updateViewScreen = false;
        public bool doScreenShot = false;

        public RenderLogic(GameEngine gameEngine)
        {
            this.GameEngine = gameEngine;
        }

        public void InitRender()
        {
            CreateCamera();
            CreateShader();

            CreateVertex();

            SpriteBatch = new SpriteBatch(GameEngine.GraphicsDevice);
        }

        //Création de la caméra
        private void CreateCamera()
        {
            Zoom = 50;
            UpdateCamera();
            Projection = Matrix.CreatePerspectiveFieldOfView(MathHelper.Pi / 4f, GameEngine.GraphicsDevice.Viewport.Width / GameEngine.GraphicsDevice.Viewport.Height, 0.01f, 1000.0f);
            //Projection = Matrix.CreatePerspective(this.GraphicsDevice.Viewport.Width, this.GraphicsDevice.Viewport.Height, 0.01f, 1000.0f);
            World = Matrix.Identity;
        }

        private void UpdateCamera()
        {
            View = Matrix.CreateLookAt(CameraPosition + new Vector3(VecTranslation, Zoom), CameraTarget +new Vector3(VecTranslation, 0), CameraUp);
            //effect.View = View;
        }

        private void CreateShader()
        {
            effect = new BasicEffect(GameEngine.GraphicsDevice);
            //effect.World = Matrix.CreateRotationY((float)gameTime.TotalGameTime.TotalMilliseconds/800);
            UpdateShader();

            effect.PreferPerPixelLighting = true;
            effect.VertexColorEnabled = false;
            effect.LightingEnabled = true;
            effect.TextureEnabled = true;
            effect.Texture = GameEngine.Content.Load<Texture2D>("Hexa");
        }

        public void UpdateShader()
        {
            effect.View = View;
            effect.Projection = Projection;
            effect.World = World;
        }

        public void CreateVertex()
        {
            vBuffer = new VertexBuffer(GameEngine.GraphicsDevice, typeof(VertexPositionNormalTexture), Map.Cells.Count * 12, BufferUsage.None);

            List<VertexPositionNormalTexture> vertex = new List<VertexPositionNormalTexture>();

            foreach (Cell cell in Map.Cells)
            {
                vertex.Add(new VertexPositionNormalTexture(Map.Points[cell.Points[3]], Map.Normals[cell.Points[3]], new Vector2(0.75f, 1f)));
                vertex.Add(new VertexPositionNormalTexture(Map.Points[cell.Points[2]], Map.Normals[cell.Points[2]], new Vector2(1f, 0.5f)));
                vertex.Add(new VertexPositionNormalTexture(Map.Points[cell.Points[1]], Map.Normals[cell.Points[1]], new Vector2(0.75f, 0f)));

                vertex.Add(new VertexPositionNormalTexture(Map.Points[cell.Points[6]], Map.Normals[cell.Points[6]], new Vector2(0.75f, 0f)));
                vertex.Add(new VertexPositionNormalTexture(Map.Points[cell.Points[4]], Map.Normals[cell.Points[4]], new Vector2(0.26f, 1f)));
                vertex.Add(new VertexPositionNormalTexture(Map.Points[cell.Points[3]], Map.Normals[cell.Points[3]], new Vector2(0.75f, 1f)));

                vertex.Add(new VertexPositionNormalTexture(Map.Points[cell.Points[6]], Map.Normals[cell.Points[6]], new Vector2(0.26f, 0f)));
                vertex.Add(new VertexPositionNormalTexture(Map.Points[cell.Points[5]], Map.Normals[cell.Points[5]], new Vector2(0f, 0.5f)));
                vertex.Add(new VertexPositionNormalTexture(Map.Points[cell.Points[4]], Map.Normals[cell.Points[4]], new Vector2(0.26f, 1f)));

                vertex.Add(new VertexPositionNormalTexture(Map.Points[cell.Points[6]], Map.Normals[cell.Points[6]], new Vector2(0.26f, 0f)));
                vertex.Add(new VertexPositionNormalTexture(Map.Points[cell.Points[3]], Map.Normals[cell.Points[3]], new Vector2(0.75f, 1f)));
                vertex.Add(new VertexPositionNormalTexture(Map.Points[cell.Points[1]], Map.Normals[cell.Points[1]], new Vector2(0.75f, 0f)));

                //vertex.Add(new VertexPositionNormalTexture(Map.Points[cell.Points[1]], Map.Normals[cell.Points[1]], new Vector2(0.75f, 0f))); 
                //vertex.Add(new VertexPositionNormalTexture(Map.Points[cell.Points[2]], Map.Normals[cell.Points[2]], new Vector2(1f, 0.5f)));
                //vertex.Add(new VertexPositionNormalTexture(Map.Points[cell.Points[3]], Map.Normals[cell.Points[3]], new Vector2(0.75f, 1f)));


                //vertex.Add(new VertexPositionNormalTexture(Map.Points[cell.Points[3]], Map.Normals[cell.Points[3]], new Vector2(0.75f, 1f)));
                //vertex.Add(new VertexPositionNormalTexture(Map.Points[cell.Points[4]], Map.Normals[cell.Points[4]], new Vector2(0.26f, 1f)));
                //vertex.Add(new VertexPositionNormalTexture(Map.Points[cell.Points[6]], Map.Normals[cell.Points[6]], new Vector2(0.75f, 0f)));

                //vertex.Add(new VertexPositionNormalTexture(Map.Points[cell.Points[4]], Map.Normals[cell.Points[4]], new Vector2(0.26f, 1f)));
                //vertex.Add(new VertexPositionNormalTexture(Map.Points[cell.Points[5]], Map.Normals[cell.Points[5]], new Vector2(0f, 0.5f)));
                //vertex.Add(new VertexPositionNormalTexture(Map.Points[cell.Points[6]], Map.Normals[cell.Points[6]], new Vector2(0.26f, 0f)));

                //vertex.Add(new VertexPositionNormalTexture(Map.Points[cell.Points[1]], Map.Normals[cell.Points[1]], new Vector2(0.75f, 0f)));
                //vertex.Add(new VertexPositionNormalTexture(Map.Points[cell.Points[3]], Map.Normals[cell.Points[3]], new Vector2(0.75f, 1f)));
                //vertex.Add(new VertexPositionNormalTexture(Map.Points[cell.Points[6]], Map.Normals[cell.Points[6]], new Vector2(0.26f, 0f))); 
            }

            vBuffer.SetData<VertexPositionNormalTexture>(vertex.ToArray());

            GameEngine.GraphicsDevice.SetVertexBuffer(vBuffer);
            GameEngine.GraphicsDevice.Textures[0] = GameEngine.Content.Load<Texture2D>("Hexa");
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        public void Draw(GameTime gameTime)
        {
            GameEngine.GraphicsDevice.Clear(Color.Black);

            UpdateCamera();
            UpdateShader();

            foreach (EffectPass pass in effect.CurrentTechnique.Passes)
            {
                pass.Apply();
                GameEngine.GraphicsDevice.DrawPrimitives(PrimitiveType.TriangleList, 0, Map.Cells.Count * 12);
            }
        }
    }
}
