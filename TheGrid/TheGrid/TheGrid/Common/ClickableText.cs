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
        private String _text;
        private Vector2 _position;
        private bool isIn = false;
        private ButtonState leftMouseButtonState = ButtonState.Released;
        public bool IsOn = false;

        public Vector2 Position
        {
            get { return _position; }
            set { _position = value; }
        }

        public int Width
        {
            get { return (int)_spriteFontMouseIn.MeasureString(_text).X; }
        }

        public int Height
        {
            get { return (int)_spriteFontMouseIn.MeasureString(_text).Y; }
        }
        #endregion

        #region Evènements
        public delegate void ClickTextHandler(ClickableText clickableText, MouseState mouseState, GameTime gameTime);
        public event ClickTextHandler ClickText;
        #endregion

        public ClickableText(UILogic uiLogic, string spriteFontMouseIn, string spriteFontMouseOut, string text, Vector2 position)
            : base(uiLogic)
        {
            this._spriteFontMouseIn = uiLogic.GameEngine.Content.Load<SpriteFont>(@"Content\Font\" + spriteFontMouseIn);
            this._spriteFontMouseOut = uiLogic.GameEngine.Content.Load<SpriteFont>(@"Content\Font\" + spriteFontMouseOut);
            this._text = text;
            this._position = position;
        }

        public override void Update(GameTime gameTime)
        {
            if (this.Position.X <= Controller.mouseState.X && this.Position.X + this.Width >= Controller.mouseState.X &&
                this.Position.Y <= Controller.mouseState.Y && this.Position.Y + this.Height >= Controller.mouseState.Y)
            {
                isIn = true;

                if (Controller.mouseState.LeftButton == ButtonState.Pressed)
                {
                    leftMouseButtonState = ButtonState.Pressed;
                }
                else if (Controller.mouseState.LeftButton == ButtonState.Released && leftMouseButtonState == ButtonState.Pressed && ClickText != null)
                {
                    leftMouseButtonState = ButtonState.Released;
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
            if (IsOn)
            {
                if (isIn)
                    Render.SpriteBatch.DrawString(_spriteFontMouseIn, _text, this.Position, color);
                else
                    Render.SpriteBatch.DrawString(_spriteFontMouseOut, _text, this.Position, color);
            }
            else
            {
                if (isIn)
                    Render.SpriteBatch.DrawString(_spriteFontMouseIn, _text, this.Position, color);
                else
                    Render.SpriteBatch.DrawString(_spriteFontMouseOut, _text, this.Position, color);
            }
        }

        public override void Draw(GameTime gameTime)
        {
            Draw(gameTime, Color.White);
        }
    }
}
