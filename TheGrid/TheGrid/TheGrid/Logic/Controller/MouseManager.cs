using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;

namespace TheGrid.Logic.Controller
{
    class MouseManager
    {
        private ButtonState mouseButtonState;
        private MouseButtons mouseButton;
        private Point mousePosition;
        private int mouseWheelValue;

        public delegate void MouseFirstPressedHandler(MouseButtons mouseButton, MouseState mouseState, GameTime gameTime);
        public delegate void MousePressedHandler(MouseButtons mouseButton, MouseState mouseState, GameTime gameTime, Point distance);
        public delegate void MouseReleasedHandler(MouseButtons mouseButton, MouseState mouseState, GameTime gameTime, Point distance);
        public delegate void MouseWheelChangedHandler(MouseState mouseState, GameTime gameTime, int prevMousewheel);

        public event MousePressedHandler MousePressed;
        public event MouseFirstPressedHandler MouseFirstPressed;
        public event MouseReleasedHandler MouseReleased;
        public event MouseWheelChangedHandler MouseWheelChanged;

        public MouseManager(MouseButtons mouseButton)
        {
            this.mouseButton = mouseButton;
        }

        public void Update(MouseState mouseState, GameTime gameTime)
        {
            bool pressed = false;
            int newMouseWheelValue = 0;

            switch (mouseButton)
            {
                case MouseButtons.LeftButton:
                    pressed = mouseState.LeftButton == ButtonState.Pressed;
                    break;
                case MouseButtons.RightButton:
                    pressed = mouseState.RightButton == ButtonState.Pressed;
                    break;
                case MouseButtons.MiddleButton:
                    pressed = mouseState.MiddleButton == ButtonState.Pressed;
                    break;
                case MouseButtons.Wheel:
                    newMouseWheelValue = mouseState.ScrollWheelValue;
                    break;
                default:
                    break;
            }

            if (newMouseWheelValue != mouseWheelValue)
            {
                if (MouseWheelChanged != null)
                    MouseWheelChanged(mouseState, gameTime, mouseWheelValue);

                mouseWheelValue = newMouseWheelValue;
            }

            if (mouseButtonState == ButtonState.Released && pressed)
            {
                mousePosition = new Point(mouseState.X, mouseState.Y);

                if (MouseFirstPressed != null)
                    MouseFirstPressed(mouseButton, mouseState, gameTime);
            }
            else if (mouseButtonState == ButtonState.Pressed && pressed)
            {
                if (MousePressed != null)
                    MousePressed(mouseButton, mouseState, gameTime, new Point(mousePosition.X - mouseState.X, mousePosition.Y - mouseState.Y));
            }
            else if (mouseButtonState == ButtonState.Pressed && !pressed)
            {
                if (MouseReleased != null)
                    MouseReleased(mouseButton, mouseState, gameTime, new Point(mousePosition.X - mouseState.X, mousePosition.Y - mouseState.Y));
            }

            mouseButtonState = pressed ? ButtonState.Pressed : ButtonState.Released;
        }
    }

    public enum MouseButtons
    {
        LeftButton,
        RightButton,
        MiddleButton,
        Wheel
    }
}
