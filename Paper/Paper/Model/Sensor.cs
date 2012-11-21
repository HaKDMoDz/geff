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
    }

    public enum SensorType
    {
        Button,
        Camera,
        Nearness,
        RemoteControl
    }
}
