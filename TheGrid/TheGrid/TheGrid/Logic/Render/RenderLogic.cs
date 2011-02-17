using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using TheGrid.Model;
using Microsoft.Xna.Framework.Input;
using TheGrid.Common;
using TheGrid.Model.Instrument;

namespace TheGrid.Logic.Render
{
    public class RenderLogic
    {
        public const float ZOOM_IN_MAX = -30f;
        public const float ZOOM_OUT_MAX = -1f;

        public SpriteBatch SpriteBatch;
        public SpriteFont FontMenu { get; set; }
        public SpriteFont FontText { get; set; }
        public SpriteFont FontTextSmall { get; set; }
        public SpriteFont FontMap { get; set; }
        public SpriteFont FontMapBig { get; set; }
        public GameEngine GameEngine { get; set; }
        public GraphicsDevice GraphicsDevice { get { return GameEngine.GraphicsDevice; } }

        public float ScreenWidth
        {
            get
            {
                return this.GameEngine.GraphicsDevice.Viewport.Width;
            }
        }

        public float ScreenHeight
        {
            get
            {
                return this.GameEngine.GraphicsDevice.Viewport.Height;
            }
        }

        public BasicEffect effect;
        public BasicEffect effectSprite;
        public BasicEffect effectUI;

        public Matrix View;
        public Matrix Projection;
        public Matrix World;

        public Vector3 CameraPosition = new Vector3(3000, 2500, -11f);
        public Vector3 CameraTarget = new Vector3(3000, 2500, 0f);

        public Vector3 CameraUp = new Vector3(0f, -1f, 0f);

        public bool updateViewScreen = false;
        public bool doScreenShot = false;

        public Texture2D texHexa2D = null;
        public Texture2D texHexa2DClip = null;
        public Texture2D texMusician = null;
        public Texture2D texEmpty = null;
        public Texture2D texEmptyGradient = null;
        private Texture2D textDirection = null;
        private Texture2D texRepeater = null;
        private Texture2D texSpeed = null;
        private Texture2D texEffect = null;

        private Texture2D texMusicianStart = null;
        private Texture2D texMusicianStop = null;

        public Texture2D texPlay = null;
        public Texture2D texPause = null;
        public Texture2D texStop = null;
        public Texture2D texSoloChannel = null;
        public Texture2D texMuteChannel = null;
        public Texture2D texSoloMusician = null;
        public Texture2D texMuteMusician = null;
        public Texture2D texTimeMarker = null;

        Dictionary<String, Microsoft.Xna.Framework.Graphics.Model> meshModels;

        Microsoft.Xna.Framework.Graphics.Model meshMusician;

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
            FontText = GameEngine.Content.Load<SpriteFont>(@"Font\FontText");
            FontTextSmall = GameEngine.Content.Load<SpriteFont>(@"Font\FontTextSmall");
            FontMap = GameEngine.Content.Load<SpriteFont>(@"Font\FontMap");
            FontMapBig = GameEngine.Content.Load<SpriteFont>(@"Font\FontMapBig");

            texHexa2D = GameEngine.Content.Load<Texture2D>(@"Texture\Hexa_2D");
            texHexa2DClip = GameEngine.Content.Load<Texture2D>(@"Texture\Hexa_2D_Clip");
            texMusician = GameEngine.Content.Load<Texture2D>(@"Texture\Musician");
            textDirection = GameEngine.Content.Load<Texture2D>(@"Texture\Direction");
            texRepeater = GameEngine.Content.Load<Texture2D>(@"Texture\Repeater");
            texSpeed = GameEngine.Content.Load<Texture2D>(@"Texture\Speed");
            texEffect = GameEngine.Content.Load<Texture2D>(@"Texture\Icon\Effect");

            texMusicianStart = GameEngine.Content.Load<Texture2D>(@"Texture\Icon\MusicianStart");
            texMusicianStop = GameEngine.Content.Load<Texture2D>(@"Texture\Icon\MusicianStop");
            texEmpty = GameEngine.Content.Load<Texture2D>(@"Texture\HexaEmpty");
            texEmptyGradient = GameEngine.Content.Load<Texture2D>(@"Texture\ImgEmptyGradient");

            texPlay = GameEngine.Content.Load<Texture2D>(@"Texture\Icon\Play");
            texPause = GameEngine.Content.Load<Texture2D>(@"Texture\Icon\Pause");
            texStop = GameEngine.Content.Load<Texture2D>(@"Texture\Icon\Stop");
            texSoloChannel = GameEngine.Content.Load<Texture2D>(@"Texture\Icon\SoloChannel");
            texMuteChannel = GameEngine.Content.Load<Texture2D>(@"Texture\Icon\MuteChannel");
            texSoloMusician = GameEngine.Content.Load<Texture2D>(@"Texture\Icon\SoloMusician");
            texMuteMusician = GameEngine.Content.Load<Texture2D>(@"Texture\Icon\MuteMusician");
            texTimeMarker = GameEngine.Content.Load<Texture2D>(@"Texture\Icon\TimeMarker");
        }

        private void Initilize3DModel()
        {
            meshModels = new Dictionary<string, Microsoft.Xna.Framework.Graphics.Model>();

            //meshMusician = GameEngine.Content.Load<Microsoft.Xna.Framework.Graphics.Model>(@"3DModel\IcoMusician");
        }

        //Création de la caméra
        private void CreateCamera()
        {
            UpdateCamera();
            Projection = Matrix.CreatePerspective(GameEngine.GraphicsDevice.Viewport.Width, GameEngine.GraphicsDevice.Viewport.Height, 1f, 100f);
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

            effectUI = new BasicEffect(GameEngine.GraphicsDevice);

            effectUI.LightingEnabled = false;
            effectUI.TextureEnabled = false;
            effectUI.VertexColorEnabled = true;

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

            effectUI.View = Matrix.CreateLookAt(new Vector3(ScreenWidth / 2, ScreenHeight / 2, -10f), new Vector3(ScreenWidth / 2, ScreenHeight / 2, 0f), CameraUp);
            //effectUI.Projection = Projection;
            //effectUI.World = Matrix.CreateWorld(Vector3.Zero, Vector3.Forward, Vector3.Up);

            //effectUI.View = View;
            effectUI.Projection = Matrix.CreateOrthographic(GameEngine.GraphicsDevice.Viewport.Width, GameEngine.GraphicsDevice.Viewport.Height, 1f, 100f); ;
            effectUI.World = World;

            //float angle = (float)gameTime.TotalGameTime.TotalMilliseconds / 1000f;
            //effect.DirectionalLight0.Direction = new Vector3((float)Math.Cos(angle), (float)Math.Sin(angle), -1f);
        }


        public void MoveCamera(Vector2 translation, GameTime gameTime)
        {
            float deltaTranslation = GameEngine.Render.CameraPosition.Z * ZOOM_OUT_MAX * 0.5f;

            Vector2 vecTempTranslation = deltaTranslation * gameTime.ElapsedGameTime.Milliseconds * translation;

            GameEngine.Render.CameraPosition += Tools.GetVector3(vecTempTranslation);
            GameEngine.Render.CameraTarget += Tools.GetVector3(vecTempTranslation);
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
            //---

            //--- Affiche les musiciens
            foreach (Channel channel in Context.Map.Channels)
            {
                foreach (Musician musician in channel.ListMusician)
                {
                    DrawMusician(musician, gameTime);
                }
            }
            SpriteBatch.End();
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
                colorChannel = new Color(0.3f, 0.3f, 0.3f);

            if (cell.Life > 0f)
            {
                colorChannel = Color.Lerp(colorChannel, Color.White, cell.Life);
            }

            if (cell.Clip != null)
            {
                SpriteBatch.Draw(texHexa2DClip, cellLocation, colorChannel);
            }
            else
            {
                SpriteBatch.Draw(texHexa2D, cellLocation, colorChannel);
            }

            //--- Instrument
            if (cell.Clip != null && cell.Clip.Instrument != null)
            {
                Texture2D texInstrument = null;

                if (cell.Clip.Instrument is InstrumentStart)
                {
                    texInstrument = texMusicianStart;
                }
                else if (cell.Clip.Instrument is InstrumentStop)
                {
                    texInstrument = texMusicianStop;
                }
                else if (cell.Clip.Instrument is InstrumentSample)
                {
                    texInstrument = GameEngine.Content.Load<Texture2D>(@"Texture\Icon\Instrument" + cell.Channel.Name);
                }
                
                if (cell.Clip.Instrument is InstrumentEffect)
                {
                    texInstrument = texEffect;

                    string effectName = ((InstrumentEffect)cell.Clip.Instrument).ChannelEffect.Name;
                    Vector2 deltaEffectName = new Vector2(FontMap.MeasureString(effectName).X / 2, FontMap.MeasureString(effectName).Y / 5);

                    SpriteBatch.DrawString(FontMap, effectName, cellLocation + midCellSize - deltaEffectName, Color.White);
                    SpriteBatch.Draw(texInstrument, cellLocation + midCellSize - new Vector2(texInstrument.Width / 2, texInstrument.Height), null, Color.White);
                }
                else
                {
                    SpriteBatch.Draw(texInstrument, cellLocation + midCellSize - new Vector2(texInstrument.Width / 2, texInstrument.Height / 2), null, Color.White);
                }

                if (cell.Clip.Instrument is InstrumentSample)
                {
                    string sampleName = ((InstrumentSample)cell.Clip.Instrument).Sample.Name;
                    Vector2 deltaSampleName = new Vector2(FontMap.MeasureString(sampleName).X / 2, FontMap.MeasureString(sampleName).Y / 5);

                    SpriteBatch.DrawString(FontMap, sampleName, cellLocation + midCellSize - deltaSampleName, Color.White);
                }
            }
            //---

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
                    double angle = MathHelper.TwoPi / 6 * i - MathHelper.TwoPi / 12;
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

        private void DrawMusician(Musician musician, GameTime gameTime)
        {
            //Matrix localWorld = Matrix.CreateTranslation(CameraTarget) * Matrix.CreateTranslation(musician.Position * HexaWidth);

            //meshMusician.Draw(localWorld, View, Projection);

            if (musician.CurrentCell == null || !musician.IsPlaying)
                return;

            Vector2 midCellSize = new Vector2(HexaWidth / 2, HexaWidth / 2);
            Vector2 cellLocation = Tools.GetVector2(musician.Position) * texHexa2D.Width + midCellSize - midCellSize / 2;

            SpriteBatch.Draw(texMusician, cellLocation, musician.Channel.Color);
        }
    }
}
