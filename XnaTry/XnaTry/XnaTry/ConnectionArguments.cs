using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Windows.Forms;

namespace XnaTry
{
    public class ConnectionArguments
    {
        public string Name { get; }
        public string Hostname { get; }
        public string TeamName { get; }

        public ConnectionArguments(IList<string> args)
        {
            if (args.Count < 2)
            {
                MessageBox.Show("Please run the game through the launcher", "Don't be a dick :)", MessageBoxButtons.OK, MessageBoxIcon.Information);
                Process.GetCurrentProcess().Kill();
            }

            Name = "giorag";
            Hostname = "localhost";
            TeamName = Convert.ToBoolean(new Random().Next(0, 2)) ? "Good" : "Bad";

            var argIndex = 0;
            if (args.Count > argIndex)
                Name = args[argIndex];
            if (args.Count > ++argIndex)
                Hostname = args[argIndex];
            if (args.Count > ++argIndex)
                TeamName = args[argIndex];
        }
    }
}