using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using System.Xml.Serialization;
using TheGrid.Model.Effect;
using TheGrid.Logic.Sound;
using TheGrid.Common;
using JSNet;

namespace TheGrid.Model
{
    [Serializable]
    public class Channel
    {
        [XmlIgnore]
        public Color Color { get; set; }
        public string Name { get; set; }
        [XmlIgnore]
        public Cell CellStart { get; set; }
        [XmlIgnore]
        public List<Musician> ListMusician { get; set; }
        [XmlIgnore]
        public List<Sample> ListSample { get; set; }
        [XmlIgnore]
        public List<TimeValue<float>> ListSpeed { get; set; }
        //public TimeSpan ElapsedTime { get; set; }
        public List<ChannelEffect> ListEffect { get; set; }

        [XmlIgnore]
        public TypePlaying TypePlaying { get; set; }

        [XmlIgnore]
        public int CountMusicianPlaying
        {
            get
            {
                return this.ListMusician.Count(m => m.IsPlaying);
            }
        }

        public Channel()
        {
            ListSample = new List<Sample>();
            ListMusician = new List<Musician>();

            ListEffect = new List<ChannelEffect>();
            ListSpeed = new List<TimeValue<float>>();
        }

        public Channel(string name, Color color)
            : this()
        {
            Name = name;
            Color = color;

            InitChannelEffect();
        }

        public void InitChannelEffect()
        {
            ListEffect = new List<ChannelEffect>();

            ListEffect.Add(new ChannelEffect(this, "Volume"));
            ListEffect.Add(new ChannelEffect(this, "Chorus"));
            ListEffect.Add(new ChannelEffect(this, "Delay"));
            ListEffect.Add(new ChannelEffect(this, "Flanger"));
            ListEffect.Add(new ChannelEffect(this, "Tremolo"));
            ListEffect.Add(new ChannelEffect(this, "SuperPitch"));
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

        public void Update(SoundLogic soundLogic, GameTime gameTime)
        {
            foreach (ChannelEffect channelEffect in ListEffect)
            {
                channelEffect.Update(soundLogic, gameTime);
            }
        }
    }
}
