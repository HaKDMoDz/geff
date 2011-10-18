using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace InitAgent
{
    public partial class Form1 : Form
    {
        List<Jour> listJour = new List<Jour>();

        int RP = 0;
        int RPr = 0;
        int RPs = 0;
        int RPt = 0;
        int MA = 0;
        int BA = 0;
        int RH = 0;
        int VT = 0;
        int VC = 0;
        int Di = 0;
        int CA = 0;
        int RM = 0;
        int RU = 0;
        int We = 0;

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            calendar.SelectionStart = new DateTime(2012, 03, 29);
        }

        private void btnRépartir_Click(object sender, EventArgs e)
        {
            try
            {
                CalculerRepartition();

                txt.ForeColor = Color.Black;
                txt.Clear();
                txtMessage.Clear();

                AfficherRepartition();
            }
            catch (Exception ex)
            {
                txtMessage.ForeColor = Color.Red;

                txtMessage.AppendText(ex.Message);
            }
            finally
            {
                string message = "";

                VerificationRepartition();
                if (We != (int)numWe.Value)
                    message += String.Format("\r\nNombre de Week end évalué incorrect {0} vs {1}", We, numWe.Value);


                if (message != "")
                    message = "\r\n-----------------------------------------\r\n" + message;

                txtMessage.ForeColor = Color.Red;

                txtMessage.AppendText(message);

            }
        }

        private void AfficherRepartition()
        {
            Dictionary<DayOfWeek, String> dicNomJour = new Dictionary<DayOfWeek, string>();
            //dicNomJour.Add(DayOfWeek.Friday, "Vendredi   ");
            //dicNomJour.Add(DayOfWeek.Monday, "Lundi        ");
            //dicNomJour.Add(DayOfWeek.Saturday, "Samedi      ");
            //dicNomJour.Add(DayOfWeek.Sunday, "Dimanche");
            //dicNomJour.Add(DayOfWeek.Thursday, "Jeudi        ");
            //dicNomJour.Add(DayOfWeek.Tuesday, "Mardi        ");
            //dicNomJour.Add(DayOfWeek.Wednesday, "Mercredi     ");

            //dicNomJour.Add(DayOfWeek.Friday, "Vendredi   ");
            //dicNomJour.Add(DayOfWeek.Monday, "Lundi          ");
            //dicNomJour.Add(DayOfWeek.Saturday, "Samedi        ");
            //dicNomJour.Add(DayOfWeek.Sunday, "Dimanche");
            //dicNomJour.Add(DayOfWeek.Thursday, "Jeudi          ");
            //dicNomJour.Add(DayOfWeek.Tuesday, "Mardi          ");
            //dicNomJour.Add(DayOfWeek.Wednesday, "Mercredi   ");

            foreach (Jour jour in listJour)
            {
                int txtLength = txt.TextLength;
                //string detail = dicNomJour[jour.Date.DayOfWeek] + "\t" + jour.Date.ToShortDateString();
                //detail += jour.TypeAbsence.Trim() != "" ? " : " + jour.TypeAbsence + " " + jour.Numero : "";
                //detail += jour.Détail != "" ? " (" + jour.Détail + ")" : "";
                //detail += "\r\n";

                txt.AppendText(jour.ToString());

                if (jour.Date.DayOfWeek == DayOfWeek.Sunday)
                {
                    txt.SelectionStart = txtLength;
                    txt.SelectionLength = txt.TextLength - txtLength;
                    txt.SelectionColor = Color.Blue;
                }
            }
        }

        private void CalculerRepartition()
        {
            listJour = new List<Jour>();

            for (int i = 1; i <= calendar.SelectionStart.DayOfYear; i++)
            {
                Jour jour = new Jour();
                jour.Date = new DateTime(2011, 12, 31).AddDays(i);

                listJour.Add(jour);
            }

            RP = (int)numRP.Value;
            RPr = (int)numRPr.Value;
            RPs = (int)numRPs.Value;
            RPt = (int)numRPt.Value;
            MA = (int)numMA.Value;
            BA = (int)numBA.Value;
            RH = (int)numRH.Value;
            VT = (int)numVT.Value;
            VC = (int)numVC.Value;
            Di = (int)numDI.Value;
            CA = (int)numCA.Value;
            RM = (int)numRM.Value;
            RU = (int)numRU.Value;
            We = (int)numWe.Value;

            //--- Répartition des RPs (Un RP isolé sur un dimanche)
            for (int i = 1; i <= numRPs.Value; i++)
            {
                for (int j = 0; j < listJour.Count; j++)
                {
                    Jour jour = listJour[j];
                    Jour jourJm1 = null;
                    Jour jourJp1 = null;

                    if (j > 0)
                        jourJm1 = listJour[j - 1];
                    if (j < listJour.Count - 1)
                        jourJp1 = listJour[j + 1];

                    if (jour.TypeAbsence == "" && jour.Date.DayOfWeek == DayOfWeek.Sunday && RP > 0)
                    {
                        RP--;
                        RPs--;

                        jour.TypeAbsence = "RP";
                        jour.Numero = (int)numRP.Value - RP;
                        jour.Détail = "RPs " + i.ToString() + "/" + numRPs.Value.ToString();
                        jour.AjouterEtat(EtatJour.Simple);
                        jour.AjouterEtat(EtatJour.Dimanche);

                        if (jourJm1 != null)
                            jourJm1.AjouterEtat(EtatJour.InterditRP);
                        if (jourJp1 != null)
                            jourJp1.AjouterEtat(EtatJour.InterditRP);

                        break;
                    }
                }
            }
            //---

            if (RPs > 0)
                throw new Exception(String.Format("Impossible de mettre en place la quantité de RPs. {0} RPs ne sont pas affectables. Nombre de RP restant : {1}", RPs, RP));

            //--- Répartition des We (Samedi et Dimanche en RP)
            for (int i = 1; i <= numWe.Value; i++)
            {
                for (int j = 0; j < listJour.Count; j++)
                {
                    Jour jourJ = listJour[j];
                    Jour jourJp1 = null;

                    if (j < listJour.Count - 1)
                        jourJp1 = listJour[j + 1];

                    if (jourJp1 != null && jourJ.ContientEtat(EtatJour.Vide) && jourJp1.ContientEtat(EtatJour.Vide) && jourJ.Date.DayOfWeek == DayOfWeek.Saturday && jourJp1.Date.DayOfWeek == DayOfWeek.Sunday && RP >= 2)
                    {
                        jourJ.TypeAbsence = "RP";
                        jourJ.Numero = (int)numRP.Value - RP + 1;
                        jourJ.Détail = "We " + i.ToString() + "/" + numWe.Value.ToString();

                        jourJp1.TypeAbsence = "RP";
                        jourJp1.Numero = (int)numRP.Value - RP + 2;
                        jourJp1.Détail = "We " + i.ToString() + "/" + numWe.Value.ToString() + " Di " + ((int)numDI.Value - Di + 1).ToString() + "/" + numDI.Value.ToString();

                        jourJ.AjouterEtat(EtatJour.WeekEnd);
                        jourJp1.AjouterEtat(EtatJour.WeekEnd);
                        jourJp1.AjouterEtat(EtatJour.Dimanche);

                        RP -= 2;
                        We--;
                        Di--;

                        break;
                    }
                }
            }
            //---

            //--- Verrouille les Week end pour ne plus poser de RP dessus
            for (int j = 0; j < listJour.Count; j++)
            {
                Jour jourJ = listJour[j];
                Jour jourJp1 = null;

                if (j < listJour.Count - 1)
                    jourJp1 = listJour[j + 1];

                if (jourJp1 != null && jourJ.ContientEtat(EtatJour.Vide) && jourJp1.ContientEtat(EtatJour.Vide) && jourJ.Date.DayOfWeek == DayOfWeek.Saturday && jourJp1.Date.DayOfWeek == DayOfWeek.Sunday)
                {
                    jourJ.AjouterEtat(EtatJour.InterditRP);
                    jourJp1.AjouterEtat(EtatJour.InterditRP);
                }
            }
            //---

            if (We > 0)
                throw new Exception(String.Format("Impossible de mettre en place la quantité de Week end. {0} Week end ne sont pas affectables. Nombre de RP restant : {1}", We, RP));

            if (Di < 0)
                throw new Exception(String.Format("Impossible de mettre en place la quantité de Week end. {0} Dimanches sont consommés en trop par rapport à la quantité de Di saisie", Math.Abs(Di)));


            //--- Répartition des RPt (exactement 3 jours RP consécutifs, peut être combiné avec les We)
            int indexNextRPT = 2;

            for (int i = 1; i <= numRPt.Value; i++)
            {
                bool found = false;

                for (int j = 0; j < listJour.Count; j++)
                {
                    Jour jour = listJour[j];

                    if (jour.Date.DayOfWeek == DayOfWeek.Saturday && jour.ContientEtat(EtatJour.WeekEnd) && !jour.ContientEtat(EtatJour.Triple) && RP >= 1) //---> Le Dimanche suivant est forcément un week end
                    {
                        //--- Détecter le lundi et le vendredi
                        //Jour jourVendredi = null;
                        //Jour jourLundi = null;

                        //if (j > 0)
                        //    jourVendredi = listJour[j - 1];
                        //if (j + 2 < listJour.Count - 1)
                        //    jourLundi = listJour[j + 2];
                        //---

                        Jour jourRPT = null;


                        //--- Choisir le lundi en priorité si premier choix puis alterner
                        if (j+indexNextRPT>0 && j+indexNextRPT< listJour.Count-1 != null)
                        {
                            jourRPT = listJour[j+indexNextRPT];

                            if (indexNextRPT == 2)
                                indexNextRPT = -1;
                            else
                                indexNextRPT = 2;
                        }

                        if (jourRPT == null)
                        {
                            jourRPT = listJour[indexNextRPT];
                        }
                        //---


                        if (jourRPT != null)
                        {
                            listJour[j].TypeAbsence = "RP";
                            listJour[j].Détail += " RPt " + i.ToString() + "/" + numRPt.Value.ToString();
                            listJour[j].AjouterEtat(EtatJour.Triple);

                            listJour[j + 1].TypeAbsence = "RP";
                            listJour[j + 1].Détail += " RPt " + i.ToString() + "/" + numRPt.Value.ToString();
                            listJour[j + 1].AjouterEtat(EtatJour.Triple);

                            jourRPT.TypeAbsence = "RP";
                            jourRPT.Détail += " RPt " + i.ToString() + "/" + numRPt.Value.ToString();
                            jourRPT.AjouterEtat(EtatJour.Triple);

                            if (jourRPT.Date.DayOfWeek == DayOfWeek.Friday && j - 2 >= 0)
                                listJour[j - 2].AjouterEtat(EtatJour.InterditRP);

                            if (jourRPT.Date.DayOfWeek == DayOfWeek.Monday && j + 3 < listJour.Count-1)
                                listJour[j +3].AjouterEtat(EtatJour.InterditRP);

                            ////--- Bloque le jeudi et le lundi afin de ne pas poser de RP desuss par la suite
                            //if (j - 2 > 0)
                            //    listJour[j - 2].AjouterEtat(EtatJour.InterditRP);
                            //if (j + 2 < listJour.Count - 1)
                            //    listJour[j + 2].AjouterEtat(EtatJour.InterditRP);
                            ////---

                            //for (int k = -1; k < 2; k++)
                            //{
                            //    listJour[j + k].TypeAbsence = "RP";
                            //    listJour[j + k].Détail += " RPt " + i.ToString() + "/" + numRPt.Value.ToString();
                            //    listJour[j + k].AjouterEtat(EtatJour.Triple);
                            //}

                            RP--;
                            RPt--;
                            found = true;

                            break;
                        }
                    }

                    #region old
                    /*
                    if (etape == 0 && !jour.ContientEtat(EtatJour.Triple) &&
                        ((jour.Date.DayOfWeek == DayOfWeek.Friday && jour.TypeAbsence == "")
                        ||
                        (jour.Date.DayOfWeek == DayOfWeek.Saturday && jour.TypeAbsence == "RP")
                        ))
                    {
                        etape = 1;
                        found = true;
                    }
                    else if (etape == 1 && !jour.ContientEtat(EtatJour.Triple) &&
                        ((jour.Date.DayOfWeek == DayOfWeek.Saturday && jour.TypeAbsence == "RP")
                        ||
                        (jour.Date.DayOfWeek == DayOfWeek.Sunday && jour.TypeAbsence == "RP")
                        ))
                    {
                        etape = 2;
                        found = true;
                    }
                    else if (etape == 2 && !jour.ContientEtat(EtatJour.Triple) && RP >= 1 &&
                        ((jour.Date.DayOfWeek == DayOfWeek.Sunday && jour.TypeAbsence == "RP")
                        ||
                        (jour.Date.DayOfWeek == DayOfWeek.Monday && jour.TypeAbsence == "")
                        ))
                    {
                        listJour[j - 2].TypeAbsence = "RP";
                        listJour[j - 2].Détail += " RPt " + i.ToString() + "/" + numRPt.Value.ToString();

                        //listJour[j - 1].TypeAbsence = "RP";
                        listJour[j - 1].Détail += " RPt " + i.ToString() + "/" + numRPt.Value.ToString();

                        listJour[j].TypeAbsence = "RP";
                        listJour[j].Détail += " RPt " + i.ToString() + "/" + numRPt.Value.ToString();

                        jour.AjouterEtat(EtatJour.Triple);
                        listJour[j - 1].AjouterEtat(EtatJour.Triple);
                        listJour[j - 2].AjouterEtat(EtatJour.Triple);

                        RPt--;
                        RP--;

                        etape = 3;

                        break;
                    }

                    if (!found)
                        etape = 0;
                */
                    #endregion
                }

                if (!found && RP >= 3)
                {
                    int jourVideConsecutif = 0;

                    for (int j = 0; j < listJour.Count; j++)
                    {
                        Jour jour = listJour[j];

                        if (jour.Etat == EtatJour.Vide)
                            jourVideConsecutif++;
                        else
                            jourVideConsecutif = 0;

                        if (jourVideConsecutif == 3)
                        {
                            listJour[j - 2].TypeAbsence = "RP";
                            listJour[j - 2].Détail += " RPt " + i.ToString() + "/" + numRPt.Value.ToString();
                            listJour[j - 2].AjouterEtat(EtatJour.Triple);

                            listJour[j - 1].TypeAbsence = "RP";
                            listJour[j - 1].Détail += " RPt " + i.ToString() + "/" + numRPt.Value.ToString();
                            listJour[j - 1].AjouterEtat(EtatJour.Triple);

                            listJour[j].TypeAbsence = "RP";
                            listJour[j].Détail += " RPt " + i.ToString() + "/" + numRPt.Value.ToString();
                            listJour[j].AjouterEtat(EtatJour.Triple);

                            RPt--;
                            RP -= 3;

                            break;
                        }
                    }
                }
            }
            //---

            if (RPt > 0)
                throw new Exception(String.Format("Impossible de mettre en place la quantité de RPt. {0} RPt ne sont pas affectables. Nombre de RP restant : {1}", RPt, RP));


            //--- Supprime tous les interdit
            for (int j = 0; j < listJour.Count; j++)
            {
                Jour jour = listJour[j];

                if (jour.ContientEtat(EtatJour.InterditRP))
                    jour.Etat = EtatJour.Vide;



            }
            //---

            //--- Ajoute l'interdit autour des RPs
            for (int j = 0; j < listJour.Count; j++)
            {
                Jour jour = listJour[j];
                Jour jourJm1 = null;
                Jour jourJp1 = null;

                if (j > 0)
                    jourJm1 = listJour[j - 1];
                if (j < listJour.Count - 1)
                    jourJp1 = listJour[j + 1];

                if (jour.ContientEtat(EtatJour.Simple))
                {
                    if (jourJm1 != null)
                        jourJm1.Etat = EtatJour.InterditRP;
                    if (jourJp1 != null)
                        jourJp1.Etat = EtatJour.InterditRP;
                }
            }
            //---


            //--- Détermine les Jour interdit de RP afin de ne pas former des RPt
            int jourRempliConsecutif = 0;
            for (int j = 0; j < listJour.Count; j++)
            {
                Jour jour = listJour[j];

                if (jour.TypeAbsence == "RP")
                {
                    jourRempliConsecutif++;
                }
                else if (jour.TypeAbsence == "")
                {
                    if (jourRempliConsecutif >= 2)
                        jour.AjouterEtat(EtatJour.InterditRP);

                    jourRempliConsecutif = 0;
                }
            }
            //---

            listJour.Reverse();


            //--- Détermine les Jour interdit de RP afin de ne pas former des RPt
            jourRempliConsecutif = 0;
            for (int j = 0; j < listJour.Count; j++)
            {
                Jour jour = listJour[j];

                if (jour.TypeAbsence == "RP")
                {
                    jourRempliConsecutif++;
                }
                else if (jour.TypeAbsence == "")
                {
                    if (jourRempliConsecutif >= 2)
                        jour.AjouterEtat(EtatJour.InterditRP);

                    jourRempliConsecutif = 0;
                }
            }
            //---

            listJour.Reverse();

            //--- Répartition des Di
            int nbDiAAffecte = Di;
            for (int i = 1; i <= nbDiAAffecte; i++)
            {
                for (int j = 0; j < listJour.Count; j++)
                {
                    Jour jour = listJour[j];
                    Jour jourJp1 = null;

                    if (j < listJour.Count - 1)
                        jourJp1 = listJour[j + 1];


                    if (jour.Date.DayOfWeek == DayOfWeek.Sunday && jour.Etat == EtatJour.Vide && jourJp1 != null && jourJp1.Etat == EtatJour.Vide && (CA + RM + RP >= 2))
                    {
                        //--- Jour J (Dimanche)
                        if (RP > 0)
                        {
                            jour.TypeAbsence = "RP";
                            RP--;
                        }
                        else if (CA > 0)
                        {
                            jour.TypeAbsence = "CA";
                            CA--;
                        }
                        else if (RM > 0)
                        {
                            jour.TypeAbsence = "RM";
                            RM--;
                        }
                        //---

                        //--- Jour J+1 (Lundi)
                        if (RP > 0)
                        {
                            jourJp1.TypeAbsence = "RP";
                            RP--;
                        }
                        else if (CA > 0)
                        {
                            jourJp1.TypeAbsence = "CA";
                            CA--;
                        }
                        else if (RM > 0)
                        {
                            jourJp1.TypeAbsence = "RM";
                            RM--;
                        }
                        //---

                        jour.Détail += " Di " + ((int)numDI.Value - Di + 1).ToString() + "/" + numDI.Value.ToString();
                        jour.AjouterEtat(EtatJour.Dimanche);
                        jourJp1.Détail += " Di " + ((int)numDI.Value - Di + 1).ToString() + "/" + numDI.Value.ToString();
                        jourJp1.AjouterEtat(EtatJour.Dimanche);

                        Di--;

                        break;
                    }

                    //if (jour.TypeAbsence == "" && jour.Date.DayOfWeek == DayOfWeek.Monday && RP > 0 && !jour.ContientEtat(EtatJour.InterditRP) && (jourJm1 == null || (jourJm1.ContientEtat(EtatJour.Dimanche) && !jourJm1.ContientEtat(EtatJour.Simple))))
                    //{
                    //    jour.TypeAbsence = "RP";
                    //    jour.Détail += " Di " + ((int)numDI.Value - Di).ToString() + "/" + numDI.Value.ToString();
                    //    jour.AjouterEtat(EtatJour.Dimanche);
                    //    RP--;
                    //}
                }
            }

            //---> Bloque les dimanches restant afin de ne pas mettre d'absence dessus
            foreach (Jour jour in listJour)
            {
                if (jour.TypeAbsence == "" && jour.Date.DayOfWeek == DayOfWeek.Sunday)
                {
                    jour.TypeAbsence = " ";
                }
            }
            //---

            if (Di > 0)
                throw new Exception(String.Format("Impossible de mettre en place la quantité de Dimanches. {0} Di ne sont pas affectables. Nombre de CA : {1}, nombre de RM {2}", Di, CA, RM));

            return;

            //--- Répartir les RP
            int nbRPConsecutif = 0;

            for (int j = 0; j < listJour.Count; j++)
            {
                if (RP == 0)
                    break;

                if (nbRPConsecutif == 2)
                {
                    nbRPConsecutif = 0;
                    continue;

                }

                Jour jour = listJour[j];
                Jour jourJm1 = null;
                Jour jourJp1 = null;

                if (j > 0)
                    jourJm1 = listJour[j - 1];
                if (j < listJour.Count - 1)
                    jourJp1 = listJour[j + 1];


                if (jour.TypeAbsence == "" && (jourJm1 == null || jourJm1.Détail == "") && (jourJp1 == null || jourJp1.Détail == ""))
                {
                    jour.TypeAbsence = "RP";
                    nbRPConsecutif++;

                    RP--;
                }
                else
                {
                    nbRPConsecutif = 0;
                }
            }

            if (RP > 0)
                throw new Exception(String.Format("Impossible de mettre en place la quantité de RP. {0} RP ne sont pas affectables", RP));
            //---

            //--- Répartir les MA
            //---> Calculer le nombre de MA FAC selon la quantité de RPs
            float RPMax = 116f;
            int MAFac = (int)Math.Round(((float)RPr * 365f) / RPMax, MidpointRounding.AwayFromZero);

            for (int i = 1; i <= numMA.Value; i++)
            {
                foreach (Jour jour in listJour)
                {
                    if (jour.TypeAbsence == "" || jour.TypeAbsence == " " || jour.TypeAbsence == "RP")
                    {
                        jour.TypeAbsence = "MA" + jour.TypeAbsence;
                        if (jour.Numero == 0)
                            jour.Numero = i;

                        if (i <= MAFac)
                            jour.Détail += " FAC";

                        MA--;
                        break;
                    }
                }
            }

            if (MA > 0)
                throw new Exception(String.Format("Impossible de mettre en place la quantité de MA. {0} MA ne sont pas affectables", MA));
            //---

            //--- Répartir les BA
            //---> Calculer le nombre de MA FAC selon la quantité de RPs
            //float RPMax = 116f;
            //int MAFac = (int)Math.Round(((float)RPr * 365f) / RPMax, MidpointRounding.AwayFromZero);

            for (int i = 1; i <= numBA.Value; i++)
            {
                foreach (Jour jour in listJour)
                {
                    if (jour.TypeAbsence == "" || jour.TypeAbsence == " " || jour.TypeAbsence == "RP")
                    {
                        jour.TypeAbsence = "BA" + jour.TypeAbsence;
                        if (jour.Numero == 0)
                            jour.Numero = i;

                        //if (i <= MAFac)
                        //    jour.Détail += " FAC";

                        BA--;
                        break;
                    }
                }
            }

            if (BA > 0)
                throw new Exception(String.Format("Impossible de mettre en place la quantité de BA. {0} BA ne sont pas affectables", BA));
            //---

            //--- Répartir les RH
            for (int i = 1; i <= numRH.Value; i++)
            {
                foreach (Jour jour in listJour)
                {
                    if (jour.TypeAbsence == "" || jour.TypeAbsence == " " || jour.TypeAbsence == "MA" || jour.TypeAbsence == "BA")
                    {
                        jour.TypeAbsence += "RH";
                        jour.Numero = i;

                        RH--;
                        break;
                    }
                }
            }

            if (RH > 0)
                throw new Exception(String.Format("Impossible de mettre en place la quantité de RH. {0} RH ne sont pas affectables", RH));
            //---

            //--- Répartir les VT
            for (int i = 1; i <= numVT.Value; i++)
            {
                foreach (Jour jour in listJour)
                {
                    if (jour.TypeAbsence == "" || jour.TypeAbsence == " " || jour.TypeAbsence == "MA" || jour.TypeAbsence == "BA")
                    {
                        jour.TypeAbsence += "VT";
                        jour.Numero = i;

                        VT--;
                        break;
                    }
                }
            }

            if (VT > 0)
                throw new Exception(String.Format("Impossible de mettre en place la quantité de VT. {0} VT ne sont pas affectables", VT));
            //---

            //--- Répartir les VC
            for (int i = 1; i <= numVC.Value; i++)
            {
                foreach (Jour jour in listJour)
                {
                    if (jour.TypeAbsence == "" || jour.TypeAbsence == " " || jour.TypeAbsence == "MA" || jour.TypeAbsence == "BA")
                    {
                        jour.TypeAbsence += "VC";
                        jour.Numero = i;

                        VC--;
                        break;
                    }
                }
            }

            if (VC > 0)
                throw new Exception(String.Format("Impossible de mettre en place la quantité de VC. {0} VC ne sont pas affectables", VC));
            //---

            //--- Répartir les CA
            for (int i = (int)numCA.Value - CA; i < numCA.Value; i++)
            {
                foreach (Jour jour in listJour)
                {
                    if (jour.TypeAbsence == "")
                    {
                        jour.TypeAbsence = "CA";

                        CA--;
                        break;
                    }
                }
            }

            if (CA > 0)
                throw new Exception(String.Format("Impossible de mettre en place la quantité de CA. {0} CA ne sont pas affectables", CA));
            //---

            //--- Répartir les RM
            for (int i = (int)numRM.Value - RM; i < numRM.Value; i++)
            {
                foreach (Jour jour in listJour)
                {
                    if (jour.TypeAbsence == "")
                    {
                        jour.TypeAbsence = "RM";

                        RM--;
                        break;
                    }
                }
            }

            if (RM > 0)
                throw new Exception(String.Format("Impossible de mettre en place la quantité de RM. {0} RM ne sont pas affectables", RM));
            //---

            //--- Répartir les RU
            for (int i = 1; i <= numRU.Value; i++)
            {
                foreach (Jour jour in listJour)
                {
                    if (jour.TypeAbsence == "" || jour.TypeAbsence == " ")
                    {
                        jour.TypeAbsence = "RU";
                        jour.Numero = i;

                        RU--;
                        break;
                    }
                }
            }

            if (RU > 0)
                throw new Exception(String.Format("Impossible de mettre en place la quantité de RU. {0} RU ne sont pas affectables", RU));
            //---

            //--- Met à jour les numéros
            int countRP = 1;
            int countCA = 1;
            int countRM = 1;

            foreach (Jour jour in listJour)
            {
                if (jour.TypeAbsence.Contains("RP"))
                {
                    jour.Numero = countRP;
                    countRP++;
                }
                if (jour.TypeAbsence.Contains("CA"))
                {
                    jour.Numero = countCA;
                    countCA++;
                }
                if (jour.TypeAbsence.Contains("RM"))
                {
                    jour.Numero = countRM;
                    countRM++;
                }
            }
            //---

        }

        private void VerificationRepartition()
        {
            RP = 0;
            RPr = 0;
            RPs = 0;
            RPt = 0;
            MA = 0;
            BA = 0;
            RH = 0;
            VT = 0;
            VC = 0;
            Di = 0;
            CA = 0;
            RM = 0;
            RU = 0;
            We = 0;

            for (int j = 0; j < listJour.Count; j++)
            {
                Jour jour = listJour[j];
                Jour jourSuivant = null;
                if (j < listJour.Count - 1)
                    jourSuivant = listJour[j + 1];

                if (jour.TypeAbsence.Contains("RP"))
                    RP++;
                if (jour.TypeAbsence.Contains("MA"))
                    MA++;
                if (jour.TypeAbsence.Contains("BA"))
                    BA++;
                if (jour.TypeAbsence.Contains("RH"))
                    RH++;
                if (jour.TypeAbsence.Contains("VT"))
                    VT++;
                if (jour.TypeAbsence.Contains("VC"))
                    VC++;
                if (jour.TypeAbsence.Contains("RM"))
                    RM++;
                if (jour.TypeAbsence.Contains("RU"))
                    RU++;

                //--- Compte les WE
                if (jour.Date.DayOfWeek == DayOfWeek.Saturday && jour.TypeAbsence.Contains("RP"))
                {
                    if (j < (listJour.Count - 1))
                    {
                        TimeSpan ecartAbsSuivant = jourSuivant.Date.Subtract(jour.Date);
                        if ((jourSuivant.Date.DayOfWeek == DayOfWeek.Sunday)
                            && jourSuivant.TypeAbsence.Contains("RP")
                            && ecartAbsSuivant.Days == 1)
                        {
                            We++;
                        }
                    }
                }
                //---

            }
        }

        private void numRP_ValueChanged(object sender, EventArgs e)
        {
            btnRépartir.PerformClick();
        }

        private void calendar_DateChanged(object sender, DateRangeEventArgs e)
        {
            btnRépartir.PerformClick();
        }
    }

    public class Jour
    {
        public string TypeAbsence = "";
        public string Détail = "";
        public int Numero = 0;
        public DateTime Date;
        public EtatJour Etat = EtatJour.Vide;

        public bool ContientEtat(EtatJour etat)
        {
            return (this.Etat & etat) == etat;
        }

        public void AjouterEtat(EtatJour etat)
        {
            if (Etat == EtatJour.Vide)
                Etat = etat;
            else
                Etat |= etat;
        }

        public override string ToString()
        {
            Dictionary<DayOfWeek, String> dicNomJour = new Dictionary<DayOfWeek, string>();

            dicNomJour.Add(DayOfWeek.Friday, "Vendredi   ");
            dicNomJour.Add(DayOfWeek.Monday, "Lundi          ");
            dicNomJour.Add(DayOfWeek.Saturday, "Samedi        ");
            dicNomJour.Add(DayOfWeek.Sunday, "Dimanche");
            dicNomJour.Add(DayOfWeek.Thursday, "Jeudi          ");
            dicNomJour.Add(DayOfWeek.Tuesday, "Mardi          ");
            dicNomJour.Add(DayOfWeek.Wednesday, "Mercredi   ");

            string detail = dicNomJour[Date.DayOfWeek] + "\t" + Date.ToShortDateString();
            detail += TypeAbsence.Trim() != "" ? " : " + TypeAbsence + " " + Numero : "";
            detail += Détail != "" ? " (" + Détail + ")" : "";
            detail += "\r\n";

            return detail;
        }
    }

    [Flags()]
    public enum EtatJour
    {
        Vide = 1,
        Simple = 2,
        WeekEnd = 4,
        Dimanche = 8,
        Triple = 16,
        InterditRP = 32
    }
}
