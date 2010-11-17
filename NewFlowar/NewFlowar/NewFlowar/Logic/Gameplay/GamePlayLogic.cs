using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NewFlowar.Model;

namespace NewFlowar.Logic.GamePlay
{
    public class GamePlayLogic
    {
        public Map Map { get; set; }

        public GamePlayLogic()
        {
            Map = new Map(20, 20);
            Map.CreateGrid();
        }
    }
}
