using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using TheGrid.Logic.UI;
using TheGrid.Model.UI;
using TheGrid.Logic.Controller;

namespace TheGrid.Common
{
    public class ClickableText : UIComponent
    {
        #region Propriétés
        private SpriteFont _spriteFont;
        private Color _colorIn;
        private Color _colorOut;
        private Color _backColorIn;
        private Color _backColorOut;
        private bool IsCheckable;
        public String Text { get; set; }
        private Vector2 _position;
        private bool isIn = false;
        public bool IsChecked = false;

        public Vector2 Position
        {
            get { return _position; }
            set { _position = value; }
        }
        #endregion

        #region Evènements
        public delegate void ClickTextHandler(ClickableText clickableText, MouseState mouseState, GameTime gameTime);
        public event ClickTextHandler ClickText;

        public delegate void MiddleButtonClickTextHandler(ClickableText clickableText, MouseState mouseState, GameTime gameTime);
        public event MiddleButtonClickTextHandler MiddleButtonClickText;

        public delegate void MouseEnterHandler(ClickableText clickableText, MouseState mouseState, GameTime gameTime);
        public event MouseEnterHandler MouseEnter;
        #endregion


        public ClickableText(UILogic uiLogic, TimeSpan creationTime, string spriteFont, string text, Vector2 position, Color colorOut, Color colorIn, Color backColorOut, Color backColorIn, bool isCheckable)
            : base(uiLogic, creationTime)
        {
            this._spriteFont = uiLogic.GameEngine.Content.Load<SpriteFont>(@"Font\" + spriteFont);
            this.Text = text;
            this._position = position;
            this.Rec = new Rectangle((int)this.Position.X, (int)this.Position.Y, (int)this._spriteFont.MeasureString(text).X, (int)this._spriteFont.MeasureString(text).Y);

            this.Alive = true;
            this.Visible = true;
            this._colorIn = colorIn;
            this._colorOut = colorOut;
            this._backColorIn = backColorIn;
            this._backColorOut = backColorOut;
            this.IsCheckable = isCheckable;

            MouseManager mouseLeftButton = AddMouse(MouseButtons.LeftButton);
            MouseManager mouseMiddleButton = AddMouse(MouseButtons.MiddleButton);

            mouseLeftButton.MouseReleased += new MouseManager.MouseReleasedHandler(MouseLeftButton_MouseReleased);
            mouseMiddleButton.MouseFirstPressed += new MouseManager.MouseFirstPressedHandler(mouseMiddleButton_MouseFirstPressed);
        }

        void mouseMiddleButton_MouseFirstPressed(MouseButtons mouseButton, MouseState mouseState, GameTime gameTime)
        {
            if (isIn)
            {
                MouseHandled = true;

                if (MiddleButtonClickText != null)
                    MiddleButtonClickText(this, mouseState, gameTime);
            }
        }

        void MouseLeftButton_MouseReleased(MouseButtons mouseButton, MouseState mouseState, GameTime gameTime, Point distance)
        {
            if (isIn)
            {
                MouseHandled = true;

                if (IsCheckable)
                    IsChecked = !IsChecked;

                if (ClickText != null)
                    ClickText(this, mouseState, gameTime);
            }
        }

        public override void Update(GameTime gameTime)
        {
            MouseHandled = false;

            if (Visible && this.Rec.Contains(Controller.mousePositionPoint))
            {
                if (!isIn && MouseEnter != null)
                    MouseEnter(this, Controller.mouseState, gameTime);

                isIn = true;
            }
            else
            {
                isIn = false;
            }

            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            if (isIn || IsChecked)
            {
                if (_backColorIn != null)
                    Render.SpriteBatch.Draw(Render.texEmpty, Rec, _backColorIn);

                Render.SpriteBatch.DrawString(_spriteFont, Text, this.Position, _colorIn);
            }
            else
            {
                if (_backColorOut != null)
                    Render.SpriteBatch.Draw(Render.texEmpty, Rec, _backColorOut);

                Render.SpriteBatch.DrawString(_spriteFont, Text, this.Position, _colorOut);
            }
        }
    }
}
