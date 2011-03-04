﻿using System;
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
    public class ListSample : UIComponent
    {
        private Cell _cell;
        public SpriteFont Font { get; set; }
        public int ScrollValue { get; set; }
        public bool IsCheckable { get; set; }

        public delegate void SelectedSampleChangedHandler(Sample sample);
        public event SelectedSampleChangedHandler SelectedSampleChanged;

        public ListSample(UILogic uiLogic, TimeSpan creationTime, Cell cell, Channel channel, Rectangle rec, SpriteFont font, bool checkable)
            : base(uiLogic, creationTime)
        {
            this.Alive = true;
            this.Visible = true;
            this.ListUIChildren = new List<UIComponent>();
            this._cell = cell;
            this.Font = font;
            this.IsCheckable = checkable;

            Vector2 sizeLibraryName = font.MeasureString(new String(' ', 40)) + new Vector2(Ribbon.MARGE * 2, Ribbon.MARGE);

            if(sizeLibraryName.X> rec.Width)
                sizeLibraryName.X = rec.Width;

            int countMaxItem = (rec.Height - 2 * MARGE) / (int)sizeLibraryName.Y;

            Rec = new Rectangle(
                rec.X, 
                rec.Y, 
                Math.Min((int)sizeLibraryName.X, rec.Width),
                (int)(countMaxItem * sizeLibraryName.Y));

            //--- Charge la liste des samples
            Vector2 vec = new Vector2(Rec.X + Ribbon.MARGE, Rec.Y + Ribbon.MARGE);

            foreach (Sample sample in channel.ListSample)
            {
                ClickableText txtSample = new ClickableText(this.UI, creationTime, font, sample.Name.Substring(0, Math.Min(20, sample.Name.Length)), vec, VisualStyle.ForeColor, VisualStyle.ForeColor, VisualStyle.BackColorLight, VisualStyle.BackForeColorMouseOver, checkable);
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

            //--- Charge les boutons
            //TODO améliorer la scroll
            if (countMaxItem > channel.ListSample.Count)
            {
                ClickableImage imgUp = new ClickableImage(UI, GetNewTimeSpan(), "ArrowUp", GetIcon("ArrowUp"), GetIcon("ArrowUp"), new Vector2(Rec.Right - GetIcon("ArrowUp").Width, Rec.Top));
                ListUIChildren.Add(imgUp);

                ClickableImage imgDown = new ClickableImage(UI, GetNewTimeSpan(), "ArrowDown", GetIcon("ArrowDown"), GetIcon("ArrowDown"), new Vector2(Rec.Right - GetIcon("ArrowUp").Width, Rec.Bottom - GetIcon("ArrowUp").Height));
                ListUIChildren.Add(imgDown);
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

                if (SelectedSampleChanged != null)
                    SelectedSampleChanged((Sample)clickableText.Tag);
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
