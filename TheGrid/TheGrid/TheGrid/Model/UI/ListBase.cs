using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using TheGrid.Logic.UI;
using TheGrid.Common;

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

        public delegate void SelectedItemChangedHandler(Object item);
        public event SelectedItemChangedHandler SelectedItemChanged;

        public ListBase(UILogic uiLogic, TimeSpan creationTime, Rectangle rec, SpriteFont font, bool checkable)
            : base(uiLogic, creationTime)
        {
            this.Font = font;
            this.IsCheckable = checkable;

            sizeText = font.MeasureString(new String(' ', 40)) + new Vector2(Ribbon.MARGE * 2, Ribbon.MARGE);

            if (sizeText.X > rec.Width)
                sizeText.X = rec.Width;

            countMaxItem = (rec.Height - 2 * MARGE) / (int)sizeText.Y;

            Rec = new Rectangle(
                rec.X,
                rec.Y,
                Math.Min((int)sizeText.X, rec.Width),
                (int)(countMaxItem * sizeText.Y));
        }

        public void OnSelectedItemChanged(Object item)
        {
            if (SelectedItemChanged != null)
                SelectedItemChanged(item);
        }

        public void UpdateScrollbar()
        {
            //--- Charge les boutons
            //TODO améliorer la scroll
            if (countMaxItem > CountItem)
            {
                ClickableImage imgUp = new ClickableImage(UI, GetNewTimeSpan(), "ArrowUp", GetIcon("ArrowUp"), GetIcon("ArrowUp"), new Vector2(Rec.Right - GetIcon("ArrowUp").Width, Rec.Top));
                imgUp.ClickImage += new ClickableImage.ClickImageHandler(imgUp_ClickImage);
                ListUIChildren.Add(imgUp);

                ClickableImage imgDown = new ClickableImage(UI, GetNewTimeSpan(), "ArrowDown", GetIcon("ArrowDown"), GetIcon("ArrowDown"), new Vector2(Rec.Right - GetIcon("ArrowUp").Width, Rec.Bottom - GetIcon("ArrowUp").Height));
                imgDown.ClickImage += new ClickableImage.ClickImageHandler(imgDown_ClickImage);
                ListUIChildren.Add(imgDown);
            }
            //---
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

            int scrollMinValue = (int)Math.Min(0, -(CountItem - countMaxItem) * sizeText.Y);

            if (ScrollValue < scrollMinValue)
                ScrollValue = scrollMinValue;

            UpdateScrollValue();
        }

        private void Sort()
        {
            ListUIChildren.Sort((x, y) => x.CreationTime.CompareTo(y.CreationTime));
        }

        private void UpdateScrollValue()
        {
            int count = 0;

            Sort();

            foreach (UIComponent ui in ListUIChildren)
            {
                if (ui is ClickableText)
                {
                    ClickableText txt = ui as ClickableText;

                    txt.Position = new Vector2(txt.Position.X,Rec.Top+ count * sizeText.Y+ScrollValue);
                    txt.Rec = new Rectangle(txt.Rec.X, (int)( Rec.Top + count * sizeText.Y + ScrollValue), txt.Rec.Width, txt.Rec.Height);

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
