using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace find_utility
{
    class Command
    {
        private Find find;
        private Cd cd;
        private string HomeDirectoryName;
        private string DirectoryName;
        private List<string> attributes = new List<string>();

        public Command()
        {
            find = new Find();
            cd = new Cd();
            HomeDirectoryName = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
        }

        public void ReadCommand()
        {
            string command;
            do
            {
                Console.Write(HomeDirectoryName + ">");
                command = Console.ReadLine();
                ParseCommand(command);
            } while (command != "exit");
        }

        private void ParseCommand(string c)
        {
            string[] utility = c.Split(' ');
            string command = utility[0];

            attributes = utility.ToList();
            if (attributes.Count != 0)
                attributes.RemoveAt(0);

            RecognizeCommand(command);
        }

        private void RecognizeCommand(string command)
        {
            switch (command)
            {
                case "find":
                    find = new Find();
                    find.ExecuteCommand(HomeDirectoryName, attributes);
                    break;
                default:
                    Console.WriteLine("'" + command + "' is not recognized as an internal or external command, operable program or batch file.");
                    break;

            }
        }
    }
}
