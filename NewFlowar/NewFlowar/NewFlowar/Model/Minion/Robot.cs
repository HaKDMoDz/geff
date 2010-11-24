using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NewFlowar.Model.Minion
{
    public class Robot: MinionBase
    {
        public override string ModelName
        {
            get
            {
                return "FlowRobot1";
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

        public Robot(Cell cellStartLocation)
            : base(cellStartLocation)
        {
            _speed = 1f;
        }
    }
}