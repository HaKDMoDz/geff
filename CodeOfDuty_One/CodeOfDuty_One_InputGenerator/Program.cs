using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace CodeOfDuty_One_InputGenerator
{
    class Program
    {
        static void Main(string[] args)
        {
            int nbVecteur = 10000;
            if (args != null && args.Length>0)
                nbVecteur = int.Parse(args[0]);

            Random rnd = new Random();
            StringBuilder stringBuilder = new StringBuilder();
            for (int i = 0; i <= nbVecteur; i++)
            {
                int nbElement = rnd.Next(1,65);

                stringBuilder.AppendLine(nbElement.ToString());

                for (int j = 0; j < nbElement; j++)
                {
                    stringBuilder.Append(rnd.Next(0,100).ToString());
                    stringBuilder.Append(" "); 
                }

                stringBuilder.AppendLine();
                stringBuilder.AppendLine();
            }

            File.WriteAllText(Path.Combine(Path.GetDirectoryName( System.Reflection.Assembly.GetExecutingAssembly().Location), "input.txt"), stringBuilder.ToString());
        }
    }
}
