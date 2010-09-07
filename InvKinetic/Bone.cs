using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace InvKinetic
{
    public class Bone
    {
        public Bone ParentBone { get; set; }

        public Point PositionEnd { get; set; }
        
        public Point AbsolutePositionEnd { get; set; }

        public float Angle { get; set; }

        public Point TempPosition { get; set; }

        public int Length { get; set; }
    }
}
