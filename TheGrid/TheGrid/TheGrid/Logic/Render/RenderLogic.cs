using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using TheGrid.Model;
using Microsoft.Xna.Framework.Input;
using TheGrid.Common;

namespace TheGrid.Logic.Render
{
    public class RenderLogic
    {
        public SpriteBatch SpriteBatch;
        public SpriteFont FontMenu { get; set; }
        public GameEngine GameEngine { get; set; }

        BasicEffect effect;
        BasicEffect effectSprite;

        VertexBuffer vBuffer;

        public Matrix View;
        public Matrix Projection;
        public Matrix World;

        public Vector3 CameraPosition = new Vector3(0, 0, -3f);
        public Vector3 CameraTarget = new Vector3(0, 0, 0f);
        public Vector3 CameraUp = new Vector3(0f, -1f, 0f);

        public bool updateViewScreen = false;
        public bool doScreenShot = false;
        MeshHexa meshHexa = null;
        private Texture2D texEmpty = null;
        private Texture2D texHexa = null;
        private Texture2D texHexa2 = null;

        public Texture2D texHexa2D = null;
        private Texture2D textDirection = null;
        private Texture2D texRepeater = null;
        private Texture2D texSpeed = null;

        Dictionary<String, Microsoft.Xna.Framework.Graphics.Model> meshModels;


        public float HexaWidth = 512f;

        public RenderLogic(GameEngine gameEngine)
        {
            this.GameEngine = gameEngine;
        }

        public void InitRender()
        {
            CreateCamera();
            CreateShader();

            Initilize3DModel();

            SpriteBatch = new SpriteBatch(GameEngine.GraphicsDevice);
            FontMenu = GameEngine.Content.Load<SpriteFont>(@"Font\FontMenu");
            //texEmpty = GameEngine.Content.Load<Texture2D>(@"Texture\HexaEmpty");
            //texHexa = GameEngine.Content.Load<Texture2D>(@"Texture\ImgHexa");
            //texHexa2 = GameEngine.Content.Load<Texture2D>(@"Texture\ImgHexa2");

            texHexa2D = GameEngine.Content.Load<Texture2D>(@"Texture\Hexa_2D");
            textDirection = GameEngine.Content.Load<Texture2D>(@"Texture\Direction");
            texRepeater = GameEngine.Content.Load<Texture2D>(@"Texture\Repeater");
            texSpeed = GameEngine.Content.Load<Texture2D>(@"Texture\Speed");
        }

        private void Initilize3DModel()
        {
            meshModels = new Dictionary<string, Microsoft.Xna.Framework.Graphics.Model>();

            meshHexa = new MeshHexa(GameEngine.Content.Load<Microsoft.Xna.Framework.Graphics.Model>(@"3DModel\Hexa"));
        }

        //Création de la caméra
        private void CreateCamera()
        {
            UpdateCamera();
            //Projection = Matrix.CreatePerspectiveFieldOfView(MathHelper.Pi / 2f, (float)GameEngine.Graphics.PreferredBackBufferWidth / (float)GameEngine.Graphics.PreferredBackBufferHeight, 0.01f, 1000.0f);
            Projection = Matrix.CreatePerspective(GameEngine.GraphicsDevice.Viewport.Width, GameEngine.GraphicsDevice.Viewport.Height, 1f, 100f);
            //Projection = Matrix.CreateOrthographic(GameEngine.GraphicsDevice.Viewport.Width, GameEngine.GraphicsDevice.Viewport.Height, 1f, 100f);
            World = Matrix.Identity;
        }

        private void UpdateCamera()
        {
            View = Matrix.CreateLookAt(CameraPosition, CameraTarget, CameraUp);
        }

        private void CreateShader()
        {
            effect = new BasicEffect(GameEngine.GraphicsDevice);

            effect.VertexColorEnabled = true;
            effect.LightingEnabled = false;
            effect.EmissiveColor = new Vector3(0.2f, 0.2f, 0.3f);

            effect.TextureEnabled = false;
            effect.SpecularColor = new Vector3(0.3f, 0.5f, 0.3f);
            effect.SpecularPower = 1f;


            effectSprite = new BasicEffect(GameEngine.GraphicsDevice);

            effectSprite.LightingEnabled = false;
            effectSprite.TextureEnabled = true;
            effectSprite.VertexColorEnabled = true;

            UpdateShader(new GameTime());
        }

        public void UpdateShader(GameTime gameTime)
        {
            Matrix localview = Matrix.CreateLookAt(new Vector3(CameraPosition.X, CameraPosition.Y, CameraPosition.Z), CameraTarget, -Vector3.Up);

            effect.View = localview;
            effect.Projection = Projection;
            effect.World = World;

            effectSprite.View = View;
            effectSprite.Projection = Projection;
            effectSprite.World = World;

            //float angle = (float)gameTime.TotalGameTime.TotalMilliseconds / 1000f;
            //effect.DirectionalLight0.Direction = new Vector3((float)Math.Cos(angle), (float)Math.Sin(angle), -1f);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        public void Draw(GameTime gameTime)
        {
            GameEngine.GraphicsDevice.Clear(new Color(15, 15, 15));

            UpdateCamera();
            UpdateShader(gameTime);

            //--- Affiche la map
            SpriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.LinearClamp, DepthStencilState.Default, RasterizerState.CullNone, effectSprite, Matrix.Identity);

            foreach (Cell cell in Context.Map.Cells)
            {
                DrawCell(cell, gameTime);
            }

            SpriteBatch.End();
            //---

            //--- Affiche le menu
            if (Context.CurrentMenu != null)
                Context.CurrentMenu.Draw(GameEngine, effect, effectSprite, gameTime);
            //---
        }

        private void DrawCell(Cell cell, GameTime gameTime)
        {
            //--- TODO : gérer le frustum
            //---

            Vector2 midCellSize = new Vector2(HexaWidth / 2, HexaWidth / 2);
            Vector2 cellLocation = cell.Location * texHexa2D.Width;

            Color colorChannel = new Color(0.35f, 0.35f, 0.35f); ;

            if (cell.Channel != null)
                colorChannel = cell.Channel.Color;
            else if (cell.Clip != null)
                colorChannel = Color.White;

            SpriteBatch.Draw(texHexa2D, cellLocation, colorChannel);

            //--- Direction
            for (int i = 0; i < 6; i++)
            {
                if (cell.Clip != null && cell.Clip.Directions[i])
                {
                    double angle = MathHelper.TwoPi / 6 * i;
                    Vector2 center = new Vector2(0.5f * textDirection.Width, 0.5f * textDirection.Height + 0.34f * HexaWidth);
                    SpriteBatch.Draw(textDirection, cellLocation + midCellSize, null, Color.White, (float)angle, center, 1f, SpriteEffects.None, 0f);
                }
            }
            //---

            //--- Repeater
            for (int i = 0; i < 6; i++)
            {
                if (cell.Clip != null && cell.Clip.Repeater.HasValue && i <= cell.Clip.Repeater.Value)
                {
                    double angle = MathHelper.TwoPi / 6 * i-MathHelper.TwoPi/12;
                    Vector2 center = new Vector2(0.5f * texRepeater.Width, 0.5f * texRepeater.Height + 0.4f * HexaWidth);
                    SpriteBatch.Draw(texRepeater, cellLocation + midCellSize, null, Color.White, (float)angle, center, 1f, SpriteEffects.None, 0f);
                }
            }
            //---

            //--- Speed
            for (int i = 0; i < 4; i++)
            {
                if (
                    cell.Clip != null &&
                    cell.Clip.Speed.HasValue &&
                    cell.Clip.Speed.Value > 0 &&
                    i < cell.Clip.Speed.Value)
                {
                    Vector2 center = new Vector2(0.5f * texSpeed.Width, 0.5f * texSpeed.Height);
                    Vector2 location = new Vector2(-2f * texSpeed.Width + Math.Abs(i) * texSpeed.Width, 0.34f * HexaWidth - texSpeed.Height);
                    SpriteBatch.Draw(texSpeed, cellLocation + midCellSize + location, null, Color.White, 0f, center, 1f, SpriteEffects.None, 0f);

                }
                else if (
                    cell.Clip != null &&
                    cell.Clip.Speed.HasValue &&
                    cell.Clip.Speed.Value < 0 &&
                    i < Math.Abs(cell.Clip.Speed.Value))
                {
                    Vector2 center = new Vector2(0.5f * texSpeed.Width, 0.5f * texSpeed.Height);
                    Vector2 location = new Vector2(-2f * texSpeed.Width + Math.Abs(i) * texSpeed.Width, 0.34f * HexaWidth - texSpeed.Height);
                    SpriteBatch.Draw(texSpeed, cellLocation + midCellSize + location, null, Color.White, 0f, center, 1f, SpriteEffects.FlipHorizontally, 0f);
                }
            }
            //---
        }
    }
}
