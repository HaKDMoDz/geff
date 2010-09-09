using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace InvKinetic
{
    static class Program
    {
        /// <summary>
        /// PointF d'entrée principal de l'application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1());
        }
    }
}
