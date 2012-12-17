using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace Paper
{
    public static class Common
    {
        public static Line lineMidScreen = null;
        public static int Bottom = 0;
        public static int MaxDepth = 9;
        public static int depthUnity = 10;
        public static Tools CurrentTool = Tools.None;
        public static int CurrentColorIndex = 1;
        public static System.Drawing.Size ScreenSize { get; set; }
        public static Point Delta { get; set; }
    }

    public enum Tools
    {
        None,
        ZoneFoldingH,
        ZoneFoldingV,
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
