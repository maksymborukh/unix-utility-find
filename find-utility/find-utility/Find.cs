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

        public void ExecuteCommand(string directory, List<string> attributes)
        {
            //string[] files = (Directory.GetFiles(directory));
            //folders = Directory.GetDirectories(directory, "*", SearchOption.AllDirectories).ToList();

            AllDirectories(directory);

            foreach (string folder in folders)
            {
                //Console.WriteLine(subDir);
                string[] files = (Directory.GetFiles(folder));
                foreach (string file in files)
                {
                    Console.WriteLine(file);
                }
            }
            
        }

        private void AllDirectories(string path)
        {
            foreach (string folder in Directory.GetDirectories(path))
            {
                try
                {
                    AllDirectories(folder);
                    folders.Add(folder);
                }
                catch (UnauthorizedAccessException)
                {
                    
                }
            }
        }

    }
}
