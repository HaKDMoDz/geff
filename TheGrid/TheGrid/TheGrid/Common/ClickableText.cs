using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using TheGrid.Logic.UI;
using TheGrid.Model.UI;

namespace TheGrid.Common
{
    public class ClickableText : UIComponent
    {
        #region Propriétés
        private SpriteFont _spriteFontMouseIn;
        private SpriteFont _spriteFontMouseOut;
        private Color _colorIn;
        private Color _colorOut;
        public String Text { get; set; }
        private Vector2 _position;
        private bool isIn = false;
        private ButtonState leftMouseButtonState = ButtonState.Released;
        public bool IsChecked = false;

        public Vector2 Position
        {
            get { return _position; }
            set { _position = value; }
        }

        public int Width
        {
            get { return (int)_spriteFontMouseOut.MeasureString(Text).X; }
        }

        public int Height
        {
            get { return (int)_spriteFontMouseOut.MeasureString(Text).Y; }
        }
        #endregion

        #region Evènements
        public delegate void ClickTextHandler(ClickableText clickableText, MouseState mouseState, GameTime gameTime);
        public event ClickTextHandler ClickText;
        #endregion

        public ClickableText(UILogic uiLogic, TimeSpan creationTime, string spriteFontMouseIn, string spriteFontMouseOut, string text, Vector2 position)
            : base(uiLogic, creationTime)
        {
            this._spriteFontMouseIn = uiLogic.GameEngine.Content.Load<SpriteFont>(@"Font\" + spriteFontMouseIn);
            this._spriteFontMouseOut = uiLogic.GameEngine.Content.Load<SpriteFont>(@"Font\" + spriteFontMouseOut);
            this.Text = text;
            this._position = position;
            this.Rec = new Rectangle((int)this.Position.X, (int)this.Position.Y, (int)this._spriteFontMouseOut.MeasureString(text).X, (int)this._spriteFontMouseOut.MeasureString(text).Y);

            this.Alive = true;
            this.Visible = true;
        }

        public ClickableText(UILogic uiLogic, TimeSpan creationTime, string spriteFont, string text, Vector2 position, Color colorOut, Color colorIn)
            : base(uiLogic, creationTime)
        {
            this._spriteFontMouseOut = uiLogic.GameEngine.Content.Load<SpriteFont>(@"Font\" + spriteFont);
            this.Text = text;
            this._position = position;
            this.Rec = new Rectangle((int)this.Position.X, (int)this.Position.Y, (int)this._spriteFontMouseOut.MeasureString(text).X, (int)this._spriteFontMouseOut.MeasureString(text).Y);

            this.Alive = true;
            this.Visible = true;
            this._colorIn = colorIn;
            this._colorOut = colorOut;
        }

        public override void Update(GameTime gameTime)
        {
            MouseHandled = false;

            if (this.Position.X <= Controller.mouseState.X && this.Position.X + this.Width >= Controller.mouseState.X &&
                this.Position.Y <= Controller.mouseState.Y && this.Position.Y + this.Height >= Controller.mouseState.Y && Visible)
            {
                isIn = true;

                if (Controller.mouseState.LeftButton == ButtonState.Pressed)
                {
                    MouseHandled = true;
                    leftMouseButtonState = ButtonState.Pressed;
                }
                else if (Controller.mouseState.LeftButton == ButtonState.Released && leftMouseButtonState == ButtonState.Pressed && ClickText != null)
                {
                    leftMouseButtonState = ButtonState.Released;
                    MouseHandled = true;
                    ClickText(this, Controller.mouseState, gameTime);
                }
            }
            else
            {
                isIn = false;
            }
        }

        public void Draw(GameTime gameTime, Color color)
        {
            if (isIn || IsChecked)
            {
                if (_spriteFontMouseIn != null)
                    Render.SpriteBatch.DrawString(_spriteFontMouseIn, Text, this.Position, color);
                else
                    Render.SpriteBatch.DrawString(_spriteFontMouseOut, Text, this.Position, _colorIn);
            }
            else
            {
                if (_spriteFontMouseIn != null)
                    Render.SpriteBatch.DrawString(_spriteFontMouseOut, Text, this.Position, color);
                else
                    Render.SpriteBatch.DrawString(_spriteFontMouseOut, Text, this.Position, _colorOut);
            }
        }

        public override void Draw(GameTime gameTime)
        {
            Draw(gameTime, Color.White);
        }
    }
}
