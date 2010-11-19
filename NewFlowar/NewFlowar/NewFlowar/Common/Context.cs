using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NewFlowar.Model;

namespace NewFlowar.Common
{
    public static class Context
    {
        public static ContextType ContextType { get; set; }
        public static Cell SelectedCell { get; set; }
        public static float HeightMapRadius { get; set; }
    }

    public enum ContextType
    {
        None,
        DefineSphereHeightMap
    }
}
