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
        public Vector3 Position { get; set; }
        public Boolean IsPlaying { get; set; }
        public TypePlaying TypePlaying { get; set; }
        public List<TimeValue<Cell>> Partition { get; set; }
        public TimeSpan ElapsedTime { get; set; }

        /// <summary>
        /// Direction courante (pour l'écriture de la partition)
        /// </summary>
        public int CurrentDirection { get; set; }

        /// <summary>
        /// Facteur de vitesse, noire, croche, double croche, triple croche (pour l'écriture de la partition)
        /// </summary>
        public float SpeedFactor { get; set; }

        /// <summary>
        /// Index courant de la partition (Pour la lecture de la partition)
        /// </summary>
        public int CurrentIndex { get; set; }

        public Musician(Channel channel)
        {
            Channel = channel;
            CurrentCell = null;
            CurrentDirection = 0;
            NextCell = null;
            Partition = new List<TimeValue<Cell>>();
            Position = Vector3.Zero;
        }
    }
}
