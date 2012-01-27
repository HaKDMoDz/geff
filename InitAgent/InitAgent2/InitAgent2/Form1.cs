using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace WindowsFormsApplication1
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private StringBuilder str = new StringBuilder();
        private List<int[]> liste = new List<int[]>();

        private void button1_Click(object sender, EventArgs e)
        {
            Calculer(2);
        }

        private void Calculer(int taille)
        {
            str = new StringBuilder();
            liste = new List<int[]>();
            int[] ligne = new int[taille];
            textBox1.Clear();

            Remplir(ligne, 0);

            textBox1.Text = str.ToString();
        }

        private void Remplir(int[] ligne, int iteration)
        {
            bool stop = false;
            int maxValue = 3;
            int iterationParent = iteration;

            for (int i = 0; i < ligne.Length && !stop; i++)
            {
                for (int j = 1; j < maxValue; j++)
                {
                    if (ligne[i] == 0)
                    {
                        stop = true;
                        iteration++;

                        str.AppendLine();
                        str.AppendFormat("Itération Parent / Enfant => {0} / {1}", iterationParent, iteration);
                        str.AppendLine();

                        int[] ligneParent = new int[ligne.Length];

                        for (int k = 0; k < ligne.Length; k++)
                        {
                            ligneParent[k] = ligne[k];
                            str.AppendFormat(" : {0}", ligne[k]);
                        }

                        ligneParent[i] = j;

                        str.Append("  =>  ");
                        for (int k = 0; k < ligne.Length; k++)
                        {
                            str.AppendFormat(" : {0}", ligneParent[k]);
                        }

                        Remplir(ligneParent, iteration);
                    }
                    else if (i == ligne.Length - 1 && j == maxValue-2)
                    {
                        str.AppendLine();
                        str.AppendLine("________________________________");

                        liste.Add(ligne);
                    }
                }
            }
        }
    }
}
