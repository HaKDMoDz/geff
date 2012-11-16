using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Paper
{
    public static class Common
    {
        public static Line lineMidScreen = null;
        public static int depthUnity = 20;
        public static Tools CurrentTool = Tools.None;
    }

    public enum Tools
    {
        None,
        ZoneCuttingH,
        zonecuttingV,
        ZoneMovingH,
        ZoneMovingV,
        Folding,
        Platform,
        SensorButton,
        SensorNearness,
        SensorCamera,
        SensorRemoteControl,
        Link
    }
}
