using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CsvAndXmlConverter
{
    public class CsvAndXmlConverter
    {
        public static void Main(string[] args)
        {
            if (args.Length != 1 || args[0] == "?")
            {
                ShowUsageMessage();
            }
            else
            {
                HandleRequiredAction(args[0]);
            }

        }

        private static void HandleRequiredAction(string command)
        {
            var extension = Path.GetExtension(command);
            if (extension == "csv" || extension == "xml")
            {
                DetermineRequiredActionFromFileExtenstion(command, extension);
            }
            else
            {
                ShowUsageMessage();
            }

        }

        private static void DetermineRequiredActionFromFileExtenstion(string path, string extension)
        {
            if (extension == "csv")
            {
                //csv to xml conversion
            }
            else
            {
                //xml to csv conversion
            }
        }

        private static void ShowUsageMessage()
        {
            Console.WriteLine();
        }
    }
}
