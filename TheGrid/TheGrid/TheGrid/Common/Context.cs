using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TheGrid.Model;
using TheGrid.Model.UI.Menu;

namespace TheGrid.Common
{
    public static class Context
    {
        public static ContextType ContextType { get; set; }
        public static Cell SelectedCell { get; set; }
        public static Cell CopiedCell { get; set; }

        public static Map Map { get; set; }

        public static Player CurrentPlayer { get; set; }
        public static List<Player> Players { get; set; }
        public static TimeSpan Time { get; set; }
        public static Boolean IsPlaying { get; set; }
        public static float SpeedFactor { get; set; }
        public static TimeSpan PartitionDuration { get; set; }
        public static Boolean IsNavigatingThroughTime { get; set; }
        public static float MenuSize = 3f;
        public static GameEngine GameEngine { get; set; }
    }

    public enum ContextType
    {
        None
    }
}
