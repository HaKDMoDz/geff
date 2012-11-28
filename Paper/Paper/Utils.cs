using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Drawing;
using System;

namespace Paper
{
    public static class Utils
    {
        public static int Distance(Point point1, Point point2)
        {
            int distance = (int)Math.Sqrt((point1.X - point2.X) * (point1.X - point2.X) + (point1.Y - point2.Y) * (point1.Y - point2.Y));

            return distance;
        }


        /**
         * Return the distance from a point to a segment
         *
         * @param ps,pe the start/end of the segment
         * @param p the given point
         * @return the distance from the given point to the segment
         */
        public static int DistanceToSegment(Point ps, Point pe, Point p)
        {
            if (ps.X == pe.X && ps.Y == pe.Y) return (int)distance(ps, p);

            int sX = pe.X - ps.X;
            int sY = pe.Y - ps.Y;

            int uX = p.X - ps.X;
            int uY = p.Y - ps.Y;

            int dp = sX * uX + sY * uY;
            if (dp < 0) return (int)distance(ps, p);

            int sn2 = sX * sX + sY * sY;
            if (dp > sn2) return (int)distance(pe, p);

            double ah2 = dp * dp / sn2;
            int un2 = uX * uX + uY * uY;
            return (int)Math.Sqrt(un2 - ah2);
        }

        /**
         * return the distance between two points
         *
         * @param p1,p2 the two points
         * @return dist the distance
         */
        private static double distance(Point p1, Point p2)
        {
            int d2 = (p2.X - p1.X) * (p2.X - p1.X) + (p2.Y - p1.Y) * (p2.Y - p1.Y);
            return Math.Sqrt(d2);
        }

        /// <summary>
        /// Retourne la distance du point P au segment AB.
        /// Si A et B sont confondus, retourne la distance de A a P
        /// Si la projection perpendiculaire du point sur la droite contenant AB est hors de AB, retourne double.PositiveInfinity.
        /// Sinon retourne la distance du point à la droite contenant AB.
        /// </summary>
        /// <param name="P"></param>
        /// <param name="A"></param>
        /// <param name="B"></param>
        /// <returns></returns>
        public static int DistancePaAB(Point P, Point A, Point B)
        {
            Point AB = new Point() { X = B.X - A.X, Y = B.Y - A.Y }; // Vecteur AB
            double scalaireABAB = Scalaire(AB, AB);

            Point AP = new Point() { X = P.X - A.X, Y = P.Y - A.Y }; // Vecteur AP

            // A et B sont confondus
            if (scalaireABAB <= double.Epsilon)
            {
                return (int)Norme(AP);
            }
            else
            {
                double scalaireAPAB = Scalaire(AP, AB);

                // Si le produit scalaire est négatif : P est 'avant' A dans le sens AB.
                // S'il est plus grand que le scalaire AB AB : P est 'apres' B dans le sens AB.
                // Dans les deux cas, la projection de P sur la droite contenant AB est hors de AB
                if ((scalaireAPAB < 0d) || (scalaireAPAB > scalaireABAB))
                {
                    return int.MaxValue;
                }
                // Sinon la distance est la composante scalaire de AP sur le vecteur perpendiculaire à AB
                else
                {
                    Point perpAB = new Point() { X = AB.Y, Y = -AB.X }; // Un vecteur perpendiculaire à AB

                    return (int)Math.Abs(Scalaire(AP, perpAB) / Math.Sqrt(scalaireABAB));
                }
            }
        }

        static double Norme(Point V)
        {
            return Math.Sqrt((V.X * V.X) + (V.Y * V.Y));
        }

        static double Scalaire(Point V1, Point V2)
        {
            return (V1.X * V2.X) + (V1.Y * V2.Y);
        }
    }
}
