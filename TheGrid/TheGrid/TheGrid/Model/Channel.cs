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
        public List<Musician> ListMusician { get; set; }
        public List<Sample> ListSample { get; set; }
        public string Name { get; set; }
        public Cell CellStart { get; set; }

        public float Speed { get; set; }
        public List<float> ListSpeed { get; set; }
        public List<TimeSpan> ListSpeedTime { get; set; }

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
            Speed = 1f;

            ListSample = new List<Sample>();
            ListMusician = new List<Musician>();
            ListSpeed = new List<float>();
            ListSpeedTime = new List<TimeSpan>();
        }

        public float GetSpeedFromTime(TimeSpan elapsedTime)
        {
            int index = ListSpeedTime.FindLastIndex(t => t < elapsedTime);

            if (index != -1)
                return ListSpeed[index];
            else
                return 1f;
        }


        public Musician GetMusicianNotPlaying()
        {
            Musician musician = this.ListMusician.Find(m => !m.IsPlaying);

            if (musician == null && this.ListMusician.Count < 6)
            {
                musician = new Musician(this);
                ListMusician.Add(musician);
            }
            
            if(musician!=null)
            {
                musician.IsPlaying = true;
                musician.NextCell = null;
            }

            return musician;
        }
    }
}
