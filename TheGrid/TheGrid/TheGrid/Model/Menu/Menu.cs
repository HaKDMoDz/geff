using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace TheGrid.Model.Menu
{
    public class Menu
    {
        public Cell ParentCell { get; set; }
        public Menu ParentMenu { get; set; }
        public List<Item> Items { get; set; }
        public Boolean Opened { get; set; }
        public TimeSpan LastStateChanged { get; set; }

        public Menu(Cell parentCell, Menu parentMenu)
        {
            ParentCell = parentCell;
            ParentMenu = parentMenu;
        }

        public void Open(GameTime gameTime)
        {
        }

        public void Close(GameTime gameTime)
        {
            Opened = false;
            LastStateChanged = gameTime.TotalGameTime;
        }
    }
}
