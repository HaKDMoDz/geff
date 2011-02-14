using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using Microsoft.Xna.Framework;
using TheGrid.Logic.Sound;
using TheGrid.Common;

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
