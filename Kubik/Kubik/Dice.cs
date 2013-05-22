using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kubik
{
    public class Dice
    {
        public Face[] Faces { get; set; }
        public List<Dice> Dices = new List<Dice>();
        public int[] TabSens = new int[66];
        public int Sens { get; set; }

        int[,] tabArrow = new int[,] { { 0, 1, 4, 2, 3, 5 }, { 0, 3, 4, 2, 1, 5 }, {0, 2, 1, 3, 4, 5 }};


        public override string ToString()
        {
            string t = String.Empty;

            //for (int i = 0; i < 3; i++)
            //{
            //    for (int j = 0; j < 2; j++)
            //    {
            //        t += this.Faces[i].Values[j] + " ";
            //    }
            //}

            for (int i = 0; i < 6; i++)
            {
                int v1 = this.Faces[tabArrow[2, i] / 2].Values[tabArrow[2, i] % 2];

                t += v1 + " ";
            }

            t += " " + Sens;
            t += " => ";

            for (int i = 0; i < 6; i++)
            {
                int v1 = this.Faces[tabArrow[Sens, i] / 2].Values[tabArrow[Sens, i] % 2];

                int v2 = this.Faces[tabArrow[Sens, ((i+1)%6)] /2].Values[tabArrow[Sens, ((i+1)%6)] % 2];

                int key = v1 * 10 + v2;

                TabSens[key]++;

                t += key + " ";
            }

            t += " // ";

            return t;
        }

        public int GetColorIndex(int index, int side)
        {
            int[, ,] t = new int[2, 6, 5];

            //=== Sens 0
            //- Première case
            t[0, 0, 0] = this.Faces[0].Values[0];
            //--> up
            t[0, 0, 1] = this.Faces[2].Values[0];
            //--> right
            t[0, 0, 2] = -1;
            //--> bottom
            t[0, 0, 3] = this.Faces[2].Values[1];
            //--> left
            t[0, 0, 4] = this.Faces[1].Values[1];


            //- Deuxième case
            t[0, 1, 0] = this.Faces[1].Values[0];
            //--> up
            t[0, 1, 1] = -1;
            //--> right
            t[0, 1, 2] = this.Faces[0].Values[1];
            //--> bottom
            t[0, 1, 3] = this.Faces[2].Values[1];
            //--> left
            t[0, 1, 4] = this.Faces[0].Values[0];


            //- Troisième case
            t[0, 2, 0] = this.Faces[0].Values[1];
            //--> up
            t[0, 2, 1] = this.Faces[2].Values[0];
            //--> right
            t[0, 2, 2] = -1;
            //--> bottom
            t[0, 2, 3] = this.Faces[2].Values[1];
            //--> left
            t[0, 2, 4] = this.Faces[1].Values[0];


            //- Quatrième case
            t[0, 3, 0] = this.Faces[1].Values[1];
            //--> up
            t[0, 3, 1] = this.Faces[2].Values[0];
            //--> right
            t[0, 3, 2] = this.Faces[0].Values[0];
            //--> bottom
            t[0, 3, 3] = -1;
            //--> left
            t[0, 3, 4] = this.Faces[0].Values[1];


            //- Cinquième case
            t[0, 4, 0] = this.Faces[2].Values[0];
            //--> up
            t[0, 4, 1] = this.Faces[0].Values[0];
            //--> right
            t[0, 4, 2] = this.Faces[1].Values[1];
            //--> bottom
            t[0, 4, 3] = -1;
            //--> left
            t[0, 4, 4] = this.Faces[1].Values[0];


            //- Sixième case
            t[0, 5, 0] = this.Faces[2].Values[1];
            //--> up
            t[0, 5, 1] = this.Faces[0].Values[1];
            //--> right
            t[0, 5, 2] = this.Faces[1].Values[1];
            //--> bottom
            t[0, 5, 3] = -1;
            //--> left
            t[0, 5, 4] = this.Faces[1].Values[0];




            //=== Sens 1
            //- Première case
            t[1, 0, 0] = this.Faces[0].Values[0];
            //--> up
            t[1, 0, 1] = this.Faces[2].Values[0];
            //--> right
            t[1, 0, 2] = this.Faces[1].Values[0];
            //--> bottom
            t[1, 0, 3] = this.Faces[2].Values[1];
            //--> left
            t[1, 0, 4] = -1;


            //- Deuxième case
            t[1, 1, 0] = this.Faces[1].Values[0];
            //--> up
            t[1, 1, 1] = this.Faces[2].Values[0];
            //--> right
            t[1, 1, 2] = this.Faces[0].Values[1];
            //--> bottom
            t[1, 1, 3] = -1;
            //--> left
            t[1, 1, 4] = this.Faces[0].Values[0];


            //- Troisième case
            t[1, 2, 0] = this.Faces[0].Values[1];
            //--> up
            t[1, 2, 1] = this.Faces[2].Values[0];
            //--> right
            t[1, 2, 2] = this.Faces[1].Values[1];
            //--> bottom
            t[1, 2, 3] = this.Faces[2].Values[1];
            //--> left
            t[1, 2, 4] = -1;


            //- Quatrième case
            t[1, 3, 0] = this.Faces[1].Values[1];
            //--> up
            t[1, 3, 1] = -1;
            //--> right
            t[1, 3, 2] = this.Faces[0].Values[0];
            //--> bottom
            t[1, 3, 3] = this.Faces[2].Values[1];
            //--> left
            t[1, 3, 4] = this.Faces[0].Values[1];


            //- Cinquième case
            t[1, 4, 0] = this.Faces[2].Values[0];
            //--> up
            t[1, 4, 1] = this.Faces[0].Values[0];
            //--> right
            t[1, 4, 2] = this.Faces[1].Values[1];
            //--> bottom
            t[1, 4, 3] = -1;
            //--> left
            t[1, 4, 4] = this.Faces[1].Values[0];


            //- Sixième case
            t[1, 5, 0] = this.Faces[2].Values[1];
            //--> up
            t[1, 5, 1] = this.Faces[0].Values[1];
            //--> right
            t[1, 5, 2] = this.Faces[1].Values[1];
            //--> bottom
            t[1, 5, 3] = -1;
            //--> left
            t[1, 5, 4] = this.Faces[1].Values[0];

            return t[Sens, index, side];
        }

        public Dice()
        {
            this.Faces = new Face[3];
        }

        public Dice(int[] values)
        {
            this.Faces = new Face[3];

            this.Faces[0] = new Face(values[0], values[2]);

            this.Faces[1] = new Face(values[1], values[3]);

            this.Faces[2] = new Face(values[4], values[5]);

            GetAllDices();
        }

        public void GetAllDices()
        {
            Dices = new List<Dice>();

            for (int s = 0; s < 2; s++)
            {


                for (int i = 0; i < 2; i++)
                {
                    for (int j = 0; j < 2; j++)
                    {
                        for (int h = 0; h < 2; h++)
                        {
                            Dice dice = InvertFaces(this.Faces[0], this.Faces[1], this.Faces[2], i > 0, j > 0, h > 0);

                            dice.Sens = s;
                            string t = dice.ToString();
                            Dices.Add(dice);
                        }
                    }
                }
            }

        }

        public Dice InvertFaces(Face face1, Face face2, Face face3, bool invert1, bool invert2, bool invert3)
        {
            Dice dice = new Dice();

            Face newFace1 = face1;
            Face newFace2 = face2;
            Face newFace3 = face3;

            if (invert1)
            {
                newFace1 = new Face(face1.Values[1], face1.Values[0]);
            }

            if (invert2)
            {
                newFace2 = new Face(face2.Values[1], face2.Values[0]);
            }

            if (invert3)
            {
                newFace3 = new Face(face3.Values[1], face3.Values[0]);
            }

            dice.Faces[0] = newFace1;
            dice.Faces[1] = newFace2;
            dice.Faces[2] = newFace3;

            return dice;
        }
    }
}
