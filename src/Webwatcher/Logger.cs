using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Webwatcher
{
    internal static class Logger
    {
        /// <summary>
        /// Prints a line to the log output file
        /// </summary>
        /// <param name="message">Message to print</param>
        internal static void Log(string message)
        {
            if (!File.Exists(Environment.CurrentDirectory + @"\log.txt"))
            {
                File.WriteAllText(Environment.CurrentDirectory + @"\log.txt", "Webwatcher Output Log\n\n");
            }

            File.WriteAllLines(Environment.CurrentDirectory + @"\log.txt", File.ReadAllLines(Environment.CurrentDirectory + @"\log.txt").Append("[" + DateTime.Now + "] " + message));
        }
    }
}
