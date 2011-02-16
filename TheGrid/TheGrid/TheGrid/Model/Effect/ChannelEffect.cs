using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using Microsoft.Xna.Framework;
using TheGrid.Logic.Sound;
using TheGrid.Common;
using JSNet;

namespace TheGrid.Model.Effect
{
    [Serializable]
    public class ChannelEffect
    {
        [XmlIgnore]
        public Channel Channel { get; set; }
        public List<EffectProperty> ListEffectProperty { get; set; }
        public string Name { get; set; }

        public ChannelEffect()
        {
            this.ListEffectProperty = new List<EffectProperty>();
        }

        public ChannelEffect(Channel channel, string name) : this()
        {
            this.Channel = channel;
            this.Name = name;

            AddEffectProperty();
        }

        private void AddEffectProperty()
        {
            IList<Slider> sliders = Context.GameEngine.Sound.GetEffectParameters(Name);
            ListEffectProperty = new List<EffectProperty>();

            foreach (Slider slider in sliders)
            {
                EffectProperty effectProperty = new EffectProperty();
                effectProperty.Description = slider.Description;
                effectProperty.Default = slider.Default;
                effectProperty.Value = slider.Default;
                effectProperty.MinValue = slider.Minimum;
                effectProperty.MaxValue = slider.Maximum;
                effectProperty.Curve.Keys.Add(new CurveKey(0f, float.MinValue));

                ListEffectProperty.Add(effectProperty);
            }
        }

        public void Update(SoundLogic soundLogic, GameTime gameTime)
        {
            float[] values = new float[ListEffectProperty.Count];

            for (int i = 0; i < ListEffectProperty.Count; i++)
            {
                values[i] = ListEffectProperty[i].Curve.Evaluate((float)Context.Time.TotalMilliseconds);
            }

            soundLogic.Update(this, values, gameTime);
        }
    }
}
