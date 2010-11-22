using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using NewFlowar.Model;

namespace NewFlowar.Common
{
    public static class Tools
    {
        private static List<int[]> listTriangle = new List<int[]>();

        static Tools()
        {
            listTriangle.Add(new int[] { 3, 2, 1 });
            listTriangle.Add(new int[] { 6, 3, 1 });
            listTriangle.Add(new int[] { 6, 4, 3 });
            listTriangle.Add(new int[] { 6, 5, 4 });
        }

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

        public static double GetBellCurvePoint(double Percentage, double Midpoint)
        {
            if (Percentage > Midpoint)
            {
                Percentage = 1 - Percentage;
                return 1 - ((Percentage - ((1 - Percentage) * Percentage)) * (1 / (1 - Midpoint)));
            }
            else
            {
                return (Percentage - ((1 - Percentage) * Percentage)) * (1 / Midpoint);
            }
        }

        public static int Distance(Point point1, Point point2)
        {
            int distance = (int)Math.Sqrt((point1.X - point2.X) * (point1.X - point2.X) + (point1.Y - point2.Y) * (point1.Y - point2.Y));

            return distance;
        }

        public static float Distance(Vector2 point1, Vector2 point2)
        {
            float distance = (int)Math.Sqrt((point1.X - point2.X) * (point1.X - point2.X) + (point1.Y - point2.Y) * (point1.Y - point2.Y));

            return distance;
        }

        public static Vector3 NormalTriangle(Vector3 vec1, Vector3 vec2, Vector3 vec3)
        {
            Vector3 normal = Vector3.Zero;

            Vector3 firstvec = vec2 - vec1;
            Vector3 secondvec = vec1 - vec3;

            //Vector3 firstvec = vertices[indices[i * 3 + 1]].Position - vertices[indices[i * 3]].Position;
            //Vector3 secondvec = vertices[indices[i * 3]].Position - vertices[indices[i * 3 + 2]].Position;
            
            normal = Vector3.Cross(firstvec, secondvec);
            normal.Normalize();

            return normal;
        }

        public static Vector3 NormalCell(Map map, Cell cell)
        {
            float? pickDistance = float.MaxValue;
            float pickCurDistance = 0f;
            float barycentricU = 0f;
            float barycentricV = 0f;
            Vector3 rayPosition = new Vector3(cell.Location,50f);
            Vector3 normal = Vector3.UnitZ;

            for (int i = 0; i < 4; i++)
            {
                bool intersect = RayIntersectTriangle(rayPosition, -Vector3.UnitZ,
                                    map.Points[cell.Points[listTriangle[i][0]]],
                                    map.Points[cell.Points[listTriangle[i][2]]],
                                    map.Points[cell.Points[listTriangle[i][1]]],
                                    ref pickCurDistance, ref barycentricU, ref barycentricV);

                if (intersect && pickCurDistance < pickDistance)
                {
                    pickDistance = pickCurDistance;

                    normal = NormalTriangle(
                                    map.Points[cell.Points[listTriangle[i][0]]],
                                    map.Points[cell.Points[listTriangle[i][1]]],
                                    map.Points[cell.Points[listTriangle[i][2]]]);
                }


            }

            return normal;
        }

        public static float? RayIntersectCell(Vector3 rayPosition, Vector3 rayDirection, Map map, Cell cell)
        {
            float? pickDistance = float.MaxValue;
            float pickCurDistance = 0f;
            float barycentricU = 0f;
            float barycentricV = 0f;

            for (int i = 0; i < 4; i++)
            {
                bool intersect = RayIntersectTriangle(rayPosition, rayDirection,
                                    map.Points[cell.Points[listTriangle[i][0]]],
                                    map.Points[cell.Points[listTriangle[i][2]]],
                                    map.Points[cell.Points[listTriangle[i][1]]],
                                    ref pickCurDistance, ref barycentricU, ref barycentricV);

                if (intersect && pickCurDistance < pickDistance)
                {
                    pickDistance = pickCurDistance;
                }
            }

            if (pickDistance == float.MaxValue)
                return null;
            else
                return pickDistance;
        }

        public static bool RayIntersectTriangle(Vector3 rayPosition, Vector3 rayDirection, Vector3 tri0, Vector3 tri1, Vector3 tri2, ref float pickDistance, ref float barycentricU, ref float barycentricV)
        {
            // Find vectors for two edges sharing vert0

            Vector3 edge1, edge2;

            Vector3.Subtract(ref tri1, ref tri0, out edge1);
            Vector3.Subtract(ref tri2, ref tri0, out edge2);

            //= tri1 - tri0;
            //Vector3 edge2 = tri2 - tri0;

            // Begin calculating determinant - also used to calculate barycentricU parameter
            Vector3 pvec;
            Vector3.Cross(ref rayDirection, ref edge2, out pvec);

            // If determinant is near zero, ray lies in plane of triangle
            float det;
            Vector3.Dot(ref edge1, ref pvec, out det);

            if (det < 0.0001f)
                return false;

            // Calculate distance from vert0 to ray origin
            Vector3 tvec;

            Vector3.Subtract(ref rayPosition, ref tri0, out tvec);

            // Calculate barycentricU parameter and test bounds
            //barycentricU;

            Vector3.Dot(ref tvec, ref pvec, out barycentricU);

            if (barycentricU < 0.0f || barycentricU > det)
                return false;

            // Prepare to test barycentricV parameter
            Vector3 qvec;
            Vector3.Cross(ref tvec, ref edge1, out qvec);

            // Calculate barycentricV parameter and test bounds
            Vector3.Dot(ref rayDirection, ref qvec, out barycentricV);

            if (barycentricV < 0.0f || barycentricU + barycentricV > det)
                return false;

            // Calculate pickDistance, scale parameters, ray intersects triangle
            Vector3.Dot(ref edge2, ref qvec, out pickDistance);

            float fInvDet = 1.0f / det;
            pickDistance *= fInvDet;
            barycentricU *= fInvDet;
            barycentricV *= fInvDet;

            return true;
        }
    }
}
