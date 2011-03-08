using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using TheGrid.Logic.UI;
using TheGrid.Common;
using TheGrid.Logic.Controller;
using Microsoft.Xna.Framework.Input;

namespace TheGrid.Model.UI
{
    public class ListBase : UIComponent
    {
        public SpriteFont Font { get; set; }
        public int ScrollValue { get; set; }
        public bool IsCheckable { get; set; }
        protected int countMaxItem;
        protected Vector2 sizeText;
        protected int CountItem;
        private Rectangle recInitial;

        public delegate void SelectedItemChangedHandler(Object item);
        public event SelectedItemChangedHandler SelectedItemChanged;

        public ListBase(UILogic uiLogic, TimeSpan creationTime, Rectangle rec, SpriteFont font, bool checkable)
            : base(uiLogic, creationTime)
        {
            this.Font = font;
            this.IsCheckable = checkable;
            recInitial = rec;

            sizeText = font.MeasureString(new String(' ', 40)) + new Vector2(Ribbon.MARGE * 2, Ribbon.MARGE);

            countMaxItem = (rec.Height - 2 * MARGE) / (int)sizeText.Y;

            Rec = new Rectangle(
                recInitial.X,
                recInitial.Y,
                Math.Min((int)sizeText.X, recInitial.Width),
                (int)(countMaxItem * sizeText.Y));

            if (sizeText.X > Rec.Width)
                sizeText.X = Rec.Width - 2 * MARGE;

            //--- Molette de la souris
            MouseManager mouseWheel = AddMouse(MouseButtons.Wheel);
            mouseWheel.MouseWheelChanged += new MouseManager.MouseWheelChangedHandler(mouseWheel_MouseWheelChanged);
            //---
        }

        void mouseWheel_MouseWheelChanged(MouseState mouseState, GameTime gameTime, int prevMousewheel)
        {
            ScrollValue += (mouseState.ScrollWheelValue - prevMousewheel) / 15;

            int scrollMinValue = (int)Math.Min(0, -(CountItem + 1 - countMaxItem) * sizeText.Y);

            if (ScrollValue > 0)
                ScrollValue = 0;

            if (ScrollValue < scrollMinValue)
                ScrollValue = scrollMinValue;

            UpdateScrollValue();
        }

        public ClickableText AddItem(string itemName, object value)
        {
            Vector2 vec = new Vector2(Rec.X + Ribbon.MARGE, Rec.Y + sizeText.Y * CountItem + MARGE);
            ClickableText txtItem = new ClickableText(this.UI, GetNewTimeSpan(), Font, itemName.Substring(0, Math.Min(20, itemName.Length)), vec, VisualStyle.ForeColor, VisualStyle.ForeColor, VisualStyle.BackColorLight, VisualStyle.BackForeColorMouseOver, IsCheckable);
            txtItem.Rec = new Rectangle((int)vec.X, (int)vec.Y, (int)sizeText.X, (int)sizeText.Y);
            txtItem.Tag = value;

            ListUIChildren.Add(txtItem);
            CountItem++;

            return txtItem;
        }

        public void OnSelectedItemChanged(Object item)
        {
            if (SelectedItemChanged != null)
                SelectedItemChanged(item);
        }

        public void UpdateScrollbar()
        {
            //--- Charge les boutons
            if (countMaxItem < CountItem)
            {
                ClickableImage imgUp = new ClickableImage(UI, GetNewTimeSpan(), "ArrowUp", GetIcon("ArrowUp"), GetIcon("ArrowUp"), new Vector2(Rec.Right - GetIcon("ArrowUp").Width, Rec.Top));
                imgUp.ClickImage += new ClickableImage.ClickImageHandler(imgUp_ClickImage);
                ListUIChildren.Add(imgUp);

                ClickableImage imgDown = new ClickableImage(UI, GetNewTimeSpan(), "ArrowDown", GetIcon("ArrowDown"), GetIcon("ArrowDown"), new Vector2(Rec.Right - GetIcon("ArrowUp").Width, Rec.Bottom - GetIcon("ArrowUp").Height));
                imgDown.ClickImage += new ClickableImage.ClickImageHandler(imgDown_ClickImage);
                ListUIChildren.Add(imgDown);

                foreach (UIComponent ui in ListUIChildren)
                {
                    if (ui is ClickableText)
                    {
                        ui.Rec = new Rectangle(Rec.X + MARGE, ui.Rec.Y, Rec.Width - 2 * MARGE - imgUp.Width, ui.Rec.Height);
                    }
                }
            }
            //---

            Sort();
        }

        void imgUp_ClickImage(ClickableImage image, Microsoft.Xna.Framework.Input.MouseState mouseState, GameTime gameTime)
        {
            ScrollValue += (int)sizeText.Y;

            if (ScrollValue > 0)
                ScrollValue = 0;

            UpdateScrollValue();
        }

        void imgDown_ClickImage(ClickableImage image, Microsoft.Xna.Framework.Input.MouseState mouseState, GameTime gameTime)
        {
            ScrollValue -= (int)sizeText.Y;

            int scrollMinValue = (int)Math.Min(0, -(CountItem + 1 - countMaxItem) * sizeText.Y);

            if (ScrollValue < scrollMinValue)
                ScrollValue = scrollMinValue;

            UpdateScrollValue();
        }

        private void Sort()
        {
            ListUIChildren.Sort((x, y) => x.CreationTime.CompareTo(y.CreationTime));
        }

        public void UpdateScrollValue()
        {
            int count = 0;

            foreach (UIComponent ui in ListUIChildren)
            {
                if (ui is ClickableText)
                {
                    ClickableText txt = ui as ClickableText;

                    txt.Position = new Vector2(Rec.X + MARGE * 2, Rec.Top + count * sizeText.Y + ScrollValue + MARGE);
                    txt.Rec = new Rectangle(Rec.X + MARGE, (int)(Rec.Top + count * sizeText.Y + ScrollValue + MARGE), txt.Rec.Width, txt.Rec.Height);

                    if (txt.Position.Y < Rec.Top || txt.Position.Y + sizeText.Y > Rec.Bottom)
                        txt.Visible = false;
                    else
                        txt.Visible = true;

                    count++;
                }
            }
        }
    }
}
