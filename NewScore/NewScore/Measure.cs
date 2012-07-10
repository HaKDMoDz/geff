using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NewScore
{
    public class Measure
    {
        public float MeasureStart { get; set; }
        public float MeasureLength { get; set; }
        public List<Note> ListNote { get; set; }

        public int OffsetCle { get; set; }
        public int BaseCle { get; set; }

        public Measure(float measureStart, float measureLength)
        {
            this.MeasureStart = measureStart;
            this.MeasureLength = measureLength;
            this.ListNote = new List<Note>();
            this.BaseCle = 64;
        }

        public void AddNote(Note newNote)
        {
            this.ListNote.Add(newNote);

            if (newNote.NoteNumber < 60)
            {
                OffsetCle = 12;
                BaseCle = 43;
            }
        }
    }
}
