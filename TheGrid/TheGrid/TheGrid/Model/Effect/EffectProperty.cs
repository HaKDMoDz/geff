using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using System.Xml.Serialization;

namespace TheGrid.Model.Effect
{
    [Serializable]
    public class EffectProperty
    {
        public float Default { get; set; }
        public float MinValue { get; set; }
        public float MaxValue { get; set; }
        public String Description { get; set; }
        [XmlIgnore]
        public Curve Curve { get; set; }

        public EffectProperty()
        {
            Curve = new Curve();
        }
    }
}
