using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
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

        private void Repartir()
        {
            if (testAuto)
                return;

            txtMessage.Clear();

            try
            {
                CalculerRepartition();

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

                VerificationRepartition(ref message);
                if (We != (int)numWe.Value)
                    message += String.Format("\r\nNombre de Week end évalué incorrect {0} vs {1}", We, numWe.Value);
                if (Di != (int)numDI.Value)
                    message += String.Format("\r\nNombre de Di évalué incorrect {0} vs {1}", Di, numDI.Value);
                if (RPs != (int)numRPs.Value)
                    message += String.Format("\r\nNombre de RPs évalué incorrect {0} vs {1}", RPs, numRPs.Value);
                if (RPt != (int)numRPt.Value)
                    message += String.Format("\r\nNombre de RPt évalué incorrect {0} vs {1}", RPt, numRPt.Value);


                if (message != "")
                    message = "\r\n-----------------------------------------\r\n" + message;

                txtMessage.ForeColor = Color.Red;

                txtMessage.AppendText(message);

            }
        }

        private void AfficherRepartition()
        {
            txt.ForeColor = Color.Black;
            txt.Clear();

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


            if (We > 0)
                throw new Exception(String.Format("Impossible de mettre en place la quantité de Week end. {0} Week end ne sont pas affectables. Nombre de RP restant : {1}", We, RP));

            if (Di < 0)
                throw new Exception(String.Format("Impossible de mettre en place la quantité de Week end. {0} Dimanches sont consommés en trop par rapport à la quantité de Di saisie", Math.Abs(Di)));

            EvaluerInterdictionRP(3);

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
                        Jour jourRPT = null;

                        //--- Choisir le lundi en priorité si premier choix puis alterner
                        if (j + indexNextRPT > 0 && j + indexNextRPT < listJour.Count - 1 != null)
                        {
                            jourRPT = listJour[j + indexNextRPT];

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
                            if (jourRPT.Date.DayOfWeek == DayOfWeek.Friday && j + 2 < listJour.Count - 1)
                                listJour[j + 2].AjouterEtat(EtatJour.InterditRP);

                            if (jourRPT.Date.DayOfWeek == DayOfWeek.Monday && j - 2 >= 0)
                                listJour[j - 1].AjouterEtat(EtatJour.InterditRP);
                            if (jourRPT.Date.DayOfWeek == DayOfWeek.Monday && j + 3 < listJour.Count - 1)
                                listJour[j + 3].AjouterEtat(EtatJour.InterditRP);

                            RP--;
                            RPt--;
                            found = true;

                            break;
                        }
                    }
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


                            if (j + 1 < listJour.Count - 1)
                                listJour[j + 1].AjouterEtat(EtatJour.InterditRP);

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


            EvaluerInterdictionRP(2);

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
                        if (RP >= 2)
                        {
                            jour.TypeAbsence = "RP";
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
                        if (RP >= 2)
                        {
                            jourJp1.TypeAbsence = "RP";
                            RP -= 2;
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
                }
            }

            if (Di > 0)
                throw new Exception(String.Format("Impossible de mettre en place la quantité de Dimanches. {0} Di ne sont pas affectables. Nombre de CA : {1}, nombre de RM {2}", Di, CA, RM));

            EvaluerInterdictionRP(4);

            //--- Répartir les RP
            int nbJourVideConsecutif = 0;
            for (int j = 0; j < listJour.Count; j++)
            {
                if (RP >= 2)
                {
                    Jour jour = listJour[j];
                    Jour jourJm1 = null;
                    Jour jourJp1 = null;

                    if (j > 0)
                        jourJm1 = listJour[j - 1];
                    if (j < listJour.Count - 1)
                        jourJp1 = listJour[j + 1];


                    if (jour.Etat == EtatJour.Vide && jour.Date.DayOfWeek != DayOfWeek.Sunday)
                        nbJourVideConsecutif++;
                    else
                        nbJourVideConsecutif = 0;

                    if (nbJourVideConsecutif == 2)
                    {
                        jourJm1.TypeAbsence = "RP";
                        jourJm1.AjouterEtat(EtatJour.Absence);

                        jour.TypeAbsence = "RP";
                        jour.AjouterEtat(EtatJour.Absence);

                        //if (jourJp1 != null)
                        //    jourJp1.AjouterEtat(EtatJour.InterditRP);

                        EvaluerInterdictionRP(4);

                        RP -= 2;

                        nbJourVideConsecutif = 0;
                    }

                }
            }

            if (RP > 0)
                throw new Exception(String.Format("Impossible de mettre en place la quantité de RP. {0} RP ne sont pas affectables", RP));
            //---

            //---> Calculer le nombre de MA FAC selon la quantité de RPs
            float RPMax = 116f;
            int MABAFac = (int)Math.Ceiling(((float)RPr * 365f) / (float)RPMax);


            //--- Distribution des MA/BA Fac uniquement sur des jours vides
            foreach (Jour jour in listJour)
            {
                if (jour.ContientEtat(EtatJour.Vide) && MABAFac > 0 && (MA + BA > 0))
                {
                    if (MA > 0)
                    {
                        jour.TypeAbsence = "MA";
                        MA--;
                    }
                    else if (BA > 0)
                    {
                        jour.TypeAbsence = "BA";
                        BA--;
                    }

                    jour.Détail += " FAC";
                    MABAFac--;

                    jour.AjouterEtat(EtatJour.Absence);
                }
            }

            if (MABAFac > 0)
                throw new Exception(String.Format("Impossible de mettre en place la quantité de RP réduit selon les emplacements libres dans le calendrier. {0} MA/BA FAC ne sont pas affectables", MABAFac));
            //---

            //--- Répartir les MA
            for (int i = (int)numMA.Value - MA; i < numMA.Value; i++)
            {
                foreach (Jour jour in listJour)
                {
                    if (jour.TypeAbsence == "" || jour.TypeAbsence == " " || jour.TypeAbsence == "RP")
                    {
                        jour.TypeAbsence = "MA" + jour.TypeAbsence;

                        jour.AjouterEtat(EtatJour.Absence);

                        MA--;
                        break;
                    }
                }
            }

            if (MA > 0)
                throw new Exception(String.Format("Impossible de mettre en place la quantité de MA. {0} MA ne sont pas affectables", MA));
            //---

            //--- Répartir les BA
            for (int i = (int)numBA.Value - BA; i < numBA.Value; i++)
            {
                foreach (Jour jour in listJour)
                {
                    if (jour.TypeAbsence == "" || jour.TypeAbsence == " " || jour.TypeAbsence == "RP")
                    {
                        jour.TypeAbsence = "BA" + jour.TypeAbsence;

                        jour.AjouterEtat(EtatJour.Absence);

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
                        jour.AjouterEtat(EtatJour.Absence);

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
                        jour.AjouterEtat(EtatJour.Absence);

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
                        jour.AjouterEtat(EtatJour.Absence);

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
                    if (!jour.ContientEtat(EtatJour.InterditDi) && jour.TypeAbsence == "")
                    {
                        jour.TypeAbsence = "CA";
                        jour.AjouterEtat(EtatJour.Absence);

                        EvaluerInterdictionRP(4);

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
                    if (!jour.ContientEtat(EtatJour.InterditDi) && jour.TypeAbsence == "")
                    {
                        jour.TypeAbsence = "RM";
                        jour.AjouterEtat(EtatJour.Absence);

                        EvaluerInterdictionRP(4);

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
                        jour.AjouterEtat(EtatJour.Absence);

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
            int countMA = 1;
            int countBA = 1;

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
                if (jour.TypeAbsence.Contains("MA"))
                {
                    if (jour.TypeAbsence == "MA")
                        jour.Numero = countMA;

                    countMA++;
                }
                if (jour.TypeAbsence.Contains("BA"))
                {
                    if (jour.TypeAbsence == "BA")
                        jour.Numero = countBA;

                    countBA++;
                }
            }
            //---

        }

        private void EvaluerInterdictionRP(int niveau)
        {
            if (niveau < 1)
                return;

            //--- Supprime tous les interdits
            for (int j = 0; j < listJour.Count; j++)
            {
                Jour jour = listJour[j];

                if (jour.ContientEtat(EtatJour.InterditRP))
                    jour.Etat &= ~EtatJour.InterditRP;

                if (jour.ContientEtat(EtatJour.InterditDi))
                    jour.Etat &= ~EtatJour.InterditDi;
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
                        jourJm1.AjouterEtat(EtatJour.InterditRP);
                    if (jourJp1 != null)
                        jourJp1.AjouterEtat(EtatJour.InterditRP);
                }
            }
            //---

            if (niveau < 2)
                return;

            //--- Détermine les jours interdits de RP afin de ne pas former des RPt
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

            if (niveau < 3)
                return;

            //--- Verrouille les Week end pour ne plus poser de RP dessus de même pour les dimanches
            for (int j = 0; j < listJour.Count; j++)
            {
                Jour jourJ = listJour[j];
                Jour jourJm1 = null;
                Jour jourJp1 = null;

                if (j > 0)
                    jourJm1 = listJour[j - 1];

                if (j + 1 < listJour.Count)
                    jourJp1 = listJour[j + 1];

                if (jourJ.ContientEtat(EtatJour.Vide) && jourJ.Date.DayOfWeek == DayOfWeek.Sunday)
                {
                    jourJ.AjouterEtat(EtatJour.InterditRP);
                }

                if (jourJm1 != null && jourJm1.ContientEtat(EtatJour.Vide) && jourJ.Date.DayOfWeek == DayOfWeek.Sunday && jourJ.TypeAbsence == "RP")
                {
                    jourJm1.AjouterEtat(EtatJour.InterditDi);
                }

                if (jourJp1 != null && jourJp1.ContientEtat(EtatJour.Vide) && jourJ.Date.DayOfWeek == DayOfWeek.Sunday && jourJ.TypeAbsence == "RP")
                {
                    jourJp1.AjouterEtat(EtatJour.InterditDi);
                }
            }
            //---

            if (niveau < 4)
                return;

            //--- Verrouille les dimanches afin de ne pas placer de CA/RM/RP consécutifs autour d'un dimanche
            for (int j = 0; j < listJour.Count; j++)
            {
                Jour jourJ = listJour[j];
                Jour jourJm1 = null;
                Jour jourJp1 = null;

                if (j > 0)
                    jourJm1 = listJour[j - 1];

                if (j + 1 < listJour.Count)
                    jourJp1 = listJour[j + 1];

                if (jourJ.EstCARMRP && jourJ.Date.DayOfWeek == DayOfWeek.Sunday)
                {
                    if (jourJm1 != null)
                        jourJm1.AjouterEtat(EtatJour.InterditDi);
                    if (jourJp1 != null)
                        jourJp1.AjouterEtat(EtatJour.InterditDi);
                }

                if (jourJ.EstCARMRP && jourJ.Date.DayOfWeek == DayOfWeek.Saturday)
                {
                    if (jourJp1 != null)
                        jourJp1.AjouterEtat(EtatJour.InterditDi);
                }

                if (jourJ.EstCARMRP && jourJ.Date.DayOfWeek == DayOfWeek.Monday)
                {
                    if (jourJm1 != null)
                        jourJm1.AjouterEtat(EtatJour.InterditDi);
                }
            }
            //---
        }

        private void VerificationRepartition(ref string message)
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

            int nombreRPConsecutif = 0;

            for (int j = 0; j < listJour.Count; j++)
            {
                Jour jour = listJour[j];
                Jour jourPrecedent = null;
                Jour jourSuivant = null;

                if (j > 0)
                    jourPrecedent = listJour[j - 1];
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
                if (jour.Date.DayOfWeek == DayOfWeek.Saturday && jour.TypeAbsence.Contains("RP") && jourSuivant != null && jourSuivant.TypeAbsence.Contains("RP"))
                {
                    We++;
                }
                //---

                //--- Compte les Di
                if (jour.Date.DayOfWeek == DayOfWeek.Sunday && jour.EstCARMRP && ((jourSuivant != null && jourSuivant.EstCARMRP) || (jourPrecedent != null && jourPrecedent.EstCARMRP)))
                {
                    Di++;
                }
                //---

                //--- Compte les RPs et RPt
                if (jour.TypeAbsence.Contains("RP"))
                {
                    nombreRPConsecutif++;
                }
                else
                {
                    if (nombreRPConsecutif == 1)
                        RPs++;
                    else if (nombreRPConsecutif == 3)
                        RPt++;
                    else if (nombreRPConsecutif > 3)
                        message = "Trop de RP consecutifs : " + nombreRPConsecutif.ToString();

                    nombreRPConsecutif = 0;
                }
                //---
            }
        }

        private bool testAuto = false;

        private void TestsAuto()
        {
            Random rnd = new Random(1000);
            int countTest = 500000;
            int nbErreur = 0;
            int nbErreurVerif = 0;
            List<int> listTestPassant = new List<int>();
            List<int> listTestErreurValidation = new List<int>();

            this.WindowState = FormWindowState.Minimized;
            testAuto = true;
            this.Cursor = Cursors.WaitCursor;
            groupBox1.Visible = false;
            this.Enabled = false;

            for (int i = 0; i < countTest; i++)
            {
                numRP.Value = rnd.Next(0, 116);
                numRPr.Value = rnd.Next(0, 10);
                numRPs.Value = rnd.Next(0, 10);
                numMA.Value = rnd.Next(0, 50);
                numBA.Value = rnd.Next(0, 50);
                numRH.Value = rnd.Next(0, 10);
                numVT.Value = rnd.Next(0, 10);
                numVC.Value = rnd.Next(0, 10);
                numDI.Value = rnd.Next(0, 20);
                numRPs.Value = rnd.Next(0, 10);
                numRPt.Value = rnd.Next(0, 10);
                numCA.Value = rnd.Next(0, 28);
                numRM.Value = rnd.Next(0, 10);
                numRU.Value = rnd.Next(0, 10);
                numWe.Value = rnd.Next(0, 10);

                //---
                //txtMessage.Clear();

                bool erreur = false;

                Application.DoEvents();

                try
                {
                    CalculerRepartition();

                    //AfficherRepartition();
                }
                catch (Exception ex)
                {
                    erreur = true;
                    nbErreur++;

                    if (!ex.Message.StartsWith("Impossible"))
                    {
                        int a = 0;
                    }
                }
                finally
                {
                    string message = "";

                    VerificationRepartition(ref message);
                    if (We != (int)numWe.Value)
                        message += String.Format("\r\nNombre de Week end évalué incorrect {0} vs {1}", We, numWe.Value);
                    if (Di != (int)numDI.Value)
                        message += String.Format("\r\nNombre de Di évalué incorrect {0} vs {1}", Di, numDI.Value);
                    if (RPs != (int)numRPs.Value)
                        message += String.Format("\r\nNombre de RPs évalué incorrect {0} vs {1}", RPs, numRPs.Value);
                    if (RPt != (int)numRPt.Value)
                        message += String.Format("\r\nNombre de RPt évalué incorrect {0} vs {1}", RPt, numRPt.Value);

                    if (!erreur && message != "")
                    {
                        nbErreurVerif++;
                        listTestErreurValidation.Add(i);
                    }
                    else if (!erreur)
                    {
                        listTestPassant.Add(i);
                    }

                }
                //---
            }

            this.Enabled = true;
            groupBox1.Visible = true;
            this.Cursor = Cursors.Default;
            testAuto = false;
            this.WindowState = FormWindowState.Normal;

            //--- Bilan
            txtMessage.Clear();

            txtMessage.AppendText(String.Format("\r\n{0} Tests exécutés", countTest));
            txtMessage.AppendText(String.Format("\r\n{0} Tests en erreur de paramétrage", nbErreur));
            txtMessage.AppendText(String.Format("\r\n{0} Tests en erreur de validation : ", nbErreurVerif));

            for (int i = 0; i < listTestErreurValidation.Count; i++)
            {
                txtMessage.AppendText(listTestErreurValidation[i].ToString() + ", ");
            }

            txtMessage.AppendText(String.Format("\r\n{0} Tests passants : ", listTestPassant.Count));

            for (int i = 0; i < listTestPassant.Count; i++)
            {
                txtMessage.AppendText(listTestPassant[i].ToString() + ", ");
            }
            //---

        }

        private void numRP_ValueChanged(object sender, EventArgs e)
        {
            Repartir();
        }

        private void calendar_DateChanged(object sender, DateRangeEventArgs e)
        {
            Repartir();
        }

        private void btnTestsAuto_Click(object sender, EventArgs e)
        {
            TestsAuto();
        }

        private void btnValiderGraine_Click(object sender, EventArgs e)
        {
            Random rnd = new Random(1000);
            int a = 0;
            for (int i = 0; i < numGraine.Value; i++)
            {
                a = rnd.Next(0, 116);
                a = rnd.Next(0, 10);
                a = rnd.Next(0, 10);
                a = rnd.Next(0, 50);
                a = rnd.Next(0, 50);
                a = rnd.Next(0, 10);
                a = rnd.Next(0, 10);
                a = rnd.Next(0, 10);
                a = rnd.Next(0, 20);
                a = rnd.Next(0, 10);
                a = rnd.Next(0, 10);
                a = rnd.Next(0, 28);
                a = rnd.Next(0, 10);
                a = rnd.Next(0, 10);
                a = rnd.Next(0, 10);
            }

            testAuto = true;

            numRP.Value = rnd.Next(0, 116);
            numRPr.Value = rnd.Next(0, 10);
            numRPs.Value = rnd.Next(0, 10);
            numMA.Value = rnd.Next(0, 50);
            numBA.Value = rnd.Next(0, 50);
            numRH.Value = rnd.Next(0, 10);
            numVT.Value = rnd.Next(0, 10);
            numVC.Value = rnd.Next(0, 10);
            numDI.Value = rnd.Next(0, 20);
            numRPs.Value = rnd.Next(0, 10);
            numRPt.Value = rnd.Next(0, 10);
            numCA.Value = rnd.Next(0, 28);
            numRM.Value = rnd.Next(0, 10);
            numRU.Value = rnd.Next(0, 10);
            numWe.Value = rnd.Next(0, 10);

            testAuto = false;

            Repartir();
        }

        private void btnRepartir_Click(object sender, EventArgs e)
        {
            Repartir();
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
            if (etat == EtatJour.InterditDi || etat == EtatJour.InterditRP)
            {
                Etat |= etat;
            }
            else if (this.ContientEtat(EtatJour.Vide))
            {
                Etat &= ~EtatJour.Vide;

                Etat |= etat;
            }
            else
            {
                Etat |= etat;
            }
        }

        public bool EstCARMRP
        {
            get
            {
                return this.TypeAbsence.Contains("RP") || this.TypeAbsence.Contains("CA") || this.TypeAbsence.Contains("RM");
            }
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
        InterditRP = 32,
        InterditDi = 64,
        Absence = 128
    }
}
