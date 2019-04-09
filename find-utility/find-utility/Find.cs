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
        private bool help = false;
        private string name = "*";

        public void ExecuteCommand(string directory, List<string> attributes)
        {
            
            Attribute(attributes);
            if (!error && !attError && !help)
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
                    attCount = attCount + 2;
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
                    name = parse;
                    attCount = attCount + 2;
                }
                catch
                {
                    Console.WriteLine("find: argument incorrect.");
                    error = true;
                }
            }

            if (attributes.Contains("-help"))
            {
                Console.WriteLine("find: -maxdepth number, -name 'name', -help.");
                attCount = attCount + 1;
                help = true;
            }

            if (attributes.Count != 0)
            {
                if (attCount != attributes.Count)
                {
                    attError = true;
                    Console.WriteLine("find: argument incorrect.");
                }
            }
        }

    }
}
