using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using NewFlowar.Common;

namespace NewFlowar.Model.Minion
{
    public abstract class MinionBase
    {
        public abstract String ModelName { get; }
        public abstract float Speed { get; set; }

        public Vector3 Location { get; set; }
        public Vector2 Direction { get; set; }
        public Cell CurrentCell { get; set; }
        public List<int> Path { get; set; }
        public float PathLength { get; set; }
        public float TraveledLength { get; set; }
        public float Angle { get; set; }

        public MinionBase(Cell cellStartLocation)
        {
            Path = new List<int>();
            this.CurrentCell = cellStartLocation;
            this.Location = Tools.GetVector3(cellStartLocation.Location);
        }
    }
}
