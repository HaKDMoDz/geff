using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using TheGrid.Common;
using Microsoft.Xna.Framework.Input;
using TheGrid.Logic.UI;

namespace TheGrid.Model.UI.Menu
{
    public class Menu : UIComponent
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
        public double AngleDelta { get; set; }
        public bool ShowIcon { get; set; }
        #endregion

        #region Membres privés
        public int nbVertex = 24;

        private VertexBuffer vBuffer;
        private TimeSpan LastStateChanged { get; set; }
        #endregion

        public Menu(UILogic uiLogic, TimeSpan creationTime, Cell parentCell, Menu parentMenu, bool showIcon)
            : base(uiLogic, creationTime)
        {
            ParentCell = parentCell;
            ParentMenu = parentMenu;
            ShowIcon = showIcon;

            Items = new List<Item>();
            ListUIChildren = new List<UIComponent>();
        }

        public void Open(GameTime gameTime)
        {
            Visible = true;
            State = MenuState.Opening;
            LastStateChanged = gameTime.TotalGameTime;
        }

        public void Close(GameTime gameTime)
        {
            State = MenuState.Closing;
            LastStateChanged = gameTime.TotalGameTime;
        }

        private void CreateVertex()
        {
            List<VertexPositionColor> vertex = new List<VertexPositionColor>();

            Vector3 midHexa = new Vector3(Render.HexaWidth / 2f, Render.HexaWidth / 2f, 0f);
            Color color = new Color(0.05f, 0.05f, 0.05f, (float)PercentVisibility);
            Color color2 = new Color(0.1f, 0.1f, 0.1f, (float)PercentVisibility);

            if (color.A > 240)
                color.A = 240;
            if (color2.A > 240)
                color2.A = 240;

            nbVertex = Items.Count * 4;

            int nbPointPerItem = nbVertex / Items.Count;
            double angleItem = AngleDelta - MathHelper.PiOver2 - ((MathHelper.TwoPi / (double)Items.Count) / 2);
            double angleVertex = MathHelper.TwoPi / (double)nbVertex;

            vBuffer = new VertexBuffer(Render.GraphicsDevice, typeof(VertexPositionColor), nbVertex * 3, BufferUsage.None);

            float localSize = Context.MenuSize * Render.HexaWidth / 2f;// *(-gameEngine.Render.CameraPosition.Z / 4f) * 3f;

            for (int i = 0; i < nbVertex; i++)
            {
                double angle1 = angleItem + (double)i * angleVertex * PercentVisibility;
                double angle2 = angleItem + (double)(i + 1) * angleVertex * PercentVisibility;

                Vector3 position1 = new Vector3(localSize * (float)Math.Cos(angle1), localSize * (float)Math.Sin(angle1), 0f);
                Vector3 position2 = new Vector3(localSize * (float)Math.Cos(angle2), localSize * (float)Math.Sin(angle2), 0f);

                int index = ((i / nbPointPerItem)) % Items.Count;

                if (Items[index].MouseOver)
                    color = new Color(0.05f, 0.4f, 0.05f, (float)PercentVisibility);
                else
                    color = new Color(0.05f, 0.05f, 0.05f, (float)PercentVisibility);

                if (Items[index].Checked)
                    color = new Color(color.R + 50, color.G + 50, color.B + 50, color.A);

                if (Items[index].Color != Color.Transparent)
                    color2 = new Color(Items[index].Color.R, Items[index].Color.G, Items[index].Color.B, color2.A);

                vertex.Add(new VertexPositionColor(position2 + Tools.GetVector3(ParentCell.Location * Render.HexaWidth) + midHexa, color));
                vertex.Add(new VertexPositionColor(new Vector3(0, 0, 0f) + Tools.GetVector3(ParentCell.Location * Render.HexaWidth) + midHexa, color2));
                vertex.Add(new VertexPositionColor(position1 + Tools.GetVector3(ParentCell.Location * Render.HexaWidth) + midHexa, color));
            }

            vBuffer.SetData<VertexPositionColor>(vertex.ToArray());
        }

        public void MouseClick(GameTime gameTime)
        {
            foreach (Item item in Items)
            {
                if (item.MouseOver)
                {
                    item.OnSelected(gameTime);
                }
            }
        }

        public void MouseOver(GameTime gameTime)
        {
            float pickCurDistance = 0f;
            float barycentricU = 0f;
            float barycentricV = 0f;

            Vector3 midHexa = new Vector3(Render.HexaWidth / 2f, Render.HexaWidth / 2f, 0f);

            Vector3 mousePosition = new Vector3((float)Controller.mouseState.X - (float)Render.GraphicsDevice.Viewport.Width / 2f, (float)Controller.mouseState.Y - (float)Render.GraphicsDevice.Viewport.Height / 2f, 0f);

            Matrix mtx = Matrix.CreateScale(-Render.CameraPosition.Z) * Matrix.CreateTranslation(Render.CameraPosition.X, Render.CameraPosition.Y, Render.CameraPosition.Z);

            mousePosition = Vector3.Transform(mousePosition, mtx);

            int nbPointPerItem = nbVertex / Items.Count;
            float localSize = Context.MenuSize * 1.2f * Render.HexaWidth / 2f;
            double angleItem = AngleDelta - MathHelper.PiOver2 - ((MathHelper.TwoPi / (double)Items.Count) / 2); 
            double angleVertex = MathHelper.TwoPi / (double)nbVertex;

            int currentIndex = -1;
            for (int i = 0; i < nbVertex; i++)
            {
                double angle1 = angleItem + (double)(i+1) * angleVertex;
                double angle2 = angleItem + (double)(i) * angleVertex;

                Vector3 vec1 = new Vector3(localSize * (float)Math.Cos(angle1), localSize * (float)Math.Sin(angle1), 0f) + Tools.GetVector3(ParentCell.Location * Render.HexaWidth) + midHexa;
                Vector3 vec3 = new Vector3(localSize * (float)Math.Cos(angle2), localSize * (float)Math.Sin(angle2), 0f) + Tools.GetVector3(ParentCell.Location * Render.HexaWidth) + midHexa;

                Vector3 vec2 = Tools.GetVector3(ParentCell.Location * Render.HexaWidth) + midHexa;

                int index  = ((i / nbPointPerItem)) % Items.Count;

                if (Tools.RayIntersectTriangle(mousePosition, -Vector3.UnitZ, vec1, vec2, vec3, ref pickCurDistance, ref barycentricU, ref barycentricV))
                {
                    currentIndex = index;
                }

                Items[index].MouseOver = false;
            }

            if (currentIndex != -1)
            {
                Items[currentIndex].MouseOver = true;
            }
        }

        public override void Update(GameTime gametime)
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
                CreateVertex();

            if (PercentVisibility == 1 && State == MenuState.Opening)
                State = MenuState.Opened;
            if (PercentVisibility == 0 && State == MenuState.Closing)
            {
                State = MenuState.Closed;
                Visible = false;
            }

            if (Alive && Visible)
            {
                MouseOver(gametime);
            }
        }

        public override void Draw(GameTime gameTime)
        {
            if (PercentVisibility == 0)
                return;

            Render.effect.Alpha = (float)PercentVisibility;

            Render.GraphicsDevice.SetVertexBuffer(vBuffer);

            foreach (EffectPass pass in Render.effect.CurrentTechnique.Passes)
            {
                pass.Apply();
                Render.GraphicsDevice.DrawPrimitives(PrimitiveType.TriangleList, 0, nbVertex);
            }

            if (ShowIcon)
            {
                Vector2 nearPoint = (ParentCell.Location + new Vector2(0.5f, .5f)) * Render.HexaWidth;

                float localSize = Context.MenuSize * Render.HexaWidth / 3f;
                double angleItem = MathHelper.TwoPi / (double)Items.Count;

                double d = 1.5;
                if (Items.Count == 9)
                {
                    d = 2.3;
                }

                Color color = new Color((float)PercentVisibility, (float)PercentVisibility, (float)PercentVisibility, (float)PercentVisibility);

                Render.SpriteBatch.End();
                Render.SpriteBatch.Begin(SpriteSortMode.BackToFront, BlendState.AlphaBlend, SamplerState.LinearClamp, DepthStencilState.DepthRead, RasterizerState.CullNone, Render.effectSprite, Matrix.Identity);
                for (int i = 0; i < Items.Count; i++)
                {
                    double angle = ((double)i - d) * angleItem - AngleDelta;

                    Vector2 vec = new Vector2(localSize * (float)Math.Cos(angle), localSize * (float)Math.Sin(angle));

                    Render.SpriteBatch.Draw(UI.GameEngine.Content.Load<Texture2D>(@"Texture\Icon\" + Items[i].Name), nearPoint + vec, null, color, 0f, new Vector2(64f), Context.MenuSize, SpriteEffects.None, 0f);
                }
                Render.SpriteBatch.End();
                Render.SpriteBatch.Begin();
            }
        }

        public override void UpdateUIDependency(GameTime gameTime)
        {
            Menu menuDependecy = this.UIDependency as Menu;

            if (menuDependecy != null && menuDependecy.State == MenuState.Closed && menuDependecy.Alive)
            {
                menuDependecy.Alive = false;
                this.Open(gameTime);
            }
        }
    }
}
