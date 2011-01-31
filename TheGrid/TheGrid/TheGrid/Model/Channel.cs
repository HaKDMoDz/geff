using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace TheGrid.Model
{
    public class Channel
    {
        public Color Color { get; set; }
        public List<Musician> ListMusician { get; set; }
        public List<Sample> ListSample { get; set; }
        public string Name { get; set; }
        public Cell CellStart { get; set; }

        public float Speed { get; set; }

        public Channel(string name, Color color)
        {
            Name = name;
            Color = color;
        }
    }
}
