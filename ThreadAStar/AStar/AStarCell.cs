using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ThreadAStar.AStar
{
    public class AStarCell
    {
        public Cell Cell { get; set; }
        //Somme de G+H
        public int F { get; set; }
        //Somme de la distance parcourue
        public int G { get; set; }
        //Distance à parcourir à vol d'oiseau
        public int H { get; set; }
        public Cell ParentCell { get; set; }
    }
}
