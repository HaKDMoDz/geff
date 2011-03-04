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
        private static Cell _selectedCell;
        public static Cell SelectedCell 
        {
            get
            {
                return _selectedCell;
            }
            set
            {
                Cell oldSelectedCell = _selectedCell;
                _selectedCell = value;

                if (SelectedCellChanged != null)
                    SelectedCellChanged(oldSelectedCell, value);
            }
        }
        public static Cell CopiedCell { get; set; }
        public static Cell MovedSourceCell { get; set; }
        public static Cell MovedDestinationCell { get; set; }

        public static Map Map { get; set; }

        public static Player CurrentPlayer { get; set; }
        public static List<Player> Players { get; set; }
        public static TimeSpan Time { get; set; }
        public static StatePlaying StatePlaying { get; set; }
        public static Boolean IsNavigatingThroughTime { get; set; }
        public static float MenuSize = 3f;
        public static float PartitionRatio = 1f;
        public static GameEngine GameEngine { get; set; }

        public delegate void SelectedCellChangedHandler(Cell oldSelectedCell, Cell newSelectedCell);
        public static event SelectedCellChangedHandler SelectedCellChanged;
    }

    public enum ContextType
    {
        None
    }

    public enum StatePlaying
    {
        Playing,
        Stoped,
        Waiting
    }
}
