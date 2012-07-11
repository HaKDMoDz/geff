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

        public int LocationKey { get; set; }
        public int BaseKey { get; set; }
        public String NameKey { get; set; }
        public int NumMeasure { get; set; }

        public Measure(float measureStart, float measureLength)
        {
            this.MeasureStart = measureStart;
            this.MeasureLength = measureLength;
            this.ListNote = new List<Note>();
        }

        public void AddNote(Note newNote)
        {
            this.ListNote.Add(newNote);

            //if (newNote.NoteNumber < 60)
            //{
            //    LocationKey = 3;
            //    BaseKey = 43;
            //    NameKey = "F";
            //}

            int min = int.MaxValue;
            int max = int.MinValue;
            foreach (Note note in ListNote)
            {
                if (note.NoteNumber < min)
                    min = note.NoteNumber;
                if (note.NoteNumber > max)
                    max = note.NoteNumber;
            }

            if (min == 57 && ListNote.Count == 4)
            {
                int a = 0;
            }

            int[,] tabKey = new int[2, 2];
            tabKey[0, 0] = 43;
            tabKey[0, 1] = 54;
            tabKey[1, 0] = 64;
            tabKey[1, 1] = 75;

            int[] tabResult = new int[2];


            for (int j = min; j <= max; j++)
            {
                for (int i = 0; i < 2; i++)
                {
                    if (j >= tabKey[i, 0] && j <= tabKey[i, 1])
                        tabResult[i]++;
                }
            }

            if (tabResult[0] > tabResult[1])
            {
                LocationKey = 3;
                BaseKey = 43;
                NameKey = "F";
            }
            else
            {
                LocationKey = 1;
                BaseKey = 64;
                NameKey = "G";
            }

        }

    }
}
