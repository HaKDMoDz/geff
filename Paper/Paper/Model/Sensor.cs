using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Paper.Model
{
    public class Sensor : ComponentBase
    {
        public SensorType SensorType { get; set; }

        public Sensor(int x, int y, SensorType sensorType)
            : base(x, y)
        {
            this.SensorType = sensorType;
        }

        public override System.Drawing.Rectangle RectangleSelection
        {
            get
            {
                return new System.Drawing.Rectangle(Location.X, Location.Y, 50, 50);
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
