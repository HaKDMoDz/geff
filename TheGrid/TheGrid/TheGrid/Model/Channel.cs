using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace TheGrid.Model
{
    public class Channel
    {
        public Color Color { get; set; }
        public string Name { get; set; }
        public Cell CellStart { get; set; }
        public List<Musician> ListMusician { get; set; }
        public List<Sample> ListSample { get; set; }
        public List<TimeValue<float>> ListSpeed { get; set; }

        public TimeSpan ElapsedTime { get; set; }

        public int CountMusicianPlaying
        {
            get
            {
                return this.ListMusician.Count(m => m.IsPlaying);
            }
        }

        public Channel(string name, Color color)
        {
            Name = name;
            Color = color;

            ListSample = new List<Sample>();
            ListMusician = new List<Musician>();

            ListSpeed = new List<TimeValue<float>>();
        }

        public float GetSpeedFromTime(TimeSpan elapsedTime)
        {
            TimeValue<float> timeSpeed = ListSpeed.FindLast(s => s.Time <= elapsedTime);

            return timeSpeed.Value;
        }


        public Musician GetMusicianNotPlaying()
        {
            Musician musician = this.ListMusician.Find(m => !m.IsPlaying);

            if (musician == null && this.ListMusician.Count < 6)
            {
                musician = new Musician(this);
                ListMusician.Add(musician);
            }

            if (musician != null)
            {
                musician.IsPlaying = true;
                musician.NextCell = null;
            }

            return musician;
        }
    }
}
