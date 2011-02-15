using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TheGrid.Logic.UI;
using Microsoft.Xna.Framework;
using TheGrid.Logic.Controller;
using TheGrid.Model.Effect;
using Microsoft.Xna.Framework.Input;

namespace TheGrid.Model.UI.Effect
{
    public class EffectPanel : UIComponent
    {
        private ChannelEffect channelEffect { get; set; }
        private float widthChannelChooser = 200f;

        public EffectPanel(UILogic uiLogic, TimeSpan creationTime, ChannelEffect channelEffect)
            : base(uiLogic, creationTime)
        {
            this.Modal = true;
            this.Alive = true;
            this.Visible = true;
            this.ListUIChildren = new List<UIComponent>();

            Vector2 sizeWindow = new Vector2(widthChannelChooser + Math.Min(channelEffect.ListEffectProperty.Count,3) * (EffectPropertyChanger.WIDTH + Ribbon.MARGE), (int)Math.Round((float)channelEffect.ListEffectProperty.Count /3f,  MidpointRounding.AwayFromZero) *EffectPropertyChanger.HEIGHT);

            Rec = new Rectangle((int)(Render.ScreenWidth / 2 - sizeWindow.X / 2), (int)(Render.ScreenHeight / 2 - sizeWindow.Y / 2), (int)sizeWindow.X, (int)(sizeWindow.Y));

            Vector2 vec = new Vector2(Rec.X + widthChannelChooser + Ribbon.MARGE, Rec.Y);
            int nb = 0;

            foreach (EffectProperty effectProperty in channelEffect.ListEffectProperty)
            {
                EffectPropertyChanger effectPropertyChanger = new EffectPropertyChanger(UI, GetNewTimeSpan(), effectProperty);
                effectPropertyChanger.Rec = new Rectangle((int)vec.X, (int)vec.Y, EffectPropertyChanger.WIDTH, EffectPropertyChanger.HEIGHT);
                effectPropertyChanger.Init();

                nb++;

                if (nb % 3 == 0)
                {
                    vec.X = Rec.X + widthChannelChooser + Ribbon.MARGE;
                    vec.Y += EffectPropertyChanger.HEIGHT + Ribbon.MARGE;
                }
                else
                    vec.X += EffectPropertyChanger.WIDTH + Ribbon.MARGE;

                this.ListUIChildren.Add(effectPropertyChanger);
            }

            //---
            KeyManager keyClose = AddKey(Keys.Escape);
            keyClose.KeyReleased += new KeyManager.KeyReleasedHandler(keyClose_KeyReleased);
            //---
        }

        void keyClose_KeyReleased(Keys key, GameTime gameTime)
        {
            this.Alive = false;
        }

        public override void Draw(GameTime gameTime)
        {
            Render.SpriteBatch.Draw(Render.texEmpty, Render.GraphicsDevice.Viewport.Bounds, new Color(0.1f, 0.1f, 0.1f, 0.85f));

            Render.SpriteBatch.Draw(Render.texEmptyGradient, Rec, new Color(0.2f, 0.2f, 0.2f, 0.95f));

            base.Draw(gameTime);
        }
    }
}
