using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NewFlowar.Model;

namespace NewFlowar.Logic.AI
{
    public class AStarCell
    {
        public Cell Cell { get; set; }
        //Somme de G+H
        public float F { get; set; }
        //Somme de la distance parcourue
        public float G { get; set; }
        //Distance à parcourir à vol d'oiseau
        public float H { get; set; }
        public Cell ParentCell { get; set; }
    }
}
