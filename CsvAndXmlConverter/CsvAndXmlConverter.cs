using CsvAndXmlConverter.Converters;
using CsvAndXmlConverter.Data;
using CsvAndXmlConverter.IO;
using CsvAndXmlConverter.Properties;
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
            IConversionResult result;
            if (extension == "csv")
            {
                var converter = new CsvToXmlConverter(new StandardFileReader(), new FileWriter());
                result = converter.ConvertFile(path);
            }
            else
            {
                result = new ConversionResult();
                //xml to csv conversion
            }
            var completeStatusMessage = result.Success ? Resources.SuccessfulConversionMessage : Resources.FailedConvrsionMessage;
            Console.WriteLine(completeStatusMessage);
            Console.WriteLine(result.ResultMessage);
        }

        private static void ShowUsageMessage()
        {
            Console.WriteLine(Resources.UsageMessage);
        }
    }
}
