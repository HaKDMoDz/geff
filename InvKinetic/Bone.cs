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
        public Bone ChildBone { get; set; }

        public Point PositionEnd { get; set; }
        
        public Point AbsolutePositionEnd { get; set; }

        public float Angle { get; set; }
        public float AngleConstraintMin { get; set; }
        public float AngleConstraintMax { get; set; }

        public int Length { get; set; }
        public int Level { get; set; }
    }
}
