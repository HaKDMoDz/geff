using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TheGrid.Logic.UI;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace TheGrid.Model.UI.Note
{
    public class Key : UIComponent
    {
        private Keyboard _keyboard;
        private float _frequency;
        private string _name;
        private string _noteName;
        private int _octave;
        private int _index;
        private Texture2D _texKey;
        public bool White = true;
        private int[] typeKey = new int[] { 1, 0, 5, 0, 3, 0, 4, 1, 0, 5, 0, 4 };
        private float[] deltaBlack = new float[] { 0, 0.25f, 0, 0.5f, 0, 0.75f, 0, 0, 0.25f, 0, 0.75f };

        public Key(Keyboard keyboard, UILogic uiLogic, TimeSpan creationTime, string noteName, int octave, int index, int countPreviousWhite, float frequency)
            : base(uiLogic, creationTime)
        {
            Alive = true;
            Visible = true;

            _keyboard = keyboard;
            _noteName = noteName;
            _name = noteName + octave.ToString();
            _octave = octave;
            _index = index;
            _frequency = frequency;
            if (noteName.EndsWith("#"))
                White = false;

            int indexImage = (index + 4) % 12;
            _texKey = GetKeyImage(White, typeKey[indexImage]);
            int whiteWidth = GetKeyImage(true, 1).Width * keyboard.Rec.Height / GetKeyImage(true, 1).Height;

            int height = (int)((float)keyboard.Rec.Height * (White ? 1f : 0.65f));
            int width = _texKey.Width * height / _texKey.Height;
            int deltaLeft = 0;

            if(!White)
            {
                deltaLeft = -(int)((float)width * deltaBlack[indexImage]);
            }

            Rec = new Rectangle(countPreviousWhite * whiteWidth + deltaLeft, keyboard.Rec.Top, width, height);
        }

        private Texture2D GetKeyImage(bool white, int index)
        {
            return GetImage(@"\Keyboard\" + (white ? "White_" : "Black_") + "Key_" + index);
        }

        public override void Draw(GameTime gameTime)
        {
            Rectangle rec = new Rectangle(Rec.X + _keyboard.Delta, Rec.Y, Rec.Width, Rec.Height);

            Render.SpriteBatch.Draw(_texKey, Rec, null, Color.White, 0f,Vector2.Zero, SpriteEffects.None, White? 1f:0f);
        }
    }
}
