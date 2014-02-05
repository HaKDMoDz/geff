using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Code.Utils
{
    struct TouchMouse
    {
        public bool IsMouse;
        public TouchPhase TouchPhase;
        public Vector2 Position;
        public int FingerId;
    }
}
