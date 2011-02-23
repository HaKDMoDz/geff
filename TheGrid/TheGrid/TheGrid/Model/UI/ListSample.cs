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

namespace TheGrid.Model.UI
{
    public class ListSample : UIComponent
    {
        private Cell _cell;

        public ListSample(UILogic uiLogic, TimeSpan creationTime, Cell cell)
            : base(uiLogic, creationTime)
        {
            this.Modal = true;
            this.Alive = true;
            this.Visible = true;
            this.ListUIChildren = new List<UIComponent>();
            this._cell = cell;

            Vector2 sizeLibraryName = Render.FontText.MeasureString(new String(' ', 40)) + new Vector2(Ribbon.MARGE * 2, Ribbon.MARGE);

            Rec = new Rectangle((int)(Render.ScreenWidth / 2 - sizeLibraryName.X / 2), (int)(0.3f * Render.ScreenHeight), (int)sizeLibraryName.X, (int)(0.6f * Render.ScreenHeight));

            //--- Charge la liste des samples
            Vector2 vec = new Vector2(Rec.X + Ribbon.MARGE, Rec.Y + Ribbon.MARGE);

            foreach (Sample sample in _cell.Channel.ListSample)
            {
                ClickableText txtSample = new ClickableText(this.UI, creationTime, "FontText", sample.Name.Substring(0, Math.Min(20, sample.Name.Length)), vec, VisualStyle.ForeColor, VisualStyle.ForeColor, VisualStyle.BackColorLight, VisualStyle.BackForeColorMouseOver, false);
                txtSample.Rec = new Rectangle(txtSample.Rec.X, txtSample.Rec.Y, Rec.Width - 2 * Ribbon.MARGE, txtSample.Rec.Height);
                txtSample.Tag = sample;
                vec.Y += sizeLibraryName.Y + Ribbon.MARGE;

                txtSample.ClickText += new ClickableText.ClickTextHandler(txtSample_ClickText);
                txtSample.MiddleButtonClickText += new ClickableText.MiddleButtonClickTextHandler(txtSample_MiddleButtonClickText);
                txtSample.MouseEnter += new ClickableText.MouseEnterHandler(txtSample_MouseEnter);
                txtSample.MouseLeave += new ClickableText.MouseLeaveHandler(txtSample_MouseLeave);
                ListUIChildren.Add(txtSample);
            }
            //---

            //---
            KeyManager keyClose = AddKey(Keys.Escape);
            keyClose.KeyReleased += new KeyManager.KeyReleasedHandler(keyClose_KeyReleased);
            //---
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
            _cell.InitClip();
            _cell.Clip.Instrument = new InstrumentSample((Sample)clickableText.Tag);
            
            UI.GameEngine.GamePlay.EvaluateMuscianGrid();

            this.Alive = false;
        }

        public override void Draw(GameTime gameTime)
        {
            Render.SpriteBatch.Draw(Render.texEmpty, Render.GraphicsDevice.Viewport.Bounds, VisualStyle.BackColorModalScreen);

            Render.SpriteBatch.Draw(Render.texEmptyGradient, Rec, VisualStyle.BackColorLight);

            base.Draw(gameTime);
        }
    }
}
