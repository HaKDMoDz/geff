using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace TheGrid.Model.Menu
{
    public class Item
    {
        public Menu ParentMenu { get; set; }
        public bool Checked { get; set; }
        public int Value { get; set; }
        public String Name { get; set; }

        public Item(Menu parentMenu, string name) : this(parentMenu, name, 0) { }

        public Item(Menu parentMenu, string name, int value)
        {
            ParentMenu = parentMenu;
            Name = name;
            Value = value;
        }

        public delegate void SelectedHandler(Item item, GameTime gameTime);
        public event SelectedHandler Selected;
        protected virtual void OnSelected(GameTime gameTime)
        {
            if(Selected != null)
                Selected(this, gameTime);
        }
    }
}