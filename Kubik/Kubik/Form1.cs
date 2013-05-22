using Kubik.Properties;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Kubik
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        int[,] tabArrow = new int[,] { { 0, 1, 4, 2, 3, 5 }, { 0, 3, 4, 2, 1, 5 } };

        List<int[]> linesInt = new List<int[]>();

        private void btnGo_Click(object sender, EventArgs e)
        {
            string txt = txtSrc.Text;
            string txt2 = String.Empty;

            string[] lines = txt.Split(new string[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);


            linesInt = new List<int[]>();

            int[] tabSens = new int[100];

            foreach (string line in lines)
            {
                string[] values = line.Split(new string[] { " " }, StringSplitOptions.RemoveEmptyEntries);

                int[] lineInt = new int[7];

                int im = 0;
                foreach (string value in values)
                {
                    int valueInt = int.Parse(value);

                    lineInt[im] = valueInt;
                    im++;
                }

                linesInt.Add(lineInt);

                for (int i = 0; i < 3; i++)
                {
                    int h = i;

                    if (i == 2)
                        h = 4;

                    int v1 = int.Parse(values[h]);

                    int j = h + 2;

                    if (i == 2)
                        j = 5;

                    int v2 = int.Parse(values[j]);

                    txt2 += (Math.Pow(2, v1 - 1) + Math.Pow(2, v2 - 1)).ToString() + " ";
                }

                txt2 += Environment.NewLine;


                for (int i = 0; i < 6; i++)
                {
                    int sens = lineInt[6];

                    int v1 = int.Parse(values[tabArrow[sens, i]]);

                    int v2 = int.Parse(values[tabArrow[sens, (i + 1) % 6]]);

                    txt2 += v1.ToString() + v2.ToString() + " ";

                    tabSens[int.Parse(v1.ToString() + v2.ToString())]++;
                }

                txt2 += Environment.NewLine;
                txt2 += Environment.NewLine;

            }

            string txt3 = String.Empty;

            for (int i = 1; i < 7; i++)
            {
                for (int j = 1; j < 7; j++)
                {
                    if (i != j)
                    {
                        int t = int.Parse(i.ToString() + j.ToString());

                        txt3 += t.ToString() + " : " + tabSens[t].ToString() + Environment.NewLine;
                    }
                }
            }

            txtDst.Text = txt2;

            txtSens.Text = txt3;

            Calc();

            DrawDice();
        }

        private void DrawDice()
        {
            List<Color> listColor = new List<Color>();

            listColor.Add(Color.Yellow);
            listColor.Add(Color.Lime);
            listColor.Add(Color.DodgerBlue);
            listColor.Add(Color.BlueViolet);
            listColor.Add(Color.Red);
            listColor.Add(Color.Orange);

            Image img = new Bitmap(pic.Width, 84 + 15 * 212);

            Graphics g = Graphics.FromImage(img);
            g.Clear(Color.Black);

            int j = 0;
            foreach (Dice dice in dices)
            {
                for (int i = 0; i < 6; i++)
                {
                    for (int h = 0; h < 5; h++)
                    {
                        int indexColor = dice.GetColorIndex(i, h);
                        if (indexColor > -1)
                        {
                            Color color = listColor[indexColor - 1];

                            float[][] colorMatrixElements = new float[][] { 
                           new float[] {(float)color.R/255f,  0,  0,  0, 0},
                           new float[] {0,  (float)color.G/255f,  0,  0, 0},
                           new float[] {0,  0,  (float)color.B/255f,  0, 0},
                           new float[] {0,  0,  0,  1f, 0},
                           new float[] {0,  0,  0,  0f, 0},};

                            ColorMatrix colorMatrix = new ColorMatrix(colorMatrixElements);

                            ImageAttributes imageAttributes = new ImageAttributes();
                            imageAttributes.SetColorMatrix(
                               colorMatrix,
                               ColorMatrixFlag.Default,
                               ColorAdjustType.Bitmap);

                            //Rectangle rec = new Rectangle(20 + i * 64, 84 + j * 192, 64, 64);
                            Rectangle rec = new Rectangle(20 + i * 64, 84 + j * 212, 64, 64);

                            Bitmap bmp = null;

                            if (h == 0)
                                bmp = Resources.middle;
                            else if (h == 1)
                                bmp = Resources.up;
                            else if (h == 2)
                                bmp = Resources.right;
                            else if (h == 3)
                                bmp = Resources.bottom;
                            else if (h == 4)
                                bmp = Resources.left;

                            if (i == 4 || i == 5)
                            {
                                rec.X = 20 + 2 * 64;
                            }

                            if (i == 4)
                            {
                                rec.Y -= 64;
                            }

                            if (i == 5)
                            {
                                rec.Y += 64;
                            }

                            g.DrawImage(bmp, rec, 0f, 0f, rec.Width, rec.Height, GraphicsUnit.Pixel, imageAttributes);
                        }
                    }
                }

                j++;
            }

            pic.Height = img.Height;

            pic.Image = img;
        }

        int cmp = 0;
        int nbGood = 0;

        List<Dice> dices = new List<Dice>();

        private void CalcDice(int index, int[] tabSens, string diceScheme)
        {
            cmp++;

            if (index < dices.Count)
            {


                for (int j = 0; j < dices[index].Dices.Count; j++)
                {
                    int[] newTabSens = new int[66];

                    for (int i = 0; i < 65; i++)
                    {
                        newTabSens[i] = tabSens[i];
                    }


                    bool isGood = true;

                    for (int h = 0; h < dices[index].Dices[j].TabSens.Length; h++)
                    {
                        newTabSens[h] += dices[index].Dices[j].TabSens[h];

                        if (newTabSens[h] > 3)
                        {
                            isGood = false;
                        }
                    }

                    if (isGood)
                    {
                        string newDiceScheme = (string)diceScheme.Clone();

                        newDiceScheme += "d" + index + " " + j + " ";

                        CalcDice(index + 1, newTabSens, newDiceScheme);
                    }


                }

            }
            else
            {
                bool isGood2 = true;

                for (int h = 0; h < tabSens.Length; h++)
                {
                    if (tabSens[h] > 3)
                        isGood2 = false;
                }

                if (isGood2)
                {
                    nbGood++;

                    string tt = String.Empty;

                    string[] g = diceScheme.Split(new String[] { "d" }, StringSplitOptions.RemoveEmptyEntries);

                    foreach (string st in g)
                    {
                        if (st != " ")
                        {
                            string[] st2 = st.Split(new String[] { " " }, StringSplitOptions.RemoveEmptyEntries);

                            int d1 = int.Parse(st2[0]);
                            int d2 = int.Parse(st2[1]);

                            tt += dices[d1].Dices[d2];
                            tt += Environment.NewLine;
                        }
                    }


                }
            }
        }

        private void Calc()
        {
            int[] tabSens = new int[100];
            string txt = String.Empty;


            int[,] inversion = new int[,] { { 0, 2 }, { 1, 3 }, { 4, 5 } };

            dices = new List<Dice>();

            ///-------
            foreach (int[] line in linesInt)
            {
                Dice dice = new Dice(line);
                dice.Sens = line[6];

                dices.Add(dice);
            }

            cmp = 0;
            nbGood = 0;
            //CalcDice(0, tabSens, " ");

            int a = 0;

            //----

            //int h = 0;
            //foreach (int[] line in linesInt)
            //{
            //    bool goodTabArrow1 = true;
            //    bool goodTabArrow2 = true;

            //    int[] offset = new int[6];
            //    string error = String.Empty;

            //    int l = h % 2;
            //    h++;

            //    for (int i = 0; i < 6; i++)
            //    {
            //        int v1 = line[tabArrow[l, i]];
            //        int v2 = line[tabArrow[l, (i + 1) % 6]];

            //        int key = int.Parse(v1.ToString() + v2.ToString());


            //        if (tabSens[key] + 1 < 4)
            //        {
            //            offset[i] = key;
            //        }
            //        else
            //        {
            //            error += " " + key;
            //            goodTabArrow1 = false;
            //        }
            //    }

            //    if (!goodTabArrow1)
            //    {
            //        h++;
            //        l = h % 2;

            //        error += " || ";

            //        for (int i = 0; i < 6; i++)
            //        {
            //            int v1 = line[tabArrow[l, i]];
            //            int v2 = line[tabArrow[l, (i + 1) % 6]];

            //            int key = int.Parse(v1.ToString() + v2.ToString());

            //            if (tabSens[key] + 1 < 4)
            //            {
            //                offset[i] = key;
            //            }
            //            else
            //            {
            //                error += " " + key;

            //                goodTabArrow2 = false;
            //            }
            //        }
            //    }


            //    if (!goodTabArrow2)
            //    {
            //        int[] tab = new int[6];
            //        tab[0] = line[2];
            //        tab[1] = line[1];
            //        tab[2] = line[0];
            //        tab[3] = line[3];
            //        tab[4] = line[4];
            //        tab[5] = line[5];
            //    }


            //    if (goodTabArrow1 || goodTabArrow2)
            //    {
            //        error += "// ";
            //        for (int i = 0; i < 6; i++)
            //        {
            //            error += " " + offset[i];
            //            tabSens[offset[i]]++;
            //        }
            //    }

            //    if (goodTabArrow1)
            //    {
            //        txt += l + 1 + error;
            //    }
            //    else if (goodTabArrow2)
            //    {
            //        txt += l + 1 + error;
            //    }
            //    else
            //    {
            //        txt += "0" + error;
            //    }

            //txt += Environment.NewLine;

            //}

            //string txt3 = String.Empty;

            //for (int i = 1; i < 7; i++)
            //{
            //    for (int j = 1; j < 7; j++)
            //    {
            //        if (i != j)
            //        {
            //            int t = int.Parse(i.ToString() + j.ToString());

            //            txt3 += t.ToString() + " : " + tabSens[t].ToString() + Environment.NewLine;
            //        }
            //    }
            //}

            //txtSens.Text = txt3;

            //txtDst.Text = txt;
        }

        private void txtSrc_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
