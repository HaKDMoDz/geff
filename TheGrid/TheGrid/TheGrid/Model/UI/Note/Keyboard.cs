using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TheGrid.Logic.UI;
using Microsoft.Xna.Framework;

namespace TheGrid.Model.UI.Note
{
    public class Keyboard : UIComponent
    {
        private NotePanel _notePannel;
        private List<Key> _listKey;
        private string[] _noteTable = new string[12];
        public int Delta = 0;

        public Keyboard(NotePanel notePannel, UILogic uiLogic, TimeSpan creationTime)
            : base(uiLogic, creationTime)
        {
            Alive = true;
            Visible = true;

            _notePannel = notePannel;

            Rec = new Rectangle((int)notePannel.leftPartWidth, notePannel.Rec.Top, (int)((float)notePannel.Rec.Width - notePannel.leftPartWidth), (int)notePannel.Rec.Height);

            CreateNoteTable();
            CreateKeys();
        }

        private void CreateNoteTable()
        {
            _noteTable = new string[12] { "A", "A#", "B", "C", "C#", "D", "D#", "E", "F", "F#", "G", "G#" };
        }

        private void CreateKeys()
        {
            _listKey = new List<Key>();
            ListUIChildren = new List<UIComponent>();
            int countPreviousWhite = 0;

            for (int octave = 0; octave < 7; octave++)
            {
                for (int note = 0; note < 12; note++)
                {
                    int realOctave = GetRealOctave(octave, note);
                    Key key = new Key(this, UI, GetNewTimeSpan(), _noteTable[note], realOctave, octave * 12 + note, countPreviousWhite, GetFrequency(octave, note + 1));
                    _listKey.Add(key);
                    ListUIChildren.Add(key);

                    if (key.White)
                        countPreviousWhite++;
                }
            }
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
    }
}
