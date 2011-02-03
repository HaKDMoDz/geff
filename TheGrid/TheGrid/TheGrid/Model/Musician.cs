using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TheGrid.Model.Instrument;
using Microsoft.Xna.Framework;

namespace TheGrid.Model
{
    public class Musician
    {
        public Channel Channel { get; set; }
        public Cell CurrentCell { get; set; }
        public Cell NextCell { get; set; }
        public int CurrentIndex { get; set; }

        public int CurrentDirection { get; set; }
        
        public List<Cell> Partition  { get; set; }
        public List<TimeSpan> PartitionTime { get; set; }

        public Vector3 Position { get; set; }
        public Boolean IsPlaying { get; set; }

        public Musician(Channel channel)
        {
            Channel = channel;
            CurrentCell = null;
            CurrentDirection = 0;
            NextCell = null;
            Partition = new List<Cell>();
            PartitionTime = new List<TimeSpan>();
            Position = Vector3.Zero;
        }
    }
}
