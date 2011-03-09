using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TheGrid.Logic.UI;
using TheGrid.Model.Effect;
using Microsoft.Xna.Framework;
using TheGrid.Model.Instrument;
using Microsoft.Xna.Framework.Graphics;
using TheGrid.Common;
using TheGrid.Logic.Controller;
using Microsoft.Xna.Framework.Input;

namespace TheGrid.Model.UI.Effect
{
    public class EffectPropertyChanger : UIComponent
    {
        public const int WIDTH = 150;
        public const int HEIGHT = 200;
        private EffectProperty effectProperty { get; set; }
        private Vector2 vecDescription = Vector2.Zero;
        private Vector2 vecMinValue = Vector2.Zero;
        private Vector2 vecMaxValue = Vector2.Zero;
        private Vector2 vecValue = Vector2.Zero;
        private Vector2 vecCenter = Vector2.Zero;
        private bool mouseOverPot;
        private bool valueCanBeChanged;
        private float prevValue;
        public int nbVertex = 50;
        private VertexBuffer vBuffer;

        public EffectPropertyChanger(UILogic uiLogic, UIComponent parent, TimeSpan creationTime, EffectProperty effectProperty)
            : base(uiLogic, parent, creationTime)
        {
            this.Modal = false;
            this.Alive = true;
            this.Visible = true;
            this.ListUIChildren = new List<UIComponent>();
            this.effectProperty = effectProperty;

            //---
            MouseManager mouseLeft = AddMouse(MouseButtons.LeftButton);
            mouseLeft.MouseFirstPressed += new MouseManager.MouseFirstPressedHandler(mouseLeft_MouseFirstPressed);
            mouseLeft.MousePressed += new MouseManager.MousePressedHandler(mouseLeft_MousePressed);
            mouseLeft.MouseReleased += new MouseManager.MouseReleasedHandler(mouseLeft_MouseReleased);
            //---
        }

        void mouseLeft_MouseReleased(MouseButtons mouseButton, MouseState mouseState, GameTime gameTime, Point distance)
        {
            valueCanBeChanged = false;

            CreateVertex();
        }

        void mouseLeft_MousePressed(MouseButtons mouseButton, MouseState mouseState, GameTime gameTime, Point distance)
        {
            if (valueCanBeChanged)
            {
                float percent = distance.X / ((float)WIDTH);

                if (percent > 1f)
                    percent = 1f;

                effectProperty.Value = prevValue + percent * (effectProperty.MaxValue - effectProperty.MinValue);

                if (effectProperty.Value > effectProperty.MaxValue)
                    effectProperty.Value = effectProperty.MaxValue;

                if (effectProperty.Value < effectProperty.MinValue)
                    effectProperty.Value = effectProperty.MinValue;

                CreateVertex();
            }
        }

        void mouseLeft_MouseFirstPressed(MouseButtons mouseButton, MouseState mouseState, GameTime gameTime)
        {
            if (mouseOverPot)
            {
                prevValue = effectProperty.Value;
                valueCanBeChanged = true;
            }
        }

        public void Init()
        {
            vecCenter = new Vector2((float)Rec.X + WIDTH *0.5f, Rec.Y + HEIGHT *0.6f);

            Vector2 sizeText = Render.FontTextSmall.MeasureString(effectProperty.Description);
            vecDescription = new Vector2(Rec.X + Ribbon.MARGE + Rec.Width * 0.5f - sizeText.X / 2, Rec.Y + Ribbon.MARGE);

            sizeText = Render.FontTextSmall.MeasureString(effectProperty.MinValue.ToString());
            vecMinValue = new Vector2(Rec.X + Ribbon.MARGE + Rec.Width * 0.2f - sizeText.X / 2, Rec.Y + +Ribbon.MARGE * 5);

            sizeText = Render.FontTextSmall.MeasureString(effectProperty.MaxValue.ToString());
            vecMaxValue = new Vector2(Rec.X + Ribbon.MARGE + Rec.Width * 0.8f - sizeText.X / 2, Rec.Y + +Ribbon.MARGE * 5);

            sizeText = Render.FontTextSmall.MeasureString(effectProperty.Value.ToString());
            vecValue = new Vector2(Rec.X + Ribbon.MARGE + Rec.Width * 0.5f - sizeText.X / 2, Rec.Y + Ribbon.MARGE * 5);

            CreateVertex();
        }

        private void CreateVertex()
        {
            List<VertexPositionColor> vertex = new List<VertexPositionColor>();

            Color color = new Color(0.05f, 0.05f, 0.05f, 1f);
            Color color2 = new Color(0.3f, 0.3f, 0.3f, 1f);
            Color colorInside = new Color(0.5f, 0.5f, 0.5f, 1f);
            Color colorOutside = new Color(0.1f, 0.1f, 0.1f, 1f);
            Vector3 vecCenter3 = Tools.GetVector3(vecCenter);

            if (mouseOverPot || valueCanBeChanged)
            {
                color = new Color(0.05f, 0.1f, 0.05f, 1f);
                color2 = new Color(0.3f, 0.4f, 0.3f, 1f);
            }
            else
            {
                color = new Color(0.05f, 0.05f, 0.05f, 1f);
                color2 = new Color(0.3f, 0.3f, 0.3f, 1f);
            }

            vBuffer = new VertexBuffer(Render.GraphicsDevice, typeof(VertexPositionColor), nbVertex * 3, BufferUsage.None);

            float localSize = Context.MenuSize * 20f;
            float percent = (effectProperty.Value - effectProperty.MinValue) / (effectProperty.MaxValue - effectProperty.MinValue);

            int nbVertexInside = (int)((float)nbVertex * percent);

            if (nbVertexInside == 0)
                percent = 0f;

            int nbVertexOutside = nbVertex - nbVertexInside;
            double angleItemInside = MathHelper.TwoPi * percent;
            double angleItemOutside = MathHelper.TwoPi * (1f - percent);
            double angleVertexInside = angleItemInside / (double)nbVertexInside;
            double angleVertexOutside = angleItemOutside / (double)nbVertexOutside;

            for (int i = 0; i < nbVertexInside; i++)
            {
                double angle1 = (double)i * angleVertexInside - MathHelper.PiOver2;
                double angle2 = (double)(i + 1) * angleVertexInside - MathHelper.PiOver2;

                Vector3 position1 = new Vector3(localSize * (float)Math.Cos(angle1), localSize * (float)Math.Sin(angle1), 0f);
                Vector3 position2 = new Vector3(localSize * (float)Math.Cos(angle2), localSize * (float)Math.Sin(angle2), 0f);

                vertex.Add(new VertexPositionColor(position2 + vecCenter3, color2));
                vertex.Add(new VertexPositionColor(vecCenter3, colorInside));
                vertex.Add(new VertexPositionColor(position1 + vecCenter3, color2));
            }

            for (int i = 0; i < nbVertexOutside; i++)
            {
                double angle1 = angleItemInside + (double)i * angleVertexOutside - MathHelper.PiOver2;
                double angle2 = angleItemInside + (double)(i + 1) * angleVertexOutside - MathHelper.PiOver2;

                Vector3 position1 = new Vector3(localSize * (float)Math.Cos(angle1), localSize * (float)Math.Sin(angle1), -5f);
                Vector3 position2 = new Vector3(localSize * (float)Math.Cos(angle2), localSize * (float)Math.Sin(angle2), -5f);

                vertex.Add(new VertexPositionColor(position2 + vecCenter3, color));
                vertex.Add(new VertexPositionColor(vecCenter3, colorOutside));
                vertex.Add(new VertexPositionColor(position1 + vecCenter3, color));
            }

            vBuffer.SetData<VertexPositionColor>(vertex.ToArray());
        }

        public override void Update(GameTime gameTime)
        {
            bool prevMouseOverPot = mouseOverPot;

            mouseOverPot = false;

            if (Vector2.Distance(Controller.mousePosition, vecCenter) < Context.MenuSize * 20f)
                mouseOverPot = true;

            if (prevMouseOverPot != mouseOverPot)
                CreateVertex();

            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            Render.SpriteBatch.Draw(Render.texEmptyGradient, Rec, VisualStyle.BackColorLight2);

            Render.SpriteBatch.DrawString(Render.FontTextSmall, effectProperty.Description, vecDescription, Color.White);
            Render.SpriteBatch.DrawString(Render.FontTextSmall, effectProperty.MinValue.ToString(), vecMinValue, Color.White);
            Render.SpriteBatch.DrawString(Render.FontTextSmall, effectProperty.MaxValue.ToString(), vecMaxValue, Color.White);
            Render.SpriteBatch.DrawString(Render.FontTextSmall, ((int)effectProperty.Value).ToString(), vecValue, Color.White);

            Render.SpriteBatch.End();

            //--- Affichage du potard
            Render.GraphicsDevice.SetVertexBuffer(vBuffer);

            foreach (EffectPass pass in Render.effectUI.CurrentTechnique.Passes)
            {
                pass.Apply();
                Render.GraphicsDevice.DrawPrimitives(PrimitiveType.TriangleList, 0, nbVertex);
            }
            //---

            Render.SpriteBatch.Begin();

            base.Draw(gameTime);
        }
    }
}
