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
        private bool attError = false;
        private int maxDepth = -1;
        private string name = "*";

        public void ExecuteCommand(string directory, List<string> attributes)
        {
            
            Attribute(attributes);
            if (!error && !attError)
            {
                AllDirectories(directory, 0, maxDepth);
                folders.Add(directory);

                foreach (string folder in folders)
                {
                    try
                    {
                        string[] files = (Directory.GetFiles(folder, name));
                        foreach (string file in files)
                        {
                            Console.WriteLine(file);
                        }
                    }
                    catch (UnauthorizedAccessException)
                    {
                        Console.WriteLine(folder + ": Permission denied");
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
            int attCount = 0;
            if (attributes.Contains("-maxdepth"))
            {
                try
                {
                    bool success = Int32.TryParse(attributes.ElementAt(attributes.IndexOf("-maxdepth") + 1), out maxDepth);
                    attCount = attCount + 1;
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
                catch
                {
                    Console.WriteLine("find: argument incorrect.");
                }

            }
            if(attributes.Contains("-name"))
            {
                try
                {
                    string parse;
                    parse = attributes.ElementAt(attributes.IndexOf("-name") + 1);
                    parse = parse.Substring(1, parse.Length - 2);
                    //parse = parse + '"';
                    name = parse;
                    attCount = attCount + 1;
                }
                catch
                {
                    Console.WriteLine("find: argument incorrect.");
                    error = true;
                }
            }
            if (attributes.Count != 0)
            {
                if (attCount != attributes.Count / 2)
                {
                    attError = true;
                    Console.WriteLine("find: argument incorrect.");
                }
            }
        }

    }
}
