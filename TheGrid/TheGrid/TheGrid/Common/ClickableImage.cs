using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using TheGrid.Model.UI;
using TheGrid.Logic.UI;

namespace TheGrid.Common
{
    public class ClickableImage : UIComponent
    {
        #region Propriétés
        private Texture2D _textureMouseIn;
        private Texture2D _textureMouseOut;
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
            get { return _textureMouseIn.Width; }
        }

        public int Height
        {
            get { return _textureMouseIn.Height; }
        }
        #endregion

        #region Evènements
        public delegate void ClickImageHandler(ClickableImage image, MouseState mouseState, GameTime gameTime);
        public event ClickImageHandler ClickImage;
        #endregion

        public ClickableImage(UILogic uiLogic, Texture2D textureMouseIn, Texture2D textureMouseOut, Vector2 position)
            : base(uiLogic)
        {
            this._textureMouseIn = textureMouseIn;
            this._textureMouseOut = textureMouseOut;
            this.Position = position;
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
                else if (Controller.mouseState.LeftButton == ButtonState.Released && leftMouseButtonState == ButtonState.Pressed && ClickImage != null)
                {
                    leftMouseButtonState = ButtonState.Released;
                    ClickImage(this, Controller.mouseState, gameTime);
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
                    Render.SpriteBatch.Draw(this._textureMouseOut, this.Position, null, color);
                else
                    Render.SpriteBatch.Draw(this._textureMouseIn, this.Position, null, color);
            }
            else
            {
                if (isIn)
                    Render.SpriteBatch.Draw(this._textureMouseIn, this.Position, null, color);
                else
                    Render.SpriteBatch.Draw(this._textureMouseOut, this.Position, null, color);
            }
        }

        public override void Draw(GameTime gameTime)
        {
            Draw(gameTime, Color.White);
        }
    }
}
