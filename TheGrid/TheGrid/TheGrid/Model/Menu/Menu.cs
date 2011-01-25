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
        #endregion

        #region Membres privés
        private int nbVertex = 24;
        private float size = 10f;

        private double percentVisibility = 0;
        private VertexBuffer vBuffer;
        private TimeSpan LastStateChanged { get; set; }
        #endregion

        public Menu(Cell parentCell, Menu parentMenu)
        {
            ParentCell = parentCell;
            ParentMenu = parentMenu;

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
                percentVisibility = elapsedTimeMenu / MENU_ANIMATION_DURATION;

            if (State == MenuState.Closing)
                percentVisibility = 1 - elapsedTimeMenu / MENU_ANIMATION_DURATION;

            if (percentVisibility > 1)
                percentVisibility = 1;
            if (percentVisibility < 0)
                percentVisibility = 0;

            if (State == MenuState.Opening || State == MenuState.Closing || State == MenuState.Opened)
                CreateVertex(gameEngine);

            if (percentVisibility == 1 && State == MenuState.Opening)
                State = MenuState.Opened;
            if (percentVisibility == 0 && State == MenuState.Closing)
                State = MenuState.Closed;
        }

        public virtual void Draw(GameEngine gameEngine, BasicEffect effect, GameTime gameTime)
        {
            if (percentVisibility == 0)
                return;

            effect.Alpha = (float)percentVisibility;
            gameEngine.GraphicsDevice.SetVertexBuffer(vBuffer);

            foreach (EffectPass pass in effect.CurrentTechnique.Passes)
            {
                pass.Apply();
                gameEngine.GraphicsDevice.DrawPrimitives(PrimitiveType.TriangleList, 0, nbVertex);
            }
        }

        private void CreateVertex(GameEngine gameEngine)
        {
            vBuffer = new VertexBuffer(gameEngine.GraphicsDevice, typeof(VertexPositionColor), nbVertex * 3, BufferUsage.None);

            List<VertexPositionColor> vertex = new List<VertexPositionColor>();

            Color color = new Color(0.05f, 0.05f, 0.05f, (float)percentVisibility);
            Color color2 = new Color(0.08f, 0.08f, 0.09f, (float)percentVisibility);

            int nbPointPerItem = nbVertex / Items.Count;

            for (int i = 0; i < nbVertex; i++)
            {
                double angle1 = MathHelper.PiOver2 + MathHelper.Pi - MathHelper.TwoPi * percentVisibility * (float)(i) / (float)nbVertex;
                double angle2 = MathHelper.PiOver2 + MathHelper.Pi - MathHelper.TwoPi * percentVisibility * (float)(i + 1) / (float)nbVertex;

                if (i == nbVertex - 1)
                    angle2 = MathHelper.PiOver2 + MathHelper.Pi - MathHelper.TwoPi * percentVisibility - 0.01f;

                Vector3 position1 = new Vector3(size * (float)Math.Cos(angle1), size * (float)Math.Sin(angle1), 1f);
                Vector3 position2 = new Vector3(size * (float)Math.Cos(angle2), size * (float)Math.Sin(angle2), 1f);

                int index = i / nbPointPerItem;
                if (index < Items.Count && Items[index].MouseOver)
                {
                    color.B = 100;
                }
                else
                    color.B = 10;

                vertex.Add(new VertexPositionColor(new Vector3(0, 0, 1f) + Tools.GetVector3(ParentCell.Location), color2));
                vertex.Add(new VertexPositionColor(position1 + Tools.GetVector3(ParentCell.Location), color));
                vertex.Add(new VertexPositionColor(position2 + Tools.GetVector3(ParentCell.Location), color));
            }

            vBuffer.SetData<VertexPositionColor>(vertex.ToArray());
        }

        public void MouseClick(GameEngine gameEngine, GameTime gameTime, MouseState mouseState)
        {
            float pickCurDistance = 0f;
            float barycentricU = 0f;
            float barycentricV = 0f;

            Vector3 nearsource = new Vector3((float)mouseState.X, (float)mouseState.Y, 0f);
            Vector3 farsource = new Vector3((float)mouseState.X, (float)mouseState.Y, 1f);

            Matrix world = Matrix.CreateTranslation(0, 0, 0);

            Vector3 nearPoint = gameEngine.GraphicsDevice.Viewport.Unproject(nearsource,
                gameEngine.Render.Projection, gameEngine.Render.View, world);

            Vector3 farPoint = gameEngine.GraphicsDevice.Viewport.Unproject(farsource,
                gameEngine.Render.Projection, gameEngine.Render.View, world);

            Vector3 direction = farPoint - nearPoint;
            direction.Normalize();

            int currentIndex = -1;
            for (int i = 0; i < Items.Count; i++)
            {
                double angle1 = -MathHelper.TwoPi * (float)(i) / (float)(Items.Count);
                double angle2 = -MathHelper.TwoPi * (float)(i + 1) / (float)(Items.Count);

                Vector3 vec1 = new Vector3(size * (float)Math.Cos(angle1), size * (float)Math.Sin(angle1), 1f);
                Vector3 vec3 = new Vector3(size * (float)Math.Cos(angle2), size * (float)Math.Sin(angle2), 1f);

                vec1 = vec1 + Tools.GetVector3(ParentCell.Location);

                Vector3 vec2 = new Vector3(0, 0, 1f) + Tools.GetVector3(ParentCell.Location);

                vec3 = vec3 + Tools.GetVector3(ParentCell.Location);

                if (Tools.RayIntersectTriangle(nearPoint, direction, vec1, vec2, vec3, ref pickCurDistance, ref barycentricU, ref barycentricV))
                {
                    currentIndex = i;
                }
            }

            if (currentIndex != -1)
            {
                //Items[currentIndex].OnSelected(gameTime);

                gameEngine.Window.Title = currentIndex.ToString();
            }
        }

        public void MouseOver(GameEngine gameEngine, GameTime gameTime, MouseState mouseState)
        {
            float pickCurDistance = 0f;
            float barycentricU = 0f;
            float barycentricV = 0f;

            Vector3 nearsource = new Vector3((float)mouseState.X, (float)mouseState.Y, 0f);
            Vector3 farsource = new Vector3((float)mouseState.X, (float)mouseState.Y, 1f);

            Matrix world = Matrix.CreateTranslation(0, 0, 0);

            Vector3 nearPoint = gameEngine.GraphicsDevice.Viewport.Unproject(nearsource,
                gameEngine.Render.Projection, gameEngine.Render.View, world);

            Vector3 farPoint = gameEngine.GraphicsDevice.Viewport.Unproject(farsource,
                gameEngine.Render.Projection, gameEngine.Render.View, world);

            Vector3 direction = farPoint - nearPoint;
            direction.Normalize();

            int currentIndex = -1;
            for (int i = 0; i < Items.Count; i++)
            {
                double angle1 = -MathHelper.TwoPi * ((float)(i)+0.5f) / (float)(Items.Count);
                double angle2 = -MathHelper.TwoPi * ((float)(i + 1)+0.5f) / (float)(Items.Count);

                Vector3 vec1 = new Vector3(size * 1.2f * (float)Math.Cos(angle1), size *1.2f* (float)Math.Sin(angle1), 1f);
                Vector3 vec3 = new Vector3(size *1.2f* (float)Math.Cos(angle2), size *1.2f* (float)Math.Sin(angle2), 1f);

                vec1 = vec1 + Tools.GetVector3(ParentCell.Location);

                Vector3 vec2 = new Vector3(0, 0, 1f) + Tools.GetVector3(ParentCell.Location);

                vec3 = vec3 + Tools.GetVector3(ParentCell.Location);

                if (Tools.RayIntersectTriangle(nearPoint, direction, vec1, vec2, vec3, ref pickCurDistance, ref barycentricU, ref barycentricV))
                {
                    currentIndex = i;
                }

                Items[i].MouseOver = false;
            }

            if (currentIndex == 0)
                currentIndex = Items.Count;

            if (currentIndex != -1)
            {
                currentIndex -= 1;

                Items[currentIndex].MouseOver = true;

                gameEngine.Window.Title = currentIndex.ToString();
            }
        }
    }
}
