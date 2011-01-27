﻿using System;
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
        #endregion

        #region Membres privés
        private int nbVertex = 24;
        private float size = 10f;

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

        public virtual void Draw(GameEngine gameEngine, BasicEffect effect, GameTime gameTime)
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


            Vector3 nearPoint = gameEngine.GraphicsDevice.Viewport.Project(new Vector3(ParentCell.Location,10f),
                gameEngine.Render.Projection, gameEngine.Render.View, gameEngine.Render.World);

            float localSize = size * 300f / gameEngine.Render.CameraPosition.Z;
            float delta = 32f * localSize / 70f;

            gameEngine.Render.SpriteBatch.Begin();//SpriteSortMode.BackToFront, null, null, null, RasterizerState.CullClockwise, effect, mtx);
            for (int i = 0; i < Items.Count; i++)
            {
                string text = Items[i].Name;

                if (Items[i].Value != 0)
                    text = Items[i].Value.ToString();

                double angle =  MathHelper.TwoPi* (float)(i-1) / (float)(Items.Count);

                Vector2 vec = new Vector2(localSize * (float)Math.Cos(angle) - delta, localSize * (float)Math.Sin(angle) - delta);
                
                Rectangle rec = new Rectangle((int)(nearPoint.X + vec.X), (int)(nearPoint.Y + vec.Y),(int)(delta*2f), (int)(delta*2f));

                gameEngine.Render.SpriteBatch.Draw(gameEngine.Content.Load<Texture2D>(@"Texture\Icon\" + text), rec, new Color ((float)PercentVisibility,(float)PercentVisibility,(float)PercentVisibility, (float)PercentVisibility));
            }
            gameEngine.Render.SpriteBatch.End();
        }

        private void CreateVertex(GameEngine gameEngine)
        {
            vBuffer = new VertexBuffer(gameEngine.GraphicsDevice, typeof(VertexPositionColor), nbVertex * 3, BufferUsage.None);

            List<VertexPositionColor> vertex = new List<VertexPositionColor>();

            Color color = new Color(0.05f, 0.05f, 0.05f, (float)PercentVisibility);
            Color color2 = new Color(0.1f, 0.1f, 0.1f, (float)PercentVisibility);

            if (color.A > 240)
                color.A = 240;
            if (color2.A > 240)
                color2.A = 240;

            int nbPointPerItem = nbVertex / Items.Count;

            for (int i = 0; i < nbVertex; i++)
            {
                double angle1 = MathHelper.PiOver2 + MathHelper.Pi - MathHelper.TwoPi * PercentVisibility * (float)(i) / (float)nbVertex;
                double angle2 = MathHelper.PiOver2 + MathHelper.Pi - MathHelper.TwoPi * PercentVisibility * (float)(i + 1) / (float)nbVertex;

                if (i == nbVertex - 1)
                    angle2 = MathHelper.PiOver2 + MathHelper.Pi - MathHelper.TwoPi * PercentVisibility - 0.01f;

                Vector3 position1 = new Vector3(size * (float)Math.Cos(angle1), size * (float)Math.Sin(angle1), 10f);
                Vector3 position2 = new Vector3(size * (float)Math.Cos(angle2), size * (float)Math.Sin(angle2), 10f);

                int index = i / nbPointPerItem;
                if (index < Items.Count && Items[index].MouseOver)
                {
                    color.G = 100;
                }
                else
                    color.G = 10;

                vertex.Add(new VertexPositionColor(new Vector3(0, 0, 12f) + Tools.GetVector3(ParentCell.Location), color2));
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
                double angle1 = -MathHelper.TwoPi * ((float)(i) + 0.5f) / (float)(Items.Count);
                double angle2 = -MathHelper.TwoPi * ((float)(i + 1) + 0.5f) / (float)(Items.Count);

                Vector3 vec1 = new Vector3(size * 1.2f * (float)Math.Cos(angle1), size * 1.2f * (float)Math.Sin(angle1), 10f);
                Vector3 vec3 = new Vector3(size * 1.2f * (float)Math.Cos(angle2), size * 1.2f * (float)Math.Sin(angle2), 10f);

                vec1 = vec1 + Tools.GetVector3(ParentCell.Location);

                Vector3 vec2 = new Vector3(0, 0, 12f) + Tools.GetVector3(ParentCell.Location);

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
