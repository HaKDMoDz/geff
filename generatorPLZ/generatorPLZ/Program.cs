using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace generatorPLZ
{
    class Program
    {
        static string projectName;
        static bool delete = false;

        static void Main(string[] args)
        {
            projectName = Console.ReadLine();

            if (projectName.StartsWith("d "))
            {
                delete = true;
                projectName = projectName.Substring(2, projectName.Length - 2);
            }

            if (projectName == "")
                projectName = "test";

            Directory.SetCurrentDirectory(@"D:\Geff\Log\geff\PLZ");

            CopyAllFiles(@"src\plz\logic\controller\");
            CopyAllFiles(@"src\plz\logic\gameplay\");
            CopyAllFiles(@"src\plz\logic\render\");
            CopyAllFiles(@"src\plz\logic\ui\screens\");
            CopyAllFiles(@"src\plz\model\");
        }

        private static void CopyAllFiles(string source)
        {
            string newDirectory = Path.Combine(Directory.GetCurrentDirectory(), source, projectName);

            if (delete)
            {
                Directory.Delete(newDirectory, true);
            }
            else
            {
                source = Path.Combine(Directory.GetCurrentDirectory(), source, "template");

                Directory.CreateDirectory(newDirectory);

                foreach (string file in Directory.GetFiles(source))
                {
                    FileInfo fileInfo = new FileInfo(file);

                    if (fileInfo.Extension.ToUpper() == ".JAVA")
                    {
                        Encoding enc = Encoding.GetEncoding(1252);
                        string text = File.ReadAllText(fileInfo.FullName, enc);
                        text = text.Replace("template", projectName);
                        File.WriteAllText(Path.Combine(newDirectory, fileInfo.Name), text);
                    }
                    else
                    {
                        File.Copy(fileInfo.FullName, Path.Combine(newDirectory, fileInfo.Name), true);
                    }
                }
            }
        }
    }
}
