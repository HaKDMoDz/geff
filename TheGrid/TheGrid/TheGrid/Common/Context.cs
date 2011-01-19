using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TheGrid.Model;

namespace TheGrid.Common
{
    public static class Context
    {
        public static ContextType ContextType { get; set; }
        public static Cell SelectedCell { get; set; }
        //public static MinionBase SelectedMinion { get; set; }

        //public static float HeightMapRadius { get; set; }
        //public static bool DetachedHexaActivated { get; set; }
        //public static bool GoToFlagActivated { get; set; }

        public static Player CurrentPlayer { get; set; }
        public static List<Player> Players { get; set; }
    }

    public enum ContextType
    {
        None,
        DefineSphereHeightMap
    }
}
