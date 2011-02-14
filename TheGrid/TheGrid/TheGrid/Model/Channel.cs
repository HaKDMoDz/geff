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
        public Color Color { get; set; }
        public string Name { get; set; }
        [XmlIgnore]
        public Cell CellStart { get; set; }
        [XmlIgnore]
        public List<Musician> ListMusician { get; set; }
        [XmlIgnore]
        public List<Sample> ListSample { get; set; }
        public List<TimeValue<float>> ListSpeed { get; set; }
        public TimeSpan ElapsedTime { get; set; }
        public List<ChannelEffect> ListEffect { get; set; }

        [XmlIgnore]
        public TypePlaying TypePlaying { get; set; }

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

            ListEffect.Add(AddEffect("Volume"));
            ListEffect.Add(AddEffect("Chorus"));
            ListEffect.Add(AddEffect("Delay"));
            ListEffect.Add(AddEffect("Flanger"));
            ListEffect.Add(AddEffect("Tremolo"));
            ListEffect.Add(AddEffect("SuperPitch"));
        }

        private ChannelEffect AddEffect(string effectName)
        {
            IList<Slider> sliders = Context.GameEngine.Sound.GetEffectParameters(effectName);

            ChannelEffect channelEffect = new ChannelEffect(this, effectName);

            foreach (Slider slider in sliders)
            {
                EffectProperty effectProperty = new EffectProperty();
                effectProperty.Description = slider.Description;
                effectProperty.Default = slider.Default;
                effectProperty.MinValue = slider.Minimum;
                effectProperty.MaxValue = slider.Maximum;
                effectProperty.Curve.Keys.Add(new CurveKey(0f, float.MinValue));

                channelEffect.ListEffectProperty.Add(effectProperty);
            }

            return channelEffect;
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
