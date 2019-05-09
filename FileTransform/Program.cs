using System;
using System.IO;

namespace FileTransform
{
    class Program
    {
        private static string _inputFolder;
        private static string _outputFolder;
        static void Main(string[] args)
        {
            if (args.Length != 2)
            {
                Console.WriteLine("Wrong number of arguments. Please provide input folder path and output folder path separated by space as arguments.");
                return;
            }
                

            _inputFolder = args[0];
            _outputFolder = args[1];


            DirectoryInfo inf = new DirectoryInfo(_inputFolder);
            Directory.CreateDirectory(_outputFolder);

            var zipTool = new ZipTool();
            foreach (FileInfo file in inf.GetFiles("*.gz"))

            {
                zipTool.Decomp(file, _outputFolder);

                string curFile = file.FullName;
                string origName = curFile.Remove(curFile.Length - file.Extension.Length);
                string newPath = _outputFolder + "\\" + Path.GetFileName(origName);

                if (File.Exists(newPath))
                {
                    EditFile(newPath);
                    zipTool.CompressBack(newPath);
                }
            }
        }

        private static void EditFile(string newPath)
        {
            string xmlData = File.ReadAllText(newPath);
            var replacedData = xmlData.Replace("\"D:", "\"C:").Replace("\"d:", "\"C:");
            File.WriteAllText(newPath, replacedData);

        }
    }
}
