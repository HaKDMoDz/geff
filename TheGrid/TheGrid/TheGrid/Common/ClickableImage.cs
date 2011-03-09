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
        public Color Color;
        public string Name { get; set; }

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

        public ClickableImage(UILogic uiLogic, UIComponent parent, TimeSpan creationTime, string name, Texture2D textureMouseIn, Texture2D textureMouseOut, Vector2 position)
            : base(uiLogic, parent, creationTime)
        {
            this._textureMouseIn = textureMouseIn;
            this._textureMouseOut = textureMouseOut;
            this.Position = position;
            this.Color = Color.White;
            this.Rec = new Rectangle((int)this.Position.X, (int)this.Position.Y, this._textureMouseIn.Width, this._textureMouseIn.Height);

            this.Alive = true;
            this.Visible = true;
            this.Name = name;
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
                else if (Controller.mouseState.LeftButton == ButtonState.Released && leftMouseButtonState == ButtonState.Pressed && ClickImage != null)
                {
                    leftMouseButtonState = ButtonState.Released;
                    MouseHandled = true;
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
            Draw(gameTime, this.Color);
        }
    }
}
