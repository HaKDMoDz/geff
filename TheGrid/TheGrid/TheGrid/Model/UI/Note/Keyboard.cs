using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TheGrid.Logic.UI;
using Microsoft.Xna.Framework;
using TheGrid.Logic.Controller;
using Microsoft.Xna.Framework.Input;
using TheGrid.Common;
using TheGrid.Logic.Sound;
using TheGrid.Model.Instrument;
using NAudio.Midi;

namespace TheGrid.Model.UI.Note
{
    public class Keyboard : UIComponent
    {
        private NotePanel _notePannel;
        private List<Key> _listKey;
        private string[] _noteTable = new string[12];
        public int Delta = 0;
        private int prevDelta = 0;
        int countWhite = 0;

        public Keyboard(NotePanel notePannel, UILogic uiLogic, TimeSpan creationTime)
            : base(uiLogic, notePannel, creationTime)
        {
            Alive = true;
            Visible = true;

            _notePannel = notePannel;

            Rec = new Rectangle((int)notePannel.leftPartWidth, notePannel.Rec.Top, (int)((float)notePannel.Rec.Width - notePannel.leftPartWidth), (int)notePannel.Rec.Height);

            CreateNoteTable();
            CreateKeys();

            MouseManager mouseRightButton = AddMouse(MouseButtons.RightButton);
            mouseRightButton.MouseFirstPressed += new MouseManager.MouseFirstPressedHandler(mouseRightButton_MouseFirstPressed);
            mouseRightButton.MousePressed += new MouseManager.MousePressedHandler(mouseRightButton_MousePressed);
            mouseRightButton.MouseReleased += new MouseManager.MouseReleasedHandler(mouseRightButton_MouseReleased);

            UI.GameEngine.Sound.MidiNoteEvent += new SoundLogic.MidiNoteEventHandler(Sound_MidiNoteEvent);
        }

        void Sound_MidiNoteEvent(int noteKey, string noteName)
        {
            PerformNote(noteKey, noteName);
        }

        private void CreateNoteTable()
        {
            _noteTable = new string[12] { "A", "A#", "B", "C", "C#", "D", "D#", "E", "F", "F#", "G", "G#" };
        }

        private void CreateKeys()
        {
            _listKey = new List<Key>();
            ListUIChildren = new List<UIComponent>();
            countWhite = 0;

            for (int octave = 0; octave < 8; octave++)
            {
                for (int note = 0; note < 12; note++)
                {
                    if (octave * 12 + note < 88)
                    {
                        int realOctave = GetRealOctave(octave, note);
                        Key key = new Key(this, UI, GetNewTimeSpan(), _noteTable[note], realOctave, octave * 12 + note, countWhite, GetFrequency(octave, note + 1));
                        key.ClickKey += new Key.ClickKeyHandler(key_ClickKey);

                        _listKey.Add(key);
                        ListUIChildren.Add(key);

                        if (key.White)
                            countWhite++;
                    }
                }
            }

            ListUIChildren.Sort((x, y) => (((Key)x).White ? 0 : 1) - (((Key)y).White ? 0 : 1));

            foreach (UIComponent component in ListUIChildren)
            {
                component.CreationTime = GetNewTimeSpan();
            }
        }

        void key_ClickKey(Key key, MouseState mouseState, GameTime gameTime)
        {
            PerformNote(key);
        }

        public void PerformNote(Key key)
        {
            PerformNote(key.NoteKey, key.NoteName);
        }

        public void PerformNote(int noteKey, string noteName)
        {
            PlayNote(noteKey);

            if (!_notePannel.Training)
                _notePannel.AddNote(noteKey, noteName);
        }

        public void PlayNote(int noteKey)
        {
            if (_notePannel.Sample != null)
                UI.GameEngine.Sound.PlayNote(_notePannel.Sample, noteKey);
        }

        private int GetRealOctave(int octave, int note)
        {
            return octave + (int)((note + 9) / 12);
        }

        private float GetFrequency(int octave, int note)
        {
            double frequency = 0;
            double position = (octave * 12 + note);

            frequency = 440D * Math.Pow((Math.Pow(2D, (1D / 12D))), (position - 49D));

            return (float)frequency;
        }

        void mouseRightButton_MouseReleased(MouseButtons mouseButton, MouseState mouseState, GameTime gameTime, Point distance)
        {
            if (Rec.Contains(Controller.mousePositionPoint))
            {
                MouseHandled = true;

                if (prevDelta > int.MinValue)
                {
                    prevDelta = int.MinValue;
                }
            }
        }

        void mouseRightButton_MouseFirstPressed(MouseButtons mouseButton, MouseState mouseState, GameTime gameTime)
        {
            if (Rec.Contains(Controller.mousePositionPoint))
            {
                MouseHandled = true;

                if (Rec.Contains(UI.GameEngine.Controller.mousePositionPoint))
                {
                    prevDelta = Delta;
                }
            }
        }

        void mouseRightButton_MousePressed(MouseButtons mouseButton, MouseState mouseState, GameTime gameTime, Point distance)
        {
            if (Rec.Contains(Controller.mousePositionPoint))
            {
                MouseHandled = true;

                if (prevDelta > int.MinValue)
                {
                    Delta = prevDelta - distance.X;

                    Delta = (int)MathHelper.Clamp((float)Delta, -_listKey[0].Width * countWhite + Rec.Width, 0f);

                    if (mouseState.X > Rec.Right)
                    {
                        Mouse.SetPosition(Rec.Left, mouseState.Y);

                        prevDelta += Rec.Width;
                    }
                    else if (mouseState.X < Rec.Left)
                    {
                        Mouse.SetPosition(Rec.Right, mouseState.Y);

                        prevDelta -= Rec.Width;
                    }

                    if (mouseState.Y > Rec.Bottom)
                    {
                        Mouse.SetPosition(mouseState.X, Rec.Bottom);
                    }
                    else if (mouseState.Y < Rec.Top)
                    {
                        Mouse.SetPosition(mouseState.X, Rec.Top);
                    }
                }

                //prevDelta = (int)MathHelper.Clamp((float)prevDelta, -_listKey[0].Width * countWhite + Rec.Width, 0f);
            }
        }

        public override void Draw(GameTime gameTime)
        {
            Render.SpriteBatch.Draw(Render.texEmpty, Rec, VisualStyle.BackColorLight);

            base.Draw(gameTime);
        }
    }
}
