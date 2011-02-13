using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;

namespace TheGrid.Logic.Controller
{
    public class KeyManager
    {
        private KeyState keyState;
        private Keys key;

        public delegate void KeyFirstPressedHandler(Keys key, GameTime gameTime);
        public delegate void KeyPressedHandler(Keys key, GameTime gameTime);
        public delegate void KeyReleasedHandler(Keys key, GameTime gameTime);

        public event KeyFirstPressedHandler KeyFirstPressed;
        public event KeyPressedHandler KeyPressed;
        public event KeyReleasedHandler KeyReleased;

        public KeyManager(Keys key)
        {
            this.key = key;
            this.keyState = KeyState.Up;
        }

        public void Reset()
        {
            keyState = KeyState.Up;
        }

        public void Update(KeyboardState keyBoardState, GameTime gameTime)
        {
            bool pressed = keyBoardState.IsKeyDown(key);

            if (keyState == KeyState.Up && pressed)
            {
                if(KeyFirstPressed != null)
                    KeyFirstPressed(key, gameTime);
            }
            else if (keyState == KeyState.Down && pressed)
            {
                if (KeyPressed != null)
                    KeyPressed(key, gameTime);
            }
            else if (keyState == KeyState.Down && !pressed)
            {
                if (KeyReleased != null)
                    KeyReleased(key, gameTime);
            }

            keyState = pressed ? KeyState.Down : KeyState.Up;
        }
    }
}
