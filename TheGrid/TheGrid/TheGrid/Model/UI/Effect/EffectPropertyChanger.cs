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

namespace TheGrid.Model.UI.Effect
{
    public class EffectPropertyChanger : UIComponent
    {
        public const int WIDTH = 200;
        public const int HEIGHT = 300;
        private EffectProperty effectProperty { get; set; }
        private Vector2 vecDescription = Vector2.Zero;
        private Vector2 vecMinValue = Vector2.Zero;
        private Vector2 vecMaxValue = Vector2.Zero;
        private Vector2 vecValue = Vector2.Zero;

        public int nbVertex = 50;
        private VertexBuffer vBuffer;

        public EffectPropertyChanger(UILogic uiLogic, TimeSpan creationTime, EffectProperty effectProperty)
            : base(uiLogic, creationTime)
        {
            this.Modal = true;
            this.Alive = true;
            this.Visible = true;
            this.ListUIChildren = new List<UIComponent>();
            this.effectProperty = effectProperty;
        }

        public void Init()
        {
            Vector2 sizeText = Render.FontText.MeasureString(effectProperty.Description);
            vecDescription = new Vector2(Rec.X + Ribbon.MARGE + Rec.Width * 0.5f - sizeText.X / 2, Rec.Y + Ribbon.MARGE);

            sizeText = Render.FontText.MeasureString(effectProperty.MinValue.ToString());
            vecMinValue = new Vector2(Rec.X + Ribbon.MARGE + Rec.Width * 0.2f - sizeText.X / 2, Rec.Y + +Ribbon.MARGE * 5);

            sizeText = Render.FontText.MeasureString(effectProperty.MaxValue.ToString());
            vecMaxValue = new Vector2(Rec.X + Ribbon.MARGE + Rec.Width * 0.8f - sizeText.X / 2, Rec.Y + +Ribbon.MARGE * 5);

            sizeText = Render.FontText.MeasureString(effectProperty.Value.ToString());
            vecValue = new Vector2(Rec.X + Ribbon.MARGE + Rec.Width * 0.5f - sizeText.X / 2, Rec.Y + Ribbon.MARGE * 5);

            CreateVertex();
        }

        private void CreateVertex()
        {
            List<VertexPositionColor> vertex = new List<VertexPositionColor>();

            Vector3 center = new Vector3((float)Rec.X + WIDTH / 2f, 0f, 0f);

            Color color = new Color(0.05f, 0.05f, 0.05f, 1f);
            Color colorInside = new Color(0.5f, 0.5f, 0.5f, 1f);
            Color colorOutside = new Color(0.1f, 0.1f, 0.1f, 1f);

            vBuffer = new VertexBuffer(Render.GraphicsDevice, typeof(VertexPositionColor), nbVertex * 3, BufferUsage.None);

            float localSize = Context.MenuSize * 20f;// Render.HexaWidth / 2f;
            float percent = Math.Abs(effectProperty.Value) / Math.Abs((effectProperty.MaxValue - effectProperty.MinValue));
            int nbVertexInside = (int)((float)nbVertex * percent);
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

                vertex.Add(new VertexPositionColor(position2 + center, color));
                vertex.Add(new VertexPositionColor(center, colorInside));
                vertex.Add(new VertexPositionColor(position1 + center, color));
            }

            for (int i = 0; i < nbVertexOutside; i++)
            {
                double angle1 = angleItemInside + (double)i * angleVertexOutside - MathHelper.PiOver2;
                double angle2 = angleItemInside + (double)(i + 1) * angleVertexOutside - MathHelper.PiOver2;

                Vector3 position1 = new Vector3(localSize * (float)Math.Cos(angle1), localSize * (float)Math.Sin(angle1), -5f);
                Vector3 position2 = new Vector3(localSize * (float)Math.Cos(angle2), localSize * (float)Math.Sin(angle2), -5f);

                vertex.Add(new VertexPositionColor(position2 + center, color));
                vertex.Add(new VertexPositionColor(center, colorOutside));
                vertex.Add(new VertexPositionColor(position1 + center, color));
            }

            vBuffer.SetData<VertexPositionColor>(vertex.ToArray());
        }

        public override void Draw(GameTime gameTime)
        {
            Render.SpriteBatch.Draw(Render.texEmptyGradient, Rec, new Color(0.2f, 0.2f, 0.2f, 0.95f));

            Render.SpriteBatch.DrawString(Render.FontText, effectProperty.Description, vecDescription, Color.White);
            Render.SpriteBatch.DrawString(Render.FontText, effectProperty.MinValue.ToString(), vecMinValue, Color.White);
            Render.SpriteBatch.DrawString(Render.FontText, effectProperty.MaxValue.ToString(), vecMaxValue, Color.White);
            Render.SpriteBatch.DrawString(Render.FontText, effectProperty.Value.ToString(), vecValue, Color.White);

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
