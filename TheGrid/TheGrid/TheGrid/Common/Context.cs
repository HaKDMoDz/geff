using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TheGrid.Model;
using TheGrid.Model.Menu;

namespace TheGrid.Common
{
    public static class Context
    {
        public static ContextType ContextType { get; set; }
        public static Cell SelectedCell { get; set; }

        public static Menu CurrentMenu { get; set; }
        public static Menu NextMenu { get; set; }
        public static Map Map { get; set; }
        public static List<Channel> Channels { get; set; }

        public static Player CurrentPlayer { get; set; }
        public static List<Player> Players { get; set; }
    }

    public enum ContextType
    {
        None
    }
}
