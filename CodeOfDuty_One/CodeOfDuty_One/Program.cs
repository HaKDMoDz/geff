using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading.Tasks;

namespace CodeOfDuty_One
{
    class Program
    {
        public static List<Vecteur> Vecteurs { get; set; }

        static void Main(string[] args)
        {
            Start();
        }


        /// <summary>
        /// Méthode de démarrage du CodeOfDuty One
        /// </summary>
        private static void Start()
        {
            //---> Charge les vecteurs en mémoire
            ReadInputFile();

            //---> Calcul les itérations
            Compute();

            //---> Ecrit le résultat des itérations dans le fichier output.txt
            WriteOutputFile();
        }

        /// <summary>
        /// Lecture du fichier input.txt et stockage des vecteur dans une structure en mémoire
        /// </summary>
        private static void ReadInputFile()
        {
            Vecteurs = new List<Vecteur>();

            string fileName = Path.Combine(Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location), "input.txt");

            string[] fileContent = File.ReadAllLines(fileName);

            for (int i = 0; i < fileContent.Length - 1; i += 3)
            {
                Vecteur vecteur = new Vecteur();

                vecteur.Iterations = new List<Iteration>();

                Iteration iterationInitiale = new Iteration();
                iterationInitiale.Elements = new byte[int.Parse(fileContent[i])];
                vecteur.Iterations.Add(iterationInitiale);

                string[] elements = fileContent[i + 1].Split(new String[] { " " }, StringSplitOptions.RemoveEmptyEntries);

                int sommeElements = 0;
                for (int j = 0; j < elements.Length; j++)
                {
                    iterationInitiale.Elements[j] = byte.Parse(elements[j]);

                    //---> Somme des valeurs pour le calcul de la moyenne à atteindre
                    //     Cette somme est calculée ici afin de ne pas boucler de nouveau plus tard
                    sommeElements += iterationInitiale.Elements[j];
                }

                //--- Calcul de la moyenne des éléments
                //    Si la moyenne ne peut être atteindre pour tous les éléments
                //    alors la propriété moyenne est fixée à 255 pour l'exclusion du calcul (la moyenne max théorique étant 99)
                if (sommeElements % iterationInitiale.Elements.Length != 0)
                    vecteur.Moyenne = 255;
                else
                    vecteur.Moyenne = (byte)(sommeElements / iterationInitiale.Elements.Length);
                //---

                Vecteurs.Add(vecteur);
            }
        }

        /// <summary>
        /// Calcule et stockage des itérations
        /// </summary>
        private static void Compute()
        {
            //--- Parallélisation du calcul
            Parallel.ForEach<Vecteur>(Vecteurs, vecteur =>
            //for (int m = 0; m < Vecteurs.Count; m++)
            {
                //Vecteur vecteur = Vecteurs[m];

                byte nbElement = (byte)vecteur.Iterations[0].Elements.Length;

                //--- Exclusion du calcul si la moyenne déterminée est 255
                if (vecteur.Moyenne < 255 && nbElement > 1)
                {
                    bool ecart = true;
                    int i = 0;
                    short[] ecartMoyenneElements = new short[nbElement];

                    //--- Evaluation de la distance à la moyenne pour chaque élément
                    for (byte j = 0; j < nbElement; j++)
                    {
                        ecartMoyenneElements[j] = (Int16)(vecteur.Moyenne - vecteur.Iterations[0].Elements[j]);
                    }

                    //--- Tant que l'écart maximum pour un élement est différent de zéro, on fait une nouvelle itération
                    while (ecart)
                    {
                        Iteration nouvelleIteration = new Iteration();
                        nouvelleIteration.Elements = new byte[nbElement];
                        vecteur.Iterations.Add(nouvelleIteration);

                        //---> le tableau membre[] contient la somme des écarts à la moyenne à gauche et à droite 
                        //      de l'élément courant et du voisin de gauche de l'élément courant
                        short[,] membres = new short[2, 2];

                        //---> Optimisation : Seule le premier élémént calcul la somme de tous les éléments à sa droite
                        //     les autres éléments s'appuiront toujours sur les membres de leur voisin de gauche
                        membres[1, 0] = (short)(ecartMoyenneElements.Sum(ecartMoyenne => (short)ecartMoyenne) - ecartMoyenneElements[0]);
                        membres[1, 1] = membres[1, 0];

                        for (byte j = 0; j < nbElement; j++)
                        {
                            //---> Calcul la valeur du membre gauche de l'élément selon son voisin de gauche
                            if (j > 0)
                                membres[0, 1] = (short)(membres[0, 0] + ecartMoyenneElements[j - 1]);

                            //---> Calcul la valeur du membre droite selon son voisin de gauche
                            if (j > 0 && j < nbElement - 1)
                                membres[1, 1] = (short)(membres[1, 0] - ecartMoyenneElements[j]);
                            else if (j == nbElement - 1)
                                membres[1, 1] = 0;

                            //---> Si le membre gauche ou le membre de droite a besoin de valeurs
                            //     et que l'élément courant peut donner un de ses éléments
                            if ((membres[0, 1] > 0 || membres[1, 1] > 0) && vecteur.Iterations[i].Elements[j] > 0)
                            {
                                //---> Détermine si la distribution doit se faire à gauche ou à droite
                                int indexDistribution = membres[0, 1] >= membres[1, 1] ? -1 : 1;

                                //---> Décrémente la valeur de l'élément courant pour l'itération actuelle
                                nouvelleIteration.Elements[j] += (byte)(vecteur.Iterations[i].Elements[j] - 1);
                                //---> L'écart à la moyenne pour l'élément courant est incrémenté
                                ecartMoyenneElements[j] += 1;
                                //---> Incrémente la valeur de l'élément à gauche ou à droite de l'élément courant
                                nouvelleIteration.Elements[j + indexDistribution] += 1;
                                //---> L'écart à la moyenne pour l'élément à gauche ou à droite de l'élément courant est décrémente
                                ecartMoyenneElements[j + indexDistribution] -= 1;

                                //---> Décrémente le membre gauche ou droite de l'élément courant
                                if (indexDistribution == -1)
                                    membres[0, 1] -= 1;
                                if (indexDistribution == 1)
                                    membres[1, 1] -= 1;
                            }
                            else
                            {
                                nouvelleIteration.Elements[j] += vecteur.Iterations[i].Elements[j];
                            }

                            membres[0, 0] = membres[0, 1];
                            membres[1, 0] = membres[1, 1];
                        }

                        ecart = ecartMoyenneElements.Sum(ecartMoyenne => (short)Math.Abs(ecartMoyenne)) > 0;

                        i++;
                    }
                }
            });
        }

        /// <summary>
        /// Ecriture des itérations dans le fichier output.txt
        /// </summary>
        private static void WriteOutputFile()
        {
            StringBuilder stringBuilder = new StringBuilder();
            for (int i = 0; i < Vecteurs.Count; i++)
            {
                Vecteur vecteur = Vecteurs[i];

                if (vecteur.Moyenne == 255)
                {
                    stringBuilder.AppendLine("-1");
                }
                else
                {
                    byte nbElement = (byte)vecteur.Iterations[0].Elements.Length;
                    stringBuilder.Append(vecteur.Iterations.Count - 1);

                    for (int j = 0; j < vecteur.Iterations.Count; j++)
                    {
                        Iteration iteration = vecteur.Iterations[j];

                        stringBuilder.AppendLine();
                        stringBuilder.Append(j);
                        stringBuilder.Append(" : (");

                        for (int k = 0; k < nbElement; k++)
                        {
                            stringBuilder.Append(iteration.Elements[k]);
                            if (k < nbElement - 1)
                                stringBuilder.Append(", ");
                        }

                        stringBuilder.Append(")");
                    }
                    stringBuilder.AppendLine();

                }

                stringBuilder.AppendLine();
            }

            File.WriteAllText(Path.Combine(Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location), "output.txt"), stringBuilder.ToString());
        }
    }

    public struct Vecteur
    {
        public byte Moyenne { get; set; }
        public List<Iteration> Iterations { get; set; }
    }

    public struct Iteration
    {
        public byte[] Elements { get; set; }
    }
}
