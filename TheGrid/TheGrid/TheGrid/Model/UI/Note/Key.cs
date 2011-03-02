using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TheGrid.Logic.UI;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using TheGrid.Logic.Controller;
using TheGrid.Common;

namespace TheGrid.Model.UI.Note
{
    public class Key : UIComponent
    {
        private Keyboard _keyboard;
        private float _frequency;
        private string _name;
        private string _noteName;
        private int _octave;
        public int NoteKey;
        private Texture2D _texKey;
        public bool White = true;
        private int[] typeKey = new int[] { 1, 0, 2, 0, 3, 0, 4, 1, 0, 6, 0, 4 };
        private float[] deltaBlack = new float[] { 0, 0.75f, 0, 0.5f, 0, 0.25f, 0, 0, 0.75f, 0, 0.25f };
        public int Width = 0;
        private bool isIn = false;

        #region Evènements
        public delegate void ClickKeyHandler(Key key, MouseState mouseState, GameTime gameTime);
        public event ClickKeyHandler ClickKey;

        public delegate void MouseEnterHandler(Key key, MouseState mouseState, GameTime gameTime);
        public event MouseEnterHandler MouseEnter;

        public delegate void MouseLeaveHandler(Key key, MouseState mouseState, GameTime gameTime);
        public event MouseLeaveHandler MouseLeave;
        #endregion

        public Key(Keyboard keyboard, UILogic uiLogic, TimeSpan creationTime, string noteName, int octave, int index, int countPreviousWhite, float frequency)
            : base(uiLogic, creationTime)
        {
            Alive = true;
            Visible = false;

            _keyboard = keyboard;
            _noteName = noteName;
            _name = noteName + octave.ToString();
            _octave = octave;
            NoteKey = index;
            _frequency = frequency;
            if (noteName.EndsWith("#"))
                White = false;

            int indexImage = (index + 4) % 12;
            _texKey = GetKeyImage(White, typeKey[indexImage]);
            int whiteWidth = GetKeyImage(true, 1).Width * keyboard.Rec.Height / GetKeyImage(true, 1).Height;

            int height = (int)((float)keyboard.Rec.Height * (White ? 1f : 0.64f));
            Width = _texKey.Width * height / _texKey.Height;
            int deltaLeft = 0;

            if (!White)
            {
                deltaLeft = -(int)((float)Width * (deltaBlack[indexImage]));
            }

            Rec = new Rectangle(keyboard.Rec.Left + countPreviousWhite * whiteWidth + deltaLeft, keyboard.Rec.Top, Width, height);

            //---
            MouseManager mouseLeftButton = AddMouse(MouseButtons.LeftButton);
            mouseLeftButton.MouseReleased += new MouseManager.MouseReleasedHandler(MouseLeftButton_MouseReleased);
            //---
        }

        void MouseLeftButton_MouseReleased(MouseButtons mouseButton, MouseState mouseState, GameTime gameTime, Point distance)
        {
            if (isIn)
            {
                MouseHandled = true;

                if (ClickKey != null)
                    ClickKey(this, mouseState, gameTime);
            }
        }

        private Texture2D GetKeyImage(bool white, int index)
        {
            return GetImage(@"\Keyboard\" + (white ? "White_" : "Black_") + "Key_" + index);
        }

        public override void Update(GameTime gameTime)
        {
            MouseHandled = false;
            Rectangle rec = new Rectangle(Rec.X + _keyboard.Delta, Rec.Y, Rec.Width, Rec.Height);

            if (Visible && IsMouseOverKey())
            {
                if (!isIn && MouseEnter != null)
                    MouseEnter(this, Controller.mouseState, gameTime);

                isIn = true;
            }
            else
            {

                if (Visible && isIn && MouseLeave != null)
                    MouseLeave(this, Controller.mouseState, gameTime);

                isIn = false;
            }

            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            Rectangle rec = new Rectangle(Rec.X + _keyboard.Delta, Rec.Y, Rec.Width, Rec.Height);

            if (_keyboard.Rec.Intersects(rec))
            {
                Color colorKey = Color.White;

                if (isIn)
                    colorKey = VisualStyle.BackForeColorMouseOver;

                Rectangle recSource = _texKey.Bounds;

                if (rec.Left < _keyboard.Rec.Left)
                {
                    int newWidth = rec.Width - (_keyboard.Rec.Left - rec.Left);
                    int newSourceX = (int)((float)_texKey.Bounds.Width * (1f-(float)newWidth / (float)rec.Width));
                    recSource = new Rectangle(newSourceX, _texKey.Bounds.Y, _texKey.Bounds.Width - newSourceX, _texKey.Bounds.Height);

                    rec = new Rectangle(_keyboard.Rec.Left, rec.Y, newWidth, rec.Height);
                }

                Render.SpriteBatch.Draw(_texKey, rec, recSource, colorKey);

                if(recSource == _texKey.Bounds)
                    Render.SpriteBatch.DrawString(Render.FontTextSmall, _name, new Vector2(rec.X + rec.Width / 2 - Render.FontTextSmall.MeasureString(_name).X / 2, Rec.Bottom - 60), White ? Color.Black : Color.White);
            }
        }

        private bool IsMouseOverKey()
        {
            Rectangle rec = new Rectangle(Rec.X + _keyboard.Delta, Rec.Y, Rec.Width, Rec.Height);
            if (rec.Contains(Controller.mousePositionPoint))
            {
                uint[] pixelData = new uint[1];

                //Vector3 mousePosition = new Vector3((float)UI.GameEngine.Controller.mouseState.X - (float)UI.GameEngine.GraphicsDevice.Viewport.Width / 2f, (float)UI.GameEngine.Controller.mouseState.Y - (float)GameEngine.GraphicsDevice.Viewport.Height / 2f, 0f);
                //Vector3 mousePosition = new Vector3((float)UI.GameEngine.Controller.mouseState.X - (float)UI.GameEngine.GraphicsDevice.Viewport.Width / 2f, (float)UI.GameEngine.Controller.mouseState.Y - (float)GameEngine.GraphicsDevice.Viewport.Height / 2f, 0f);
                //Matrix mtx = Matrix.CreateScale(-GameEngine.Render.CameraPosition.Z) * Matrix.CreateTranslation(UI.GameEngine.Render.CameraPosition.X, UI.GameEngine.Render.CameraPosition.Y, UI.GameEngine.Render.CameraPosition.Z);
                //mousePosition = Vector3.Transform(mousePosition, mtx);

                Point mousePosition = new Point(Controller.mousePositionPoint.X - _keyboard.Delta-Rec.Left, Controller.mousePositionPoint.Y-Rec.Top);

                if (_texKey.Bounds.Contains(mousePosition))
                {
                    _texKey.GetData<uint>(0, new Rectangle(mousePosition.X, mousePosition.Y, 1, 1), pixelData, 0, 1);

                    if (((pixelData[0] & 0xFF000000) >> 24) > 20)
                        return true;
                }
            }

            return false;
        }
    }
}
