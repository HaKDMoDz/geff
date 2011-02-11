using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using TheGrid.Logic.UI;
using System.IO;
using TheGrid.Common;
using System.Windows.Forms;
using TheGrid.Model.Instrument;

namespace TheGrid.Model.UI
{
    public class ListSample : UIComponent
    {
        private Cell _cell;

        public ListSample(UILogic uiLogic, TimeSpan creationTime, Cell cell)
            : base(uiLogic, creationTime)
        {
            this.Alive = true;
            this.Visible = true;
            this.ListUIChildren = new List<UIComponent>();
            this._cell = cell;

            Vector2 sizeLibraryName = Render.FontText.MeasureString(new String(' ', 40)) + new Vector2(Ribbon.MARGE * 2, Ribbon.MARGE * 2);

            Rec = new Rectangle((int)(Render.ScreenWidth / 2 - sizeLibraryName.X / 2), (int)(0.3f * Render.ScreenHeight), (int)sizeLibraryName.X, (int)(0.6f * Render.ScreenHeight));

            //--- Charge la liste des samples
            Vector2 vec = new Vector2(Rec.X + Ribbon.MARGE, Rec.Y + Ribbon.MARGE);

            foreach (Sample sample in _cell.Channel.ListSample)
            {
                ClickableText txtSample = new ClickableText(this.UI, creationTime, "FontText", sample.Name.Substring(0, Math.Min(20, sample.Name.Length)), vec, Color.White, Color.LightBlue);
                txtSample.Tag = sample;
                vec.Y += sizeLibraryName.Y + Ribbon.MARGE;

                txtSample.ClickText += new ClickableText.ClickTextHandler(txtSample_ClickText);
                ListUIChildren.Add(txtSample);
            }
            //---
        }

        void txtSample_ClickText(ClickableText clickableText, Microsoft.Xna.Framework.Input.MouseState mouseState, GameTime gameTime)
        {
            _cell.InitClip();
            _cell.Clip.Instrument = new InstrumentSample((Sample)clickableText.Tag);

            this.Alive = false;
        }

        public override void Draw(GameTime gameTime)
        {
            Render.SpriteBatch.Draw(Render.texEmptyGradient, Rec, new Color(0.2f, 0.2f, 0.2f, 0.95f));

            base.Draw(gameTime);
        }
    }
}
