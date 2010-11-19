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
        BasicEffect effectCube;

        VertexBuffer vBuffer;

        Matrix View;
        Matrix Projection;
        Matrix World;

        public Vector3 CameraPosition = new Vector3(0f, 0f, 50f);
        public Vector3 CameraTarget = new Vector3(0f, 30f, 0f);
        public Vector3 CameraUp = Vector3.Backward;
        
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
            InitializeCube();

            SpriteBatch = new SpriteBatch(GameEngine.GraphicsDevice);
        }

        //Création de la caméra
        private void CreateCamera()
        {
            UpdateCamera();
            Projection = Matrix.CreatePerspectiveFieldOfView(MathHelper.Pi / 2f, GameEngine.GraphicsDevice.Viewport.Width / GameEngine.GraphicsDevice.Viewport.Height, 0.01f, 1000.0f);
            //Projection = Matrix.CreatePerspective(this.GraphicsDevice.Viewport.Width, this.GraphicsDevice.Viewport.Height, 0.01f, 1000.0f);
            World = Matrix.Identity;
        }

        private void UpdateCamera()
        {
            View = Matrix.CreateLookAt(CameraPosition, CameraTarget, CameraUp);
        }

        private void CreateShader()
        {
            effect = new BasicEffect(GameEngine.GraphicsDevice);
            //effect.World = Matrix.CreateRotationY((float)gameTime.TotalGameTime.TotalMilliseconds/800);

            //effect.PreferPerPixelLighting = true;
            effect.VertexColorEnabled = false;
            effect.LightingEnabled = true;
            effect.EmissiveColor = new Vector3(0.2f, 0.2f, 0.25f);
            //effect.DirectionalLight0 = new DirectionalLight(

            effect.TextureEnabled = true;
            effect.SpecularColor = new Vector3(0.3f, 0.35f, 0.3f);
            effect.SpecularPower = 1f;
            //effect.SpecularColor = new Vector3(1, 1, 1);
            //effect.SpecularPower = 1f;
            effect.Texture = GameEngine.Content.Load<Texture2D>("Hexa");
            effectCube = new BasicEffect(GameEngine.GraphicsDevice);

            effectCube.PreferPerPixelLighting = false;
            effectCube.VertexColorEnabled = true;
            effectCube.LightingEnabled = false;
            effectCube.TextureEnabled = false;
            //effectCube.SpecularColor = new Vector3(1, 1, 1);
            //effectCube.SpecularPower = 1f;

            UpdateShader(new GameTime());
        }

        public void UpdateShader(GameTime gameTime)
        {
            effect.View = View;
            effect.Projection = Projection;
            effect.World = World;

            float angle = (float)gameTime.TotalGameTime.TotalMilliseconds/1000f;
            effect.DirectionalLight0.Direction = new Vector3((float)Math.Cos(angle), (float)Math.Sin(angle), -1f);


            effectCube.View = View;
            effectCube.Projection = Projection;
            //effect.World = World;

            effectCube.World = Matrix.Multiply(World, Matrix.CreateTranslation(CameraTarget+new Vector3(0,0,10)));
        }

        public void CreateVertex()
        {
            vBuffer = new VertexBuffer(GameEngine.GraphicsDevice, typeof(VertexPositionNormalTexture), Map.Cells.Count * 12, BufferUsage.None);

            List<VertexPositionNormalTexture> vertex = new List<VertexPositionNormalTexture>();

            foreach (Cell cell in Map.Cells)
            {
                AddVertex(3, 2, 1, vertex, cell);
                AddVertex(6, 4, 3, vertex, cell);
                AddVertex(6, 5, 4, vertex, cell);
                AddVertex(6, 3, 1, vertex, cell);
            }

            vBuffer.SetData<VertexPositionNormalTexture>(vertex.ToArray());

            GameEngine.GraphicsDevice.SetVertexBuffer(vBuffer);
            GameEngine.GraphicsDevice.Textures[0] = GameEngine.Content.Load<Texture2D>("Hexa");
        }

        private void AddVertex(int index1, int index2, int index3, List<VertexPositionNormalTexture> vertex, Cell cell)
        {
            Dictionary<int, Vector2> uv = new Dictionary<int, Vector2>();
            uv.Add(1, new Vector2(0.75f, 0f));
            uv.Add(2, new Vector2(1f, 0.5f));
            uv.Add(3, new Vector2(0.75f, 1f));
            uv.Add(4, new Vector2(0.26f, 1f));
            uv.Add(5, new Vector2(0f, 0.5f));
            uv.Add(6, new Vector2(0.26f, 0f));

            //Vector3 vec1 = Map.Points[cell.Points[index2]] - Map.Points[cell.Points[index1]];
            //Vector3 vec2 = Map.Points[cell.Points[index2]] - Map.Points[cell.Points[index3]];
            //Vector3 normal = Vector3.Cross(vec1, vec2);

            //vertex.Add(new VertexPositionNormalTexture(Map.Points[cell.Points[index1]], normal, uv[index1]));
            //vertex.Add(new VertexPositionNormalTexture(Map.Points[cell.Points[index2]], normal, uv[index2]));
            //vertex.Add(new VertexPositionNormalTexture(Map.Points[cell.Points[index3]], normal, uv[index3]));

            vertex.Add(new VertexPositionNormalTexture(Map.Points[cell.Points[index1]], Map.Normals[cell.Points[index1]], uv[index1]));
            vertex.Add(new VertexPositionNormalTexture(Map.Points[cell.Points[index2]], Map.Normals[cell.Points[index2]], uv[index2]));
            vertex.Add(new VertexPositionNormalTexture(Map.Points[cell.Points[index3]], Map.Normals[cell.Points[index3]], uv[index3]));
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        public void Draw(GameTime gameTime)
        {
            GameEngine.GraphicsDevice.Clear(Color.Black);

            UpdateCamera();
            UpdateShader(gameTime);

            GameEngine.GraphicsDevice.SetVertexBuffer(vBuffer);

            foreach (EffectPass pass in effect.CurrentTechnique.Passes)
            {
                pass.Apply();
                GameEngine.GraphicsDevice.DrawPrimitives(PrimitiveType.TriangleList, 0, Map.Cells.Count * 12);
            }

            //GameEngine.GraphicsDevice.SetVertexBuffer(vBufferCube);

            //foreach (EffectPass pass in effectCube.CurrentTechnique.Passes)
            //{
            //    pass.Apply();
            //    GameEngine.GraphicsDevice.DrawPrimitives(PrimitiveType.TriangleList, 0, 36);
            //}
        }


        VertexDeclaration cubeVertexDeclaration;
        VertexPositionColor[] nonIndexedCube;
        VertexBuffer vBufferCube;

        /// <summary>
        /// Creates an array of non-indexed position/colored data 
        /// representing a cube.
        /// </summary>
        private void InitializeCube()
        {
            cubeVertexDeclaration = VertexPositionColor.VertexDeclaration;

            nonIndexedCube = new VertexPositionColor[36];

            Vector3 topLeftFront = new Vector3(-1.0f, 1.0f, 1.0f);
            Vector3 bottomLeftFront = new Vector3(-1.0f, -1.0f, 1.0f);
            Vector3 topRightFront = new Vector3(1.0f, 1.0f, 1.0f);
            Vector3 bottomRightFront = new Vector3(1.0f, -1.0f, 1.0f);
            Vector3 topLeftBack = new Vector3(-1.0f, 1.0f, -1.0f);
            Vector3 topRightBack = new Vector3(1.0f, 1.0f, -1.0f);
            Vector3 bottomLeftBack = new Vector3(-1.0f, -1.0f, -1.0f);
            Vector3 bottomRightBack = new Vector3(1.0f, -1.0f, -1.0f);

            // Front face
            nonIndexedCube[0] =
                new VertexPositionColor(topLeftFront, Color.Red);
            nonIndexedCube[1] =
                new VertexPositionColor(bottomLeftFront, Color.Red);
            nonIndexedCube[2] =
                new VertexPositionColor(topRightFront, Color.Red);
            nonIndexedCube[3] =
                new VertexPositionColor(bottomLeftFront, Color.Red);
            nonIndexedCube[4] =
                new VertexPositionColor(bottomRightFront, Color.Red);
            nonIndexedCube[5] =
                new VertexPositionColor(topRightFront, Color.Red);

            // Back face 
            nonIndexedCube[6] =
                new VertexPositionColor(topLeftBack, Color.Orange);
            nonIndexedCube[7] =
                new VertexPositionColor(topRightBack, Color.Orange);
            nonIndexedCube[8] =
                new VertexPositionColor(bottomLeftBack, Color.Orange);
            nonIndexedCube[9] =
                new VertexPositionColor(bottomLeftBack, Color.Orange);
            nonIndexedCube[10] =
                new VertexPositionColor(topRightBack, Color.Orange);
            nonIndexedCube[11] =
                new VertexPositionColor(bottomRightBack, Color.Orange);

            // Top face
            nonIndexedCube[12] =
                new VertexPositionColor(topLeftFront, Color.Yellow);
            nonIndexedCube[13] =
                new VertexPositionColor(topRightBack, Color.Yellow);
            nonIndexedCube[14] =
                new VertexPositionColor(topLeftBack, Color.Yellow);
            nonIndexedCube[15] =
                new VertexPositionColor(topLeftFront, Color.Yellow);
            nonIndexedCube[16] =
                new VertexPositionColor(topRightFront, Color.Yellow);
            nonIndexedCube[17] =
                new VertexPositionColor(topRightBack, Color.Yellow);

            // Bottom face 
            nonIndexedCube[18] =
                new VertexPositionColor(bottomLeftFront, Color.Purple);
            nonIndexedCube[19] =
                new VertexPositionColor(bottomLeftBack, Color.Purple);
            nonIndexedCube[20] =
                new VertexPositionColor(bottomRightBack, Color.Purple);
            nonIndexedCube[21] =
                new VertexPositionColor(bottomLeftFront, Color.Purple);
            nonIndexedCube[22] =
                new VertexPositionColor(bottomRightBack, Color.Purple);
            nonIndexedCube[23] =
                new VertexPositionColor(bottomRightFront, Color.Purple);

            // Left face
            nonIndexedCube[24] =
                new VertexPositionColor(topLeftFront, Color.Blue);
            nonIndexedCube[25] =
                new VertexPositionColor(bottomLeftBack, Color.Blue);
            nonIndexedCube[26] =
                new VertexPositionColor(bottomLeftFront, Color.Blue);
            nonIndexedCube[27] =
                new VertexPositionColor(topLeftBack, Color.Blue);
            nonIndexedCube[28] =
                new VertexPositionColor(bottomLeftBack, Color.Blue);
            nonIndexedCube[29] =
                new VertexPositionColor(topLeftFront, Color.Blue);

            // Right face 
            nonIndexedCube[30] =
                new VertexPositionColor(topRightFront, Color.Green);
            nonIndexedCube[31] =
                new VertexPositionColor(bottomRightFront, Color.Green);
            nonIndexedCube[32] =
                new VertexPositionColor(bottomRightBack, Color.Green);
            nonIndexedCube[33] =
                new VertexPositionColor(topRightBack, Color.Green);
            nonIndexedCube[34] =
                new VertexPositionColor(topRightFront, Color.Green);
            nonIndexedCube[35] =
                new VertexPositionColor(bottomRightBack, Color.Green);

            vBufferCube = new VertexBuffer(GameEngine.GraphicsDevice, typeof(VertexPositionColor), 36, BufferUsage.None);

            vBufferCube.SetData<VertexPositionColor>(nonIndexedCube);
        }
    }
}
