using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using TheGrid.Common;
using Microsoft.Xna.Framework.Input;

namespace TheGrid.Model.Menu
{
    public class Menu
    {
        #region Constants
        private const double MENU_ANIMATION_DURATION = 200;
        #endregion

        #region Membres publiques
        public Cell ParentCell { get; set; }
        public Menu ParentMenu { get; set; }
        public List<Item> Items { get; set; }
        public MenuState State { get; set; }
        public double PercentVisibility { get; set; }
        public double AngleDeltaMouse { get; set; }
        public double AngleDeltaRender { get; set; }
        public double AngleDeltaRenderIcon { get; set; }

        public bool ShowIcon { get; set; }
        #endregion

        #region Membres privés
        private int nbVertex = 24;
        private float size = 1.5f;

        private VertexBuffer vBuffer;
        private TimeSpan LastStateChanged { get; set; }
        #endregion

        public Menu(Cell parentCell, Menu parentMenu, bool showIcon)
        {
            ParentCell = parentCell;
            ParentMenu = parentMenu;
            ShowIcon = showIcon;

            Items = new List<Item>();
        }

        public void Open(GameTime gameTime)
        {
            State = MenuState.Opening;
            LastStateChanged = gameTime.TotalGameTime;
        }

        public void Close(GameTime gameTime)
        {
            State = MenuState.Closing;
            LastStateChanged = gameTime.TotalGameTime;
        }

        public virtual void Update(GameEngine gameEngine, GameTime gametime)
        {
            double elapsedTimeMenu = gametime.TotalGameTime.Subtract(LastStateChanged).TotalMilliseconds;

            if (State == MenuState.Opening && elapsedTimeMenu > 0)
                PercentVisibility = elapsedTimeMenu / MENU_ANIMATION_DURATION;

            if (State == MenuState.Closing)
                PercentVisibility = 1 - elapsedTimeMenu / MENU_ANIMATION_DURATION;

            if (PercentVisibility > 1)
                PercentVisibility = 1;
            if (PercentVisibility < 0)
                PercentVisibility = 0;

            if (State == MenuState.Opening || State == MenuState.Closing || State == MenuState.Opened)
                CreateVertex(gameEngine);

            if (PercentVisibility == 1 && State == MenuState.Opening)
                State = MenuState.Opened;
            if (PercentVisibility == 0 && State == MenuState.Closing)
                State = MenuState.Closed;
        }

        public virtual void Draw(GameEngine gameEngine, BasicEffect effect, BasicEffect effectSprite, GameTime gameTime)
        {
            if (PercentVisibility == 0)
                return;

            effect.Alpha = (float)PercentVisibility;

            gameEngine.GraphicsDevice.SetVertexBuffer(vBuffer);

            foreach (EffectPass pass in effect.CurrentTechnique.Passes)
            {
                pass.Apply();
                gameEngine.GraphicsDevice.DrawPrimitives(PrimitiveType.TriangleList, 0, nbVertex);
            }


            if (ShowIcon)
            {
                Vector3 nearPoint = new Vector3(ParentCell.Location * gameEngine.Render.HexaWidth, 0f);
                //gameEngine.GraphicsDevice.Viewport.Project(new Vector3(ParentCell.Location*512f, 5f),
                //    gameEngine.Render.Projection, gameEngine.Render.View, gameEngine.Render.World);

                //float localSize = size * 380f / gameEngine.Render.CameraPosition.Z;
                //float delta = 32f * localSize / 80f;

                float localSize = size * gameEngine.Render.HexaWidth / 3f;
                float delta = -size * 120f;
                double angleItem = MathHelper.TwoPi / (double)Items.Count;

                double d = 1.5;
                if (Items.Count == 9)
                {
                    d = 2.3;
                    //delta = 32f * localSize / 100f;
                }

                gameEngine.Render.SpriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.LinearClamp, DepthStencilState.Default, RasterizerState.CullNone, effectSprite, Matrix.Identity);
                for (int i = 0; i < Items.Count; i++)
                {
                    double angle = ((double)i - d) * angleItem - AngleDeltaRenderIcon;

                    Vector2 vec = new Vector2(localSize * (float)Math.Cos(angle) - delta, localSize * (float)Math.Sin(angle) - delta);

                    Rectangle rec = new Rectangle((int)(nearPoint.X + vec.X), (int)(nearPoint.Y + vec.Y), (int)(128f * size), (int)(128f * size));

                    gameEngine.Render.SpriteBatch.Draw(gameEngine.Content.Load<Texture2D>(@"Texture\Icon\" + Items[i].Name), rec, new Color((float)PercentVisibility, (float)PercentVisibility, (float)PercentVisibility, (float)PercentVisibility));
                }
                gameEngine.Render.SpriteBatch.End();
            }
        }


        private void CreateVertex(GameEngine gameEngine)
        {
            List<VertexPositionColor> vertex = new List<VertexPositionColor>();

            Vector3 midHexa = new Vector3(gameEngine.Render.HexaWidth / 2f, gameEngine.Render.HexaWidth / 2f, 0f);
            Color color = new Color(0.05f, 0.05f, 0.05f, (float)PercentVisibility);
            Color color2 = new Color(0.1f, 0.1f, 0.1f, (float)PercentVisibility);

            if (color.A > 240)
                color.A = 240;
            if (color2.A > 240)
                color2.A = 240;

            //if (Items.Count > 8)
                nbVertex = Items.Count * 4;

            int nbPointPerItem = nbVertex / Items.Count;
            double angleItem =  MathHelper.TwoPi / (double)Items.Count - AngleDeltaRender - MathHelper.Pi;
            double angleVertex = MathHelper.TwoPi / (double)nbVertex;

            vBuffer = new VertexBuffer(gameEngine.GraphicsDevice, typeof(VertexPositionColor), nbVertex * 3, BufferUsage.None);

            float localSize = size * gameEngine.Render.HexaWidth / 2f;// *(-gameEngine.Render.CameraPosition.Z / 4f) * 3f;

            for (int i = 0; i < nbVertex; i++)
            {
                double angle1 = angleItem + (double)i * angleVertex * PercentVisibility;
                double angle2 = angleItem + (double)(i + 1) * angleVertex * PercentVisibility;

                Vector3 position1 = new Vector3(localSize * (float)Math.Cos(angle1), localSize * (float)Math.Sin(angle1), 0f);
                Vector3 position2 = new Vector3(localSize * (float)Math.Cos(angle2), localSize * (float)Math.Sin(angle2), 0f);

                int index = ((i / nbPointPerItem) ) % Items.Count;
                
                if(Items[index].MouseOver)
                    color = new Color(0.05f, 0.4f, 0.05f, (float)PercentVisibility);
                else
                    color = new Color(0.05f, 0.05f, 0.05f, (float)PercentVisibility);

                if (Items[index].Checked)
                    color = new Color(color.R + 50, color.G + 50, color.B + 50, color.A);

                if (Items[index].Color != Color.Transparent)
                    color2 = new Color(Items[index].Color.R, Items[index].Color.G, Items[index].Color.B, color2.A);

                vertex.Add(new VertexPositionColor(position2 + Tools.GetVector3(ParentCell.Location * gameEngine.Render.HexaWidth) + midHexa, color));
                vertex.Add(new VertexPositionColor(new Vector3(0, 0, 0f) + Tools.GetVector3(ParentCell.Location * gameEngine.Render.HexaWidth) + midHexa, color2));
                vertex.Add(new VertexPositionColor(position1 + Tools.GetVector3(ParentCell.Location * gameEngine.Render.HexaWidth) + midHexa, color));
            }

            vBuffer.SetData<VertexPositionColor>(vertex.ToArray());
        }

        public void MouseClick(GameEngine gameEngine, GameTime gameTime, MouseState mouseState)
        {
            foreach (Item item in Items)
            {
                if (item.MouseOver)
                {
                    item.OnSelected(gameTime);
                }
            }
        }

        public void MouseOver(GameEngine gameEngine, GameTime gameTime, MouseState mouseState)
        {
            float pickCurDistance = 0f;
            float barycentricU = 0f;
            float barycentricV = 0f;

            Vector3 midHexa = new Vector3(gameEngine.Render.HexaWidth / 2f, gameEngine.Render.HexaWidth / 2f, 0f);

            Vector3 mousePosition = new Vector3((float)mouseState.X - (float)gameEngine.GraphicsDevice.Viewport.Width / 2f, (float)mouseState.Y - (float)gameEngine.GraphicsDevice.Viewport.Height / 2f, 0f);

            Matrix mtx = Matrix.CreateScale(-gameEngine.Render.CameraPosition.Z) * Matrix.CreateTranslation(gameEngine.Render.CameraPosition.X, gameEngine.Render.CameraPosition.Y, gameEngine.Render.CameraPosition.Z);

            mousePosition = Vector3.Transform(mousePosition, mtx);

            //Ray ray = new Ray(mousePosition, Vector3.UnitZ);

            float localSize = size * 1.2f * gameEngine.Render.HexaWidth/2f;
            double angleItem = MathHelper.TwoPi / (double)Items.Count;

            int currentIndex = -1;
            for (int i = 0; i < Items.Count; i++)
            {
                double angle2 = ((double)i - 0.5) * angleItem - MathHelper.PiOver2 +AngleDeltaMouse;
                double angle1 = ((double)i + 0.5) * angleItem - MathHelper.PiOver2 +AngleDeltaMouse;

                Vector3 vec1 = new Vector3(localSize * (float)Math.Cos(angle1), localSize * (float)Math.Sin(angle1), 0f) + Tools.GetVector3(ParentCell.Location * gameEngine.Render.HexaWidth)+midHexa;
                Vector3 vec3 = new Vector3(localSize * (float)Math.Cos(angle2), localSize * (float)Math.Sin(angle2), 0f) + Tools.GetVector3(ParentCell.Location * gameEngine.Render.HexaWidth) + midHexa;

                Vector3 vec2 = Tools.GetVector3(ParentCell.Location * gameEngine.Render.HexaWidth) + midHexa;

                if (Tools.RayIntersectTriangle(mousePosition, -Vector3.UnitZ, vec1, vec2, vec3, ref pickCurDistance, ref barycentricU, ref barycentricV))
                {
                    currentIndex = i;
                }

                Items[i].MouseOver = false;
            }

            if (currentIndex != -1)
            {
                Items[currentIndex].MouseOver = true;

                gameEngine.Window.Title = (currentIndex).ToString();
            }
            else
                gameEngine.Window.Title = "";
        }
    }
}
