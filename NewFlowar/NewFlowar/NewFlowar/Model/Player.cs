using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NewFlowar.Model.Minion;

namespace NewFlowar.Model
{
    public class Player
    {
        public byte PlayerId { get; set; }
        public String PlayerName { get; set; }

        public List<MinionBase> Minions { get; set; }
    }
}
