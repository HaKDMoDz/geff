using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Paper
{
    public static class Triangulation
    {
        public static List<Edge> GetLinks(List<Vector2> listPoint)
        {
            int nv = listPoint.Count;
            //if (nv < 3)
            //    throw new ArgumentException("Need at least three vertices for triangulation");

            int trimax = 4 * nv;

            // Find the maximum and minimum listPoint bounds.
            // This is to allow calculation of the bounding supertriangle
            float xmin = listPoint[0].X;
            float ymin = listPoint[0].Y;
            float xmax = xmin;
            float ymax = ymin;
            for (int i = 1; i < nv; i++)
            {
                if (listPoint[i].X < xmin) xmin = listPoint[i].X;
                if (listPoint[i].X > xmax) xmax = listPoint[i].X;
                if (listPoint[i].Y < ymin) ymin = listPoint[i].Y;
                if (listPoint[i].Y > ymax) ymax = listPoint[i].Y;
            }

            float dx = xmax - xmin;
            float dy = ymax - ymin;
            float dmax = (dx > dy) ? dx : dy;

            float xmid = (xmax + xmin) * 0.5f;
            float ymid = (ymax + ymin) * 0.5f;


            // Set up the supertriangle
            // This is a triangle which encompasses all the sample points.
            // The supertriangle coordinates are added to the end of the
            // listPoint list. The supertriangle is the first triangle in
            // the triangle list.
            listPoint.Add(new Vector2((xmid - 2 * dmax), (ymid - dmax)));
            listPoint.Add(new Vector2(xmid, (ymid + 2 * dmax)));
            listPoint.Add(new Vector2((xmid + 2 * dmax), (ymid - dmax)));
            List<Triangle> Triangle = new List<Triangle>();
            Triangle.Add(new Triangle(nv, nv + 1, nv + 2)); //SuperTriangle placed at index 0

            // Include each point one at a time into the existing mesh
            for (int i = 0; i < nv; i++)
            {
                List<Edge> Edges = new List<Edge>(); //[trimax * 3];
                // Set up the edge buffer.
                // If the point (listPoint(i).x,listPoint(i).y) lies inside the circumcircle then the
                // three edges of that triangle are added to the edge buffer and the triangle is removed from list.
                for (int j = 0; j < Triangle.Count; j++)
                {
                    if (InCircle(listPoint[i], listPoint[Triangle[j].p1], listPoint[Triangle[j].p2], listPoint[Triangle[j].p3]))
                    {
                        Edges.Add(new Edge(Triangle[j].p1, Triangle[j].p2));
                        Edges.Add(new Edge(Triangle[j].p2, Triangle[j].p3));
                        Edges.Add(new Edge(Triangle[j].p3, Triangle[j].p1));
                        Triangle.RemoveAt(j);
                        j--;
                    }
                }
                if (i >= nv) continue; //In case we the last duplicate point we removed was the last in the array

                // Remove duplicate edges
                // Note: if all triangles are specified anticlockwise then all
                // interior edges are opposite pointing in direction.
                for (int j = Edges.Count - 2; j >= 0; j--)
                {
                    for (int k = Edges.Count - 1; k >= j + 1; k--)
                    {
                        if (Edges[j].Equals(Edges[k]))
                        {
                            Edges.RemoveAt(k);
                            Edges.RemoveAt(j);
                            k--;
                            continue;
                        }
                    }
                }
                // Form new triangles for the current point
                // Skipping over any tagged edges.
                // All edges are arranged in clockwise order.
                for (int j = 0; j < Edges.Count; j++)
                {
                    //if (Triangle.Count >= trimax)
                    //    throw new ApplicationException("Exceeded maximum edges");
                    Triangle.Add(new Triangle(Edges[j].p1, Edges[j].p2, i));
                }
                Edges.Clear();
                Edges = null;
            }
            // Remove triangles with supertriangle vertices
            // These are triangles which have a listPoint number greater than nv
            for (int i = Triangle.Count - 1; i >= 0; i--)
            {
                if (Triangle[i].p1 >= nv || Triangle[i].p2 >= nv || Triangle[i].p3 >= nv)
                    Triangle.RemoveAt(i);
            }
            //Remove SuperTriangle vertices
            listPoint.RemoveAt(listPoint.Count - 1);
            listPoint.RemoveAt(listPoint.Count - 1);
            listPoint.RemoveAt(listPoint.Count - 1);
            Triangle.TrimExcess();


            //---

            List<Edge> listEdge = new List<Edge>();

            //for (int i = 0; i < nv; i++)
            //{
            //    listBlobLink.Add(new BlobLink());
            //}

            for (int i = 0; i < Triangle.Count; i++)
            {
                //listBlobLink[Triangle[i].p1].listLink.Add(Triangle[i].p2);
                //listBlobLink[Triangle[i].p1].listLink.Add(Triangle[i].p3);

                if (!listEdge.Exists(edge => (edge.p1 == Triangle[i].p1 && edge.p2 == Triangle[i].p2) || (edge.p1 == Triangle[i].p2 && edge.p2 == Triangle[i].p1)))
                {
                    listEdge.Add(new Edge(Triangle[i].p1, Triangle[i].p2));
                }

                if (!listEdge.Exists(edge => (edge.p1 == Triangle[i].p2 && edge.p2 == Triangle[i].p3) || (edge.p1 == Triangle[i].p3 && edge.p2 == Triangle[i].p2)))
                {
                    listEdge.Add(new Edge(Triangle[i].p2, Triangle[i].p3));
                }

                if (!listEdge.Exists(edge => (edge.p1 == Triangle[i].p3 && edge.p2 == Triangle[i].p1) || (edge.p1 == Triangle[i].p1 && edge.p2 == Triangle[i].p3)))
                {
                    listEdge.Add(new Edge(Triangle[i].p3, Triangle[i].p1));
                }
            }

            return listEdge;
            //return Triangle;
        }

        /// <summary>
        /// Returns true if the point (p) lies inside the circumcircle made up by points (p1,p2,p3)
        /// </summary>
        /// <remarks>
        /// NOTE: A point on the edge is inside the circumcircle
        /// </remarks>
        /// <param name="p">Point to check</param>
        /// <param name="p1">First point on circle</param>
        /// <param name="p2">Second point on circle</param>
        /// <param name="p3">Third point on circle</param>
        /// <returns>true if p is inside circle</returns>
        private static bool InCircle(Vector2 p, Vector2 p1, Vector2 p2, Vector2 p3)
        {
            //Return TRUE if the point (xp,yp) lies inside the circumcircle
            //made up by points (x1,y1) (x2,y2) (x3,y3)
            //NOTE: A point on the edge is inside the circumcircle

            if (System.Math.Abs(p1.Y - p2.Y) < double.Epsilon && System.Math.Abs(p2.Y - p3.Y) < double.Epsilon)
            {
                //INCIRCUM - F - Points are coincident !!
                return false;
            }

            double m1, m2;
            double mx1, mx2;
            double my1, my2;
            double xc, yc;

            if (System.Math.Abs(p2.Y - p1.Y) < double.Epsilon)
            {
                m2 = -(p3.X - p2.X) / (p3.Y - p2.Y);
                mx2 = (p2.X + p3.X) * 0.5;
                my2 = (p2.Y + p3.Y) * 0.5;
                //Calculate CircumCircle center (xc,yc)
                xc = (p2.X + p1.X) * 0.5;
                yc = m2 * (xc - mx2) + my2;
            }
            else if (System.Math.Abs(p3.Y - p2.Y) < double.Epsilon)
            {
                m1 = -(p2.X - p1.X) / (p2.Y - p1.Y);
                mx1 = (p1.X + p2.X) * 0.5;
                my1 = (p1.Y + p2.Y) * 0.5;
                //Calculate CircumCircle center (xc,yc)
                xc = (p3.X + p2.X) * 0.5;
                yc = m1 * (xc - mx1) + my1;
            }
            else
            {
                m1 = -(p2.X - p1.X) / (p2.Y - p1.Y);
                m2 = -(p3.X - p2.X) / (p3.Y - p2.Y);
                mx1 = (p1.X + p2.X) * 0.5;
                mx2 = (p2.X + p3.X) * 0.5;
                my1 = (p1.Y + p2.Y) * 0.5;
                my2 = (p2.Y + p3.Y) * 0.5;
                //Calculate CircumCircle center (xc,yc)
                xc = (m1 * mx1 - m2 * mx2 + my2 - my1) / (m1 - m2);
                yc = m1 * (xc - mx1) + my1;
            }

            double dx = p2.X - xc;
            double dy = p2.Y - yc;
            double rsqr = dx * dx + dy * dy;
            //double r = Math.Sqrt(rsqr); //Circumcircle radius
            dx = p.X - xc;
            dy = p.Y - yc;
            double drsqr = dx * dx + dy * dy;

            return (drsqr <= rsqr);
        }
    }

    /// <summary>
    /// Triangle made from three point indexes
    /// </summary>
    public struct Triangle
    {
        /// <summary>
        /// First vertex index in triangle
        /// </summary>
        public int p1;
        /// <summary>
        /// Second vertex index in triangle
        /// </summary>
        public int p2;
        /// <summary>
        /// Third vertex index in triangle
        /// </summary>
        public int p3;
        /// <summary>
        /// Initializes a new instance of a triangle
        /// </summary>
        /// <param name="point1">Vertex 1</param>
        /// <param name="point2">Vertex 2</param>
        /// <param name="point3">Vertex 3</param>
        public Triangle(int point1, int point2, int point3)
        {
            p1 = point1; p2 = point2; p3 = point3;
        }
    }

    /// <summary>
    /// Edge made from two point indexes
    /// </summary>
    public class Edge : IEquatable<Edge>
    {
        /// <summary>
        /// Start of edge index
        /// </summary>
        public int p1;
        /// <summary>
        /// End of edge index
        /// </summary>
        public int p2;
        /// <summary>
        /// Initializes a new edge instance
        /// </summary>
        /// <param name="point1">Start edge vertex index</param>
        /// <param name="point2">End edge vertex index</param>
        public Edge(int point1, int point2)
        {
            p1 = point1; p2 = point2;
        }
        /// <summary>
        /// Initializes a new edge instance with start/end indexes of '0'
        /// </summary>
        public Edge()
            : this(0, 0)
        {
        }

        #region IEquatable<dEdge> Members

        /// <summary>
        /// Checks whether two edges are equal disregarding the direction of the edges
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public bool Equals(Edge other)
        {
            return
                ((this.p1 == other.p2) && (this.p2 == other.p1)) ||
                ((this.p1 == other.p1) && (this.p2 == other.p2));
        }

        #endregion
    }

    public class BlobLink
    {
        public List<int> listLink = new List<int>();
    }
}
