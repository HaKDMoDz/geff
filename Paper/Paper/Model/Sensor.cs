using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using System.Drawing;

namespace Paper.Model
{
    [Serializable()]
    public class Sensor : ComponentBase, IMoveable
    {
        public SensorType SensorType { get; set; }

        public Sensor()
        {
        }

        public Sensor(int x, int y, SensorType sensorType)
            : base()
        {
            this.Location = new Point(x, y);
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

        public Point Location
        {
            get;
            set;
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
