using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace Paper.Model
{
    [Serializable()]
    public class Sensor : ComponentBase
    {
        public SensorType SensorType { get; set; }

        public Sensor()
        {
        }

        public Sensor(int x, int y, SensorType sensorType)
            : base(x, y)
        {
            this.SensorType = sensorType;
        }

        [XmlIgnore]
        public override System.Drawing.Rectangle RectangleSelection
        {
            get
            {
                return new System.Drawing.Rectangle(Location.X + Common.Delta.X, Location.Y + Common.Delta.Y, 32, 32);
            }
        }
    }

    public enum SensorType
    {
        Button,
        Camera,
        Nearness,
        RemoteControl
    }
}
