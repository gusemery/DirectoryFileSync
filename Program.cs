using System;

namespace MyApp // Note: actual namespace depends on the project name.
{
    internal class Program
    {
        static string srcRoot;
        static string destRoot;
        static int fileCount = 0;
        static void Main(string[] args)
        {
            if (args.Length < 2 || string.IsNullOrEmpty(args[0]) || string.IsNullOrEmpty(args[1]))
            {
                Console.WriteLine("Both source and destination directories must be specified.");
                Console.ReadKey();
                return;
            }    
            srcRoot = args[0];
            destRoot = args[1];

            if (!srcRoot.EndsWith('\\'))
            {
                srcRoot = String.Concat(srcRoot, @"\");

            }

            if (!destRoot.EndsWith('\\'))
            {
                destRoot = String.Concat(destRoot, @"\");

            }
            ProcessDirectory(srcRoot, destRoot);

            Console.WriteLine("Finished");
            Console.ReadKey();
        }

        static void ProcessDirectory(string src, string dest)
        {
            var dirs = Directory.GetDirectories(src);
            foreach(var dir in dirs)
            {
                var destDir = destRoot + dir.Replace(srcRoot, string.Empty);
                Console.WriteLine("Creating directory {0}..", destDir);
                var destDirInfo = Directory.CreateDirectory(destDir);

                var children = Directory.GetDirectories(dir);
                if (children.Length > 0)
                {
                    
                    
                    ProcessDirectory(dir, destDir);
                }
                CopyFiles(dir, destDir);
            }
        }

        private static void CopyFiles(string dir, string dest)
        {
            if (!dest.EndsWith('\\'))
            {
                dest = String.Concat(dest, @"\");
                
            }
            var files = Directory.GetFiles(dir);
            foreach (string file in files)
            {
                var fName = file.Substring(dir.Length + 1);
                
                if (!Directory.Exists(file))
                {
                    try
                    {
                        var destFName = Path.Combine(dest, fName);
                        Console.WriteLine("Copying file {0} to {1}..", file, destFName);
                        File.Copy(file, destFName);
                    }
                    catch (Exception ex)
                    {
                        string Error = "Error copying file " + file;
                        File.WriteAllText("errors.txt", Error);
                        //throw;
                    }
                }
            }
            //Thread.Sleep(3000);
            fileCount++;
        }
    }
}