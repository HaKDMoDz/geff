using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using Paper.Model;
using Paper.Properties;
using System.IO;

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


        private static List<Folding> GetFoldings(Scene scene)
        {
            List<Folding> listFoldings = new List<Folding>();

            foreach (ComponentBase componenent in scene.listComponent)
            {
                if (componenent is Folding)
                {
                    listFoldings.Add((Folding)componenent);
                }
            }

            return listFoldings;
        }

        private static string ReplaceValue(string source, string code, float value)
        {
            return source.Replace(code, value.ToString().Replace(",", "."));
        }

        public static void ExportToFBX(Scene scene, string fileName)
        {
            String strTemplate = Resources.FBX_Exporter_Template_Papier;

            strTemplate = strTemplate.Replace("{YEAR}", DateTime.Now.Year.ToString());
            strTemplate = strTemplate.Replace("{MONTH}", DateTime.Now.Month.ToString());
            strTemplate = strTemplate.Replace("{DAY}", DateTime.Now.Day.ToString());
            strTemplate = strTemplate.Replace("{HOUR}", DateTime.Now.Hour.ToString());
            strTemplate = strTemplate.Replace("{MINUTE}", DateTime.Now.Minute.ToString());
            strTemplate = strTemplate.Replace("{SECOND}", DateTime.Now.Second.ToString());
            strTemplate = strTemplate.Replace("{MILLISECOND}", DateTime.Now.Millisecond.ToString());

            strTemplate = strTemplate.Replace("{DATETIME}", DateTime.Now.ToString());

            List<Folding> listFoldings = GetFoldings(scene);

            strTemplate = strTemplate.Replace("{OBJECT_COUNT}", (listFoldings.Count * 6 + 2).ToString());
            strTemplate = strTemplate.Replace("{MODEL_COUNT}", (listFoldings.Count * 4).ToString());
            strTemplate = strTemplate.Replace("{GEOMETRY_COUNT}", (listFoldings.Count).ToString());
            strTemplate = strTemplate.Replace("{DEFORMER_COUNT}", (listFoldings.Count * 3).ToString());


            int indexStart = strTemplate.IndexOf("{MODEL}");
            int indexEnd = strTemplate.IndexOf("{/MODEL}");

            string strSubTemplate = strTemplate.Substring(indexStart + 7, indexEnd - indexStart - 7);
            StringBuilder strFile = new StringBuilder();

            strFile.Append(strTemplate.Substring(0, indexStart));

            float d = 40f;
            int nbFolding = 0;
            foreach (Folding folding in listFoldings)
            {
                string strFoldingModel = strSubTemplate;

                strFoldingModel = strFoldingModel.Replace("{ARMATURE_NAME}", "Armature_" + nbFolding.ToString());
                strFoldingModel = strFoldingModel.Replace("{FOLDING_NAME}", "Folding_" + nbFolding.ToString());


                strFoldingModel = ReplaceValue(strFoldingModel, "{P0.X}", (float)folding.Location.X / d);

                strFoldingModel = ReplaceValue(strFoldingModel, "{P0.X}", (float)folding.Location.X / d);
                strFoldingModel = ReplaceValue(strFoldingModel, "{P0.Y}", (float)folding.Location.Y / d);
                strFoldingModel = ReplaceValue(strFoldingModel, "{P0.Z}", (float)folding.Height / d);

                strFoldingModel = ReplaceValue(strFoldingModel, "{P1.X}", (float)folding.Location.X / d + (float)folding.Width / d);
                strFoldingModel = ReplaceValue(strFoldingModel, "{P1.Y}", (float)folding.Location.Y / d);
                strFoldingModel = ReplaceValue(strFoldingModel, "{P1.Z}", (float)folding.Height / d);

                strFoldingModel = ReplaceValue(strFoldingModel, "{P2.X}", (float)folding.Location.X / d + (float)folding.Width / d);
                strFoldingModel = ReplaceValue(strFoldingModel, "{P2.Y}", (0));
                strFoldingModel = ReplaceValue(strFoldingModel, "{P2.Z}", (float)folding.Height / d);

                strFoldingModel = ReplaceValue(strFoldingModel, "{P3.X}", (float)folding.Location.X / d);
                strFoldingModel = ReplaceValue(strFoldingModel, "{P3.Y}", (0));
                strFoldingModel = ReplaceValue(strFoldingModel, "{P3.Z}", (float)folding.Height / d);


                strFoldingModel = ReplaceValue(strFoldingModel, "{P4.X}", (float)folding.Location.X / d);
                strFoldingModel = ReplaceValue(strFoldingModel, "{P4.Y}", (float)folding.Location.Y / d);
                strFoldingModel = ReplaceValue(strFoldingModel, "{P4.Z}", (0));

                strFoldingModel = ReplaceValue(strFoldingModel, "{P5.X}", (float)folding.Location.X / d + (float)folding.Width / d);
                strFoldingModel = ReplaceValue(strFoldingModel, "{P5.Y}", (float)folding.Location.Y / d);
                strFoldingModel = ReplaceValue(strFoldingModel, "{P5.Z}", (0));


                strFoldingModel = strFoldingModel.Replace("{BONE1_NAME}", "Bone_" + (nbFolding * 2).ToString());
                strFoldingModel = ReplaceValue(strFoldingModel, "{FOLDING_DEEP}", (float)folding.Height / d);
                strFoldingModel = ReplaceValue(strFoldingModel, "{FOLDING_HEIGHT}", (float)folding.Location.Y / d);

                strFoldingModel = strFoldingModel.Replace("{BONE2_NAME}", "Bone_" + (nbFolding * 2 + 1).ToString());

                strFile.AppendLine();
                strFile.Append(strFoldingModel);

                nbFolding++;
            }


            int indexStart2 = strTemplate.IndexOf("{POSE}");
            int indexEnd2 = strTemplate.IndexOf("{/POSE}");

            strSubTemplate = strTemplate.Substring(indexStart2 + 6, indexEnd2 - indexStart2 - 6);

            strFile.Append(strTemplate.Substring(indexEnd + 8, indexStart2 - indexEnd - 8));

            strFile = strFile.Replace("{POSE_COUNT}", (nbFolding * 4).ToString());

            nbFolding = 0;
            foreach (Folding folding in listFoldings)
            {
                string strFoldingModel = strSubTemplate;

                strFoldingModel = strFoldingModel.Replace("{ARMATURE_NAME}", "Armature_" + nbFolding.ToString());
                strFoldingModel = strFoldingModel.Replace("{FOLDING_NAME}", "Folding_" + nbFolding.ToString());
                strFoldingModel = strFoldingModel.Replace("{BONE1_NAME}", "Bone_" + (nbFolding * 2).ToString());
                strFoldingModel = strFoldingModel.Replace("{BONE2_NAME}", "Bone_" + (nbFolding * 2 + 1).ToString());

                strFile.AppendLine();
                strFile.Append(strFoldingModel);

                nbFolding++;
            }



            int indexStart3 = strTemplate.IndexOf("{MODEL_RELATIONS}");
            int indexEnd3 = strTemplate.IndexOf("{/MODEL_RELATIONS}");

            strSubTemplate = strTemplate.Substring(indexStart3 + 17, indexEnd3 - indexStart3 - 17);

            strFile.Append(strTemplate.Substring(indexEnd2 + 7, indexStart3 - indexEnd2 - 7));

            nbFolding = 0;
            foreach (Folding folding in listFoldings)
            {
                string strFoldingModel = strSubTemplate;

                strFoldingModel = strFoldingModel.Replace("{ARMATURE_NAME}", "Armature_" + nbFolding.ToString());
                strFoldingModel = strFoldingModel.Replace("{FOLDING_NAME}", "Folding_" + nbFolding.ToString());
                strFoldingModel = strFoldingModel.Replace("{BONE1_NAME}", "Bone_" + (nbFolding * 2).ToString());
                strFoldingModel = strFoldingModel.Replace("{BONE2_NAME}", "Bone_" + (nbFolding * 2 + 1).ToString());

                strFile.AppendLine();
                strFile.Append(strFoldingModel);

                nbFolding++;
            }

            int indexStart4 = strTemplate.IndexOf("{MODEL_CONNECTIONS}");
            int indexEnd4 = strTemplate.IndexOf("{/MODEL_CONNECTIONS}");

            strSubTemplate = strTemplate.Substring(indexStart4 + 19, indexEnd4 - indexStart4 - 19);

            strFile.Append(strTemplate.Substring(indexEnd3 + 18, indexStart4 - indexEnd3 - 18));

            nbFolding = 0;
            foreach (Folding folding in listFoldings)
            {
                string strFoldingModel = strSubTemplate;

                strFoldingModel = strFoldingModel.Replace("{ARMATURE_NAME}", "Armature_" + nbFolding.ToString());
                strFoldingModel = strFoldingModel.Replace("{FOLDING_NAME}", "Folding_" + nbFolding.ToString());
                strFoldingModel = strFoldingModel.Replace("{BONE1_NAME}", "Bone_" + (nbFolding * 2).ToString());
                strFoldingModel = strFoldingModel.Replace("{BONE2_NAME}", "Bone_" + (nbFolding * 2 + 1).ToString());

                strFile.AppendLine();
                strFile.Append(strFoldingModel);

                nbFolding++;
            }

            strFile.Append(strTemplate.Substring(indexEnd4 + 20));

            File.WriteAllText(fileName, strFile.ToString());
        }

        public static void ExportToCOLLADA(Scene scene, string fileName)
        {
            String strTemplate = Resources.COLLADA_Exporter_Template_Papier;

            strTemplate = strTemplate.Replace("{DATETIME}", DateTime.Now.ToString());

            List<Folding> listFoldings = GetFoldings(scene);

            int indexStart = strTemplate.IndexOf("{MODEL}");
            int indexEnd = strTemplate.IndexOf("{/MODEL}");

            string strSubTemplate = strTemplate.Substring(indexStart + 7, indexEnd - indexStart - 7);
            StringBuilder strFile = new StringBuilder();

            strFile.Append(strTemplate.Substring(0, indexStart));

            float d = 40f;
            float dh = d;

            for (int i = 0; i < listFoldings.Count + 1; i++)
            {
                string strFoldingModel = strSubTemplate;

                if (i < listFoldings.Count)
                {
                    Folding folding = listFoldings[i];

                    strFoldingModel = strFoldingModel.Replace("{ARMATURE_NAME}", "Armature_" + i.ToString());
                    strFoldingModel = strFoldingModel.Replace("{FOLDING_NAME}", "Folding_" + i.ToString());

                    strFoldingModel = ReplaceValue(strFoldingModel, "{P0.X}", (float)folding.Location.X / d);
                    strFoldingModel = ReplaceValue(strFoldingModel, "{P0.Z}", (float)-folding.Location.Y / d + folding.Height/d);
                    strFoldingModel = ReplaceValue(strFoldingModel, "{P0.Y}", (float)-folding.Height / d);

                    strFoldingModel = ReplaceValue(strFoldingModel, "{P1.X}", (float)folding.Location.X / d + (float)folding.Width / d);
                    strFoldingModel = ReplaceValue(strFoldingModel, "{P1.Z}", (float)-folding.Location.Y / d + folding.Height / d);
                    strFoldingModel = ReplaceValue(strFoldingModel, "{P1.Y}", (float)-folding.Height / dh);

                    strFoldingModel = ReplaceValue(strFoldingModel, "{P2.X}", (float)folding.Location.X / d + (float)folding.Width / d);
                    strFoldingModel = ReplaceValue(strFoldingModel, "{P2.Z}", (0));
                    strFoldingModel = ReplaceValue(strFoldingModel, "{P2.Y}", (float)-folding.Height / dh);

                    strFoldingModel = ReplaceValue(strFoldingModel, "{P3.X}", (float)folding.Location.X / d);
                    strFoldingModel = ReplaceValue(strFoldingModel, "{P3.Z}", (0));
                    strFoldingModel = ReplaceValue(strFoldingModel, "{P3.Y}", (float)-folding.Height / dh);


                    strFoldingModel = ReplaceValue(strFoldingModel, "{P4.X}", (float)folding.Location.X / d);
                    strFoldingModel = ReplaceValue(strFoldingModel, "{P4.Z}", (float)-folding.Location.Y / d + folding.Height / d);
                    strFoldingModel = ReplaceValue(strFoldingModel, "{P4.Y}", (0));

                    strFoldingModel = ReplaceValue(strFoldingModel, "{P5.X}", (float)folding.Location.X / d + (float)folding.Width / d);
                    strFoldingModel = ReplaceValue(strFoldingModel, "{P5.Z}", (float)-folding.Location.Y / d + folding.Height / d);
                    strFoldingModel = ReplaceValue(strFoldingModel, "{P5.Y}", (0));
                }
                else
                {
                    strFoldingModel = strFoldingModel.Replace("{ARMATURE_NAME}", "Armature_" + i.ToString());
                    strFoldingModel = strFoldingModel.Replace("{FOLDING_NAME}", "Folding_" + i.ToString());


                    strFoldingModel = ReplaceValue(strFoldingModel, "{P1.X}", 0); //0
                    strFoldingModel = ReplaceValue(strFoldingModel, "{P1.Z}", 0f);
                    strFoldingModel = ReplaceValue(strFoldingModel, "{P1.Y}", 0f);

                    strFoldingModel = ReplaceValue(strFoldingModel, "{P0.X}", 20f); //1
                    strFoldingModel = ReplaceValue(strFoldingModel, "{P0.Z}", 0f);
                    strFoldingModel = ReplaceValue(strFoldingModel, "{P0.Y}", 0f);


                    strFoldingModel = ReplaceValue(strFoldingModel, "{P2.X}", 0f); //2
                    strFoldingModel = ReplaceValue(strFoldingModel, "{P2.Z}", 20f);
                    strFoldingModel = ReplaceValue(strFoldingModel, "{P2.Y}", 0f);

                    strFoldingModel = ReplaceValue(strFoldingModel, "{P3.X}", 20f); //3
                    strFoldingModel = ReplaceValue(strFoldingModel, "{P3.Z}", 20f);
                    strFoldingModel = ReplaceValue(strFoldingModel, "{P3.Y}", 0f);


                    strFoldingModel = ReplaceValue(strFoldingModel, "{P4.X}", 20f);
                    strFoldingModel = ReplaceValue(strFoldingModel, "{P4.Z}", 0f);
                    strFoldingModel = ReplaceValue(strFoldingModel, "{P4.Y}", -20f);
                    
                    strFoldingModel = ReplaceValue(strFoldingModel, "{P5.X}", 0f);
                    strFoldingModel = ReplaceValue(strFoldingModel, "{P5.Z}", 0f);
                    strFoldingModel = ReplaceValue(strFoldingModel, "{P5.Y}", -20f);

                }

                strFile.AppendLine();
                strFile.Append(strFoldingModel);
            }

            int indexStart3 = strTemplate.IndexOf("{SCENE}");
            int indexEnd3 = strTemplate.IndexOf("{/SCENE}");

            strSubTemplate = strTemplate.Substring(indexStart3 + 7, indexEnd3 - indexStart3 - 7);

            strFile.Append(strTemplate.Substring(indexEnd + 8, indexStart3 - indexEnd - 8));

            for (int i = 0; i < listFoldings.Count + 1; i++)
            {
                string strFoldingModel = strSubTemplate;

                //strFoldingModel = strFoldingModel.Replace("{ARMATURE_NAME}", "Armature_" + nbFolding.ToString());
                strFoldingModel = strFoldingModel.Replace("{FOLDING_NAME}", "Folding_" + i.ToString());
                //strFoldingModel = strFoldingModel.Replace("{BONE1_NAME}", "Bone_" + (nbFolding * 2).ToString());
                //strFoldingModel = strFoldingModel.Replace("{BONE2_NAME}", "Bone_" + (nbFolding * 2 + 1).ToString());

                strFile.AppendLine();
                strFile.Append(strFoldingModel);
            }


            strFile.Append(strTemplate.Substring(indexEnd3 + 8));

            File.WriteAllText(fileName, strFile.ToString());
        }

        private static int AddVertexToList(ref List<Vertex> listVertex, Vertex vertex)
        {
            int index = listVertex.FindIndex(v=> v.X == vertex.X && v.Y == vertex.Y && v.Z == vertex.Z);

            if(index == -1)
            {
                listVertex.Add(vertex);
                index = listVertex.Count-1;
            }

            return index;
        }

        private static void ComputeVertexCuttingFace(Cutting cutting, ref List<Vertex> listVertex, ref List<int> listVertexIndex)
        {
            if (cutting.Cuttings.Count == 0 && !cutting.IsEmpty)
            {

                float d = 40f;
                float height = cutting.ParentFolding.Height /d;
                //---
                Vertex vertex = new Vertex();

                vertex.X = cutting.Rectangle.Right / d;
                vertex.Z = -cutting.Rectangle.Top / d + height;
                vertex.Y = -height;

                int index = AddVertexToList(ref listVertex, vertex);

                listVertexIndex.Add(index);
                //---

                //---
                vertex = new Vertex();

                vertex.X = cutting.Rectangle.Left / d;
                vertex.Z = -cutting.Rectangle.Top / d + height;
                vertex.Y = -height;

                index = AddVertexToList(ref listVertex, vertex);

                listVertexIndex.Add(index);
                //---

                //---
                vertex = new Vertex();

                vertex.X = cutting.Rectangle.Left / d;
                vertex.Z = -cutting.Rectangle.Bottom / d + height;
                vertex.Y = -height;

                index = AddVertexToList(ref listVertex, vertex);

                listVertexIndex.Add(index);
                //---

                //---
                vertex = new Vertex();

                vertex.X = cutting.Rectangle.Right / d;
                vertex.Z = -cutting.Rectangle.Bottom / d + height;
                vertex.Y = -height;

                index = AddVertexToList(ref listVertex, vertex);

                listVertexIndex.Add(index);
                //---
            }
            else
            {
                foreach (Cutting cuttingChild in cutting.Cuttings)
                {
                    ComputeVertexCuttingFace(cuttingChild, ref listVertex, ref listVertexIndex);
                }
            }
        }

        private static void ComputeVertexCuttingTop(Cutting cutting, ref List<Vertex> listVertex, ref List<int> listVertexIndex)
        {
            if (cutting.Cuttings.Count == 0 && !cutting.IsEmpty)
            {

                float d = 40f;
                float deep = (cutting.ParentFolding.RecFace.Top - cutting.ParentFolding.Height) / d - 12f;
                float height = -cutting.ParentFolding.RecFace.Top / d;// -cutting.ParentFolding.Height / d;

                //--- 1
                Vertex vertex = new Vertex();

                vertex.X = cutting.Rectangle.Right / d;
                vertex.Z = -deep;
                vertex.Y = -cutting.Rectangle.Top / d-deep;

                int index = AddVertexToList(ref listVertex, vertex);

                listVertexIndex.Add(index);
                //---

                //--- 2
                vertex = new Vertex();

                vertex.X = cutting.Rectangle.Left / d;
                vertex.Z = -deep;
                vertex.Y = -cutting.Rectangle.Top / d - deep;

                index = AddVertexToList(ref listVertex, vertex);

                listVertexIndex.Add(index);
                //---

                //--- 3
                vertex = new Vertex();

                vertex.X = cutting.Rectangle.Left / d;
                vertex.Z = -deep;
                vertex.Y = -cutting.Rectangle.Bottom / d - deep;

                index = AddVertexToList(ref listVertex, vertex);

                listVertexIndex.Add(index);
                //---

                //--- 4
                vertex = new Vertex();

                vertex.X = cutting.Rectangle.Right / d;
                vertex.Z = -deep;
                vertex.Y = -cutting.Rectangle.Bottom / d - deep;

                index = AddVertexToList(ref listVertex, vertex);

                listVertexIndex.Add(index);
                //---
            }
            else
            {
                foreach (Cutting cuttingChild in cutting.Cuttings)
                {
                    ComputeVertexCuttingTop(cuttingChild, ref listVertex, ref listVertexIndex);
                }
            }
        }

        public static void ExportToCOLLADA2(Scene scene, string fileName)
        {
            String strTemplate = Resources.COLLADA_Exporter_Template_Papier2;

            strTemplate = strTemplate.Replace("{DATETIME}", DateTime.Now.ToString());

            List<Folding> listFoldings = GetFoldings(scene);

            int indexStart = strTemplate.IndexOf("{MODEL}");
            int indexEnd = strTemplate.IndexOf("{/MODEL}");

            string strSubTemplate = strTemplate.Substring(indexStart + 7, indexEnd - indexStart - 7);
            StringBuilder strFile = new StringBuilder();

            strFile.Append(strTemplate.Substring(0, indexStart));

            float d = 40f;
            float dh = d;

            for (int i = 0; i < listFoldings.Count + 1; i++)
            {
                string strFoldingModel = strSubTemplate;

                if (i < listFoldings.Count)
                {
                    Folding folding = listFoldings[i];

                    strFoldingModel = strFoldingModel.Replace("{ARMATURE_NAME}", "Armature_" + i.ToString());
                    strFoldingModel = strFoldingModel.Replace("{FOLDING_NAME}", "Folding_" + i.ToString());

                    //----
                    List<Vertex> listVertex = new List<Vertex>();
                    List<int> listVertexIndex = new List<int>();

                    ComputeVertexCuttingFace(folding.CuttingFace, ref listVertex, ref listVertexIndex);
                    int t = listVertexIndex.Count;
                    ComputeVertexCuttingTop(folding.CuttingTop, ref listVertex, ref listVertexIndex);

                    string FOLDING_FACE_CONFIGURATION = String.Empty;
                    string FOLDING_VERTEX_NORMAL_INDEX = String.Empty;
                    string FOLDING_VERTEX = String.Empty;

                    for (int j = 0; j < listVertex.Count; j++)
                    {
                        Vertex vertex = listVertex[j];
                        FOLDING_VERTEX += vertex.ToString() + " ";
                    }

                    for (int j = 0; j < listVertexIndex.Count; j++)
                    {
                        if(j%4==0)
                            FOLDING_FACE_CONFIGURATION += "4 ";

                        FOLDING_VERTEX_NORMAL_INDEX += listVertexIndex[j] + (j<t ?" 0 ": " 1 ");
                    }
                    //-----

                    strFoldingModel = strFoldingModel.Replace("{FOLDING_VERTEX_COUNT}", (listVertex.Count*3).ToString());
                    strFoldingModel = strFoldingModel.Replace("{FOLDING_FACE_COUNT}", (listVertexIndex.Count / 4).ToString());
                    strFoldingModel = strFoldingModel.Replace("{FOLDING_VERTEX}", FOLDING_VERTEX);
                    strFoldingModel = strFoldingModel.Replace("{FOLDING_FACE_CONFIGURATION}", FOLDING_FACE_CONFIGURATION);
                    strFoldingModel = strFoldingModel.Replace("{FOLDING_VERTEX_NORMAL_INDEX}", FOLDING_VERTEX_NORMAL_INDEX);
                }
                else
                {
                    //---> Construction de la scène
                    strFoldingModel = strFoldingModel.Replace("{ARMATURE_NAME}", "Armature_" + i.ToString());
                    strFoldingModel = strFoldingModel.Replace("{FOLDING_NAME}", "Folding_" + i.ToString());
                    string t = "{P2.X} {P2.Y} {P2.Z} {P3.X} {P3.Y} {P3.Z} {P5.X} {P5.Y} {P5.Z} {P1.X} {P1.Y} {P1.Z} {P0.X} {P0.Y} {P0.Z} {P4.X} {P4.Y} {P4.Z}";


                    t = ReplaceValue(t, "{P1.X}", 0); //0
                    t = ReplaceValue(t, "{P1.Z}", 0f);
                    t = ReplaceValue(t, "{P1.Y}", 0f);

                    t = ReplaceValue(t, "{P0.X}", 20f); //1
                    t = ReplaceValue(t, "{P0.Z}", 0f);
                    t = ReplaceValue(t, "{P0.Y}", 0f);


                    t = ReplaceValue(t, "{P2.X}", 0f); //2
                    t = ReplaceValue(t, "{P2.Z}", 20f);
                    t = ReplaceValue(t, "{P2.Y}", 0f);

                    t = ReplaceValue(t, "{P3.X}", 20f); //3
                    t = ReplaceValue(t, "{P3.Z}", 20f);
                    t = ReplaceValue(t, "{P3.Y}", 0f);


                    t = ReplaceValue(t, "{P4.X}", 20f);
                    t = ReplaceValue(t, "{P4.Z}", 0f);
                    t = ReplaceValue(t, "{P4.Y}", -20f);

                    t = ReplaceValue(t, "{P5.X}", 0f);
                    t = ReplaceValue(t, "{P5.Z}", 0f);
                    t = ReplaceValue(t, "{P5.Y}", -20f);

                    strFoldingModel = strFoldingModel.Replace("{FOLDING_VERTEX_COUNT}", "18");
                    strFoldingModel = strFoldingModel.Replace("{FOLDING_FACE_COUNT}", "2");
                    strFoldingModel = strFoldingModel.Replace("{FOLDING_VERTEX}", t);
                    strFoldingModel = strFoldingModel.Replace("{FOLDING_FACE_CONFIGURATION}", "4 4 ");
                    strFoldingModel = strFoldingModel.Replace("{FOLDING_VERTEX_NORMAL_INDEX}", "2 0 5 0 4 0 3 0 0 1 3 1 4 1 1 1");

                }

                strFile.AppendLine();
                strFile.Append(strFoldingModel);
            }

            int indexStart3 = strTemplate.IndexOf("{SCENE}");
            int indexEnd3 = strTemplate.IndexOf("{/SCENE}");

            strSubTemplate = strTemplate.Substring(indexStart3 + 7, indexEnd3 - indexStart3 - 7);

            strFile.Append(strTemplate.Substring(indexEnd + 8, indexStart3 - indexEnd - 8));

            for (int i = 0; i < listFoldings.Count + 1; i++)
            {
                string strFoldingModel = strSubTemplate;

                //strFoldingModel = strFoldingModel.Replace("{ARMATURE_NAME}", "Armature_" + nbFolding.ToString());
                strFoldingModel = strFoldingModel.Replace("{FOLDING_NAME}", "Folding_" + i.ToString());
                //strFoldingModel = strFoldingModel.Replace("{BONE1_NAME}", "Bone_" + (nbFolding * 2).ToString());
                //strFoldingModel = strFoldingModel.Replace("{BONE2_NAME}", "Bone_" + (nbFolding * 2 + 1).ToString());

                strFile.AppendLine();
                strFile.Append(strFoldingModel);
            }


            strFile.Append(strTemplate.Substring(indexEnd3 + 8));

            File.WriteAllText(fileName, strFile.ToString());
        }
    }
}
