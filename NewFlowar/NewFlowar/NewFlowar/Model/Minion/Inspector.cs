using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NewFlowar.Common;

namespace NewFlowar.Model.Minion
{
    public class Inspector : MinionBase
    {
        public override string ModelName
        {
            get
            {
                return "FlowInspector";
            }
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

        public Inspector(Cell cellStartLocation) : base(cellStartLocation)
        {
            _speed = 1f;
        }
    }
}
