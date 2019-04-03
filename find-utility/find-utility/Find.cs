using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace find_utility
{
    class Find
    {
        private List<string> folders = new List<string>();
        private bool error = false;
        private int maxDepth = -1;

        public void ExecuteCommand(string directory, List<string> attributes)
        {
            
            Attribute(attributes);
            if (!error)
            {
                AllDirectories(directory, 0, maxDepth);
                folders.Add(directory);

                foreach (string folder in folders)
                {
                    try
                    {
                        string[] files = (Directory.GetFiles(folder));
                        foreach (string file in files)
                        {
                            Console.WriteLine(file);
                        }
                    }
                    catch (UnauthorizedAccessException)
                    {
                        Console.WriteLine(folder + ": Access denied");
                    }
                }
                Console.WriteLine();
            }

        }

        private void AllDirectories(string path, int indent, int depth)
        {
            if ((depth == -1 ) || (depth != indent))
            {
                try
                {
                    if ((File.GetAttributes(path) & FileAttributes.ReparsePoint)
                            != FileAttributes.ReparsePoint)
                    {
                        foreach (string folder in Directory.GetDirectories(path))
                        {
                            folders.Add(folder);
                            AllDirectories(folder, indent + 1, depth);
                        }
                    }
                }
                catch (UnauthorizedAccessException)
                {

                }
            }
        }

        private void Attribute(List<string> attributes)
        {
            if (attributes.Contains("-maxdepth"))
            {
                bool success = Int32.TryParse(attributes.ElementAt(attributes.IndexOf("-maxdepth") + 1), out maxDepth);
                if (!success)
                {
                    Console.WriteLine("find: unknown argument.");
                    error = true;
                }
                if (maxDepth < 0)
                {
                    maxDepth = -1;
                }
            }
        }

    }
}
