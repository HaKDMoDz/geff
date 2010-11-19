using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace NewFlowar.Common
{
    public static class Tools
    {
        public static float GetAngle(Vector2 vec1, Vector2 vec2)
        {
            float dot = vec1.X * vec2.X + vec1.Y * vec2.Y;
            float pdot = vec1.X * vec2.Y - vec1.Y * vec2.X;
            return (float)Math.Atan2(pdot, dot);
        }

        public static Vector3 GetVector3(Vector2 vec)
        {
            return new Vector3(vec, 0f);
        }

        public static Vector2 GetVector2(Microsoft.Xna.Framework.Vector3 vec)
        {
            return new Vector2(vec.X, vec.Y);
        }

        public static Vector2 GetPerpendicularVector(Vector2 vec)
        {
            return new Vector2(vec.Y, -vec.X);
        }

        public static Vector3 GetVector3WithoutZ(Vector3 vec)
        {
            return new Vector3(vec.X, vec.Y, 0f);
        }
    }
}
