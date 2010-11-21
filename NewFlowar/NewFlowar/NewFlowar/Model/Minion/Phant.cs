using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NewFlowar.Model.Minion
{
    public class Phant : MinionBase
    {
        public override string ModelName
        {
            get { return "FlowPhant"; }
        }

        private float _speed;
        public override float Speed
        {
            get
            {
                return _speed;
            }
            set
            {
                _speed = value;
            }
        }

        public Phant(Cell cellStartLocation)
            : base(cellStartLocation)
        {
            _speed = 0.5f;
        }
    }
}
