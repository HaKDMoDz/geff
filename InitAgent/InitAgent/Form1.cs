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
            txt.ForeColor = Color.Black;
            txt.Clear();

            try
            {
                CalculerRepartition();
                AfficherRepartition();
            }
            catch (Exception ex)
            {
                txt.ForeColor = Color.Red;

                txt.AppendText(ex.Message);
            }
        }

        private void AfficherRepartition()
        {
            Dictionary<DayOfWeek, String> dicNomJour = new Dictionary<DayOfWeek, string>();
            dicNomJour.Add(DayOfWeek.Friday, "Vendredi   ");
            dicNomJour.Add(DayOfWeek.Monday, "Lundi        ");
            dicNomJour.Add(DayOfWeek.Saturday, "Samedi      ");
            dicNomJour.Add(DayOfWeek.Sunday, "Dimanche");
            dicNomJour.Add(DayOfWeek.Thursday, "Jeudi        ");
            dicNomJour.Add(DayOfWeek.Tuesday, "Mardi        ");
            dicNomJour.Add(DayOfWeek.Wednesday, "Mercredi     ");

            foreach (Jour jour in listJour)
            {
                int txtLength = txt.TextLength;
                string detail = dicNomJour[jour.Date.DayOfWeek] + "\t" + jour.Date.ToShortDateString();
                detail += jour.TypeAbsence.Trim() != "" ? " : " + jour.TypeAbsence + " " + jour.Numero : "";
                detail +=  jour.Détail != "" ?  " (" + jour.Détail +")" : "";
                detail += "\r\n";

                txt.AppendText(detail);

                if (jour.Date.DayOfWeek == DayOfWeek.Sunday)
                {
                    txt.SelectionStart = txtLength;
                    txt.SelectionLength = txt.TextLength - txtLength;
                    txt.SelectionColor = Color.Blue;
                }
            }
        }

        private void AjouterEtat(Jour jour, EtatJour etat)
        {
            if (jour.Etat == EtatJour.Vide)
                jour.Etat = etat;
            else
                jour.Etat |= etat;
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
                foreach (Jour jour in listJour)
                {
                    if (jour.TypeAbsence == "" && jour.Date.DayOfWeek == DayOfWeek.Sunday && RP>0)
                    {
                        RP--;
                        RPs--;
                        Di--;

                        jour.TypeAbsence = "RP";
                        jour.Numero = (int)numRP.Value - RP;
                        jour.Détail = "RPs " + i.ToString() + "/" + numRPs.Value.ToString() + " Di " + ((int)numDI.Value-Di).ToString() + "/" + numDI.Value.ToString();;
                        AjouterEtat(jour, EtatJour.Simple);
                        AjouterEtat(jour, EtatJour.Dimanche);

                        break;
                    }
                }
            }
            //---

            if (RPs > 0)
                throw new Exception(String.Format("Impossible de mettre en place la quantité de RPs. {0} RPs ne sont pas affectables. Nombre de RP restant : {1}", RPs, RP));
            if (Di<0)
                throw new Exception(String.Format("Impossible de mettre en place la quantité de RPs. {0} Dimanches sont consommés en trop par rapport à la quantité de Di saisie", Math.Abs(Di)));

            //--- Répartition des We (Samedi et Dimanche en RP)
            for (int i = 1; i <= numWe.Value; i++)
            {
                for(int j = 0; j < listJour.Count; j++)
                {
                    Jour jourJ = listJour[j];
                    Jour jourJp1 = null;

                    if (j < listJour.Count - 1)
                        jourJp1 = listJour[j + 1];

                    if (jourJp1 != null && jourJ.TypeAbsence == "" && jourJp1.TypeAbsence == "" && jourJ.Date.DayOfWeek == DayOfWeek.Saturday && jourJp1.Date.DayOfWeek == DayOfWeek.Sunday && RP>=2)
                    {

                        jourJ.TypeAbsence = "RP";
                        jourJ.Numero = (int)numRP.Value - RP +1;
                        jourJ.Détail = "We " + i.ToString() + "/" + numWe.Value.ToString();

                        jourJp1.TypeAbsence = "RP";
                        jourJp1.Numero = (int)numRP.Value - RP+2;
                        jourJp1.Détail = "We " + i.ToString() + "/" + numWe.Value.ToString() + " Di " + ((int)numDI.Value-Di+1).ToString() + "/" + numDI.Value.ToString();

                        AjouterEtat(jourJ, EtatJour.WeekEnd);
                        AjouterEtat(jourJp1, EtatJour.WeekEnd);
                        AjouterEtat(jourJp1, EtatJour.Dimanche);

                        RP -= 2;
                        We--;
                        Di--;

                        break;
                    }
                }
            }
            //---

            if (We > 0)
                throw new Exception(String.Format("Impossible de mettre en place la quantité de Week end. {0} Week end ne sont pas affectables. Nombre de RP restant : {1}", We, RP));

            if (Di < 0)
                throw new Exception(String.Format("Impossible de mettre en place la quantité de Week end. {0} Dimanches sont consommés en trop par rapport à la quantité de Di saisie", Math.Abs(Di)));

            //--- Répartition des RPt (exactement 3 jours RP consécutifs, peut être combiné avec les We)
            for (int i = 1; i <= numRPt.Value; i++)
            {
                int etape = 0;

                for (int j = 0; j < listJour.Count; j++)
                {
                    Jour jour = listJour[j];
                    bool found = false;

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
                    else if (etape == 2 && !jour.ContientEtat(EtatJour.Triple) && RP>=1 &&
                        ((jour.Date.DayOfWeek == DayOfWeek.Sunday && jour.TypeAbsence == "RP") 
                        || 
                        (jour.Date.DayOfWeek == DayOfWeek.Monday && jour.TypeAbsence == "")
                        ))
                    {
                        listJour[j - 2].TypeAbsence = "RP";
                        listJour[j - 2].Numero = listJour[j - 2].Numero == 0 ? (int)numRP.Value - RP + 1 : listJour[j - 2].Numero;
                        listJour[j - 2].Détail += " RPt " + i.ToString() + "/" + numRPt.Value.ToString();

                        //listJour[j - 1].TypeAbsence = "RP";
                        //listJour[j - 1].Numero = listJour[j - 1].Numero == 0 ? (int)numRP.Value - RP + 1 : listJour[j - 1].Numero;
                        listJour[j - 1].Détail += " RPt " + i.ToString() + "/" + numRPt.Value.ToString();

                        listJour[j].TypeAbsence = "RP";
                        listJour[j].Numero = listJour[j].Numero == 0 ? (int)numRP.Value - RP + 1 : listJour[j].Numero;
                        listJour[j].Détail += " RPt " + i.ToString() + "/" + numRPt.Value.ToString();

                        AjouterEtat(jour, EtatJour.Triple);
                        AjouterEtat(listJour[j - 1], EtatJour.Triple);
                        AjouterEtat(listJour[j - 2], EtatJour.Triple);

                        RPt--;
                        RP--;

                        etape = 3;

                        break;
                    }

                    if (!found)
                        etape = 0;
                }

                if (etape != 3 && RP>=3)
                {
                    int jourVideConsecutif = 0;

                    for (int j = 0; j < listJour.Count; j++)
                    {
                        Jour jour = listJour[j];

                        if (jour.Etat == EtatJour.Vide)
                        {
                            jourVideConsecutif++;

                            if (j == 0 || j == listJour.Count - 1)
                                jourVideConsecutif++;
                        }
                        else
                            jourVideConsecutif = 0;


                        if ((jourVideConsecutif == 4 && j == listJour.Count-1) || jourVideConsecutif==5)
                        {
                            listJour[j - 3].TypeAbsence = "RP";
                            listJour[j - 3].Numero = (int)numRP.Value - RP + 1;
                            listJour[j - 3].Détail += " RPt " + i.ToString() + "/" + numRPt.Value.ToString();
                            AjouterEtat(listJour[j - 3], EtatJour.Triple);

                            listJour[j - 2].TypeAbsence = "RP";
                            listJour[j - 2].Numero = (int)numRP.Value - RP + 2;
                            listJour[j - 2].Détail += " RPt " + i.ToString() + "/" + numRPt.Value.ToString();
                            AjouterEtat(listJour[j - 2], EtatJour.Triple);

                            listJour[j-1].TypeAbsence = "RP";
                            listJour[j-1].Numero = (int)numRP.Value - RP + 3;
                            listJour[j-1].Détail += " RPt " + i.ToString() + "/" + numRPt.Value.ToString();
                            AjouterEtat(listJour[j-1], EtatJour.Triple);

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

            //--- Répartition des Di
            int nbDiAAffecte = Di;
            for (int i = 1; i <= nbDiAAffecte; i++)
            {
                foreach (Jour jour in listJour)
                {
                    if (jour.TypeAbsence == "" && jour.Date.DayOfWeek == DayOfWeek.Sunday)
                    {
                        if (CA > 0)
                        {
                            jour.TypeAbsence = "CA";
                            jour.Numero = (int)numCA.Value - CA+1;
                            jour.Détail += " Di " + ((int)numDI.Value - Di + 1).ToString() + "/" + numDI.Value.ToString();
                            AjouterEtat(jour, EtatJour.Dimanche);
                            Di--;
                            CA--;
                        }
                        else if (RM > 0)
                        {
                            jour.TypeAbsence = "RM";
                            jour.Numero = (int)numRM.Value - RM+1;
                            jour.Détail += " Di " + ((int)numDI.Value - Di + 1).ToString() + "/" + numDI.Value.ToString();
                            AjouterEtat(jour, EtatJour.Dimanche);
                            Di--;
                            RM--;
                        }
                    }
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

            //--- Répartir les MA
            //---> Calculer le nombre de MA FAC selon la quantité de RPs
            float RPMax = 116f;
            int MAFac = (int)Math.Round(((float)RPr*365f)/RPMax , MidpointRounding.AwayFromZero);

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
            for (int i = CA - (int)numCA.Value+1; i <= numCA.Value; i++)
            {
                foreach (Jour jour in listJour)
                {
                    if (jour.TypeAbsence == "")
                    {
                        jour.TypeAbsence = "CA";
                        jour.Numero = i;

                        CA--;
                        break;
                    }
                }
            }

            if (CA > 0)
                throw new Exception(String.Format("Impossible de mettre en place la quantité de CA. {0} CA ne sont pas affectables", CA));
            //---

            //--- Répartir les RM
            for (int i = RM - (int)numRM.Value + 1; i <= numRM.Value; i++)
            {
                foreach (Jour jour in listJour)
                {
                    if (jour.TypeAbsence == "")
                    {
                        jour.TypeAbsence = "RM";
                        jour.Numero = i;

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
    }

    [Flags()]
    public enum EtatJour
    {
        Vide = 1,
        Simple = 2,
        WeekEnd = 4,
        Dimanche = 8,
        Triple = 16
    }
}
