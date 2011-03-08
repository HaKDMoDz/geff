using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using TheGrid.Logic.UI;
using System.IO;
using TheGrid.Common;
using TheGrid.Model.Instrument;
using TheGrid.Logic.Controller;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;

namespace TheGrid.Model.UI
{
    public class ListSample : ListBase
    {
        private Cell _cell;

        public ListSample(UILogic uiLogic, TimeSpan creationTime, Cell cell, Channel channel, Rectangle rec, SpriteFont font, bool checkable)
            : base(uiLogic, creationTime, rec, font, checkable)
        {
            this.Alive = true;
            this.Visible = true;
            this.ListUIChildren = new List<UIComponent>();
            this._cell = cell;

            LoadSample(channel);

            //---
            KeyManager keyClose = AddKey(Keys.Escape);
            keyClose.KeyReleased += new KeyManager.KeyReleasedHandler(keyClose_KeyReleased);
            //---
        }

        public void LoadSample(Channel channel)
        {
            //--- Charge la liste des samples
            ListUIChildren.Clear();

            Vector2 vec = new Vector2(Rec.X + Ribbon.MARGE, Rec.Y + Ribbon.MARGE);

            foreach (Sample sample in channel.ListSample)
            {
                ClickableText txtSample = AddItem(sample.Name, sample);
                    
                txtSample.ClickText += new ClickableText.ClickTextHandler(txtSample_ClickText);
                txtSample.MiddleButtonClickText += new ClickableText.MiddleButtonClickTextHandler(txtSample_MiddleButtonClickText);
                txtSample.MouseEnter += new ClickableText.MouseEnterHandler(txtSample_MouseEnter);
                txtSample.MouseLeave += new ClickableText.MouseLeaveHandler(txtSample_MouseLeave);

                if (IsCheckable && ListUIChildren.Count == 1)
                {
                    txtSample.IsChecked = true;
                    OnSelectedItemChanged(sample);
                }
            }
            //---

            UpdateScrollbar();
            UpdateScrollValue();
        }

        void txtSample_MiddleButtonClickText(ClickableText clickableText, MouseState mouseState, GameTime gameTime)
        {
            UI.GameEngine.Sound.PlaySample((Sample)clickableText.Tag);
        }

        void txtSample_MouseEnter(ClickableText clickableText, MouseState mouseState, GameTime gameTime)
        {
            UI.GameEngine.Sound.PlaySample((Sample)clickableText.Tag);
        }

        void txtSample_MouseLeave(ClickableText clickableText, MouseState mouseState, GameTime gameTime)
        {
            UI.GameEngine.Sound.Stop(((Sample)clickableText.Tag).Name);
        }

        void keyClose_KeyReleased(Keys key, GameTime gameTime)
        {
            this.Alive = false;
        }

        void txtSample_ClickText(ClickableText clickableText, Microsoft.Xna.Framework.Input.MouseState mouseState, GameTime gameTime)
        {
            UI.GameEngine.Sound.Stop(((Sample)clickableText.Tag).Name);

            if (IsCheckable)
            {
                bool checkItem = clickableText.IsChecked;

                foreach (UIComponent component in ListUIChildren)
                {
                    if(component is ClickableText)
                        ((ClickableText)component).IsChecked = false;
                }

                clickableText.IsChecked = checkItem;

                OnSelectedItemChanged(clickableText.Tag);
            }

            if (_cell != null)
            {
                _cell.InitClip();
                _cell.Clip.Instrument = new InstrumentSample((Sample)clickableText.Tag);

                UI.GameEngine.GamePlay.EvaluateMuscianGrid();

                this.Alive = false;
            }
        }

        public override void Draw(GameTime gameTime)
        {
            if(Modal)
                Render.SpriteBatch.Draw(Render.texEmpty, Render.GraphicsDevice.Viewport.Bounds, VisualStyle.BackColorModalScreen);

            Render.SpriteBatch.Draw(Render.texEmptyGradient, Rec, VisualStyle.BackColorLight);

            base.Draw(gameTime);
        }
    }
}
